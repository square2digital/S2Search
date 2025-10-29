import {
  ApiRootEndpoint,
  AutoCompleteURL,
  DocumentCountURL,
  SearchAPIEndpoint,
  ThemeURL,
} from '@/common/Constants';
import { ApiConfig, ApiResponse } from '@/types/apiTypes';
import { SearchRequest } from '@/types/searchTypes';
import axios, { AxiosError, AxiosResponse } from 'axios';
import https from 'https';

// Constants
const HTTPS_AGENT = new https.Agent({
  rejectUnauthorized: false, // Only for development
});

// Utility functions
function getApiKey(): string {
  return process.env.NEXT_PUBLIC_S2SEARCH_API_KEY || '';
}

function convertToQueryParams(params: SearchRequest): Record<string, string> {
  const queryParams: Record<string, string> = {};

  if (params.searchTerm) queryParams.searchTerm = params.searchTerm;
  if (params.filters) queryParams.filters = params.filters; // filters is now a string, not array
  if (params.orderBy) queryParams.orderBy = params.orderBy;
  if (params.pageNumber !== undefined)
    queryParams.pageNumber = params.pageNumber.toString();
  if (params.pageSize !== undefined)
    queryParams.pageSize = params.pageSize.toString();
  if (params.numberOfExistingResults !== undefined)
    queryParams.numberOfExistingResults =
      params.numberOfExistingResults.toString();
  if (params.customerEndpoint)
    queryParams.customerEndpoint = params.customerEndpoint;

  return queryParams;
}

function buildApiConfig(includeApiKey: boolean = true): ApiConfig {
  const config: ApiConfig = {
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
    },
    httpsAgent: HTTPS_AGENT,
    timeout: 30000,
  };

  if (includeApiKey) {
    const apiKey = getApiKey();
    if (apiKey) {
      config.headers['s2search-api-Key'] = apiKey;
    }
  }

  return config;
}

// Main API client
export class ApiClient {
  private baseUrl: string;

  constructor() {
    this.baseUrl = ApiRootEndpoint;
  }

  private async handleRequest<T>(
    promise: Promise<AxiosResponse<T>>
  ): Promise<ApiResponse<T>> {
    try {
      const response = await promise;
      return {
        data: response.data,
        status: response.status,
        success: true,
      };
    } catch (error) {
      const axiosError = error as AxiosError;
      const status = axiosError.response?.status || 500;

      // Only log unexpected errors in development (not 404s which are expected when backend is offline)
      if (process.env.NODE_ENV === 'development' && status !== 404) {
        console.warn('API Warning:', {
          message: axiosError.message,
          code: axiosError.code,
          status: status,
        });
      }

      return {
        data: null as T,
        status: status,
        success: false,
        error: axiosError.message || 'Unknown API error',
      };
    }
  }

  // Core invoke API method
  async invokeSearchAPI<T = any>(
    endpoint: string,
    includeApiKey: boolean = true,
    params?: SearchRequest
  ): Promise<ApiResponse<T>> {
    const config = buildApiConfig(includeApiKey);
    const url = endpoint.startsWith('http')
      ? endpoint
      : `${this.baseUrl}${endpoint}`;

    // Convert SearchRequest to query parameters if provided
    if (params) {
      const queryParams = convertToQueryParams(params);
      const urlWithParams = new URL(url);
      Object.entries(queryParams).forEach(([key, value]) => {
        urlWithParams.searchParams.set(key, value);
      });
      return this.handleRequest(axios.get<T>(urlWithParams.toString(), config));
    }

    return this.handleRequest(axios.get<T>(url, config));
  }

  async search(params: SearchRequest): Promise<ApiResponse> {
    const searchEndpoint = SearchAPIEndpoint;
    return this.invokeSearchAPI(`${searchEndpoint}`, false, params);
  }

  async getFacets(params: SearchRequest): Promise<ApiResponse> {
    const facetEndpoint = process.env.NEXT_PUBLIC_FACET_API_ENDPOINT;
    return this.invokeSearchAPI(`${facetEndpoint}`, false, params);
  }

  // Specific API methods
  async getTheme(customerEndpoint: string): Promise<ApiResponse> {
    return this.invokeSearchAPI(`${ThemeURL}/${customerEndpoint}`, true);
  }

  async getConfiguration(customerEndpoint: string): Promise<ApiResponse> {
    return this.invokeSearchAPI(
      `/api/configuration/search/${customerEndpoint}`,
      true
    );
  }

  async getDocumentCount(
    customerEndpoint: string
  ): Promise<ApiResponse<number>> {
    return this.invokeSearchAPI(
      `${DocumentCountURL}/${customerEndpoint}`,
      true
    );
  }

  async autoSuggest(
    searchTerm: string,
    customerEndpoint: string
  ): Promise<ApiResponse> {
    return this.invokeSearchAPI(
      `${AutoCompleteURL}/${searchTerm}/${customerEndpoint}`,
      true
    );
  }
}

// Singleton instance
export const apiClient = new ApiClient();
