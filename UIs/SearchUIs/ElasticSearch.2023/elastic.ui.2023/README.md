# S2Search - Modern TypeScript UI

A cutting-edge Next.js 14 application built with TypeScript, Tailwind CSS, and the App Router for the ultimate vehicle search experience.

## 🚀 Technology Stack

- **Next.js 14** - Latest App Router with React Server Components
- **TypeScript 5.x** - Full type safety and modern language features
- **Tailwind CSS 3.3** - Utility-first CSS framework
- **React 18** - Latest React with concurrent features
- **ESLint & Prettier** - Code quality and formatting

## ✨ Key Features

- 🔍 **Advanced Vehicle Search** - Intelligent search with filters and facets
- 🎨 **Modern Design** - Clean, responsive interface with Tailwind CSS
- ⚡ **Performance Optimized** - Server-side rendering and static optimization
- 📱 **Mobile First** - Responsive design for all device sizes
- 🛡️ **Type Safe** - Full TypeScript coverage for reliability
- 🚀 **Fast Loading** - Optimized bundles and lazy loading

## 🏗️ Architecture

### App Router Structure

```
app/
├── globals.css         # Global styles with Tailwind
├── layout.tsx         # Root layout component
├── page.tsx          # Home page component
└── loading.tsx       # Loading UI component
```

### Key Technologies

- **Server Components** - Optimal performance with RSC
- **Static Generation** - Pre-rendered pages for speed
- **TypeScript** - Type-safe development experience
- **Tailwind CSS** - Utility-first styling approach

## 🛠️ Getting Started

### Prerequisites

- **Node.js 18+** - [Download here](https://nodejs.org/)
- **npm, yarn, or pnpm** - Package manager

### Installation & Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/square2digital/S2Search.git
   cd S2Search/UIs/SearchUIs/ElasticSearch.2023/elastic.ui.2023
   ```

2. **Install dependencies:**

   ```bash
   npm install
   # or
   yarn install
   # or
   pnpm install
   ```

3. **Run development server:**

   ```bash
   npm run dev
   # or
   yarn dev
   # or
   pnpm dev
   ```

4. **Open in browser:**
   Navigate to [http://localhost:3000](http://localhost:3000)

### Development Commands

```bash
# Development server
npm run dev

# Production build
npm run build

# Start production server
npm start

# Lint code
npm run lint

# Type checking
npx tsc --noEmit
```

## 🔧 Configuration

### Environment Variables

Create a `.env.local` file:

```env
# API Configuration
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
NEXT_PUBLIC_SEARCH_ENDPOINT=/api/search
NEXT_PUBLIC_ELASTICSEARCH_INDEX=vehicles

# Environment
NODE_ENV=development
```

### TypeScript Configuration

The project uses modern TypeScript configuration in `tsconfig.json`:

```json
{
  "compilerOptions": {
    "target": "ES2022",
    "lib": ["dom", "dom.iterable", "es6"],
    "allowJs": true,
    "skipLibCheck": true,
    "strict": true,
    "noEmit": true,
    "esModuleInterop": true,
    "module": "esnext",
    "moduleResolution": "bundler",
    "resolveJsonModule": true,
    "isolatedModules": true,
    "jsx": "preserve",
    "incremental": true,
    "plugins": [
      {
        "name": "next"
      }
    ],
    "paths": {
      "@/*": ["./*"]
    }
  }
}
```

### Tailwind Configuration

Custom Tailwind setup for design system consistency:

```javascript
// tailwind.config.js
module.exports = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "#006bd1",
        secondary: "#003c75",
      },
    },
  },
  plugins: [],
};
```

## 🧪 Testing & Quality

### Type Safety

```bash
# Run TypeScript compiler check
npx tsc --noEmit

# Watch mode for continuous checking
npx tsc --watch --noEmit
```

### Code Quality

```bash
# Lint code
npm run lint

# Format code
npx prettier --write .

# Check for unused exports
npx ts-unused-exports tsconfig.json
```

## 📦 Build & Deployment

### Production Build

```bash
# Create optimized production build
npm run build

# Start production server
npm start
```

### Docker Deployment

```dockerfile
FROM node:18-alpine AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app
COPY package*.json ./
RUN npm ci

FROM node:18-alpine AS builder
WORKDIR /app
COPY . .
COPY --from=deps /app/node_modules ./node_modules
RUN npm run build

FROM node:18-alpine AS runner
WORKDIR /app
ENV NODE_ENV production
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder /app/public ./public
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

USER nextjs
EXPOSE 3000
ENV PORT 3000
CMD ["node", "server.js"]
```

### Kubernetes Deployment

Integration with the main S2Search Kubernetes setup in [`K8s/Legacy/K8s.Local.Development.Environment`](../../../../K8s/Legacy/K8s.Local.Development.Environment).

## 🔮 Performance Features

### Next.js 14 Optimizations

- **App Router** - Modern routing with layouts
- **Server Components** - Optimal rendering strategy
- **Static Generation** - Pre-rendered pages for speed
- **Image Optimization** - Automatic image optimization
- **Font Optimization** - Automatic font loading optimization

### Bundle Analysis

```bash
# Analyze bundle size
npm run build

# Use webpack-bundle-analyzer (if configured)
npm run analyze
```

## 🤝 Contributing

1. Follow TypeScript best practices
2. Use functional components with hooks
3. Implement proper error boundaries
4. Ensure mobile responsiveness
5. Write meaningful commit messages

### Code Standards

- **TypeScript**: Strict mode enabled, no `any` types
- **React**: Functional components with TypeScript
- **Styling**: Tailwind CSS utility classes
- **Formatting**: Prettier with ESLint integration

## 🔗 Integration

### API Integration

Connects to:

- [`S2Search.API`](../../../../APIs/S2Search.API) - Main backend API
- [`S2Search.Elastic.API`](../../ElasticSearch/S2Search.Elastic.API) - Elasticsearch API

### Related Projects

- **Admin UI**: [`S2Search.Admin.NextJS.ReactUI`](../../../AdminUI/S2Search.Admin.NextJS.ReactUI)
- **Legacy UI**: [`S2Search.Elastic.NextJS.ReactUI`](../../ElasticSearch/S2Search.Elastic.NextJS.ReactUI)
- **Azure UI**: [`S2Search.Search.NextJS.ReactUI`](../../AzureCognitiveServices/S2Search.Search.NextJS.ReactUI)

## 📚 Learn More

### Next.js Resources

- [Next.js Documentation](https://nextjs.org/docs) - Learn about Next.js features and API
- [Learn Next.js](https://nextjs.org/learn) - Interactive Next.js tutorial
- [Next.js GitHub](https://github.com/vercel/next.js/) - Next.js repository

### TypeScript Resources

- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [React TypeScript Cheatsheet](https://react-typescript-cheatsheet.netlify.app/)

## 📄 License

© 2025 Square2 Digital Ltd. All rights reserved. See [LICENSE](../../../../LICENSE) for details.

---

_Built with ❤️ using Next.js 14, TypeScript, and Tailwind CSS_
