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

  const searchRequest = req.body;
  const facetEndpoint = process.env.NEXT_PUBLIC_FACET_API_ENDPOINT;

  try {
    const response = await apiClient.getFacets({
      ...searchRequest,
    });

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      const errorDetails = {
        message: 'Failed to fetch facets from upstream service',
        endpoint: facetEndpoint,
        requestPayload: searchRequest,
        upstreamError: response.error,
        status: response.status,
        timestamp: new Date().toISOString(),
        requestId: req.headers['x-request-id'] || 'unknown',
      };

      console.error('Facet API Upstream Error:', errorDetails);

      res.status(response.status || 500).json({
        error: 'Failed to fetch facets',
        details: errorDetails,
      });
    }
  } catch (error) {
    const errorDetails = {
      message: 'Internal server error in facet API handler',
      endpoint: facetEndpoint,
      requestPayload: searchRequest,
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

    console.error('Facet API Internal Error:', errorDetails);

    res.status(500).json({
      error: 'Internal server error',
      details: errorDetails,
    });
  }
}
