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

import {
  insertQueryStringParam,
  removeFullQueryString,
} from '@/common/functions/QueryStringFunctions';

import { getSelectedFacets } from '@/common/functions/FacetFunctions';

import { DefaultPageSize, DefaultTheme } from '../common/Constants';
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
import { useRouter } from 'next/router';
import type { RootState } from '../store';
import { SearchRequest } from '../types/searchTypes'; // Changed from 'import type' to regular import

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
  const [facetsLoadedFromUrl, setFacetsLoadedFromUrl] =
    useState<boolean>(false);

  const router = useRouter();

  // Destructure specific props to avoid whole props object dependency
  const {
    saveLoading,
    saveNetworkError,
    saveVehicleData,
    saveSearchCount,
    saveDefaultFacetData,
    savePreviousRequest,
    reduxVehicleData,
    savePrimaryColour,
    saveSecondaryColour,
    saveNavBarColour,
    saveLogoURL,
    saveMissingImageURL,
    saveTotalDocumentCount,
    saveSearchTerm,
    reduxSearchTerm,
    reduxOrderBy,
    reduxPageNumber,
    reduxFacetSelectors,
  } = props;

  const updateQueryStringURL = useCallback(() => {
    if (reduxFacetSelectors.length > 0) {
      insertQueryStringParam(
        'facetselectors',
        JSON.stringify(reduxFacetSelectors)
      );

      insertQueryStringParam('orderby', reduxOrderBy);
    }

    if (reduxSearchTerm.length > 0) {
      insertQueryStringParam('searchterm', reduxSearchTerm);
    }

    if (reduxFacetSelectors.length === 0 && reduxSearchTerm.length === 0) {
      removeFullQueryString();
    }
  }, [reduxFacetSelectors, reduxOrderBy, reduxSearchTerm]);

  const triggerSearch = useCallback(
    (searchRequest: SearchRequest) => {
      saveLoading(true);
      saveNetworkError(false);

      LogDetails({ searchRequest });

      fetch('/api/search', {
        body: JSON.stringify(searchRequest),
      })
        .then(response => response.json())
        .then(function (responseObject: any) {
          saveLoading(false);

          // Check if the response is an error object
          if (responseObject && responseObject.error) {
            console.error('Search API returned error:', responseObject.error);
            saveNetworkError(true);
            return;
          }

          if (
            responseObject &&
            responseObject.results &&
            Array.isArray(responseObject.results)
          ) {
            if (searchRequest.pageNumber === 0) {
              // First page - replace existing results
              saveVehicleData(responseObject.results);
            } else {
              // Additional page - append to existing results
              saveVehicleData([...reduxVehicleData, ...responseObject.results]);
            }

            saveSearchCount(responseObject.results.length);

            // Handle facets from search response
            if (responseObject.facets && Array.isArray(responseObject.facets)) {
              saveDefaultFacetData(responseObject.facets);
            }

            savePreviousRequest(searchRequest);
          } else {
            // No results or invalid response structure
            if (searchRequest.pageNumber === 0) {
              saveVehicleData([]);
            }
            saveSearchCount(0);
          }
        })
        .catch(error => {
          console.error('Search API call failed:', error);
          saveLoading(false);
          saveNetworkError(true);
        });
    },
    [
      saveLoading,
      saveNetworkError,
      saveVehicleData,
      saveSearchCount,
      saveDefaultFacetData,
      savePreviousRequest,
      reduxVehicleData,
    ]
  );

  // Setup theme configuration from API
  const getThemeFromAPI = useCallback((): void => {
    if (themeConfigured) return;

    const applyTheme = (themeData: any) => {
      // Use API values when available, only fallback to defaults when API value is missing
      savePrimaryColour(
        themeData?.primaryHexColour ?? DefaultTheme.primaryHexColour
      );
      saveSecondaryColour(
        themeData?.secondaryHexColour ?? DefaultTheme.secondaryHexColour
      );
      saveNavBarColour(
        themeData?.navBarHexColour ?? DefaultTheme.navBarHexColour
      );
      saveLogoURL(themeData?.logoURL ?? DefaultTheme.logoURL);
      saveMissingImageURL(
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
  }, [
    themeConfigured,
    savePrimaryColour,
    saveSecondaryColour,
    saveNavBarColour,
    saveLogoURL,
    saveMissingImageURL,
  ]);

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
          saveTotalDocumentCount(documentCount);
        }
      })
      .catch(error => {
        console.error('Failed to fetch document count:', error);
      });
  }, [saveTotalDocumentCount]);

  // ******************************************************************************************
  // ** this useEffect hook will setup configurations and theme settings - it is run once only
  // ******************************************************************************************
  useEffect(() => {
    getThemeFromAPI();
    getDocumentCountAPI();
  }, []);

  // *********************************************************************************************************************
  // ** Simplified search effect - triggers when search parameters change
  // *********************************************************************************************************************
  useEffect(() => {
    // Only search if we have loaded initial state
    if (!facetsLoadedFromUrl) return;

    updateQueryStringURL();

    const searchRequest = new SearchRequest(
      reduxSearchTerm,
      getSelectedFacets(reduxFacetSelectors), // This returns string[], constructor will handle conversion
      reduxOrderBy,
      reduxPageNumber,
      DefaultPageSize,
      reduxVehicleData.length,
      window.location.host
    );

    triggerSearch(searchRequest);
  }, [
    reduxSearchTerm,
    reduxOrderBy,
    reduxPageNumber,
    reduxFacetSelectors,
    facetsLoadedFromUrl,
    triggerSearch,
    reduxVehicleData.length,
    updateQueryStringURL,
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
            Search Term: {reduxSearchTerm}
            <br />
            Results Count: {reduxVehicleData.length}
            <br />
            Page Number: {reduxPageNumber}
            <br />
            Order By: {reduxOrderBy}
            <br />
            Selected Facets:{' '}
            {reduxFacetSelectors.filter((f: any) => f.checked).length}
          </Alert>
        </Box>
      )}
    </Box>
  );
};

export default connector(VehicleSearchApp);
