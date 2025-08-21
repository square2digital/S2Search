import { configureStore } from '@reduxjs/toolkit';
import searchReducer from './slices/searchSlice';
import themeReducer from './slices/themeSlice';
import uiReducer from './slices/uiSlice';
import facetReducer from './slices/facetSlice';
import configReducer from './slices/configSlice';

export const store = configureStore({
  reducer: {
    search: searchReducer,
    theme: themeReducer,
    ui: uiReducer,
    facet: facetReducer,
    config: configReducer,
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: ['persist/PERSIST', 'persist/REHYDRATE'],
        // Performance: ignore large state paths
        ignoredPaths: ['search.vehicleData', 'facet.facetData'],
      },
      // Enable immutability check only in development
      immutableCheck: process.env.NODE_ENV === 'development',
    }),
  devTools: process.env.NODE_ENV !== 'production',
  // Preloaded state optimization
  preloadedState: undefined,
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
