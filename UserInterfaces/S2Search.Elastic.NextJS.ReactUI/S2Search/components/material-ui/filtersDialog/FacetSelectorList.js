import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import FacetSelector from "../filtersDialog/FacetSelector";
import Grid from "@mui/material/Grid";
import { makeStyles } from "@mui/styles";
import facetActions from "../../../redux/actions/facetActions";
import componentActions from "../../../redux/actions/componentActions";
import searchActions from "../../../redux/actions/searchActions";
import { StaticFacets } from "../../../common/Constants";
import {
  getDefaultFacetsWithSelections,
  isSelectFacetMenuAlreadySelected,
} from "../../../common/functions/FacetFunctions";

const useStyles = makeStyles((theme) => ({
  content: {
    flexGrow: 1,
    padding: theme.spacing(1),
  },
}));

const FacetSelectionList = (props) => {
  const classes = useStyles();
  const [facetState, setfacetState] = useState({});

  useEffect(() => {
    generateFacetSelectors(props.reduxSelectedFacet);
  }, [props.reduxFacetData]);

  const generateFacetSelectors = (facetKeyName) => {
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

    facetData = facetsToLoad.filter((x) => x.facetKey === facetKeyName);

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
  };

  const handleChecked = (theFacet) => {
    let theFacetUpdated = theFacet;
    const updatedFacetItems = [];

    theFacet.facetItems.map((facetItem) => {
      if (
        props.reduxFacetSelectors.some(
          (f) => f.facetDisplayText === facetItem.facetDisplayText
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
  };

  return (
    <main className={classes.content} style={{ paddingTop: "75px" }}>
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

const mapDispatchToProps = (dispatch) => {
  return {
    saveVehicleData: (vehicleData) =>
      dispatch(searchActions.saveVehicleData(vehicleData)),
    savePageFrom: (pageFrom) => dispatch(searchActions.savePageFrom(pageFrom)),
    saveFacetSelectors: (resetFacetArray) =>
      dispatch(facetActions.saveFacetSelectors(resetFacetArray)),
    saveSearchTerm: (searchTerm) =>
      dispatch(searchActions.saveSearchTerm(searchTerm)),
    saveOrderby: (orderBy) => dispatch(searchActions.saveOrderby(orderBy)),
    saveDialogOpen: (dialogOpen) =>
      dispatch(componentActions.saveDialogOpen(dialogOpen)),
    saveSelectedFacet: (facetName) =>
      dispatch(componentActions.saveSelectedFacet(facetName)),
  };
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxSearchCount: reduxState.searchReducer.searchCount,
    reduxVehicleData: reduxState.searchReducer.vehicleData,
    reduxOrderBy: reduxState.searchReducer.orderBy,
    reduxPageFrom: reduxState.searchReducer.pageFrom,
    reduxNetworkError: reduxState.searchReducer.networkError,
    reduxPreviousRequest: reduxState.searchReducer.previousRequest,

    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxDefaultFacetData: reduxState.facetReducer.defaultFacetData,
    reduxFacetData: reduxState.facetReducer.facetData,
    reduxDialogOpen: reduxState.componentReducer.dialogOpen,
    reduxSelectedFacet: reduxState.facetReducer.selectedFacet,
    reduxFacetSelectedKeys: reduxState.facetReducer.facetSelectedKeys,
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
