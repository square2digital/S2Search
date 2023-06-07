import { useEffect } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import searchActions from "../../redux/actions/searchActions";
import facetActions from "../../redux/actions/facetActions";
import componentActions from "../../redux/actions/componentActions";
import { DefaultPageFrom } from "../Constants";

const ResetFacets = (props) => {
  useEffect(() => {
    resetFacetsData(props);
  }, [props.reduxResetFacets]);

  const resetFacetsData = (props) => {
    if (props.reduxResetFacets === true) {
      props.saveVehicleData([]);
      props.savePageFrom(DefaultPageFrom);
      props.saveFacetSelectors([]);
      props.saveSearchTerm("");
      props.saveDialogOpen(false);
      props.saveOrderby("");

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

const mapStateToProps = (reduxState) => {
  return {
    searchCount: reduxState.searchReducer.searchCount,
    reduxResultsCount: reduxState.searchReducer.searchCount,
    vehicleData: reduxState.searchReducer.vehicleData,

    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    defaultFacetData: reduxState.facetReducer.defaultFacetData,
    facetData: reduxState.facetReducer.facetData,
    reduxSelectedFacet: reduxState.facetReducer.selectedFacet,
    reduxLoading: reduxState.componentReducer.loading,

    reduxResetFacets: reduxState.facetReducer.resetFacets,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveVehicleData: (vehicleData) =>
      dispatch(searchActions.saveVehicleData(vehicleData)),
    savePageFrom: (pageFrom) => dispatch(searchActions.savePageFrom(pageFrom)),
    saveFacetSelectors: (resetFacetArray) =>
      dispatch(facetActions.saveFacetSelectors(resetFacetArray)),
    saveSearchTerm: (searchTerm) =>
      dispatch(searchActions.saveSearchTerm(searchTerm)),
    saveOrderby: (orderBy) => dispatch(searchActions.saveOrderby(orderBy)),
    saveDialogOpen: (dialogOpen) =>
      dispatch(componentActions.saveDialogOpen(dialogOpen)),
    saveSelectedFacet: (facet) =>
      dispatch(componentActions.saveSelectedFacet(facet)),
    saveFacetSelectedKeys: (facetSelectedKeys) =>
      dispatch(facetActions.saveFacetSelectedKeys(facetSelectedKeys)),
    saveResetFacets: (resetFacets) =>
      dispatch(facetActions.saveResetFacets(resetFacets)),
  };
};

ResetFacets.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxResultsCount: PropTypes.number,
  defaultFacetData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,

  saveVehicleData: PropTypes.func,
  savePageFrom: PropTypes.func,
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
