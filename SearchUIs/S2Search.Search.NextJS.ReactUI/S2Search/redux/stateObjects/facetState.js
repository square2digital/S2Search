import { getQueryStringFacetsSelectors } from "../../common/functions/QueryStringFunctions";

export default {
  defaultFacetData: [],
  facetData: [],
  facetSelectors: getQueryStringFacetsSelectors(),
  facetSelectedKeys: ["make"],
  selectedFacet: "make",
  resetFacets: false,
  facetChipDeleted: 0,
};
