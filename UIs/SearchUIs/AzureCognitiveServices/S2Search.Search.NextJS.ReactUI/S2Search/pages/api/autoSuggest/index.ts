import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  try {
    const { searchTerm } = req.query;
    
    if (!searchTerm || typeof searchTerm !== 'string') {
      return res.status(400).json({ error: 'searchTerm is required' });
    }

    const host = req.headers.host || 'localhost:2997';
    const response = await apiClient.autoSuggest(searchTerm, host);

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      console.error('AutoSuggest API Error:', response.error);
      res.status(500).json({ error: 'Failed to fetch auto suggestions' });
    }
  } catch (error) {
    console.error('AutoSuggest API Error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}