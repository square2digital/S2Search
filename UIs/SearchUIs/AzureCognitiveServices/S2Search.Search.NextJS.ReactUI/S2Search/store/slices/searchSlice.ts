import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface VehicleData {
  vehicleID: string;
  make: string;
  model: string;
  variant: string;
  location: string;
  price: number;
  monthlyPrice: number;
  mileage: number;
  fuelType: string;
  transmission: string;
  doors: number;
  engineSize: number;
  bodyStyle: string;
  colour: string;
  year: number;
  description: string;
  manufactureColour: string;
  vrm: string;
  imageURL: string;
}

interface SearchRequest {
  searchTerm: string;
  filters: string;
  orderBy: string;
  pageNumber: number;
  pageSize: number;
}

interface SearchState {
  searchTerm: string;
  searchCount: number;
  totalDocumentCount: number;
  vehicleData: VehicleData[];
  orderBy: string;
  pageNumber: number;
  hasMoreResults: boolean;
  networkError: boolean;
  previousRequest: SearchRequest | null;
}

const initialState: SearchState = {
  searchTerm: '',
  searchCount: 0,
  totalDocumentCount: 0,
  vehicleData: [],
  orderBy: '',
  pageNumber: 0,
  hasMoreResults: false,
  networkError: false,
  previousRequest: null,
};

const searchSlice = createSlice({
  name: 'search',
  initialState,
  reducers: {
    setSearchTerm: (state, action: PayloadAction<string>) => {
      state.searchTerm = action.payload;
    },
    setSearchCount: (state, action: PayloadAction<number>) => {
      state.searchCount = action.payload;
    },
    setTotalDocumentCount: (state, action: PayloadAction<number>) => {
      state.totalDocumentCount = action.payload;
    },
    setVehicleData: (state, action: PayloadAction<VehicleData[]>) => {
      state.vehicleData = action.payload;
    },
    appendVehicleData: (state, action: PayloadAction<VehicleData[]>) => {
      state.vehicleData.push(...action.payload);
    },
    setOrderBy: (state, action: PayloadAction<string>) => {
      state.orderBy = action.payload;
    },
    setPageNumber: (state, action: PayloadAction<number>) => {
      state.pageNumber = action.payload;
    },
    incrementPageNumber: state => {
      state.pageNumber += 1;
    },
    setHasMoreResults: (state, action: PayloadAction<boolean>) => {
      state.hasMoreResults = action.payload;
    },
    setNetworkError: (state, action: PayloadAction<boolean>) => {
      state.networkError = action.payload;
    },
    setPreviousRequest: (state, action: PayloadAction<SearchRequest>) => {
      state.previousRequest = action.payload;
    },
    resetSearch: () => initialState,
  },
});

export const {
  setSearchTerm,
  setSearchCount,
  setTotalDocumentCount,
  setVehicleData,
  appendVehicleData,
  setOrderBy,
  setPageNumber,
  incrementPageNumber,
  setHasMoreResults,
  setNetworkError,
  setPreviousRequest,
  resetSearch,
} = searchSlice.actions;

export default searchSlice.reducer;
