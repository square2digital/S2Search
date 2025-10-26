import React from 'react';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import InputBase from '@mui/material/InputBase';
import Divider from '@mui/material/Divider';
import RotateLeftIcon from '@mui/icons-material/RotateLeft';
import { MobileMaxWidth } from '../../../../common/Constants';
import {
  checkForEnter,
  generatePlaceholder,
  resetFilters,
  disableResetFiltersButton,
  updateSearchTerm,
} from './searchBarSharedFunctions';
import { useWindowSize } from '../../../../hooks/useWindowSize';
import useDynamicPlaceholder from './DynamicPlaceholder';
import { useAppDispatch, useAppSelector } from '../../../../store/hooks';
import {
  setSearchTerm,
  setVehicleData,
  setPageNumber,
  setOrderBy,
  setSearchCount,
} from '../../../../store/slices/searchSlice';
import {
  resetFacets,
  setFacetSelectors,
  setFacetSelectedKeys,
} from '../../../../store/slices/facetSlice';

// Inline styles object (converted from makeStyles)
const styles = {
  divider: {
    height: 28,
    margin: 4,
  },
};

interface SearchBarProps {
  placeholderText?: string;
}

export const SearchBar: React.FC<SearchBarProps> = (props) => {
  const dispatch = useAppDispatch();
  const reduxSearchTerm = useAppSelector(state => state.search.searchTerm);
  const reduxFacetSelectors = useAppSelector(state => state.facet.facetSelectors);
  const reduxConfigPlaceholders = useAppSelector(state => state.config.placeholderArray);

  const { width: windowWidth } = useWindowSize();
  const dynamicPlaceholder = useDynamicPlaceholder(reduxConfigPlaceholders);

  // Create props object to match old interface
  const searchBarProps = {
    reduxSearchTerm,
    reduxFacetSelectors,
    searchTerm: reduxSearchTerm,
    placeholderText: props.placeholderText,
    saveSearchTerm: (term: string) => dispatch(setSearchTerm(term)),
    saveVehicleData: (data: any[]) => dispatch(setVehicleData(data)),
    saveFacetSelectors: (selectors: any[]) => dispatch(setFacetSelectors(selectors)),
    saveFacetSelectedKeys: (keys: any[]) => dispatch(setFacetSelectedKeys(keys)),
    savePageNumber: (page: number) => dispatch(setPageNumber(page)),
    saveOrderby: (order: string) => dispatch(setOrderBy(order)),
    saveSearchCount: (count: number) => dispatch(setSearchCount(count)),
    saveResetFacets: () => dispatch(resetFacets()),
  };

  const updateSearch = (event: React.ChangeEvent<HTMLInputElement>): void => {
    updateSearchTerm(event.target.value, searchBarProps);
  };

  const reset = (): void => {
    resetFilters(searchBarProps);
  };

  const disableResetButton = (): boolean => {
    return disableResetFiltersButton(searchBarProps);
  };

  return (
    <>
      <Paper
        component="form"
        style={{
          padding: '2px 4px',
          display: 'flex',
          height: 45,
        }}
      >
        <InputBase
          style={{
            marginLeft: 1,
            flex: 1,
          }}
          placeholder={generatePlaceholder(
            searchBarProps,
            windowWidth,
            MobileMaxWidth,
            dynamicPlaceholder
          )}
          onKeyDown={checkForEnter}
          onChange={updateSearch}
          value={reduxSearchTerm}
        />
        <Divider style={styles.divider} orientation="vertical" />
        <IconButton
          style={{ paddingLeft: 2, paddingRight: 2 }}
          color="primary"
          onClick={reset}
          disabled={disableResetButton()}
          aria-label="directions"
        >
          <RotateLeftIcon />
        </IconButton>
      </Paper>
    </>
  );
};

export default SearchBar;