import * as actionTypes from "../actions/actionTypes";
import initialFacetState from "../../redux/stateObjects/facetState";
import { GenerateFacetArray } from "../../common/functions/SharedFunctions";

const facetReducer = (state = initialFacetState, action) => {
  switch (action.type) {
    case actionTypes.DEFAULT_FACET_DATA: {
      return { ...state, defaultFacetData: action.defaultFacetData };
    }
    case actionTypes.FACET_DATA: {
      let array = GenerateFacetArray(state.facetData, action.facetData);
      let newobj = { ...state, facetData: array };
      return newobj;
    }
    case actionTypes.FACET_SELECTORS: {
      return {
        ...state,
        facetSelectors: action.facetSelectors,
      };
    }
    case actionTypes.SELECTED_FACET:
      return {
        ...state,
        selectedFacet: action.selectedFacet,
      };
    case actionTypes.FACET_SELECTED_KEYS: {
      return {
        ...state,
        facetSelectedKeys: action.facetSelectedKeys,
      };
    }
    case actionTypes.RESET_FACETS: {
      return {
        ...state,
        resetFacets: action.resetFacets,
      };
    }
    case actionTypes.FACET_CHIP_DELETED: {
      return {
        ...state,
        facetChipDeleted: action.facetChipDeleted,
      };
    }
    default:
      return state;
  }
};

export default facetReducer;
