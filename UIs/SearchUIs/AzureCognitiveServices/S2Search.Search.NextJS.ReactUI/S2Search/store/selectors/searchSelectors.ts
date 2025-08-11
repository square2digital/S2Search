import { createSelector } from '@reduxjs/toolkit';
import type { RootState } from '../index';

// Search selectors
export const selectSearchState = (state: RootState) => state.search;

export const selectSearchTerm = createSelector(
  [selectSearchState],
  (search) => search.searchTerm
);

export const selectVehicleData = createSelector(
  [selectSearchState],
  (search) => search.vehicleData
);

export const selectSearchCount = createSelector(
  [selectSearchState],
  (search) => search.searchCount
);

export const selectTotalDocumentCount = createSelector(
  [selectSearchState],
  (search) => search.totalDocumentCount
);

export const selectHasMoreResults = createSelector(
  [selectSearchCount, selectVehicleData],
  (searchCount, vehicleData) => vehicleData.length < searchCount
);

export const selectIsSearchEmpty = createSelector(
  [selectVehicleData],
  (vehicleData) => vehicleData.length === 0
);

export const selectNetworkError = createSelector(
  [selectSearchState],
  (search) => search.networkError
);
