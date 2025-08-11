import { configureStore } from '@reduxjs/toolkit';
import rootReducer from './reducers/rootReducer';

export default function store() {
  return configureStore({
    reducer: rootReducer,
    middleware: getDefaultMiddleware =>
      getDefaultMiddleware({
        serializableCheck: {
          ignoredActions: ['persist/PERSIST'],
        },
      }),
    devTools: process.env.NODE_ENV !== 'production',
  });
}
