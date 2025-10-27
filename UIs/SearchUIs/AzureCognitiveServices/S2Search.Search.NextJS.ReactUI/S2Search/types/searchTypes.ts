import { DefaultPageNumber, DefaultPageSize } from '@/common/Constants';

export interface ISearchRequest {
  searchTerm: string;
  filters: string; // Changed from string[] to string to match C# API
  orderBy: string;
  pageNumber: number;
  pageSize: number;
  numberOfExistingResults?: number;
  customerEndpoint: string;
}

export class SearchRequest implements ISearchRequest {
  searchTerm: string;
  filters: string; // Changed from string[] to string to match C# API
  orderBy: string;
  pageNumber: number;
  pageSize: number;
  numberOfExistingResults?: number;
  customerEndpoint: string;

  constructor(
    searchTerm?: string,
    filters?: string | string[], // Accept both string and string[] for flexibility
    orderBy?: string,
    pageNumber?: number,
    pageSize?: number,
    numberOfExistingResults?: number,
    customerEndpoint?: string
  ) {
    this.searchTerm = searchTerm?.trim() || '';

    // Convert filters array to string if needed, otherwise use as-is
    if (Array.isArray(filters)) {
      this.filters = filters.join(' OR ');
    } else {
      this.filters = filters || '';
    }

    this.orderBy = orderBy || 'price desc';
    this.pageNumber = pageNumber ?? DefaultPageNumber;
    this.pageSize = pageSize ?? DefaultPageSize;
    this.numberOfExistingResults = numberOfExistingResults ?? this.pageSize;
    this.customerEndpoint = customerEndpoint || '';
  }
}

export interface SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}
