import { NextApiRequest, NextApiResponse } from 'next';
import { apiClient } from '../../../lib/api/client';
import { DefaultTheme } from '../../../common/Constants';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  try {
    const host = req.headers.host || 'localhost:2997';
    const response = await apiClient.getTheme(host);

    if (response.success) {
      res.status(200).json(response.data);
    } else {
      console.error('Failed to call Theme from API:', response.error);
      res.status(500).json(DefaultTheme);
    }
  } catch (error) {
    console.error('Theme API Error:', error);
    res.status(500).json(DefaultTheme);
  }
}