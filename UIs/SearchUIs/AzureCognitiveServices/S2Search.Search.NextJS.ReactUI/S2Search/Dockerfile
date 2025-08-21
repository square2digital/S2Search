# Install dependencies only when needed
FROM node:18-alpine AS deps
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat

WORKDIR /app

# Copy package files
COPY package.json package-lock.json* ./

# Install dependencies with npm ci for faster, reliable, reproducible builds
RUN npm ci --only=production && npm cache clean --force

# Rebuild the source code only when needed
FROM node:18-alpine AS builder
WORKDIR /app

# Copy dependencies from deps stage
COPY --from=deps /app/node_modules ./node_modules

# Copy all source files
COPY . .

# Install all dependencies (including devDependencies) for building
RUN npm ci

# Type check TypeScript files
RUN npm run type-check

# Build the application
RUN npm run build

# Production image, copy all the files and run next
FROM node:18-alpine AS runner
WORKDIR /app

# Set production environment
ENV NODE_ENV=production

# Disable Next.js telemetry during runtime
ENV NEXT_TELEMETRY_DISABLED=1

# Add non-root user for security
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

# Copy public assets
COPY --from=builder /app/public ./public

# Copy Next.js build output
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# Copy custom next.config.js (required for custom configuration)
COPY --from=builder /app/next.config.js ./

# Copy package.json for runtime dependencies
COPY --from=builder /app/package.json ./package.json

# Switch to non-root user
USER nextjs

# Expose the port the app runs on (default Next.js port)
EXPOSE 3000

ENV PORT=3000
ENV HOSTNAME="0.0.0.0"

# Start the application
CMD ["node", "server.js"]