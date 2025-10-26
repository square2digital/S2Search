import {
  DefaultPageNumber,
  DefaultPageSize,
} from '../../../../common/Constants';

// Define the filter type
export interface SearchFilter {
  field: string;
  value: string | number | boolean;
  operator?: string;
}

// Define the main SearchRequest type
export interface SearchRequest {
  searchTerm: string;
  filters: SearchFilter[];
  orderBy: string[];
  pageNumber: number;
  pageSize: number;
  numberOfExistingResults: number;
  callingHost: string;
}

// Parameters for creating a SearchRequest
export interface CreateSearchRequestParams {
  searchTerm?: string | null;
  filters?: SearchFilter[] | null;
  orderBy?: string[] | null;
  pageNumber?: number | null;
  pageSize?: number | null;
  numberOfExistingResults?: number | null;
  callingHost?: string | null;
}

// Utility function to create a SearchRequest with defaults
export const createSearchRequest = (params: CreateSearchRequestParams = {}): SearchRequest => {
  const {
    searchTerm,
    filters,
    orderBy,
    pageNumber,
    pageSize,
    numberOfExistingResults,
    callingHost,
  } = params;

  // Normalize searchTerm
  const normalizedSearchTerm = 
    searchTerm === undefined || searchTerm === null || searchTerm === ' ' 
      ? '' 
      : searchTerm;

  // Normalize filters
  const normalizedFilters = filters === undefined || filters === null ? [] : filters;

  // Normalize orderBy
  const normalizedOrderBy = orderBy === undefined || orderBy === null ? ['price desc'] : orderBy;

  // Normalize pageNumber
  const normalizedPageNumber = pageNumber === undefined || pageNumber === null ? DefaultPageNumber : pageNumber;

  // Normalize pageSize
  const normalizedPageSize = pageSize === undefined || pageSize === null ? DefaultPageSize : pageSize;

  // Normalize numberOfExistingResults
  const normalizedNumberOfExistingResults = 
    numberOfExistingResults === undefined ||
    numberOfExistingResults === null ||
    normalizedFilters.length === 1
      ? normalizedPageSize
      : numberOfExistingResults;

  // Normalize callingHost
  const normalizedCallingHost = callingHost === undefined || callingHost === null ? '' : callingHost;

  return {
    searchTerm: normalizedSearchTerm,
    filters: normalizedFilters,
    orderBy: normalizedOrderBy,
    pageNumber: normalizedPageNumber,
    pageSize: normalizedPageSize,
    numberOfExistingResults: normalizedNumberOfExistingResults,
    callingHost: normalizedCallingHost,
  };
};

// Utility function to create SearchRequest from plain object (for Redux deserialization)
export const searchRequestFromPlainObject = (obj: any): SearchRequest | null => {
  if (!obj) return null;
  
  return createSearchRequest({
    searchTerm: obj.searchTerm,
    filters: obj.filters,
    orderBy: obj.orderBy,
    pageNumber: obj.pageNumber,
    pageSize: obj.pageSize,
    numberOfExistingResults: obj.numberOfExistingResults,
    callingHost: obj.callingHost,
  });
};

// For backward compatibility, export a class-like constructor function
export const SearchRequestConstructor = (
  searchTerm?: string | null,
  filters?: SearchFilter[] | null,
  orderBy?: string[] | null,
  pageNumber?: number | null,
  pageSize?: number | null,
  numberOfExistingResults?: number | null,
  callingHost?: string | null
): SearchRequest => {
  return createSearchRequest({
    searchTerm,
    filters,
    orderBy,
    pageNumber,
    pageSize,
    numberOfExistingResults,
    callingHost,
  });
};

// Default export for backward compatibility
export default SearchRequestConstructor;