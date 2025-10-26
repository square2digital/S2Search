import React, { useState, useEffect } from 'react';
import InputBase from '@mui/material/InputBase';
import Autocomplete from '@mui/material/Autocomplete';
import Popper from '@mui/material/Popper';
import parse from 'autosuggest-highlight/parse';
import match from 'autosuggest-highlight/match';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import Divider from '@mui/material/Divider';
import RotateLeftIcon from '@mui/icons-material/RotateLeft';
import { MobileMaxWidth } from '../../../../common/Constants';
import { useWindowSize } from '../../../../hooks/useWindowSize';
import useDynamicPlaceholder from './DynamicPlaceholder';
import {
  checkForEnter,
  generatePlaceholder,
  resetFilters,
  disableResetFiltersButton,
  updateSearchTerm,
} from './searchBarSharedFunctions';
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

interface AutoSuggestProps {
  placeholderText?: string;
}

interface SuggestionPart {
  highlight: boolean;
  text: string;
}

export const AutoSuggest: React.FC<AutoSuggestProps> = (props) => {
  const [options, setOptions] = useState<string[]>([]);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  
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

  useEffect(() => {
    if (reduxSearchTerm) {
      if (reduxSearchTerm.length >= 2) {
        fetch(
          `/api/autoSuggest?searchTerm=${encodeURIComponent(reduxSearchTerm)}`
        )
          .then(response => response.json())
          .then(function (suggestions: string[]) {
            if (suggestions && Array.isArray(suggestions)) {
              suggestions.shift();
              setOptions(suggestions);
            } else {
              setOptions([]);
            }
          })
          .catch(error => {
            console.error('AutoSuggest error:', error);
            setOptions([]);
          });
      } else {
        setOptions([]);
      }
    }

    setShowDropdown(options.length > 0);
  }, [reduxSearchTerm, options.length]);

  const updateSearch = (_event: React.SyntheticEvent, value: string | null): void => {
    updateSearchTerm(value || '', searchBarProps);
  };

  const reset = (): void => {
    resetFilters(searchBarProps);
  };

  const saveOnChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    updateSearchTerm(event.target.value, searchBarProps);
  };

  const disableResetButton = (): boolean => {
    return disableResetFiltersButton(searchBarProps);
  };

  const PopperOverride = function (popperProps: any) {
    const styles = {
      popper: {
        width: 'fit-content',
      },
    };

    if (MobileMaxWidth > windowWidth) {
      return (
        <Popper {...popperProps} style={styles.popper} placement="bottom-start" />
      );
    } else {
      return <Popper {...popperProps} />;
    }
  };

  return (
    <Autocomplete
      PopperComponent={PopperOverride}
      id="auto-complete-suggestions"
      freeSolo
      disableClearable
      forcePopupIcon={showDropdown}
      options={options}
      onInputChange={updateSearch}
      value={reduxSearchTerm}
      renderOption={(renderProps: any, option: string) => {
        const matches = match(option, reduxSearchTerm);
        const parts: SuggestionPart[] = parse(option, matches);

        return (
          <div {...renderProps}>
            <span>
              {parts.map((part, index) => (
                <span
                  key={index}
                  style={{
                    fontWeight: !part.highlight ? 700 : 400,
                  }}
                >
                  {part.text.replace(/ /g, '\u00a0')}
                </span>
              ))}
            </span>
          </div>
        );
      }}
      renderInput={(params: any) => {
        const { ...rest } = params;
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
                {...params.InputProps}
                {...rest}
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
                variant="outlined"
                onChange={saveOnChange}
                onKeyPress={checkForEnter}
                value={reduxSearchTerm}
              />
              <Divider
                style={{
                  height: 28,
                  margin: 4,
                }}
                orientation="vertical"
              />
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
      }}
    />
  );
};

export default AutoSuggest;