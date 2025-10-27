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