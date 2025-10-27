export interface SearchRequest {
  searchTerm: string;
  filters: string;
  orderBy: string;
  pageNumber: number;
  pageSize: number;
  numberOfExistingResults?: number;
  customerEndpoint : string;
}

export interface SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}