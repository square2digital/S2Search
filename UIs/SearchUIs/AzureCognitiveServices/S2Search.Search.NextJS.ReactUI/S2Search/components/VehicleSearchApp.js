import React, { useState, useEffect, useCallback, useRef } from 'react';
import PropTypes from 'prop-types';
import FacetFullScreenDialog from './material-ui/filtersDialog/FacetFullScreenDialog';
import VehicleCardList from './material-ui/vehicleCards/VehicleCardList.tsx';
import LoadMoreResultsButton from './LoadMoreResultsButton';
import FacetChips from './material-ui/searchPage/FacetChips';
import NetworkErrorDialog from './material-ui/searchPage/NetworkErrorDialog';
import { connect } from 'react-redux';
import { createSearchRequest } from '../types/SearchRequest';

// New RTK action imports
import {
  setVehicleData,
  setPageNumber,
  setTotalDocumentCount,
  setPreviousRequest,
  setNetworkError,
  setSearchCount,
  setSearchTerm,
} from '../store/slices/searchSlice';
import {
  setFacetData,
  setDefaultFacetData,
  setFacetSelectors,
  setFacetSelectedKeys,
} from '../store/slices/facetSlice';
import { setLoading, setCancellationToken } from '../store/slices/uiSlice';
import {
  setPrimaryColour,
  setSecondaryColour,
  setNavBarColour,
  setLogoURL,
  setMissingImageURL,
} from '../store/slices/themeSlice';
import {
  setConfigData,
  setEnableAutoComplete,
  setHideIconVehicleCounts,
  setPlaceholderArray,
} from '../store/slices/configSlice';

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
import Box from '@mui/material/Box';
import HelperFunctions from '../helpers/HelperFunctions';
import { grey, red } from '@mui/material/colors';
import Alert from '@mui/material/Alert';
import FloatingTopButton from './material-ui/searchPage/FloatingTopButton';

import {
  DefaultPageSize,
  DefaultPageNumber,
  DefaultTheme,
  MillisecondsDifference,
} from '../common/Constants';

import AdaptiveNavBar from './material-ui/searchPage/navBars/AdaptiveNavBar.tsx';
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
  const [searchCount, setSearchCount] = useState(0);
  const [timestamp, setTimestamp] = useState(undefined);
  const [themeConfigured, setThemeConfigured] = useState(false);
  const [searchConfigConfigured, setSearchConfigConfigured] = useState(false);
  const [autoCompleteSearchBar, setAutoCompleteSearchBar] = useState(undefined);

  // State for managing search debouncing and preventing infinite loops
  const searchDebounceTimer = useRef(null);
  const [isSearching, setIsSearching] = useState(false);
  const [lastExecutedSearch, setLastExecutedSearch] = useState(null);
  const [facetsLoadedFromUrl, setFacetsLoadedFromUrl] = useState(false);

  const router = useRouter();

  // ******************************************************************************************
  // ** this useEffect hook will setup configurations and theme settings - it is run once only
  // ******************************************************************************************
  useEffect(() => {
    getThemeFromAPI();
    getDocumentCountAPI();
    const config = [];
    if (props.reduxConfigData.length === 0) {
      fetch('/api/configuration')
        .then(response => response.json())
        .then(function (configData) {
          // Check if the response is an error object
          if (configData && configData.error) {
            console.warn('Configuration API returned error:', configData.error);
            return;
          }

          if (
            !searchConfigConfigured &&
            configData &&
            Array.isArray(configData)
          ) {
            configData.map(function (item) {
              config.push({ key: item.key, value: item.value });
            });

            props.saveConfigData(config);
            props.savePlaceholderArray(getPlaceholdersArray(config));

            const enableAutoComplete = ConvertStringToBoolean(
              getConfigValueByKey(config, 'EnableAutoComplete').value
            );
            props.saveEnableAutoComplete(enableAutoComplete);
            setAutoCompleteSearchBar(enableAutoComplete);

            const hideBadges = ConvertStringToBoolean(
              getConfigValueByKey(config, 'HideBadges').value
            );
            props.saveHideBadges(hideBadges);

            const HideIconVehicleCounts = ConvertStringToBoolean(
              getConfigValueByKey(config, 'HideIconVehicleCounts').value
            );
            props.saveHideIconVehicleCounts(HideIconVehicleCounts);

            setSearchConfigConfigured(true);
          }
        })
        .catch(error => {
          console.error('Failed to fetch configuration:', error);
        });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // *********************************************************************************************************************
  // ** Separate useEffect for handling URL parameters (facets, search terms) - runs when route changes
  // *********************************************************************************************************************
  useEffect(() => {
    if (!router || !router.query) return;

    // Only process if we have actual query parameters
    const hasQueryParams = Object.keys(router.query).length > 0;
    if (!hasQueryParams) return;

    LogString(`URL EFFECT: Processing URL parameters`);

    // Handle search term from URL (only if different from current)
    if (
      router.query.searchterm &&
      router.query.searchterm !== props.reduxSearchTerm
    ) {
      LogString(`Loading search term from URL: ${router.query.searchterm}`);
      props.saveSearchTerm(router.query.searchterm);
    }

    // Handle facet selectors from URL (only once per URL change)
    if (router.query.facetselectors && !facetsLoadedFromUrl) {
      try {
        const facetSelectors = JSON.parse(
          decodeURIComponent(router.query.facetselectors)
        );
        LogString(`Loading ${facetSelectors.length} facet selectors from URL`);

        props.saveFacetSelectors(facetSelectors);

        // Extract facet keys for the selected keys array
        const selectedKeys = facetSelectors
          .filter(facet => facet.checked)
          .map(facet => facet.facetKey);
        props.saveFacetSelectedKeys(selectedKeys);

        // Mark facets as loaded to prevent re-loading
        setFacetsLoadedFromUrl(true);
      } catch (error) {
        LogString(`Error parsing facet selectors from URL: ${error.message}`);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [router?.query?.facetselectors, router?.query?.searchterm]);

  // *********************************************************************************************************************
  // ** Main search API function - moved here to resolve dependency order
  // *********************************************************************************************************************
  const triggerSearch = useCallback(
    request => {
      const requestPlain = request; // request is already a plain object

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
        !IsPreviousRequestDataTheSame(
          requestPlain,
          props.reduxPreviousRequest
        ) ||
        props.reduxVehicleData.length == 0
      ) {
        props.saveLoading(true);
        props.savePreviousRequest(requestPlain);

        fetch('/api/search', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(request),
        })
          .then(response => response.json())
          .then(function (searchResponse) {
            // Check if the response is an error object
            if (searchResponse && searchResponse.error) {
              console.warn('Search API returned error:', searchResponse.error);
              props.saveNetworkError(true);
              return;
            }

            if (searchResponse) {
              props.saveNetworkError(false);
              if (searchResponse.results && searchResponse.results.length > 0) {
                let vehicleSearchData = [];

                if (
                  props.reduxPageNumber !=
                  (props.reduxPreviousRequest?.pageNumber || 0)
                ) {
                  vehicleSearchData = HelperFunctions.RemoveDuplicates([
                    ...props.reduxVehicleData,
                    ...searchResponse.results,
                  ]);
                } else {
                  vehicleSearchData = searchResponse.results;
                }

                props.saveVehicleData(vehicleSearchData);
                props.saveSearchCount(searchResponse.totalResults);
              } else {
                // no results found
                props.savePageNumber(DefaultPageNumber);
                props.saveVehicleData([]);
              }

              if (searchResponse.facets) {
                props.saveFacetData(searchResponse.facets);
              }
              if (props.defaultFacetData.length === 0) {
                fetch('/api/facet', {
                  method: 'POST',
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  body: JSON.stringify(request),
                })
                  .then(response => response.json())
                  .then(function (facetResponse) {
                    // Check if the response is an error object
                    if (facetResponse && facetResponse.error) {
                      console.warn(
                        'Facet API returned error:',
                        facetResponse.error
                      );
                      return;
                    }

                    if (facetResponse && facetResponse.facets) {
                      props.saveDefaultFacetData(facetResponse.facets);
                    }
                  })
                  .catch(error => {
                    console.error('Failed to fetch facets:', error);
                  });
              }

              props.saveLoading(false);
            }
          })
          .catch(error => {
            console.error('Search API error:', error);
            props.saveLoading(false);
            props.saveNetworkError(true);
          });
      }
    },
    [props]
  );

  // *********************************************************************************************************************
  // ** Main search useEffect - triggers searches based on user input changes with debouncing
  // *********************************************************************************************************************
  useEffect(() => {
    LogString(
      `Search useEffect triggered. Search term: "${props.reduxSearchTerm}", Selected keys: ${props.reduxFacetSelectedKeys.length}, Page: ${props.reduxPageNumber}`
    );

    // Only trigger searches for meaningful changes that indicate user intent
    const hasSearchCriteria =
      props.reduxSearchTerm.length > 0 ||
      props.reduxFacetSelectedKeys.length > 0 ||
      props.reduxPageNumber > 0;

    if (!hasSearchCriteria) {
      LogString('No search criteria present, skipping search');
      return;
    }

    // Create search request
    const currentSearchRequest = createSearchRequest(
      props.reduxSearchTerm,
      getSelectedFacets(props.reduxFacetSelectors),
      props.reduxOrderBy,
      props.reduxPageNumber,
      DefaultPageSize,
      props.reduxVehicleData.length,
      typeof window !== 'undefined' ? window.location.host : 'localhost:2997'
    );

    // Handle cancellation token logic
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
      }
    }

    setTimestamp(new Date().getTime());
    setSearchCount(prev => prev + 1);

    // Use debounced search to prevent rapid successive calls
    debouncedSearch(currentSearchRequest);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    props.reduxSearchTerm,
    props.reduxOrderBy,
    props.reduxFacetChipDeleted,
    props.reduxFacetSelectedKeys,
    props.reduxPageNumber,
  ]);

  // Cleanup debounce timer on unmount
  useEffect(() => {
    return () => {
      if (searchDebounceTimer.current) {
        clearTimeout(searchDebounceTimer.current);
      }
    };
  }, []);

  const getThemeFromAPI = () => {
    if (!themeConfigured) {
      try {
        fetch('/api/theme')
          .then(response => response.json())
          .then(function (theme) {
            if (theme) {
              props.savePrimaryColour(theme.primaryHexColour);
              props.saveSecondaryColour(theme.secondaryHexColour);
              props.saveNavBarColour(theme.navBarHexColour);
              props.saveLogoURL(theme.logoURL);
              props.saveMissingImageURL(theme.missingImageURL);

              setThemeConfigured(true);
            }
          })
          .catch(error => {
            console.error('Failed to fetch theme:', error);
            props.savePrimaryColour(DefaultTheme.primaryHexColour);
            props.saveSecondaryColour(DefaultTheme.secondaryHexColour);
            props.saveNavBarColour(DefaultTheme.navBarHexColour);
            props.saveLogoURL(DefaultTheme.logoURL);
            props.saveMissingImageURL(DefaultTheme.missingImageURL);
            setThemeConfigured(true);
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
    fetch('/api/documentCount')
      .then(response => response.json())
      .then(function (documentCount) {
        // Check if the response is an error object
        if (documentCount && documentCount.error) {
          console.warn(
            'Document Count API returned error:',
            documentCount.error
          );
          return;
        }

        if (documentCount && typeof documentCount === 'number') {
          props.saveTotalDocumentCount(documentCount);
        }
      })
      .catch(error => {
        console.error('Failed to fetch document count:', error);
      });
  };

  // *********************************************************************************************************************
  // ** Actual search execution function with debouncing protection
  // *********************************************************************************************************************
  const executeSearch = useCallback(
    searchRequest => {
      // Prevent execution if already searching or if this exact search was just executed
      const searchSignature = JSON.stringify(searchRequest);
      if (isSearching || searchSignature === lastExecutedSearch) {
        LogString(
          'Skipping search execution - already in progress or duplicate'
        );
        return;
      }

      setIsSearching(true);
      setLastExecutedSearch(searchSignature);

      LogString('Executing search with request: ' + searchSignature);
      triggerSearch(searchRequest);

      // Reset searching flag after a delay
      setTimeout(() => {
        setIsSearching(false);
      }, 100);
    },
    [isSearching, lastExecutedSearch, triggerSearch]
  );

  // *********************************************************************************************************************
  // ** Debounced search function to prevent rapid successive API calls
  // *********************************************************************************************************************
  const debouncedSearch = useCallback(
    searchRequest => {
      // Clear any existing timer
      if (searchDebounceTimer.current) {
        clearTimeout(searchDebounceTimer.current);
      }

      const timer = setTimeout(() => {
        executeSearch(searchRequest);
      }, 300); // 300ms debounce delay

      searchDebounceTimer.current = timer;
    },
    [executeSearch] // Now no dependencies on the timer ref
  );

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
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxSearchCount: reduxState.search.searchCount,
    reduxVehicleData: reduxState.search.vehicleData,
    reduxOrderBy: reduxState.search.orderBy,
    reduxPageNumber: reduxState.search.pageNumber,
    reduxNetworkError: reduxState.search.networkError,
    reduxPreviousRequest: reduxState.search.previousRequest,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
    defaultFacetData: reduxState.facet.defaultFacetData,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxFacetChipDeleted: reduxState.facet.facetChipDeleted,

    reduxLoading: reduxState.ui.isLoading,
    reduxCancellationToken: reduxState.ui.cancellationToken,
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxConfigData: reduxState.config.configData,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveTotalDocumentCount: documentCount =>
      dispatch(setTotalDocumentCount(documentCount)),
    savePreviousRequest: searchRequestObj =>
      dispatch(setPreviousRequest(searchRequestObj)),
    saveNetworkError: enable => dispatch(setNetworkError(enable)),
    saveSearchCount: searchCount => dispatch(setSearchCount(searchCount)),
    saveFacetData: facets => dispatch(setFacetData(facets)),
    saveDefaultFacetData: defaultFacets =>
      dispatch(setDefaultFacetData(defaultFacets)),
    saveLoading: loading => dispatch(setLoading(loading)),
    saveCancellationToken: enable => dispatch(setCancellationToken(enable)),

    savePrimaryColour: primaryHexColour =>
      dispatch(setPrimaryColour(primaryHexColour)),
    saveSecondaryColour: secondaryHexColour =>
      dispatch(setSecondaryColour(secondaryHexColour)),
    saveNavBarColour: navBarHexColour =>
      dispatch(setNavBarColour(navBarHexColour)),
    saveLogoURL: logoURL => dispatch(setLogoURL(logoURL)),
    saveMissingImageURL: logoURL => dispatch(setMissingImageURL(logoURL)),
    saveConfigData: configData => dispatch(setConfigData(configData)),
    saveEnableAutoComplete: enableAutoComplete =>
      dispatch(setEnableAutoComplete(enableAutoComplete)),
    saveHideIconVehicleCounts: hideIconVehicleCounts =>
      dispatch(setHideIconVehicleCounts(hideIconVehicleCounts)),
    savePlaceholderArray: placeholderText =>
      dispatch(setPlaceholderArray(placeholderText)),

    // RTK facet and search actions (replacing legacy actions)
    saveFacetSelectors: facetSelectors =>
      dispatch(setFacetSelectors(facetSelectors)),
    saveFacetSelectedKeys: facetSelectedKeys =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
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
