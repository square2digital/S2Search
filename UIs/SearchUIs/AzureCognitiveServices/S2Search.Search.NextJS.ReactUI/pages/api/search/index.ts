import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse,
) {
  if (req.method !== 'POST') {
    res.setHeader('Allow', ['POST']);
    return res.status(405).json({
      error: 'Method not allowed',
      details: {
        received: req.method,
        expected: 'POST',
        endpoint: req.url,
      },
    });
  }

  // this gets the host via the node API request headers
  const customerEndpoint = req.headers.host || 'localhost:2997';
  const searchRequest = req.body;

  // Map facets to filters for backend compatibility
  const backendRequest = {
    ...searchRequest,
    customerEndpoint: customerEndpoint,
  };

  try {
    const response = await apiClient.search(backendRequest);

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      const errorDetails = {
        message: 'Failed to perform search from upstream service',
        endpoint:
          process.env.NEXT_PUBLIC_SEARCH_API_ENDPOINT ||
          'Search API endpoint not configured',
        customerEndpoint: customerEndpoint,
        requestPayload: searchRequest,
        backendRequest: backendRequest,
        upstreamError: response.error,
        status: response.status,
        timestamp: new Date().toISOString(),
        requestId: req.headers['x-request-id'] || 'unknown',
      };

      console.error('Search API Upstream Error:', errorDetails);

      res.status(response.status || 500).json({
        error: 'Search failed',
        details: errorDetails,
      });
    }
  } catch (error) {
    const errorDetails = {
      message: 'Internal server error in search API handler',
      endpoint:
        process.env.NEXT_PUBLIC_SEARCH_API_ENDPOINT ||
        'Search API endpoint not configured',
      customerEndpoint: customerEndpoint,
      requestPayload: searchRequest,
      backendRequest: backendRequest,
      originalError:
        error instanceof Error
          ? {
              name: error.name,
              message: error.message,
              stack:
                process.env.NODE_ENV === 'development'
                  ? error.stack
                  : undefined,
            }
          : error,
      timestamp: new Date().toISOString(),
      requestId: req.headers['x-request-id'] || 'unknown',
      userAgent: req.headers['user-agent'],
      ip:
        req.headers['x-forwarded-for'] ||
        req.headers['x-real-ip'] ||
        req.socket?.remoteAddress ||
        'unknown',
    };

    console.error('Search API Internal Error:', errorDetails);

    res.status(500).json({
      error: 'Internal server error',
      details: errorDetails,
    });
  }
}
