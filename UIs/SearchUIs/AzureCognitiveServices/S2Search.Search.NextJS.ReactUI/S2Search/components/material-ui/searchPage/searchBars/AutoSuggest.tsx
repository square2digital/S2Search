import RotateLeftIcon from '@mui/icons-material/RotateLeft';
import Autocomplete from '@mui/material/Autocomplete';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import InputBase from '@mui/material/InputBase';
import Popper from '@mui/material/Popper';
import match from 'autosuggest-highlight/match';
import parse from 'autosuggest-highlight/parse';
import React, { useEffect, useState } from 'react';
import { MobileMaxWidth } from '../../../../common/Constants';
import { useWindowSize } from '../../../../hooks/useWindowSize';
import { useAppDispatch, useAppSelector } from '../../../../store/hooks';
import {
  resetFacets,
  setFacetSelectedKeys,
  setFacetSelectors,
} from '../../../../store/slices/facetSlice';
import {
  setOrderBy,
  setPageNumber,
  setSearchCount,
  setSearchTerm,
  setVehicleData,
} from '../../../../store/slices/searchSlice';
import useDynamicPlaceholder from './DynamicPlaceholder';
import {
  checkForEnter,
  disableResetFiltersButton,
  generatePlaceholder,
  resetFilters,
  updateSearchTerm,
} from './searchBarSharedFunctions';

interface AutoSuggestProps {
  placeholderText?: string;
}

interface SuggestionPart {
  highlight: boolean;
  text: string;
}

export const AutoSuggest: React.FC<AutoSuggestProps> = props => {
  const [options, setOptions] = useState<string[]>([]);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);

  const dispatch = useAppDispatch();
  const reduxSearchTerm = useAppSelector(state => state.search.searchTerm);
  const reduxFacetSelectors = useAppSelector(
    state => state.facet.facetSelectors
  );
  const reduxConfigPlaceholders = useAppSelector(
    state => state.config.placeholderArray
  );

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
    saveFacetSelectors: (selectors: any[]) =>
      dispatch(setFacetSelectors(selectors)),
    saveFacetSelectedKeys: (keys: any[]) =>
      dispatch(setFacetSelectedKeys(keys)),
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

  const updateSearch = (
    _event: React.SyntheticEvent,
    value: string | null
  ): void => {
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
        <Popper
          {...popperProps}
          style={styles.popper}
          placement="bottom-start"
        />
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
            <form
              style={{
                padding: '2px 4px',
                display: 'flex',
                height: 45,
                backgroundColor: 'white',
                borderRadius: '4px',
                border: '1px solid rgba(0, 0, 0, 0.23)',
                boxShadow: 'none',
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
            </form>
          </>
        );
      }}
    />
  );
};

export default AutoSuggest;
