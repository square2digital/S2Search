class SelectedFacetData {
  constructor(
    facetKey = '',
    facetDisplayText = '',
    luceneExpression = '',
    checked = false
  ) {
    this.facetKey = facetKey;
    this.facetDisplayText = facetDisplayText;
    this.luceneExpression = luceneExpression;
    this.checked = checked;
  }
}

export default SelectedFacetData;
