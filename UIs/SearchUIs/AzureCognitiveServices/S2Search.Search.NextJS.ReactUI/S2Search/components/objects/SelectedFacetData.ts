export interface SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}

export class SelectedFacetDataClass implements SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;

  constructor(
    facetKey: string = '',
    facetDisplayText: string = '',
    luceneExpression: string = '',
    checked: boolean = false
  ) {
    this.facetKey = facetKey;
    this.facetDisplayText = facetDisplayText;
    this.luceneExpression = luceneExpression;
    this.checked = checked;
  }
}

// Factory function for creating SelectedFacetData objects
export const createSelectedFacetData = (
  facetKey: string = '',
  facetDisplayText: string = '',
  luceneExpression: string = '',
  checked: boolean = false
): SelectedFacetData => ({
  facetKey,
  facetDisplayText,
  luceneExpression,
  checked,
});

export default SelectedFacetDataClass;