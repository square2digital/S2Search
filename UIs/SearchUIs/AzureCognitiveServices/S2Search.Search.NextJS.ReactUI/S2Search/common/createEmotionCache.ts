import createCache, { EmotionCache } from '@emotion/cache';

// Create a simple emotion cache without insertion point complications
// This should be more stable during HMR and development
export default function createEmotionCache(): EmotionCache {
  return createCache({
    key: 'mui-style',
    prepend: true,
  });
}