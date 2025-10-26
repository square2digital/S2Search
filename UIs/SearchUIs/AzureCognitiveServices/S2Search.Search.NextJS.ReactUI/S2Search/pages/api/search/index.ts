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
    
    const response = await apiClient.search({
      ...searchRequest,
      callingHost: host
    });

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      console.error('Search API Error:', response.error);
      res.status(500).json({ error: 'Search failed' });
    }
  } catch (error) {
    console.error('Search API Error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}