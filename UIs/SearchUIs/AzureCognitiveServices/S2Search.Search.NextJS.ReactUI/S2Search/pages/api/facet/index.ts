import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  if (req.method !== 'POST') {
    res.setHeader('Allow', ['POST']);
    return res.status(405).json({ error: 'Method not allowed' });
  }

  try {
    const host = req.headers.host || 'localhost:2997';
    const searchRequest = req.body;
    
    const response = await apiClient.getFacets({
      ...searchRequest,
      customerEndpoint: host
    });

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      console.error('Facet API Error:', response.error);
      res.status(500).json({ error: 'Failed to fetch facets' });
    }
  } catch (error) {
    console.error('Facet API Error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}