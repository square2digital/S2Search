import { useEffect } from 'react';
import { connect } from 'react-redux';

// Import types from the store
import type { RootState } from '../../store/index';

// New RTK action imports
import {
  setFacetSelectedKeys,
  setFacetSelectors,
  setResetFacets,
  setSelectedFacet,
} from '../../store/slices/facetSlice';
import {
  setOrderBy,
  setPageNumber,
  setSearchTerm,
  setVehicleData,
} from '../../store/slices/searchSlice';
import { setDialogOpen } from '../../store/slices/uiSlice';

import { DefaultPageNumber } from '../Constants';

// Define a minimal interface with exact store types
interface ResetFacetsProps {
  // Redux state props (using exact types from store)
  searchCount: number;
  reduxResultsCount: number;
  vehicleData: any[]; // From store this is VehicleData[] but we'll use any[] to avoid import issues
  reduxFacetSelectors: any[]; // From store this is SelectedFacetData[]
  defaultFacetData: any[]; // From store this is FacetData[]
  facetData: any[]; // From store this is FacetData[]
  reduxSelectedFacet: string;
  reduxLoading: boolean;
  reduxResetFacets: boolean;

  // Action props - these match the Redux action types
  saveVehicleData: (data: any[]) => void;
  savePageNumber: (page: number) => void;
  saveFacetSelectors: (selectors: any[]) => void;
  saveSearchTerm: (term: string) => void;
  saveOrderby: (order: string) => void;
  saveDialogOpen: (open: boolean) => void;
  saveSelectedFacet: (selectedFacet: string) => void;
  saveFacetSelectedKeys: (keys: string[]) => void;
  saveResetFacets: (reset: boolean) => void;
}

const ResetFacets: React.FC<ResetFacetsProps> = props => {
  const resetFacetsData = (currentProps: ResetFacetsProps): void => {
    if (currentProps.reduxResetFacets === true) {
      currentProps.saveVehicleData([]);
      currentProps.savePageNumber(DefaultPageNumber);
      currentProps.saveFacetSelectors([]);
      currentProps.saveSearchTerm('');
      currentProps.saveDialogOpen(false);
      currentProps.saveOrderby('');

      // set the facet button to either make or model depending on whats currently selected.
      if (currentProps.reduxSelectedFacet) {
        currentProps.saveSelectedFacet(currentProps.reduxSelectedFacet);
        currentProps.saveFacetSelectedKeys([currentProps.reduxSelectedFacet]);
      }

      currentProps.saveResetFacets(false);
    }
  };

  useEffect(() => {
    resetFacetsData(props);
  }, [props]);

  return null;
};

const mapStateToProps = (reduxState: RootState) => {
  return {
    searchCount: reduxState.search.searchCount,
    reduxResultsCount: reduxState.search.searchCount,
    vehicleData: reduxState.search.vehicleData,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    defaultFacetData: reduxState.facet.defaultFacetData,
    facetData: reduxState.facet.facetData,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxLoading: reduxState.ui.isLoading,

    reduxResetFacets: reduxState.facet.resetFacets,
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveVehicleData: (vehicleData: any[]) =>
      dispatch(setVehicleData(vehicleData)),
    savePageNumber: (pageNumber: number) => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: (resetFacetArray: any[]) =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveSearchTerm: (searchTerm: string) => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: (orderBy: string) => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: (dialogOpen: boolean) =>
      dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: (facet: string) => dispatch(setSelectedFacet(facet)),
    saveFacetSelectedKeys: (facetSelectedKeys: string[]) =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveResetFacets: (resetFacets: boolean) =>
      dispatch(setResetFacets(resetFacets)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(ResetFacets);
