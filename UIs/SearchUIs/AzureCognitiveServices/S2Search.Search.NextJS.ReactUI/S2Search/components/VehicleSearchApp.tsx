import Alert from '@mui/material/Alert';
import Box from '@mui/material/Box';
import React from 'react';
import { connect, ConnectedProps } from 'react-redux';

import LoadMoreResultsButton from './LoadMoreResultsButton';
import FacetFullScreenDialog from './material-ui/filtersDialog/FacetFullScreenDialog';
import FacetChips from './material-ui/searchPage/FacetChips';
import FloatingTopButton from './material-ui/searchPage/FloatingTopButton';
import AdaptiveNavBar from './material-ui/searchPage/navBars/AdaptiveNavBar';
import NetworkErrorDialog from './material-ui/searchPage/NetworkErrorDialog';
import VehicleCardList from './material-ui/vehicleCards/VehicleCardList';

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
  // Destructure only the props we actually use in the render
  const {
    reduxSearchTerm,
    reduxPageNumber,
    reduxOrderBy,
    reduxVehicleData,
    reduxFacetSelectors,
  } = props;

  // ******************************************************************************************
  // ** this useEffect hook will setup configurations and theme settings - it is run once only
  // ******************************************************************************************
  /*   useEffect(() => {
    getThemeFromAPI();
    getDocumentCountAPI();
  }, []); */

  /*   // *********************************************************************************************************************
  // ** Simplified search effect - triggers when search parameters change
  // *********************************************************************************************************************
  useEffect(() => {
    // Only search if we have loaded initial state
    if (!facetsLoadedFromUrl) return;

    updateQueryStringURL();

    const searchRequest = new SearchRequest(
      props.reduxSearchTerm,
      getSelectedFacets(props.reduxFacetSelectors), // This returns string[], constructor will handle conversion
      props.reduxOrderBy,
      props.reduxPageNumber,
      DefaultPageSize,
      props.reduxVehicleData.length,
      window.location.host
    );

    triggerSearch(searchRequest);
  }, [
    props.reduxSearchTerm,
    props.reduxOrderBy,
    props.reduxPageNumber,
    props.reduxFacetSelectors,
    facetsLoadedFromUrl,
    triggerSearch,
    props.reduxVehicleData.length,
    updateQueryStringURL,
  ]); */

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
