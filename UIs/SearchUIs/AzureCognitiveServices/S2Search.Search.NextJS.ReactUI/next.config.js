/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  // Enable standalone output for better Docker builds
  output: 'standalone',
  compiler: {
    emotion: true,
    // Remove console.logs in production
    removeConsole:
      process.env.NODE_ENV === 'production'
        ? {
            exclude: ['error'],
          }
        : false,
  },
  // Optimize build output
  poweredByHeader: false,
  generateEtags: false,
  typescript: {
    // Allow production builds to complete even with TypeScript errors
    ignoreBuildErrors: false,
  },
  env: {
    CUSTOM_KEY: process.env.CUSTOM_KEY,
  },
  // Enhanced image optimization - using remotePatterns instead of domains
  images: {
    remotePatterns: [
      {
        protocol: 'http',
        hostname: 'localhost',
      },
    ],
    formats: ['image/webp', 'image/avif'],
    deviceSizes: [640, 750, 828, 1080, 1200, 1920, 2048, 3840],
    imageSizes: [16, 32, 48, 64, 96, 128, 256, 384],
    minimumCacheTTL: 60,
  },
  // Experimental features for performance
  experimental: {
    optimizePackageImports: ['@mui/material', '@mui/icons-material'],
  },
  // Security headers
  async headers() {
    return [
      {
        source: '/(.*)',
        headers: [
          {
            key: 'X-Frame-Options',
            value: 'DENY',
          },
          {
            key: 'X-Content-Type-Options',
            value: 'nosniff',
          },
          {
            key: 'Referrer-Policy',
            value: 'origin-when-cross-origin',
          },
          {
            key: 'Cache-Control',
            value: 'public, max-age=31536000, immutable',
          },
        ],
      },
      {
        source: '/api/(.*)',
        headers: [
          {
            key: 'Cache-Control',
            value: 'public, max-age=300, s-maxage=300',
          },
        ],
      },
    ];
  },
};

module.exports = nextConfig;
