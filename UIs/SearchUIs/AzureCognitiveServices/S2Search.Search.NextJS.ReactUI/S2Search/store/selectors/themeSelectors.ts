import { createSelector } from '@reduxjs/toolkit';
import type { RootState } from '../index';

// Theme selectors
export const selectThemeState = (state: RootState) => state.theme;

export const selectPrimaryColour = createSelector(
  [selectThemeState],
  theme => theme.primaryColour
);

export const selectSecondaryColour = createSelector(
  [selectThemeState],
  theme => theme.secondaryColour
);

export const selectNavBarColour = createSelector(
  [selectThemeState],
  theme => theme.navBarColour
);

export const selectLogoURL = createSelector(
  [selectThemeState],
  theme => theme.logoURL
);

export const selectMissingImageURL = createSelector(
  [selectThemeState],
  theme => theme.missingImageURL
);

export const selectThemeColors = createSelector([selectThemeState], theme => ({
  primary: theme.primaryColour,
  secondary: theme.secondaryColour,
  navBar: theme.navBarColour,
}));
