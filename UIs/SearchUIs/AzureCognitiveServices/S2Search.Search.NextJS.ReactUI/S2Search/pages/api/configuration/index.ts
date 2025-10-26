import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  try {
    const host = req.headers.host || 'localhost:2997';
    const response = await apiClient.getConfiguration(host);

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      // Only log non-404 errors as these are expected when backend is offline
      if (response.status !== 404) {
        console.error('Configuration API Error:', response.error);
      }
      res.status(response.status || 500).json({ 
        error: response.status === 404 ? 'Configuration service unavailable' : 'Failed to fetch configuration' 
      });
    }
  } catch (error) {
    console.error('Configuration API Error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}