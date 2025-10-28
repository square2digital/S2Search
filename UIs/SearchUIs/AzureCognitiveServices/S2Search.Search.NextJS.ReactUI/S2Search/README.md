# S2Search - Vehicle Search UI

A modern, fully TypeScript-powered Next.js application for vehicle search and discovery, built with Material-UI and Redux Toolkit.

## 🚀 Technology Stack

- **Framework**: Next.js 14.2.31 with TypeScript
- **UI Library**: Material-UI (MUI) v5
- **State Management**: Redux Toolkit with TypeScript
- **Styling**: CSS Modules + Material-UI theming
- **Search**: Azure Cognitive Services integration
- **Performance**: Optimized with modern ES2022 features

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
│   └── App.tsx          # Main application component
├── pages/               # Next.js pages (TypeScript)
│   ├── api/            # API routes for backend integration
│   ├── index.tsx       # Home page with SSR
│   └── _app.tsx        # App wrapper with theme provider
├── store/              # Redux Toolkit store (TypeScript)
│   ├── slices/         # Feature-based state slices
│   └── hooks.ts        # Typed Redux hooks
├── types/              # TypeScript type definitions
├── common/             # Shared utilities and constants
└── styles/             # Global styles and themes
```

### State Management

- **Redux Toolkit**: Modern Redux with TypeScript
- **Typed Hooks**: `useAppSelector` and `useAppDispatch`
- **Feature Slices**: Modular state management for search, facets, and UI

## 🛠️ Development

### Prerequisites

- Node.js 18+
- npm or yarn
- TypeScript knowledge recommended

### Getting Started

1. **Clone and Install**

   ```bash
   git clone <repository-url>
   cd S2Search
   npm install
   ```

2. **Development Mode**

   ```bash
   npm run dev
   ```

   Opens [http://localhost:3000](http://localhost:3000)

3. **Type Checking**

   ```bash
   npm run type-check
   # or continuous checking
   npx tsc --watch --noEmit
   ```

4. **Linting**
   ```bash
   npm run lint
   ```

### Build and Deploy

1. **Production Build**

   ```bash
   npm run build
   npm start
   ```

2. **Type Safety Check**
   ```bash
   npx tsc --noEmit
   ```

## 🔧 Configuration

### Environment Variables

Create a `.env.local` file:

```env
NEXT_PUBLIC_API_BASE_URL=your-api-endpoint
NEXT_PUBLIC_SEARCH_INDEX=your-search-index
```

### TypeScript Configuration

The project uses modern TypeScript configuration (`tsconfig.json`):

- **Target**: ES2022 for optimal performance
- **Strict Mode**: Full type safety enabled
- **Path Mapping**: Clean imports with `@/` aliases
- **Next.js Integration**: Automatic type generation

### Theme Configuration

Dynamic theming supports:

- Primary/secondary color customization
- Material-UI theme overrides
- Server-side theme injection
- Responsive breakpoints

## 🧪 Testing and Quality

### Code Quality Tools

- **TypeScript**: Compile-time type checking
- **ESLint**: Code linting with TypeScript rules
- **Next.js**: Built-in performance optimization

### Development Workflow

```bash
# Development with type checking
npm run dev

# Full type check
npm run type-check

# Lint and fix
npm run lint

# Production build test
npm run build
```

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

- **Card Layout**: Responsive vehicle cards
- **Lazy Loading**: Performance-optimized image loading
- **Infinite Scroll**: Load more results on demand
- **Sort Options**: Multiple sorting criteria

## 🔮 TypeScript Migration Benefits

This application was fully migrated from JavaScript to TypeScript, providing:

- **🛡️ Type Safety**: Compile-time error detection
- **🚀 Developer Experience**: Enhanced IDE support and autocomplete
- **📚 Self-Documentation**: Interfaces serve as living documentation
- **🔧 Refactoring Confidence**: Safe large-scale code changes
- **🎯 API Integration**: Typed API responses and error handling
- **⚡ Performance**: Better tree-shaking and optimization

## 🤝 Contributing

1. Follow TypeScript best practices
2. Use the established component patterns
3. Maintain type safety throughout
4. Test in both development and production builds
5. Ensure responsive design compatibility

## 📄 License

© 2025 Square2Digital. All rights reserved.

---

_Built with ❤️ using Next.js, TypeScript, and Material-UI_
