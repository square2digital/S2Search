export interface SearchRequest {
  searchTerm: string;
  facets: string;
  orderBy: string;
  pageNumber: number;
  pageSize: number;
  numberOfExistingResults: number;
  callingHost: string;
}

export interface SearchRequestParams {
  searchTerm?: string;
  facets?: string;
  orderBy?: string;
  pageNumber?: number;
  pageSize?: number;
  numberOfExistingResults?: number;
  callingHost?: string;
}

export interface SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}