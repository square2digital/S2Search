import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface ConfigState {
  configData: unknown[];
  enableAutoComplete: boolean;
  hideIconVehicleCounts: boolean;
  placeholderText: string;
  placeholderArray: string[];
}

const initialState: ConfigState = {
  configData: [],
  enableAutoComplete: false,
  hideIconVehicleCounts: false,
  placeholderText: 'Start typing to search...',
  placeholderArray: [],
};

const configSlice = createSlice({
  name: 'config',
  initialState,
  reducers: {
    setConfigData: (state, action: PayloadAction<unknown[]>) => {
      state.configData = action.payload;
    },
    setEnableAutoComplete: (state, action: PayloadAction<boolean>) => {
      state.enableAutoComplete = action.payload;
    },
    setHideIconVehicleCounts: (state, action: PayloadAction<boolean>) => {
      state.hideIconVehicleCounts = action.payload;
    },
    setPlaceholderText: (state, action: PayloadAction<string>) => {
      state.placeholderText = action.payload;
    },
    setPlaceholderArray: (state, action: PayloadAction<string[]>) => {
      state.placeholderArray = action.payload;
    },
    updateConfig: (state, action: PayloadAction<Partial<ConfigState>>) => {
      Object.assign(state, action.payload);
    },
  },
});

export const {
  setConfigData,
  setEnableAutoComplete,
  setHideIconVehicleCounts,
  setPlaceholderText,
  setPlaceholderArray,
  updateConfig,
} = configSlice.actions;

export default configSlice.reducer;
