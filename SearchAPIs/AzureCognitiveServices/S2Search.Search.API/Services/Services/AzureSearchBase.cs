using Azure.Search.Documents.Models;
using Domain.Extentions;
using Domain.Models.Facets;
using Domain.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Helpers.FacetOverrides;
using Services.Interfaces;
using Services.Interfaces.FacetOverrides;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class AzureSearchBase
    {
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IDisplayTextFormatHelper _displayTextFormatHelper;
        private readonly IFacetHelper _facetHelper;
        private readonly IFacetOverrideProvider _facetOverrideProvider;

        public AzureSearchBase(IAppSettings appSettings,
                                  ILoggerFactory loggerFactory,
                                  IDisplayTextFormatHelper displayTextFormatHelper,
                                  IFacetHelper facetHelper,
                                  IFacetOverrideProvider facetOverrideProvider)
        {
            _logger = loggerFactory.CreateLogger<AzureSearchService>();
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _displayTextFormatHelper = displayTextFormatHelper ?? throw new ArgumentNullException(nameof(displayTextFormatHelper));
            _facetHelper = facetHelper ?? throw new ArgumentNullException(nameof(facetHelper));
            _facetOverrideProvider = facetOverrideProvider ?? throw new ArgumentNullException(nameof(facetOverrideProvider));
        }

        protected IList<FacetGroup> ConvertFacetsToResult(IDictionary<string, IList<FacetResult>> facets)
        {
            try
            {
                List<FacetGroup> tempFacetGroupList = new List<FacetGroup>();

                foreach (var keyValuePair in facets)
                {
                    FacetGroup group = new FacetGroup();
                    group.FacetName = keyValuePair.Key;
                    group.FacetKey = group.FacetName;

                    foreach (var facetResult in keyValuePair.Value)
                    {
                        FacetItem item = new FacetItem(facetResult);

                        if (_appSettings.SearchSettings.FacetToOverrideDisplayList.Any(x => x == group.FacetName))
                        {
                            var overridenFacetItem = _facetOverrideProvider.Override(group.FacetName, item);

                            group.FacetItems.Add(overridenFacetItem);
                        }
                        else
                        {
                            group.FacetItems.Add(item);
                        }
                    }

                    FacetGroup Final = ConfigureFacetGroup(group);
                    tempFacetGroupList.Add(Final);
                }

                var orderedFacetGroupList = _facetHelper.SetFacetOrder(tempFacetGroupList);

                return orderedFacetGroupList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        private FacetGroup ConfigureFacetGroup(FacetGroup group)
        {
            try
            {
                FacetGroup NewGroup = new FacetGroup(group);

                NewGroup.FacetName = StringExtensions.FirstCharToUpper(NewGroup.FacetName);
                _displayTextFormatHelper.SetFacetGroupDisplayNames(group, NewGroup);

                List<FacetItem> TempFacetItems = new List<FacetItem>(group.FacetItems);
                group.FacetItems.Clear();

                foreach (var facetItem in TempFacetItems)
                {
                    if (facetItem.Count == 0) continue;

                    FacetItem NewFacetItem = new FacetItem();
                    NewFacetItem = facetItem;

                    if (facetItem.Type == FacetType.Range.ToString())
                    {
                        if (string.IsNullOrEmpty(facetItem.From))
                        {
                            NewFacetItem.From = "0";
                        }
                        if (string.IsNullOrEmpty(facetItem.To))
                        {
                            NewFacetItem.To = "0";
                            facetItem.To = _appSettings.SearchSettings.FacetMaxRangeToValue.ToString();
                        }
                        else
                        {
                            NewFacetItem.To = facetItem.To != null ? (Convert.ToDouble(facetItem.To)).ToString() : string.Empty;
                        }

                        if (!_appSettings.SearchSettings.FacetToOverrideDisplayList.Any(x => x == group.FacetName))
                        {
                            if (_appSettings.SearchSettings.FacetCurrencyRangesList.Contains(group.FacetName))
                            {
                                NewFacetItem.FacetDisplayText = _displayTextFormatHelper.FormatCurrencyRange(facetItem.From, facetItem.To, "£", string.Empty);
                            }

                            if (_appSettings.SearchSettings.FacetNonCurrencyRangeList.Contains(group.FacetName))
                            {
                                NewFacetItem.FacetDisplayText = _displayTextFormatHelper.FormatNonCurrencyRange(facetItem.From, facetItem.To);
                            }
                        }
                    }
                    else
                    {
                        NewFacetItem.From = null;
                        NewFacetItem.To = null;

                        if (_appSettings.SearchSettings.FacetToOverrideDisplayList.Any(x => x == group.FacetName))
                        {
                            NewFacetItem.FacetDisplayText = facetItem.FacetDisplayText;
                        }
                        else
                        {
                            NewFacetItem.FacetDisplayText = StringExtensions.FirstCharToUpper(facetItem.Value);
                        }
                    }

                    NewGroup.FacetItems.Add(NewFacetItem);
                }

                return NewGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
