import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface ThemeState {
  primaryColour: string;
  secondaryColour: string;
  navBarColour: string;
  logoURL: string;
  missingImageURL: string;
}

const initialState: ThemeState = {
  primaryColour: '#616161',
  secondaryColour: '#303f9f',
  navBarColour: '#1976d2',
  logoURL: '/images/Square_2_Logo_Colour.svg',
  missingImageURL: '/images/no-image-available.png',
};

const themeSlice = createSlice({
  name: 'theme',
  initialState,
  reducers: {
    setPrimaryColour: (state, action: PayloadAction<string>) => {
      state.primaryColour = action.payload;
    },
    setSecondaryColour: (state, action: PayloadAction<string>) => {
      state.secondaryColour = action.payload;
    },
    setNavBarColour: (state, action: PayloadAction<string>) => {
      state.navBarColour = action.payload;
    },
    setLogoURL: (state, action: PayloadAction<string>) => {
      state.logoURL = action.payload;
    },
    setMissingImageURL: (state, action: PayloadAction<string>) => {
      state.missingImageURL = action.payload;
    },
    setThemeColors: (state, action: PayloadAction<Partial<ThemeState>>) => {
      Object.assign(state, action.payload);
    },
  },
});

export const {
  setPrimaryColour,
  setSecondaryColour,
  setNavBarColour,
  setLogoURL,
  setMissingImageURL,
  setThemeColors,
} = themeSlice.actions;

export default themeSlice.reducer;
