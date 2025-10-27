import React, { useState, useEffect, useCallback } from 'react';
import { connect, ConnectedProps } from 'react-redux';
import Box from '@mui/material/Box';
import Alert from '@mui/material/Alert';

import FacetFullScreenDialog from './material-ui/filtersDialog/FacetFullScreenDialog';
import VehicleCardList from './material-ui/vehicleCards/VehicleCardList';
import LoadMoreResultsButton from './LoadMoreResultsButton';
import FacetChips from './material-ui/searchPage/FacetChips';
import NetworkErrorDialog from './material-ui/searchPage/NetworkErrorDialog';
import FloatingTopButton from './material-ui/searchPage/FloatingTopButton';
import AdaptiveNavBar from './material-ui/searchPage/navBars/AdaptiveNavBar';

import {
  insertQueryStringParam,
  getQueryStringSearchTerm,
  getQueryStringOrderBy,
} from '../common/functions/QueryStringFunctions';
import { getSelectedFacets } from '../common/functions/FacetFunctions';
import {
  getConfigValueByKey,
  getPlaceholdersArray,
} from '../common/functions/ConfigFunctions';
import { ConvertStringToBoolean } from '../common/functions/SharedFunctions';
import { LogDetails } from '../helpers/LogDetails';
import {
  DefaultPageSize,
  DefaultPageNumber,
  DefaultTheme,
} from '../common/Constants';

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

  // Setup theme configuration from API
  const getThemeFromAPI = useCallback((): void => {
    if (themeConfigured) return;

    const applyTheme = (themeData: any) => {
      props.savePrimaryColour(
        themeData.primaryHexColour || DefaultTheme.primaryHexColour
      );
      props.saveSecondaryColour(
        themeData.secondaryHexColour || DefaultTheme.secondaryHexColour
      );
      props.saveNavBarColour(
        themeData.navBarHexColour || DefaultTheme.navBarHexColour
      );
      props.saveLogoURL(themeData.logoURL || DefaultTheme.logoURL);
      props.saveMissingImageURL(
        themeData.missingImageURL || DefaultTheme.missingImageURL
      );
      setThemeConfigured(true);
    };

    fetch('/api/theme')
      .then(response => response.json())
      .then(theme => applyTheme(theme || DefaultTheme))
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

  useEffect(() => {
    getThemeFromAPI();
    getDocumentCountAPI();
    const config: Array<{ key: string; value: string }> = [];
    if (props.reduxConfigData.length === 0) {
      fetch('/api/configuration')
        .then(response => response.json())
        .then(function (configData: any) {
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
            configData.map(function (item: any) {
              config.push({ key: item.key, value: item.value });
              return item;
            });

            props.saveConfigData(config);

            // getPlaceholdersArray returns ConfigItem[], but savePlaceholderArray expects string[]
            const placeholders = getPlaceholdersArray(config).map(
              item => item.value || ''
            );
            props.savePlaceholderArray(placeholders);

            const enableAutoCompleteConfig = getConfigValueByKey(
              config,
              'EnableAutoComplete'
            );
            const enableAutoComplete = ConvertStringToBoolean(
              enableAutoCompleteConfig?.value || 'false'
            );
            props.saveEnableAutoComplete(enableAutoComplete);

            const hideBadgesConfig = getConfigValueByKey(config, 'HideBadges');
            ConvertStringToBoolean(hideBadgesConfig?.value || 'false');
            // Note: savHideBadges action might need to be added to Redux if it doesn't exist

            const hideIconVehicleCountsConfig = getConfigValueByKey(
              config,
              'HideIconVehicleCounts'
            );
            const HideIconVehicleCounts = ConvertStringToBoolean(
              hideIconVehicleCountsConfig?.value || 'false'
            );
            props.saveHideIconVehicleCounts(HideIconVehicleCounts);

            setSearchConfigConfigured(true);
          }
        })
        .catch(error => {
          console.error('Failed to fetch configuration:', error);
        });
    }
  }, [props, searchConfigConfigured, getThemeFromAPI, getDocumentCountAPI]);

  // *********************************************************************************************************************
  // ** this useEffect hook will trigger a search if the page loads with facets defined in the query string
  // *********************************************************************************************************************
  useEffect(() => {
    if (typeof window !== 'undefined' && !facetsLoadedFromUrl) {
      const searchTerm = decodeURIComponent(getQueryStringSearchTerm());
      const orderby = decodeURIComponent(getQueryStringOrderBy());

      if (searchTerm?.length > 0) {
        // Use RTK action instead of legacy action
        props.saveSearchTerm(searchTerm);
      }

      if (orderby?.length > 0) {
        // Note: Redux store might need orderBy action if not available
        // Removing console.log for production readiness
      }

      setFacetsLoadedFromUrl(true);
    }
  }, [facetsLoadedFromUrl, props]);

  // *********************************************************************************************************************
  // ** this useEffect hook will update the browsers URL query string parameters when the searchterm changes
  // *********************************************************************************************************************
  useEffect(() => {
    if (typeof window !== 'undefined') {
      if (props.reduxSearchTerm.length > 0) {
        insertQueryStringParam(
          'searchterm',
          encodeURIComponent(props.reduxSearchTerm)
        );
      } else {
        insertQueryStringParam('searchterm', '');
      }
    }
  }, [props.reduxSearchTerm]);

  // *********************************************************************************************************************
  // ** this useEffect hook will update the browsers URL query string parameters when the orderby changes
  // *********************************************************************************************************************
  useEffect(() => {
    if (typeof window !== 'undefined') {
      if (props.reduxOrderBy.length > 0) {
        // Only add orderby parameter when there's an actual sort order selected
        insertQueryStringParam(
          'orderby',
          encodeURIComponent(props.reduxOrderBy)
        );
      } else {
        // Remove the parameter when no sort order is selected (default)
        insertQueryStringParam('orderby', '');
      }
    }
  }, [props.reduxOrderBy]);

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

  // *********************************************************************************************************************
  // ** Simple search function that builds the search request and executes it
  // *********************************************************************************************************************
  const search = useCallback(
    (
      pageNumber: number = DefaultPageNumber,
      pageSize: number = DefaultPageSize,
      numberOfExistingResults: number = 0
    ) => {
      const facetFilters = getSelectedFacets(props.reduxFacetSelectors);
      const filters = facetFilters.join(' AND ');

      const searchRequest: SearchRequest = {
        searchTerm: props.reduxSearchTerm,
        filters,
        orderBy: props.reduxOrderBy,
        pageNumber,
        pageSize,
        numberOfExistingResults,
        callingHost: 'localhost:3000',
      };

      props.savePageNumber(pageNumber);
      triggerSearch(searchRequest);
    },
    [props, triggerSearch]
  );

  // *********************************************************************************************************************
  // ** UseEffect for initial search and when dependencies change
  // *********************************************************************************************************************
  useEffect(() => {
    if (searchConfigConfigured && facetsLoadedFromUrl) {
      search(0, DefaultPageSize, 0);
    }
  }, [
    props.reduxSearchTerm,
    props.reduxFacetSelectors,
    searchConfigConfigured,
    facetsLoadedFromUrl,
    search,
  ]);

  // Add separate useEffect for cancellation token logic
  useEffect(() => {
    if (props.reduxCancellationToken) {
      props.saveCancellationToken(false);
    }
  }, [props.reduxCancellationToken, props]);

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
