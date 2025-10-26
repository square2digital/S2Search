import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
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

export const AutoSuggest = props => {
  const [options, setOptions] = useState([]);
  const [showDropdown, setShowDropdown] = useState(false);
  const { width: windowWidth } = useWindowSize();
  const dynamicPlaceholder = useDynamicPlaceholder(
    props.reduxConfigPlaceholders
  );

  useEffect(() => {
    if (props.reduxSearchTerm) {
      if (props.reduxSearchTerm.length >= 2) {
        fetch(
          `/api/autoSuggest?searchTerm=${encodeURIComponent(props.reduxSearchTerm)}`
        )
          .then(response => response.json())
          .then(function (suggestions) {
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
  }, [props.reduxSearchTerm, options.length]);

  const updateSearch = (event, value) => {
    updateSearchTerm(value, props);
  };

  const reset = () => {
    resetFilters(props);
  };

  const saveOnChange = event => {
    updateSearchTerm(event.target.value, props);
  };

  const disableResetButton = () => {
    return disableResetFiltersButton(props);
  };

  const PopperOverride = function (props) {
    const styles = () => ({
      popper: {
        width: 'fit-content',
      },
    });

    if (MobileMaxWidth > windowWidth) {
      return (
        <Popper {...props} style={styles.popper} placement="bottom-start" />
      );
    } else {
      return <Popper {...props} />;
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
      value={props.reduxSearchTerm}
      renderOption={(renderProps, option) => {
        const matches = match(option, props.reduxSearchTerm);
        const parts = parse(option, matches);

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
      renderInput={params => {
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
                  props,
                  windowWidth,
                  MobileMaxWidth,
                  dynamicPlaceholder
                )}
                variant="outlined"
                onChange={saveOnChange}
                onKeyPress={checkForEnter}
                value={props.reduxSearchTerm}
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

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxConfigPlaceholders: reduxState.config.placeholderArray,
    reduxCancellationToken: reduxState.ui.cancellationToken,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveResetFacets: () => dispatch(resetFacets()),
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: facetSelectors =>
      dispatch(setFacetSelectors(facetSelectors)),
    saveFacetSelectedKeys: facetSelectedKeys =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveOrderby: orderBy => dispatch(setOrderBy(orderBy)),
    saveSearchCount: searchCount => dispatch(setSearchCount(searchCount)),
    // Note: saveCancellationToken might need to be implemented in uiSlice if still needed
    // saveCancellationToken: enable => dispatch(setCancellationToken(enable)),
  };
};

AutoSuggest.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxSearchTerm: PropTypes.string,
  saveSearchTerm: PropTypes.func,
  saveResetFacets: PropTypes.func,
  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  placeholderText: PropTypes.string,
  reduxConfigPlaceholders: PropTypes.array,
  reduxCancellationToken: PropTypes.bool,
  saveCancellationToken: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(AutoSuggest);
