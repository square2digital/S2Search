import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  try {
    const host = req.headers.host || 'localhost:2997';
    const response = await apiClient.getDocumentCount(host);

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      console.error('Document Count API Error:', response.error);
      res.status(500).json({ error: 'Failed to fetch document count' });
    }
  } catch (error) {
    console.error('Document Count API Error:', error);
    res.status(500).json({ error: 'Internal server error' });
  }
}