import React, { useState, useEffect, useCallback } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import FacetSelector from '../filtersDialog/FacetSelector';
import Grid from '@mui/material/Grid';
import {
  setVehicleData,
  setPageNumber,
  setSearchTerm,
  setOrderBy,
} from '../../../store/slices/searchSlice';
import {
  setFacetSelectors,
  setSelectedFacet,
} from '../../../store/slices/facetSlice';
import { setDialogOpen } from '../../../store/slices/uiSlice';
import { StaticFacets } from '../../../common/Constants';
import {
  getDefaultFacetsWithSelections,
  isSelectFacetMenuAlreadySelected,
} from '../../../common/functions/FacetFunctions';

const FacetSelectionList = props => {
  const [facetState, setfacetState] = useState({});

  const handleChecked = useCallback((theFacet) => {
    let theFacetUpdated = theFacet;
    const updatedFacetItems = [];

    theFacet.facetItems.map(facetItem => {
      if (
        props.reduxFacetSelectors.some(
          f => f.facetDisplayText === facetItem.facetDisplayText
        )
      ) {
        updatedFacetItems.push({ ...facetItem, selected: true });
      } else {
        updatedFacetItems.push({ ...facetItem, selected: false });
      }
    });

    if (updatedFacetItems.length > 0) {
      theFacetUpdated = { ...theFacetUpdated, facetItems: updatedFacetItems };
    }

    return theFacetUpdated;
  }, [props.reduxFacetSelectors]);

  const generateFacetSelectors = useCallback((facetKeyName) => {
    let theFacet = {};
    let currentFacet = {};
    let facetData = [];
    let selectedFacetData = [];

    // ****************
    // facets To Load - default or from Search Results?
    // ****************
    // on the first load or if reduxFacetSelectors is empty we need to load the defaultFacets.
    // when a facet is selected, from that point the facets returned from search will be displayed

    let facetsToLoad = [];
    if (props.reduxSearchTerm) {
      facetsToLoad = props.reduxFacetData;
    } else if (
      props.reduxFacetSelectors.length === 0 &&
      StaticFacets.includes(facetKeyName)
    ) {
      facetsToLoad = props.reduxDefaultFacetData;
    } else {
      if (
        isSelectFacetMenuAlreadySelected(
          props.reduxFacetSelectors,
          facetKeyName
        )
      ) {
        facetsToLoad = getDefaultFacetsWithSelections(
          facetKeyName,
          props.reduxDefaultFacetData,
          props.reduxFacetSelectors
        );
      } else {
        facetsToLoad = props.reduxFacetData;
      }
    }

    facetData = facetsToLoad.filter(x => x.facetKey === facetKeyName);

    currentFacet = facetData[0];

    theFacet = { ...currentFacet, enabled: false };

    // check if the facet is enabled
    if (props.reduxFacetData.length > 0) {
      if (selectedFacetData.length > 0) {
        theFacet = { ...currentFacet, enabled: selectedFacetData[0].enabled };
      }
    } else {
      theFacet = { ...currentFacet, enabled: false };
    }

    if (props.reduxFacetSelectors.length > 0) {
      let theFacetUpdated = handleChecked(theFacet);
      setfacetState(theFacetUpdated);
    } else {
      setfacetState(theFacet);
    }
  }, [
    props.reduxSearchTerm,
    props.reduxFacetData,
    props.reduxFacetSelectors,
    props.reduxDefaultFacetData,
    handleChecked
  ]);

  useEffect(() => {
    if (props.reduxSelectedFacet) {
      generateFacetSelectors(props.reduxSelectedFacet);
    }
  }, [props.reduxSelectedFacet, generateFacetSelectors]);

  return (
    <main
      sx={{
        flexGrow: 1,
        padding: 1,
      }}
      style={{ paddingTop: '75px' }}
    >
      <Grid container>
        {facetState.facetItems !== undefined ? (
          facetState.facetItems.map((facetSelectorItem, index) => {
            return (
              <FacetSelector
                key={`${index}-${facetSelectorItem.facetDisplayText}`}
                facet={facetSelectorItem}
                selectedFacet={facetState.facetKey}
                isChecked={facetSelectorItem.selected}
              />
            );
          })
        ) : (
          <>Loading</>
        )}
      </Grid>
    </main>
  );
};

const mapDispatchToProps = dispatch => {
  return {
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: resetFacetArray =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: orderBy => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: dialogOpen => dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: facetName => dispatch(setSelectedFacet(facetName)),
  };
};

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxSearchCount: reduxState.search.searchCount,
    reduxVehicleData: reduxState.search.vehicleData,
    reduxOrderBy: reduxState.search.orderBy,
    reduxPageNumber: reduxState.search.pageNumber,
    reduxNetworkError: reduxState.search.networkError,
    reduxPreviousRequest: reduxState.search.previousRequest,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxDefaultFacetData: reduxState.facet.defaultFacetData,
    reduxFacetData: reduxState.facet.facetData,
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
  };
};

FacetSelectionList.propTypes = {
  reduxSearchTerm: PropTypes.string,
  reduxFacetSelectors: PropTypes.array,
  reduxDefaultFacetData: PropTypes.array,
  reduxFacetData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,
  reduxFacetSelectedKeys: PropTypes.array,
  searchData: PropTypes.array,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetSelectionList);
