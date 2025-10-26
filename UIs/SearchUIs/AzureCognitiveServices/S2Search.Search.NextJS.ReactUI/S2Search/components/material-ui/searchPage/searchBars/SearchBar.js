import React, { useEffect } from 'react';
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
import { useWindowSize } from '../../../../hooks/useWindowSize';
import useDynamicPlaceholder from './DynamicPlaceholder';

// Inline styles object (converted from makeStyles)
const styles = {
  divider: {
    height: 28,
    margin: 4,
  },
};

export const SearchBar = props => {
  const { width: windowWidth } = useWindowSize();
  const dynamicPlaceholder = useDynamicPlaceholder(
    props.reduxConfigPlaceholders
  );

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
          placeholder={generatePlaceholder(
            props,
            windowWidth,
            MobileMaxWidth,
            dynamicPlaceholder
          )}
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
    saveResetFacets: () => dispatch(resetFacets()),
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: facetSelectors =>
      dispatch(setFacetSelectors(facetSelectors)),
    saveFacetSelectedKeys: facetSelectedKeys =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveOrderby: orderBy => dispatch(setOrderBy(orderBy)),
    saveSearchCount: searchCount => dispatch(setSearchCount(searchCount)),
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
  saveFacetSelectedKeys: PropTypes.func,
  saveOrderby: PropTypes.func,
  saveSearchCount: PropTypes.func,
  placeholderText: PropTypes.string,
  reduxConfigPlaceholders: PropTypes.array,
};

export default connect(mapStateToProps, mapDispatchToProps)(SearchBar);
