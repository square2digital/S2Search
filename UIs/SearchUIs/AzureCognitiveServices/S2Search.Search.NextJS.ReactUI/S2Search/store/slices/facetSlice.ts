import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface FacetData {
  facetDisplayText: string;
  value: string;
  count: number;
  type: string;
  from?: number;
  to?: number;
}

interface SelectedFacetData {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}

interface FacetState {
  facetSelectors: SelectedFacetData[];
  facetSelectedKeys: string[];
  defaultFacetData: FacetData[];
  facetData: FacetData[];
  selectedFacet: string;
  facetChipDeleted: number;
  resetFacets: boolean;
}

const initialState: FacetState = {
  facetSelectors: [],
  facetSelectedKeys: [],
  defaultFacetData: [],
  facetData: [],
  selectedFacet: '',
  facetChipDeleted: 0,
  resetFacets: false,
};

const facetSlice = createSlice({
  name: 'facet',
  initialState,
  reducers: {
    setFacetSelectors: (state, action: PayloadAction<SelectedFacetData[]>) => {
      state.facetSelectors = action.payload;
    },
    addFacetSelector: (state, action: PayloadAction<SelectedFacetData>) => {
      state.facetSelectors.push(action.payload);
    },
    removeFacetSelector: (
      state,
      action: PayloadAction<SelectedFacetData[]>
    ) => {
      state.facetSelectors = action.payload;
    },
    setFacetSelectedKeys: (state, action: PayloadAction<string[]>) => {
      state.facetSelectedKeys = action.payload;
    },
    setDefaultFacetData: (state, action: PayloadAction<FacetData[]>) => {
      state.defaultFacetData = action.payload;
    },
    setFacetData: (state, action: PayloadAction<FacetData[]>) => {
      state.facetData = action.payload;
    },
    setSelectedFacet: (state, action: PayloadAction<string>) => {
      state.selectedFacet = action.payload;
    },
    setFacetChipDeleted: (state, action: PayloadAction<number>) => {
      state.facetChipDeleted = action.payload;
    },
    setResetFacets: (state, action: PayloadAction<boolean>) => {
      state.resetFacets = action.payload;
    },
    clearAllFacets: state => {
      state.facetSelectors = [];
      state.facetSelectedKeys = [];
      state.selectedFacet = '';
    },
    resetFacets: state => {
      state.facetSelectors = [];
      state.facetSelectedKeys = [];
      state.selectedFacet = '';
      state.resetFacets = true;
    },
  },
});

export const {
  setFacetSelectors,
  addFacetSelector,
  removeFacetSelector,
  setFacetSelectedKeys,
  setDefaultFacetData,
  setFacetData,
  setSelectedFacet,
  setFacetChipDeleted,
  setResetFacets,
  clearAllFacets,
  resetFacets,
} = facetSlice.actions;

export default facetSlice.reducer;
