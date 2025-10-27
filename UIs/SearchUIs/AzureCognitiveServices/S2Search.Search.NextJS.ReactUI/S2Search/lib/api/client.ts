import { ApiConfig, SearchQueryParams, ApiResponse } from '@/types/apiTypes';
import { SearchRequest } from '@/types/searchTypes';
import axios, { AxiosResponse, AxiosError } from 'axios';
import { SearchAPIEndpoint, AutoCompleteURL, DocumentCountURL, ApiRootEndpoint } from '@/common/Constants';
import https from 'https';

// Constants
const HTTPS_AGENT = new https.Agent({
  rejectUnauthorized: false, // Only for development
});

// Utility functions
function getApiKey(): string {
  return process.env.NEXT_PUBLIC_OCP_APIM_SUBSCRIPTION_KEY || '';
}

function formatCallingHost(host: string): string {
  if (process.env.NODE_ENV === 'development') {
    return process.env.NEXT_PUBLIC_DEV_CUSTOMER_ENDPOINT || 'devtest';
  }

  return host
    .replace(/^(https?:\/\/)?(www\.)?/, '')
    .replace(/\/$/, '');
}

function buildConfig(includeApiKey: boolean = true): ApiConfig {
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
      config.headers['Ocp-Apim-Subscription-Key'] = apiKey;
    }
  }

  return config;
}

function buildSearchQueryString(params: SearchQueryParams): string {
  const queryParams = new URLSearchParams();
  
  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      queryParams.append(key, String(value));
    }
  });

  return queryParams.toString();
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
  async invokeAPI<T = any>(
    endpoint: string,
    includeApiKey: boolean = true,
    params?: SearchQueryParams
  ): Promise<ApiResponse<T>> {
    const config = buildConfig(includeApiKey);
    
    // If endpoint is already a full URL, use it as-is, otherwise prepend baseUrl
    let url = endpoint.startsWith('http') ? endpoint : `${this.baseUrl}${endpoint}`;

    if (params) {
      const queryString = buildSearchQueryString(params);
      url += `?${queryString}`;
    }

    return this.handleRequest(axios.get<T>(url, config));
  }

  async search(params: SearchRequest): Promise<ApiResponse> {
    const searchEndpoint = SearchAPIEndpoint;
    return this.invokeAPI(`${searchEndpoint}`, false, params);
  }

  async getFacets(params: SearchRequest): Promise<ApiResponse> {
    const facetEndpoint = process.env.NEXT_PUBLIC_FACET_API_ENDPOINT;
    return this.invokeAPI(`${facetEndpoint}`, false, params);
  }  

  // Specific API methods
  async getTheme(callingHost: string): Promise<ApiResponse> {
    const host = formatCallingHost(callingHost);
    return this.invokeAPI(`/api/configuration/theme/${host}`, true);
  }

  async getConfiguration(callingHost: string): Promise<ApiResponse> {
    const host = formatCallingHost(callingHost);
    return this.invokeAPI(`/api/configuration/search/${host}`, true);
  }

  async getDocumentCount(callingHost: string): Promise<ApiResponse<number>> {
    const host = formatCallingHost(callingHost);
    const countEndpoint = DocumentCountURL;
    return this.invokeAPI(`${countEndpoint}`, true, { callingHost: host });
  }

  async autoSuggest(searchTerm: string, callingHost: string): Promise<ApiResponse> {
    const host = formatCallingHost(callingHost);
    return this.invokeAPI(`${AutoCompleteURL}`, true, {
      searchTerm: searchTerm,
      callingHost: host,
    });
  }
}

// Singleton instance
export const apiClient = new ApiClient();