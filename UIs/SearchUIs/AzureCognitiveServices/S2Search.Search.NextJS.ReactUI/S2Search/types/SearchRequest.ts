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

/**
 * Factory function to create a SearchRequest object
 */
export function createSearchRequest(
  searchTerm: string = '',
  facets: string = '',
  orderBy: string = '',
  pageNumber: number = 0,
  pageSize: number = 25,
  numberOfExistingResults: number = 0,
  callingHost: string = 'localhost:3000'
): SearchRequest {
  return {
    searchTerm,
    facets,
    orderBy,
    pageNumber,
    pageSize,
    numberOfExistingResults,
    callingHost
  };
}

/**
 * Legacy constructor compatibility function
 * @deprecated Use createSearchRequest instead
 */
export const SearchRequestConstructor = createSearchRequest;

export default SearchRequest;