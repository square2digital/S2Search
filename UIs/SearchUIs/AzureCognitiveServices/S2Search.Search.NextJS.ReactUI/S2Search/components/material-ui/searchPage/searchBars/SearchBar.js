import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import {
  setSearchTerm,
  setVehicleData,
  setPageNumber,
} from '../../../../store/slices/searchSlice';
import {
  resetFacets,
  setFacetSelectors,
} from '../../../../store/slices/facetSlice';
import { DefaultLoadSpeed } from '../../../../common/Constants';
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

// Inline styles object (converted from makeStyles)
const styles = {
  divider: {
    height: 28,
    margin: 4,
  },
};

export const SearchBar = props => {
  const [windowWidth, setwindowWidth] = useState(window.innerWidth);

  useEffect(() => {
    const updateWindowDimensions = () => {
      setwindowWidth(window.innerWidth);
    };

    window.addEventListener('resize', updateWindowDimensions);

    return () => window.removeEventListener('resize', updateWindowDimensions);
  }, []);

  const updateSearch = event => {
    updateSearchTerm(event.target.value, props);
  };

  const reset = () => {
    resetFilters(props);
  };

  const disableResetButton = () => {
    return disableResetFiltersButton(props);
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
          placeholder={generatePlaceholder(props, windowWidth, MobileMaxWidth)}
          variant="outlined"
          onKeyPress={checkForEnter}
          onChange={updateSearch}
          value={props.reduxSearchTerm}
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

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxConfigPlaceholders: reduxState.config.placeholderArray,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveResetFacets: resetFacets => dispatch(resetFacets(resetFacets)),
    saveVehicleData: () => dispatch(setVehicleData([])),
    savePageNumber: () => dispatch(setPageNumber(0)),
    saveFacetSelectors: () => dispatch(setFacetSelectors([])),
  };
};

SearchBar.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxSearchTerm: PropTypes.string,
  saveSearchTerm: PropTypes.func,
  saveResetFacets: PropTypes.func,
  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  placeholderText: PropTypes.string,
  reduxConfigPlaceholders: PropTypes.array,
};

export default connect(mapStateToProps, mapDispatchToProps)(SearchBar);
