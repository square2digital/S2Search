# S2Search - Vehicle Search UI

A modern, fully TypeScript-powered Next.js application for vehicle search and discovery, built with Material-UI and Redux Toolkit.

## 🚀 Technology Stack

- **Framework**: Next.js 16.0.0 with TypeScript 5.9.3
- **UI Library**: Material-UI (MUI) v7.3.4 with Emotion styling
- **State Management**: Redux Toolkit 2.9.2 with TypeScript
- **Runtime**: React 19.2.0 with React DOM 19.2.0
- **Search**: Azure Cognitive Services integration
- **Performance**: Optimized with modern ES2022 features and Next.js standalone output

## ✨ Key Features

- **🔍 Advanced Search**: Intelligent vehicle search with auto-suggestions
- **🎛️ Dynamic Filtering**: Real-time faceted search with multiple filter options
- **📱 Responsive Design**: Mobile-first responsive interface
- **🎨 Dynamic Theming**: Configurable color themes via API
- **⚡ Performance**: Server-side rendering (SSR) for optimal loading
- **🔒 Type Safety**: Full TypeScript implementation for robust development
- **♿ Accessibility**: ARIA-compliant components and keyboard navigation

## 🏗️ Architecture

### Frontend Structure

```
├── components/           # React components (fully TypeScript)
│   ├── material-ui/     # MUI-based components
│   │   ├── searchPage/  # Search interface components
│   │   ├── filtersDialog/ # Filter management
│   │   └── vehicleCards/ # Vehicle display components
│   ├── objects/         # Shared component objects
│   ├── VehicleSearchApp.tsx # Main application component
│   ├── VehicleCard.tsx  # Individual vehicle card component
│   ├── VehicleImage.tsx # Optimized vehicle image component
│   └── LoadMoreResultsButton.tsx # Pagination component
├── pages/               # Next.js pages (TypeScript)
│   ├── api/            # API routes for backend integration
│   ├── index.tsx       # Home page with SSR
│   └── _app.tsx        # App wrapper with theme provider
├── store/              # Redux Toolkit store (TypeScript)
│   ├── slices/         # Feature-based state slices
│   │   ├── configSlice.ts    # App configuration state
│   │   ├── facetSlice.ts     # Search facets state
│   │   ├── searchSlice.ts    # Search results state
│   │   ├── themeSlice.ts     # Dynamic theming state
│   │   └── uiSlice.ts        # UI interaction state
│   ├── selectors/      # Memoized state selectors
│   ├── hooks.ts        # Typed Redux hooks
│   └── index.ts        # Store configuration
├── types/              # TypeScript type definitions
│   ├── apiTypes.ts     # API response types
│   ├── searchTypes.ts  # Search-related types
│   ├── vehicleTypes.ts # Vehicle data types
│   ├── colourTypes.ts  # Theme and color types
│   └── autosuggest-highlight.d.ts # Third-party types
├── common/             # Shared utilities and constants
├── helpers/            # Helper functions and utilities
├── hooks/              # Custom React hooks
├── lib/                # External library configurations
├── utils/              # Utility functions
├── styles/             # Global styles and themes
└── public/             # Static assets
```

### State Management

- **Redux Toolkit**: Modern Redux with TypeScript support
- **Typed Hooks**: `useAppSelector` and `useAppDispatch` with full type safety
- **Feature Slices**: Modular state management across 5 main slices:
  - `configSlice`: Application configuration and API settings
  - `facetSlice`: Search facets and filtering state
  - `searchSlice`: Search results and pagination
  - `themeSlice`: Dynamic theming and branding
  - `uiSlice`: UI interaction states and loading indicators

## 🛠️ Development

### Prerequisites

- Node.js 20+ (required for Next.js 16)
- npm 8.0.0+
- TypeScript knowledge recommended

### Getting Started

1. **Clone and Install**

   ```bash
   git clone <repository-url>
   cd S2Search/UIs/SearchUIs/AzureCognitiveServices/S2Search.Search.NextJS.ReactUI
   npm install
   ```

2. **Environment Setup**

   Copy environment configuration:

   ```bash
   cp .env.development .env.local
   # Edit .env.local with your API endpoints and keys
   ```

3. **Development Mode**

   ```bash
   npm run dev
   ```

   Opens [http://localhost:2997](http://localhost:2997) (configured port)

4. **Type Checking**

   ```bash
   npm run type-check
   # or continuous checking
   npx tsc --watch --noEmit
   ```

5. **Linting**
   ```bash
   npm run lint
   npm run lint:fix  # Auto-fix issues
   ```

### Build and Deploy

1. **Production Build**

   ```bash
   npm run build
   npm start
   ```

2. **Docker Build**

   ```bash
   docker build -t s2search-ui .
   docker run -p 3000:3000 s2search-ui
   ```

3. **Bundle Analysis**

   ```bash
   npm run build:analyze
   npm run perf:bundle
   ```

4. **Type Safety Check**
   ```bash
   npx tsc --noEmit
   ```

## 🔧 Configuration

### Environment Variables

Create a `.env.local` file with the following structure:

```env
# API Configuration
NEXT_PUBLIC_API_URL=https://your-api-endpoint
NEXT_PUBLIC_SEARCH_API_ENDPOINT=/v1/search
NEXT_PUBLIC_AUTO_COMPLETE_URL=/v1/search/AutoSuggest
NEXT_PUBLIC_DOCUMENTS_COUNT_URL=/v1/search/TotalDocumentCount
NEXT_PUBLIC_FACET_API_ENDPOINT=/v1/facet
NEXT_PUBLIC_THEME_URL=/v1/theme/GetTheme

# Authentication
NEXT_PUBLIC_S2SEARCH_API_KEY=your-api-key
NEXT_PUBLIC_DEV_CUSTOMER_ENDPOINT=your-customer-endpoint

# UI Configuration
NEXT_PUBLIC_DEFAULT_PAGE_SIZE=24
NEXT_PUBLIC_DEFAULT_PAGE_NUMBER=0
NEXT_PUBLIC_MOBILE_MAX_WIDTH=458

# Theme Configuration
NEXT_PUBLIC_THEME_PRIMARY_COLOUR=#616161
NEXT_PUBLIC_THEME_SECONDARY_COLOUR=#303f9f
NEXT_PUBLIC_THEME_NAVBAR_COLOUR=#64b5f6
NEXT_PUBLIC_THEME_LOGO_URL=your-logo-url
NEXT_PUBLIC_THEME_MISSING_IMAGE_URL=your-placeholder-image-url

# Search Placeholders
NEXT_PUBLIC_PLACEHOLDER_TEXT_1=Ford Red 2019...
NEXT_PUBLIC_PLACEHOLDER_TEXT_2=Black suv...
NEXT_PUBLIC_PLACEHOLDER_TEXT_3=Convertible Blue...
NEXT_PUBLIC_PLACEHOLDER_TEXT_4=Honda Civic grey...
NEXT_PUBLIC_PLACEHOLDER_TEXT_5=Porsche Silver...
```

### TypeScript Configuration

The project uses modern TypeScript configuration (`tsconfig.json`):

- **Target**: ES2022 for optimal performance
- **Strict Mode**: Full type safety with `noImplicitReturns` and `noFallthroughCasesInSwitch`
- **Path Mapping**: Clean imports with `@/` aliases for all major directories
- **Next.js Integration**: Automatic type generation with Next.js plugin
- **Module Resolution**: Uses `bundler` for optimal bundling
- **JSX**: React JSX transform for modern React patterns

### ESLint Configuration

Modern ESLint setup with TypeScript support:

- **Next.js Rules**: Core web vitals and recommended Next.js rules
- **TypeScript**: Full TypeScript-ESLint integration
- **React**: React and React Hooks rules
- **Custom Rules**: Optimized for TypeScript development with prop-types disabled

### Theme Configuration

Dynamic theming supports:

- **Runtime Configuration**: API-driven theme loading from backend
- **Material-UI Integration**: Full MUI theme system with Emotion styling
- **Color Customization**: Primary, secondary, and navbar color configuration
- **Brand Assets**: Dynamic logo and image URL configuration
- **Responsive Design**: Mobile-first responsive breakpoints
- **Security Headers**: Built-in security headers for production deployment

## 🧪 Testing and Quality

### Code Quality Tools

- **TypeScript**: Compile-time type checking with strict mode
- **ESLint**: Modern flat config with TypeScript, React, and Next.js rules
- **Prettier**: Code formatting with JSON configuration
- **Next.js**: Built-in performance optimization and bundle analysis

### Development Workflow

```bash
# Development with type checking
npm run dev

# Full type check
npm run type-check

# Lint and fix issues
npm run lint
npm run lint:fix

# Format code
npm run format
npm run format:check

# Production build test
npm run build

# Performance analysis
npm run build:analyze
npm run perf:bundle

# Clean build artifacts
npm run clean
```

### Performance Testing

- **Bundle Analysis**: Webpack bundle analyzer integration
- **Load Testing**: Available in `tests/loadTests/` directory
- **Lighthouse**: Performance audit configuration

## 📱 Features Deep Dive

### Search Interface

- **Auto-complete**: Real-time search suggestions
- **Search History**: Dynamic placeholder text
- **Reset Functionality**: Clear all filters and search
- **Keyboard Navigation**: Full accessibility support

### Filtering System

- **Faceted Search**: Multiple filter categories
- **Real-time Updates**: Instant result filtering
- **State Persistence**: Maintains filters across navigation
- **URL Integration**: Shareable filtered states

### Vehicle Display

- **Responsive Cards**: Adaptive vehicle card layout with Material-UI
- **Optimized Images**: Next.js Image component with WebP/AVIF support
- **Lazy Loading**: Performance-optimized image loading with placeholder support
- **Load More**: Infinite scroll with "Load More Results" functionality
- **Sort Options**: Multiple sorting criteria with Redux state management
- **Dynamic Vehicle Cards**: Component-based vehicle display system

## 🔮 TypeScript Migration Benefits

This application is built entirely in TypeScript from the ground up, providing:

- **🛡️ Type Safety**: Complete compile-time error detection across all components
- **🚀 Developer Experience**: Enhanced IDE support with full autocomplete and IntelliSense
- **📚 Self-Documentation**: Comprehensive interfaces and types serve as living documentation
- **🔧 Refactoring Confidence**: Safe large-scale code changes with compiler verification
- **🎯 API Integration**: Fully typed API responses, request objects, and error handling
- **⚡ Performance**: Better tree-shaking and optimization through static analysis
- **🏗️ Architecture**: Structured codebase with clear separation of concerns
- **🔒 Runtime Safety**: Reduced runtime errors through compile-time validation

## 🐳 Docker Deployment

The application includes a multi-stage Dockerfile optimized for production:

### Build Process

```bash
# Build the Docker image
docker build -t s2search-ui .

# Run the container
docker run -p 3000:3000 s2search-ui
```

### Docker Features

- **Multi-stage Build**: Optimized for minimal production image size
- **Node.js 20 Alpine**: Lightweight base image with security updates
- **Standalone Output**: Next.js standalone mode for optimal Docker performance
- **Security**: Non-root user execution with proper file ownership
- **Health Checks**: Built-in health monitoring capabilities
- **Environment Variables**: Production-ready environment configuration

## 🚀 Production Deployment

### Build Optimization

- **Standalone Output**: Optimized for serverless and container deployments
- **Bundle Analysis**: Webpack bundle analyzer for performance monitoring
- **Image Optimization**: WebP and AVIF format support with multiple sizes
- **Security Headers**: CSP, HSTS, and other security headers configured
- **Caching**: Optimized caching strategies for static assets and API responses

## 🤝 Contributing

1. **TypeScript First**: Follow TypeScript best practices and maintain strict type safety
2. **Component Patterns**: Use the established Material-UI and Redux patterns
3. **Code Quality**: Run `npm run lint` and `npm run type-check` before commits
4. **Testing**: Test in both development and production builds
5. **Performance**: Consider bundle size impact and use `npm run build:analyze`
6. **Accessibility**: Maintain ARIA compliance and keyboard navigation
7. **Documentation**: Update types and interfaces for API changes
8. **Environment**: Test across different environment configurations

### Development Guidelines

- Use typed Redux hooks (`useAppSelector`, `useAppDispatch`)
- Follow the established folder structure and naming conventions
- Implement proper error boundaries and loading states
- Ensure responsive design compatibility across all devices
- Write semantic HTML with proper accessibility attributes

## 📄 License

© 2025 Square2Digital. All rights reserved.

---

_Built with ❤️ using Next.js 16, React 19, TypeScript 5.9, and Material-UI 7_
