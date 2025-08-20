import { useEffect } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

// New RTK action imports
import {
  setVehicleData,
  setPageNumber,
  setSearchTerm,
  setOrderBy,
} from '../../store/slices/searchSlice';
import {
  setFacetSelectors,
  setFacetSelectedKeys,
  setResetFacets,
  setSelectedFacet,
} from '../../store/slices/facetSlice';
import { setDialogOpen } from '../../store/slices/uiSlice';

import { DefaultPageNumber } from '../Constants';

const ResetFacets = props => {
  useEffect(() => {
    resetFacetsData(props);
  }, [props.reduxResetFacets]);

  const resetFacetsData = props => {
    if (props.reduxResetFacets === true) {
      props.saveVehicleData([]);
      props.savePageNumber(DefaultPageNumber);
      props.saveFacetSelectors([]);
      props.saveSearchTerm('');
      props.saveDialogOpen(false);
      props.saveOrderby('');

      // set the facet button to either make or model depending on whats currently selected.
      if (props.reduxSelectedFacet) {
        props.saveSelectedFacet(props.reduxSelectedFacet);
        props.saveFacetSelectedKeys([props.reduxSelectedFacet]);
      }

      props.saveResetFacets(false);
    }
  };

  return null;
};

const mapStateToProps = reduxState => {
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

const mapDispatchToProps = dispatch => {
  return {
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: resetFacetArray =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: orderBy => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: dialogOpen => dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: facet => dispatch(setSelectedFacet(facet)),
    saveFacetSelectedKeys: facetSelectedKeys =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveResetFacets: resetFacets => dispatch(setResetFacets(resetFacets)),
  };
};

ResetFacets.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxResultsCount: PropTypes.number,
  defaultFacetData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,

  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  saveFacetSelectedKeys: PropTypes.func,
  saveSearchTerm: PropTypes.func,
  saveOrderby: PropTypes.func,
  saveDialogOpen: PropTypes.func,
  saveSelectedFacet: PropTypes.func,
  saveResetFacets: PropTypes.func,
  reduxResetFacets: PropTypes.bool,
};

export default connect(mapStateToProps, mapDispatchToProps)(ResetFacets);
