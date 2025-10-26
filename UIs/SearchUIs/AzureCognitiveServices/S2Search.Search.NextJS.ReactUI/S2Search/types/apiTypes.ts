export interface ApiConfig {
  headers: Record<string, string>;
  httpsAgent?: any;
  timeout?: number;
}

export interface ApiResponse<T = any> {
  data: T;
  status: number;
  success: boolean;
  error?: string;
}

export interface SearchQueryParams {
  searchTerm?: string;
  filters?: string;
  orderBy?: string;
  pageNumber?: number;
  pageSize?: number;
  numberOfExistingResults?: number;
  callingHost?: string;
}