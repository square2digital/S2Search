import Alert from '@mui/material/Alert';
import Box from '@mui/material/Box';
import React, { useCallback, useEffect, useState } from 'react';
import { connect, ConnectedProps } from 'react-redux';

import LoadMoreResultsButton from './LoadMoreResultsButton';
import FacetFullScreenDialog from './material-ui/filtersDialog/FacetFullScreenDialog';
import FacetChips from './material-ui/searchPage/FacetChips';
import FloatingTopButton from './material-ui/searchPage/FloatingTopButton';
import AdaptiveNavBar from './material-ui/searchPage/navBars/AdaptiveNavBar';
import NetworkErrorDialog from './material-ui/searchPage/NetworkErrorDialog';
import VehicleCardList from './material-ui/vehicleCards/VehicleCardList';

import { getSelectedFacets } from '@/common/functions/FacetFunctions';

import {
  DefaultPageNumber,
  DefaultPageSize,
  DefaultTheme,
} from '../common/Constants';
import {
  getConfigValueByKey,
  getPlaceholdersArray,
} from '../common/functions/ConfigFunctions';
import {
  getQueryStringSearchTerm,
  insertQueryStringParam,
} from '../common/functions/QueryStringFunctions';
import { ConvertStringToBoolean } from '../common/functions/SharedFunctions';
import { LogDetails } from '../helpers/LogDetails';

// New RTK action imports
import {
  setConfigData,
  setEnableAutoComplete,
  setHideIconVehicleCounts,
  setPlaceholderArray,
} from '../store/slices/configSlice';
import {
  setDefaultFacetData,
  setFacetData,
  setFacetSelectedKeys,
  setFacetSelectors,
} from '../store/slices/facetSlice';
import {
  setNetworkError,
  setPageNumber,
  setPreviousRequest,
  setSearchCount,
  setSearchTerm,
  setTotalDocumentCount,
  setVehicleData,
} from '../store/slices/searchSlice';
import {
  setLogoURL,
  setMissingImageURL,
  setNavBarColour,
  setPrimaryColour,
  setSecondaryColour,
} from '../store/slices/themeSlice';
import { setCancellationToken, setLoading } from '../store/slices/uiSlice';

// Type imports
import type { RootState } from '../store';
import type { SearchRequest } from '../types/searchTypes';

// Define the component's Redux state mapping
const mapStateToProps = (reduxState: RootState) => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxSearchCount: reduxState.search.searchCount,
    reduxPageNumber: reduxState.search.pageNumber,
    reduxTotalDocumentCount: reduxState.search.totalDocumentCount,
    reduxVehicleData: reduxState.search.vehicleData,
    reduxOrderBy: reduxState.search.orderBy,
    reduxNetworkError: reduxState.search.networkError,
    reduxPreviousRequest: reduxState.search.previousRequest,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
    reduxDefaultFacetData: reduxState.facet.defaultFacetData,
    reduxFacetData: reduxState.facet.facetData,

    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxLoading: reduxState.ui.isLoading,
    reduxCancellationToken: reduxState.ui.cancellationToken,

    reduxConfigData: reduxState.config.configData,

    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxFacetChipDeleted: reduxState.facet.facetChipDeleted,
  };
};

// Define the component's Redux action dispatchers
const mapDispatchToProps = {
  saveVehicleData: setVehicleData,
  savePageNumber: setPageNumber,
  saveTotalDocumentCount: setTotalDocumentCount,
  savePreviousRequest: setPreviousRequest,
  saveNetworkError: setNetworkError,
  saveSearchCount: setSearchCount,
  saveFacetData: setFacetData,
  saveDefaultFacetData: setDefaultFacetData,
  saveLoading: setLoading,
  saveCancellationToken: setCancellationToken,

  savePrimaryColour: setPrimaryColour,
  saveSecondaryColour: setSecondaryColour,
  saveNavBarColour: setNavBarColour,
  saveLogoURL: setLogoURL,
  saveMissingImageURL: setMissingImageURL,
  saveConfigData: setConfigData,
  saveEnableAutoComplete: setEnableAutoComplete,
  saveHideIconVehicleCounts: setHideIconVehicleCounts,
  savePlaceholderArray: setPlaceholderArray,

  // RTK facet and search actions (replacing legacy actions)
  saveFacetSelectors: setFacetSelectors,
  saveFacetSelectedKeys: setFacetSelectedKeys,
  saveSearchTerm: setSearchTerm,
};

// Infer the component props from Redux connect
const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

// Additional props that the component might receive
interface OwnProps {
  orderBy?: string;
  pageNumber?: number;
  width?: number;
  orderByData?: string;
  allFacetFilters?: any[];
  pageSize?: number;
  searchCount?: number;
}

// Combined props type
type VehicleSearchAppProps = PropsFromRedux & OwnProps;

const VehicleSearchApp: React.FC<VehicleSearchAppProps> = props => {
  const [themeConfigured, setThemeConfigured] = useState<boolean>(false);
  const [searchConfigConfigured, setSearchConfigConfigured] =
    useState<boolean>(false);
  const [facetsLoadedFromUrl, setFacetsLoadedFromUrl] =
    useState<boolean>(false);

  // Destructure frequently used props to avoid dependency issues
  const { saveSearchTerm } = props;

  // *********************************************************************************************************************
  // ** This function will make a call to our search API and update the Redux store with the returned search results
  // *********************************************************************************************************************
  const triggerSearch = useCallback(
    (searchRequest: SearchRequest) => {
      props.saveLoading(true);
      props.saveNetworkError(false);

      LogDetails({ searchRequest });

      fetch('/api/search', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(searchRequest),
      })
        .then(response => response.json())
        .then(function (responseObject: any) {
          props.saveLoading(false);

          // Check if the response is an error object
          if (responseObject && responseObject.error) {
            console.error('Search API returned error:', responseObject.error);
            props.saveNetworkError(true);
            return;
          }

          if (
            responseObject &&
            responseObject.results &&
            Array.isArray(responseObject.results)
          ) {
            if (searchRequest.pageNumber === 0) {
              // First page - replace existing results
              props.saveVehicleData(responseObject.results);
            } else {
              // Additional page - append to existing results
              props.saveVehicleData([
                ...props.reduxVehicleData,
                ...responseObject.results,
              ]);
            }

            props.saveSearchCount(responseObject.results.length);

            // Handle facets from search response
            if (responseObject.facets && Array.isArray(responseObject.facets)) {
              props.saveDefaultFacetData(responseObject.facets);
            }

            props.savePreviousRequest(searchRequest);
          } else {
            // No results or invalid response structure
            if (searchRequest.pageNumber === 0) {
              props.saveVehicleData([]);
            }
            props.saveSearchCount(0);
          }
        })
        .catch(error => {
          console.error('Search API call failed:', error);
          props.saveLoading(false);
          props.saveNetworkError(true);
        });
    },
    [props]
  );

  // Setup theme configuration from API
  const getThemeFromAPI = useCallback((): void => {
    if (themeConfigured) return;

    const applyTheme = (themeData: any) => {
      // Use API values when available, only fallback to defaults when API value is missing
      props.savePrimaryColour(
        themeData?.primaryHexColour ?? DefaultTheme.primaryHexColour
      );
      props.saveSecondaryColour(
        themeData?.secondaryHexColour ?? DefaultTheme.secondaryHexColour
      );
      props.saveNavBarColour(
        themeData?.navBarHexColour ?? DefaultTheme.navBarHexColour
      );
      props.saveLogoURL(themeData?.logoURL ?? DefaultTheme.logoURL);
      props.saveMissingImageURL(
        themeData?.missingImageURL ?? DefaultTheme.missingImageURL
      );
      setThemeConfigured(true);
    };

    fetch('/api/theme')
      .then(response => response.json())
      .then(theme => {
        // Only use DefaultTheme as fallback if API returns null/undefined
        applyTheme(theme ?? DefaultTheme);
      })
      .catch(error => {
        console.warn('Failed to fetch theme, using defaults:', error);
        applyTheme(DefaultTheme);
      });
  }, [themeConfigured, props]);

  const getDocumentCountAPI = useCallback((): void => {
    fetch('/api/documentCount')
      .then(response => response.json())
      .then(function (documentCount: any) {
        // Check if the response is an error object
        if (documentCount && documentCount.error) {
          console.warn(
            'Document Count API returned error:',
            documentCount.error
          );
          return;
        }

        // Check if documentCount is a valid number
        if (documentCount && typeof documentCount === 'number') {
          props.saveTotalDocumentCount(documentCount);
        }
      })
      .catch(error => {
        console.error('Failed to fetch document count:', error);
      });
  }, [props]);

  // Combined initialization effect - runs once on mount
  useEffect(() => {
    const initializeApp = async () => {
      try {
        // Initialize theme, config, and document count in parallel
        await Promise.allSettled([
          getThemeFromAPI(),
          getDocumentCountAPI(),
          // Only fetch config if not already loaded
          props.reduxConfigData.length === 0
            ? (async () => {
                const response = await fetch('/api/configuration');
                const configData = await response.json();

                if (configData?.error) {
                  console.warn(
                    'Configuration API returned error:',
                    configData.error
                  );
                  return;
                }

                if (configData && Array.isArray(configData)) {
                  const config = configData.map(item => ({
                    key: item.key,
                    value: item.value,
                  }));
                  props.saveConfigData(config);

                  // Process configuration values
                  const placeholders = getPlaceholdersArray(config).map(
                    item => item.value || ''
                  );
                  props.savePlaceholderArray(placeholders);

                  const enableAutoComplete = ConvertStringToBoolean(
                    getConfigValueByKey(config, 'EnableAutoComplete')?.value ||
                      'false'
                  );
                  props.saveEnableAutoComplete(enableAutoComplete);

                  const hideIconVehicleCounts = ConvertStringToBoolean(
                    getConfigValueByKey(config, 'HideIconVehicleCounts')
                      ?.value || 'false'
                  );
                  props.saveHideIconVehicleCounts(hideIconVehicleCounts);

                  setSearchConfigConfigured(true);
                }
              })()
            : Promise.resolve(),
        ]);
      } catch (error) {
        console.error('Failed to initialize app:', error);
      }
    };

    if (
      !searchConfigConfigured ||
      !themeConfigured ||
      props.reduxConfigData.length === 0
    ) {
      initializeApp();
    }
  }, [
    searchConfigConfigured,
    themeConfigured,
    props.reduxConfigData.length,
    getThemeFromAPI,
    getDocumentCountAPI,
    props,
  ]);

  // Load initial state from URL parameters - runs once on mount
  useEffect(() => {
    if (typeof window !== 'undefined' && !facetsLoadedFromUrl) {
      const searchTerm = decodeURIComponent(getQueryStringSearchTerm());
      // const orderby = decodeURIComponent(getQueryStringOrderBy()); // TODO: Add orderBy Redux action

      // Load search term from URL if present
      if (searchTerm?.length > 0) {
        saveSearchTerm(searchTerm);
      }

      setFacetsLoadedFromUrl(true);
    }
  }, [facetsLoadedFromUrl, saveSearchTerm]);

  // Combined URL synchronization effect
  useEffect(() => {
    if (typeof window !== 'undefined') {
      // Update search term in URL
      insertQueryStringParam(
        'searchterm',
        props.reduxSearchTerm.length > 0
          ? encodeURIComponent(props.reduxSearchTerm)
          : ''
      );

      // Update order by in URL
      insertQueryStringParam(
        'orderby',
        props.reduxOrderBy.length > 0
          ? encodeURIComponent(props.reduxOrderBy)
          : ''
      );
    }
  }, [props.reduxSearchTerm, props.reduxOrderBy]);

  // *********************************************************************************************************************
  // ** UseEffect for initial search and when dependencies change
  // *********************************************************************************************************************
  useEffect(() => {
    //if (!searchConfigConfigured || !facetsLoadedFromUrl) return;

    const facetFilters = getSelectedFacets(props.reduxFacetSelectors);

    const searchRequest: SearchRequest = {
      searchTerm: props.reduxSearchTerm,
      filters: facetFilters.join(' AND '), // Convert array to string for C# API
      orderBy: props.reduxOrderBy,
      pageNumber: DefaultPageNumber,
      pageSize: DefaultPageSize,
      numberOfExistingResults: 0, // Always start fresh for new searches
      customerEndpoint: window.location.host,
    };

    triggerSearch(searchRequest);
  }, [
    props.reduxSearchTerm,
    props.reduxFacetSelectors,
    props.reduxOrderBy,
    searchConfigConfigured,
    facetsLoadedFromUrl,
    triggerSearch,
  ]);

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AdaptiveNavBar />
      <FacetChips />
      <VehicleCardList />
      <LoadMoreResultsButton />
      <FacetFullScreenDialog />
      <NetworkErrorDialog />
      <FloatingTopButton />

      {/* Development/Debug Information */}
      {process.env.NODE_ENV === 'development' && (
        <Box sx={{ p: 2, mt: 2, bgcolor: 'grey.100' }}>
          <Alert severity="info">
            <strong>Debug Info:</strong>
            <br />
            Search Term: {props.reduxSearchTerm}
            <br />
            Results Count: {props.reduxVehicleData.length}
            <br />
            Page Number: {props.reduxPageNumber}
            <br />
            Order By: {props.reduxOrderBy}
            <br />
            Selected Facets:{' '}
            {props.reduxFacetSelectors.filter((f: any) => f.checked).length}
          </Alert>
        </Box>
      )}
    </Box>
  );
};

export default connector(VehicleSearchApp);
