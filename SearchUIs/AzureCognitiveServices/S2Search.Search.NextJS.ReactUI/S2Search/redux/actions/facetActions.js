import * as actionTypes from "./actionTypes";

const facetActions = {
  saveDefaultFacetData: function (defaultFacetData) {
    return {
      type: actionTypes.DEFAULT_FACET_DATA,
      defaultFacetData: defaultFacetData,
    };
  },

  saveFacetData: function (facetData) {
    return { type: actionTypes.FACET_DATA, facetData: facetData };
  },

  saveFacetSelectors: function (facetSelector) {
    return {
      type: actionTypes.FACET_SELECTORS,
      facetSelectors: facetSelector,
    };
  },

  saveFacetSelectedKeys: function (facetSelectedKeys) {
    return {
      type: actionTypes.FACET_SELECTED_KEYS,
      facetSelectedKeys: facetSelectedKeys,
    };
  },

  saveSelectedFacet: function (selectedFacet) {
    return { type: actionTypes.SELECTED_FACET, selectedFacet: selectedFacet };
  },

  saveResetFacets: function (resetFacets) {
    return {
      type: actionTypes.RESET_FACETS,
      resetFacets: resetFacets,
    };
  },

  saveFacetChipDeleted: function (facetChipDeleted) {
    return {
      type: actionTypes.FACET_CHIP_DELETED,
      facetChipDeleted: facetChipDeleted,
    };
  },
};

export default facetActions;
