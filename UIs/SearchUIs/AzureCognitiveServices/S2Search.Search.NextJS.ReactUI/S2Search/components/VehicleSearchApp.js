import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import FacetFullScreenDialog from './material-ui/filtersDialog/FacetFullScreenDialog';
import VehicleCardList from './material-ui/vehicleCards/VehicleCardList';
import LoadMoreResultsButton from './material-ui/vehicleCards/LoadMoreResultsButton';
import FacetChips from './material-ui/searchPage/FacetChips';
import NetworkErrorDialog from './material-ui/searchPage/NetworkErrorDialog';
import { connect } from 'react-redux';
import SearchRequest from '../pages/api/shared/request/SearchRequest';
import searchActions from '../redux/actions/searchActions';
import facetActions from '../redux/actions/facetActions';
import themeActions from '../redux/actions/themeActions';
import configActions from '../redux/actions/configActions';
import { getSelectedFacets } from '../common/functions/FacetFunctions';
import {
  getConfigValueByKey,
  getPlaceholdersArray,
} from '../common/functions/ConfigFunctions';
import {
  IsPreviousRequestDataTheSame,
  IsRequestReOrderBy,
  ConvertStringToBoolean,
  LogString,
} from '../common/functions/SharedFunctions';
import { LogDetails } from '../helpers/LogDetails';
import componentActions from '../redux/actions/componentActions';
import Box from '@mui/material/Box';
import HelperFunctions from '../helpers/HelperFunctions';
import { grey, red } from '@mui/material/colors';
import Alert from '@mui/material/Alert';
import FloatingTopButton from './material-ui/searchPage/FloatingTopButton';
import {
  SearchAndFacetsAPI,
  DocumentCountAPI,
} from '../pages/api/helper/SearchAPI';
import ThemeAPI from '../pages/api/helper/ThemeAPI';
import ConfigAPI from '../pages/api/helper/ConfigAPI';

import {
  DefaultPageSize,
  DefaultPageNumber,
  DefaultTheme,
  MillisecondsDifference,
} from '../common/Constants';

import AdaptiveNavBar from './material-ui/searchPage/navBars/AdaptiveNavBar';
import {
  insertQueryStringParam,
  removeFullQueryString,
} from '../common/functions/QueryStringFunctions';
import { useRouter } from 'next/router';

// Modern styles using theme-aware sx prop patterns
const styles = {
  root: {
    flexGrow: 1,
  },
  resultsText: {
    color: grey[600],
  },
  noResultsText: {
    color: red[600],
  },
  margin: theme => ({
    margin: theme.spacing(1),
  }),
};

const VehicleSearchApp = props => {
  const [facetSelectedKeys, setFacetSelectedKeys] = useState([]);
  const [searchCount, setSearchCount] = useState(0);
  const [timestamp, setTimestamp] = useState(undefined);
  const [themeConfigured, setThemeConfigured] = useState(false);
  const [searchConfigConfigured, setSearchConfigConfigured] = useState(false);
  const [autoCompleteSearchBar, setAutoCompleteSearchBar] = useState(undefined);
  const [queryStringParams, setQueryStringParams] = useState({});

  const router = useRouter();

  // ******************************************************************************************
  // ** this useEffect hook will setup configurations and theme settings - it is run once only
  // ******************************************************************************************
  useEffect(() => {
    getThemeFromAPI();
    getDocumentCountAPI();
    let config = [];
    if (props.reduxConfigData.length === 0) {
      ConfigAPI(window.location.host).then(function (axiosConfigResponse) {
        if (!searchConfigConfigured && axiosConfigResponse) {
          if (axiosConfigResponse.status === 200) {
            axiosConfigResponse.data.map(function (item) {
              config.push({ key: item.key, value: item.value });
            });

            props.saveConfigData(config);
            props.savePlaceholderArray(getPlaceholdersArray(config));

            const enableAutoComplete = ConvertStringToBoolean(
              getConfigValueByKey(config, 'EnableAutoComplete').value
            );
            props.saveEnableAutoComplete(enableAutoComplete);
            setAutoCompleteSearchBar(enableAutoComplete);

            const HideIconVehicleCounts = ConvertStringToBoolean(
              getConfigValueByKey(config, 'HideIconVehicleCounts').value
            );
            props.saveHideIconVehicleCounts(HideIconVehicleCounts);

            setSearchConfigConfigured(true);
          }
        }
      });
    }
  }, []);

  useEffect(() => {
    if (router && Object.keys(router.query).length > 0) {
      setQueryStringParams(router.query);
      if (queryStringParams) {
        LogString(`searchterm = ${queryStringParams.searchterm}`);
      }
    }
  }, [router.query]);

  // *********************************************************************************************************************
  // ** this useEffect hook manages the search - it will trigger on any change relating to search - see the dependencies
  // *********************************************************************************************************************
  useEffect(() => {
    if (props.reduxSearchTerm.length === 0) {
      props.saveCancellationToken(false);
    } else {
      if (timestamp) {
        const milliseconds = new Date().getTime() - timestamp;

        if (milliseconds < MillisecondsDifference) {
          props.saveCancellationToken(true);
        } else {
          props.saveCancellationToken(false);
        }

        LogString(
          `milliseconds: ${milliseconds} cancellationToken: ${props.reduxCancellationToken} props.reduxSearchTerm : ${props.reduxSearchTerm.length}`
        );
      }
    }

    setTimestamp(new Date().getTime());
    setSearchCount(searchCount + 1);
    setFacetSelectedKeys(props.reduxFacetSelectedKeys);
    updateQueryStringURL();

    triggerSearch(
      new SearchRequest(
        props.reduxSearchTerm,
        getSelectedFacets(props.reduxFacetSelectors),
        props.reduxOrderBy,
        props.reduxPageNumber,
        DefaultPageSize,
        props.reduxVehicleData.length,
        window.location.host
      )
    );
  }, [
    props.reduxSearchTerm,
    props.reduxOrderBy,
    props.reduxDialogOpen,
    props.reduxPageNumber,
    props.reduxFacetChipDeleted,
    props.reduxFacetSelectedKeys,
    queryStringParams,
    props.reduxCancellationToken,
  ]);

  const getThemeFromAPI = () => {
    if (!themeConfigured) {
      try {
        ThemeAPI().then(function (axiosThemeResponse) {
          if (axiosThemeResponse) {
            if (axiosThemeResponse.status === 200) {
              let theme = axiosThemeResponse.data;
              props.savePrimaryColour(theme.primaryHexColour);
              props.saveSecondaryColour(theme.secondaryHexColour);
              props.saveNavBarColour(theme.navBarHexColour);
              props.saveLogoURL(theme.logoURL);
              props.saveMissingImageURL(theme.missingImageURL);

              setThemeConfigured(true);
            }
          }
        });
      } catch (error) {
        props.savePrimaryColour(DefaultTheme.primaryHexColour);
        props.saveSecondaryColour(DefaultTheme.secondaryHexColour);
        props.saveNavBarColour(DefaultTheme.navBarHexColour);
        props.saveLogoURL(DefaultTheme.logoURL);
        props.saveMissingImageURL(DefaultTheme.missingImageURL);
        setThemeConfigured(true);
      }
    }
  };

  const getDocumentCountAPI = () => {
    DocumentCountAPI(window.location.host).then(
      function (axiosDocumentCountResponse) {
        if (axiosDocumentCountResponse) {
          if (axiosDocumentCountResponse.status === 200) {
            let documentCount = axiosDocumentCountResponse.data;
            props.saveTotalDocumentCount(documentCount);
          }
        }
      }
    );
  };

  const triggerSearch = request => {
    const requestPlain = request.toPlainObject();

    if (IsRequestReOrderBy(requestPlain, props.reduxPreviousRequest)) {
      request.numberOfExistingResults = props.reduxVehicleData.length;
      request.pageSize = request.numberOfExistingResults;
      request.pageNumber = 0;
      props.savePageNumber(0);
    }

    // **********************************************************
    // ** only call the API if the search request is different **
    // **********************************************************
    if (
      !IsPreviousRequestDataTheSame(requestPlain, props.reduxPreviousRequest) ||
      props.reduxVehicleData.length == 0 ||
      facetSelectedKeys !== props.reduxFacetSelectedKeys
    ) {
      props.saveLoading(true);
      props.savePreviousRequest(requestPlain);

      SearchAndFacetsAPI(request, props.reduxCancellationToken).then(
        function (axiosSearchResponse) {
          if (axiosSearchResponse) {
            if (axiosSearchResponse.status !== 200) {
              props.saveLoading(false);
              return false;
            } else {
              props.saveNetworkError(false);
              if (
                axiosSearchResponse.data.results &&
                axiosSearchResponse.data.results.length > 0
              ) {
                let vehicleSearchData = [];

                if (
                  props.reduxPageNumber != props.reduxPreviousRequest.pageNumber
                ) {
                  vehicleSearchData = HelperFunctions.RemoveDuplicates([
                    ...props.reduxVehicleData,
                    ...axiosSearchResponse.data.results,
                  ]);
                } else {
                  vehicleSearchData = axiosSearchResponse.data.results;
                }

                props.saveVehicleData(vehicleSearchData);
                props.saveSearchCount(axiosSearchResponse.data.totalResults);
              } else {
                // no results found
                props.savePageNumber(DefaultPageNumber);
                props.saveVehicleData([]);
              }

              if (axiosSearchResponse.data.facets) {
                props.saveFacetData(axiosSearchResponse.data.facets);
              }
              if (props.defaultFacetData.length === 0) {
                SearchAndFacetsAPI(request, props.reduxCancellationToken).then(
                  function (axiosFacetResponse) {
                    if (
                      axiosFacetResponse &&
                      axiosFacetResponse.status === 200
                    ) {
                      if (axiosFacetResponse.data.facets) {
                        props.saveDefaultFacetData(
                          axiosFacetResponse.data.facets
                        );
                      }
                    }
                  }
                );
              }

              props.saveLoading(false);
            }
          }
        }
      );
    }
  };

  const updateQueryStringURL = () => {
    if (props.reduxFacetSelectors.length > 0) {
      insertQueryStringParam(
        'facetselectors',
        JSON.stringify(props.reduxFacetSelectors)
      );

      insertQueryStringParam('orderby', props.reduxOrderBy);
    }

    if (props.reduxSearchTerm.length > 0) {
      insertQueryStringParam('searchterm', props.reduxSearchTerm);
    }

    if (
      props.reduxFacetSelectors.length === 0 &&
      props.reduxSearchTerm.length === 0
    ) {
      removeFullQueryString();
    }
  };

  const RenderNetworkError = () => {
    if (props.reduxNetworkError === true) {
      return <NetworkErrorDialog reduxNetworkError={props.reduxNetworkError} />;
    }

    return '';
  };

  const renderResults = () => {
    if (searchCount === 0) {
      return (
        <div style={{ position: 'relative', top: '5px' }}>
          <Alert severity="info">Loading - Stand by...</Alert>
        </div>
      );
    } else if (
      props.reduxVehicleData.length === 0 &&
      searchCount > 0 &&
      !props.reduxLoading
    ) {
      return (
        <div style={{ position: 'relative', top: '5px' }}>
          <Alert severity="error">
            No vehicles found - to restart click the reset button
          </Alert>
        </div>
      );
    } else if (
      props.reduxVehicleData.length === 0 &&
      searchCount > 0 &&
      props.reduxLoading
    ) {
      return (
        <div style={{ position: 'relative', top: '5px' }}>
          <Alert severity="warning">Loading - please wait</Alert>
        </div>
      );
    }
  };

  return !themeConfigured ? (
    <></>
  ) : (
    <div className="content-container">
      <>
        <Box p={1} sx={styles.root}>
          <Box sx={{ minHeight: 60 }}>
            <AdaptiveNavBar autoCompleteSearchBar={autoCompleteSearchBar} />
          </Box>
          <Box xs={12}>{renderResults()}</Box>
          <Box xs={12}>
            {props.reduxFacetSelectors.length > 0 ? <FacetChips /> : <></>}{' '}
            <VehicleCardList />
          </Box>
          <Box xs={12} style={{ textAlign: 'center' }}>
            <LoadMoreResultsButton />
          </Box>
        </Box>
      </>
      <FacetFullScreenDialog dialogLabel={'Filters'} />
      <RenderNetworkError />
      <FloatingTopButton />
      <LogDetails logData={props} enable={false} />
    </div>
  );
};

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxSearchCount: reduxState.searchReducer.searchCount,
    reduxVehicleData: reduxState.searchReducer.vehicleData,
    reduxOrderBy: reduxState.searchReducer.orderBy,
    reduxPageNumber: reduxState.searchReducer.pageNumber,
    reduxNetworkError: reduxState.searchReducer.networkError,
    reduxPreviousRequest: reduxState.searchReducer.previousRequest,

    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facetReducer.facetSelectedKeys,
    defaultFacetData: reduxState.facetReducer.defaultFacetData,
    reduxSelectedFacet: reduxState.facetReducer.selectedFacet,
    reduxFacetChipDeleted: reduxState.facetReducer.facetChipDeleted,

    reduxLoading: reduxState.componentReducer.loading,
    reduxCancellationToken: reduxState.componentReducer.enableToken,
    reduxDialogOpen: reduxState.componentReducer.dialogOpen,
    reduxConfigData: reduxState.configReducer.configData,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveVehicleData: vehicleData =>
      dispatch(searchActions.saveVehicleData(vehicleData)),
    savePageNumber: pageNumber =>
      dispatch(searchActions.savePageNumber(pageNumber)),
    saveTotalDocumentCount: documentCount =>
      dispatch(searchActions.saveTotalDocumentCount(documentCount)),
    savePreviousRequest: searchRequestObj =>
      dispatch(searchActions.savePreviousRequest(searchRequestObj)),
    saveNetworkError: enable =>
      dispatch(searchActions.saveNetworkError(enable)),
    saveSearchCount: searchCount =>
      dispatch(searchActions.saveSearchCount(searchCount)),
    saveFacetData: facets => dispatch(facetActions.saveFacetData(facets)),
    saveDefaultFacetData: defaultFacets =>
      dispatch(facetActions.saveDefaultFacetData(defaultFacets)),
    saveLoading: loading => dispatch(componentActions.saveLoading(loading)),
    saveCancellationToken: enable =>
      dispatch(componentActions.saveCancellationToken(enable)),

    savePrimaryColour: primaryHexColour =>
      dispatch(themeActions.savePrimaryColour(primaryHexColour)),
    saveSecondaryColour: secondaryHexColour =>
      dispatch(themeActions.saveSecondaryColour(secondaryHexColour)),
    saveNavBarColour: navBarHexColour =>
      dispatch(themeActions.saveNavBarColour(navBarHexColour)),
    saveLogoURL: logoURL => dispatch(themeActions.saveLogoURL(logoURL)),
    saveMissingImageURL: logoURL =>
      dispatch(themeActions.saveMissingImageURL(logoURL)),
    saveConfigData: configData =>
      dispatch(configActions.saveConfigData(configData)),
    saveEnableAutoComplete: enableAutoComplete =>
      dispatch(configActions.saveEnableAutoComplete(enableAutoComplete)),
    saveHideIconVehicleCounts: hideIconVehicleCounts =>
      dispatch(configActions.saveHideIconVehicleCounts(hideIconVehicleCounts)),
    savePlaceholderArray: placeholderText =>
      dispatch(configActions.savePlaceholderArray(placeholderText)),
  };
};

VehicleSearchApp.propTypes = {
  reduxSearchTerm: PropTypes.string,
  reduxSearchCount: PropTypes.number,
  reduxPageNumber: PropTypes.number,
  reduxTotalDocumentCount: PropTypes.number,
  reduxVehicleData: PropTypes.array,
  reduxOrderBy: PropTypes.string,
  reduxNetworkError: PropTypes.bool,
  reduxPreviousRequest: PropTypes.object,

  orderBy: PropTypes.string,
  pageNumber: PropTypes.number,
  width: PropTypes.number,
  orderByData: PropTypes.string,
  allFacetFilters: PropTypes.array,
  pageSize: PropTypes.number,
  searchCount: PropTypes.number,

  reduxFacetSelectors: PropTypes.array,
  reduxFacetSelectedKeys: PropTypes.array,
  defaultFacetData: PropTypes.array,
  reduxDialogOpen: PropTypes.bool,
  reduxCancellationToken: PropTypes.bool,
  reduxConfigData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,
  reduxLoading: PropTypes.bool,
  reduxFacetChipDeleted: PropTypes.number,

  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveTotalDocumentCount: PropTypes.func,
  savePreviousRequest: PropTypes.func,
  saveNetworkError: PropTypes.func,
  saveFacetData: PropTypes.func,
  saveSearchCount: PropTypes.func,
  saveDefaultFacetData: PropTypes.func,
  saveLoading: PropTypes.func,
  saveCancellationToken: PropTypes.func,

  savePrimaryColour: PropTypes.func,
  saveSecondaryColour: PropTypes.func,
  saveNavBarColour: PropTypes.func,
  saveLogoURL: PropTypes.func,
  saveMissingImageURL: PropTypes.func,
  saveConfigData: PropTypes.func,
  saveEnableAutoComplete: PropTypes.func,
  saveHideIconVehicleCounts: PropTypes.func,
  savePlaceholderArray: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(VehicleSearchApp);
