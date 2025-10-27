import { ApiConfig, ApiResponse } from '@/types/apiTypes';
import { SearchRequest } from '@/types/searchTypes';
import axios, { AxiosResponse, AxiosError } from 'axios';
import { SearchAPIEndpoint, AutoCompleteURL, DocumentCountURL, ApiRootEndpoint, ThemeURL } from '@/common/Constants';
import https from 'https';

// Constants
const HTTPS_AGENT = new https.Agent({
  rejectUnauthorized: false, // Only for development
});

// Utility functions
function getApiKey(): string {
  return process.env.NEXT_PUBLIC_S2SEARCH_API_KEY || '';
}

function buildApiConfig(includeApiKey: boolean = true): ApiConfig {
  const config: ApiConfig = {
    headers: {
      'Accept': 'application/json',
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
  ): Promise<ApiResponse<T>> {
    const config = buildApiConfig(includeApiKey);
    const url = endpoint.startsWith('http') ? endpoint : `${this.baseUrl}${endpoint}`;

    return this.handleRequest(axios.get<T>(url, config));
  }

  async search(): Promise<ApiResponse> {
    const searchEndpoint = SearchAPIEndpoint;
    return this.invokeSearchAPI(`${searchEndpoint}`, true);
  }

  async getFacets(): Promise<ApiResponse> {
    const facetEndpoint = process.env.NEXT_PUBLIC_FACET_API_ENDPOINT;
    return this.invokeSearchAPI(`${facetEndpoint}`, true);
  }  

  // Specific API methods
  async getTheme(customerEndpoint: string): Promise<ApiResponse> {
    return this.invokeSearchAPI(`${ThemeURL}/${customerEndpoint}`, true);
  }

  async getConfiguration(customerEndpoint: string): Promise<ApiResponse> {
    return this.invokeSearchAPI(`/api/configuration/search/${customerEndpoint}`, true);
  }

  async getDocumentCount(customerEndpoint: string): Promise<ApiResponse<number>> {
    return this.invokeSearchAPI(`${DocumentCountURL}/${customerEndpoint}`, true);
  }

  async autoSuggest(searchTerm: string, customerEndpoint: string): Promise<ApiResponse> {
    return this.invokeSearchAPI(`${AutoCompleteURL}/${searchTerm}/${customerEndpoint}`, true);
  }
}

// Singleton instance
export const apiClient = new ApiClient();