import { createSelector } from '@reduxjs/toolkit';
import type { RootState } from '../index';

// Search selectors
export const selectSearchState = (state: RootState) => state.search;

export const selectVehicleData = createSelector(
  [selectSearchState],
  (search) => search.vehicleData
);

export const selectSearchTerm = createSelector(
  [selectSearchState],
  (search) => search.searchTerm
);

export const selectSearchCount = createSelector(
  [selectSearchState],
  (search) => search.searchCount
);

export const selectOrderBy = createSelector(
  [selectSearchState],
  (search) => search.orderBy
);

export const selectPageNumber = createSelector(
  [selectSearchState],
  (search) => search.pageNumber
);

export const selectHasMoreResults = createSelector(
  [selectSearchState],
  (search) => search.hasMoreResults
);

export const selectIsNetworkError = createSelector(
  [selectSearchState],
  (search) => search.networkError
);

// Theme selectors
export const selectThemeState = (state: RootState) => state.theme;

export const selectPrimaryColour = createSelector(
  [selectThemeState],
  (theme) => theme.primaryColour
);

export const selectSecondaryColour = createSelector(
  [selectThemeState],
  (theme) => theme.secondaryColour
);

export const selectMissingImageURL = createSelector(
  [selectThemeState],
  (theme) => theme.missingImageURL
);

export const selectLogoURL = createSelector(
  [selectThemeState],
  (theme) => theme.logoURL
);

export const selectNavBarColour = createSelector(
  [selectThemeState],
  (theme) => theme.navBarColour
);

// UI selectors
export const selectUIState = (state: RootState) => state.ui;

export const selectIsDialogOpen = createSelector(
  [selectUIState],
  (ui) => ui.isDialogOpen
);

export const selectIsLoading = createSelector(
  [selectUIState],
  (ui) => ui.isLoading
);

export const selectCancellationToken = createSelector(
  [selectUIState],
  (ui) => ui.cancellationToken
);

// Facet selectors
export const selectFacetState = (state: RootState) => state.facet;

export const selectFacetSelectors = createSelector(
  [selectFacetState],
  (facet) => facet.facetSelectors
);

export const selectActiveFacetCount = createSelector(
  [selectFacetSelectors],
  (facetSelectors) => facetSelectors.length
);

export const selectHasActiveFacets = createSelector(
  [selectActiveFacetCount],
  (count) => count > 0
);

export const selectFacetData = createSelector(
  [selectFacetState],
  (facet) => facet.facetData
);

// Config selectors
export const selectConfigState = (state: RootState) => state.config;

export const selectEnableAutoComplete = createSelector(
  [selectConfigState],
  (config) => config.enableAutoComplete
);

export const selectPlaceholderText = createSelector(
  [selectConfigState],
  (config) => config.placeholderText
);

export const selectHideIconVehicleCounts = createSelector(
  [selectConfigState],
  (config) => config.hideIconVehicleCounts
);
