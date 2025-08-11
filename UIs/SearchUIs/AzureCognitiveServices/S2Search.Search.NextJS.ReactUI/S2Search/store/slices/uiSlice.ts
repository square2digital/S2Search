import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UIState {
  isDialogOpen: boolean;
  isLoading: boolean;
  cancellationToken: boolean;
}

const initialState: UIState = {
  isDialogOpen: false,
  isLoading: false,
  cancellationToken: false,
};

const uiSlice = createSlice({
  name: 'ui',
  initialState,
  reducers: {
    setDialogOpen: (state, action: PayloadAction<boolean>) => {
      state.isDialogOpen = action.payload;
    },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.isLoading = action.payload;
    },
    setCancellationToken: (state, action: PayloadAction<boolean>) => {
      state.cancellationToken = action.payload;
    },
    toggleDialog: (state) => {
      state.isDialogOpen = !state.isDialogOpen;
    },
  },
});

export const {
  setDialogOpen,
  setLoading,
  setCancellationToken,
  toggleDialog,
} = uiSlice.actions;

export default uiSlice.reducer;
