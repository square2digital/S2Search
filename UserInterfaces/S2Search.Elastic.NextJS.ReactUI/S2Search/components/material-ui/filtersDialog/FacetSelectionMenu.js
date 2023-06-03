import React from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Divider from "@mui/material/Divider";
import Drawer from "@mui/material/Drawer";
import componentActions from "../../../redux/actions/componentActions";
import RotateLeftIcon from "@mui/icons-material/RotateLeft";
import facetActions from "../../../redux/actions/facetActions";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import { makeStyles } from "@mui/styles";
import { setSelectedFacetButton } from "../../../common/functions/FacetFunctions";
import searchActions from "../../../redux/actions/searchActions";
import Typography from "@mui/material/Typography";
import { GenerateUniqueID } from "../../../common/functions/SharedFunctions";

const drawerWidth_xs = 143;
const drawerWidth_sm = 180;

const useStyles = makeStyles((theme) => ({
  drawer: {
    [theme.breakpoints.up("xs")]: {
      width: drawerWidth_xs,
    },
    [theme.breakpoints.up("sm")]: {
      width: drawerWidth_sm,
    },
    flexShrink: 0,
  },
  // necessary for content to be below app bar
  toolbar: theme.mixins.toolbar,
  drawerPaper: {
    [theme.breakpoints.up("xs")]: {
      width: drawerWidth_xs,
    },
    [theme.breakpoints.up("sm")]: {
      width: drawerWidth_sm,
    },
    zIndex: 0,
  },
}));

const FacetSelectionMenu = (props) => {
  const classes = useStyles();

  const resetFilters = () => {
    props.saveResetFacets(true);
  };

  const facetMenuClick = (facetKey) => {
    props.saveSelectedFacet(facetKey);

    let arr = [...props.reduxFacetSelectedKeys];
    arr.push(facetKey);
    props.saveFacetSelectedKeys(arr);
  };

  return (
    <nav className={classes.drawer}>
      <Drawer
        classes={{
          paper: classes.drawerPaper,
        }}
        variant="permanent"
        open>
        <div>
          <div className={classes.toolbar} />
          <Divider />
          <List>
            {props.defaultFacetData.map((facet) => (
              <React.Fragment key={GenerateUniqueID()}>
                <ListItem
                  button
                  onClick={() => {
                    facetMenuClick(facet.facetKey);
                  }}>
                  <ListItemText
                    disableTypography
                    primary={
                      <Typography variant="body2">
                        {facet.facetKeyDisplayName}
                      </Typography>
                    }
                  />
                  {setSelectedFacetButton(
                    facet.facetKeyDisplayName,
                    props.reduxFacetSelectors
                  )}
                </ListItem>
                <Divider />
              </React.Fragment>
            ))}

            <ListItem button key={GenerateUniqueID()} onClick={resetFilters}>
              <ListItemText
                primary={
                  <Typography variant="body2" style={{ fontWeight: "bold" }}>
                    Reset
                  </Typography>
                }
              />
              <ListItemIcon>
                <RotateLeftIcon />
              </ListItemIcon>
            </ListItem>
          </List>
        </div>
      </Drawer>
    </nav>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    searchCount: reduxState.searchReducer.searchCount,
    reduxResultsCount: reduxState.searchReducer.searchCount,
    vehicleData: reduxState.searchReducer.vehicleData,

    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facetReducer.facetSelectedKeys,
    defaultFacetData: reduxState.facetReducer.defaultFacetData,
    facetData: reduxState.facetReducer.facetData,
    reduxSelectedFacet: reduxState.facetReducer.selectedFacet,
    reduxLoading: reduxState.componentReducer.loading,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveVehicleData: (vehicleData) =>
      dispatch(searchActions.saveVehicleData(vehicleData)),
    savePageFrom: (pageFrom) => dispatch(searchActions.savePageFrom(pageFrom)),
    saveFacetSelectors: (resetFacetArray) =>
      dispatch(facetActions.saveFacetSelectors(resetFacetArray)),
    saveFacetSelectedKeys: (facetSelectedKeys) =>
      dispatch(facetActions.saveFacetSelectedKeys(facetSelectedKeys)),
    saveSearchTerm: (searchTerm) =>
      dispatch(searchActions.saveSearchTerm(searchTerm)),
    saveOrderby: (orderBy) => dispatch(searchActions.saveOrderby(orderBy)),
    saveDialogOpen: (dialogOpen) =>
      dispatch(componentActions.saveDialogOpen(dialogOpen)),
    saveSelectedFacet: (facet) =>
      dispatch(componentActions.saveSelectedFacet(facet)),
    saveResetFacets: (resetFacets) =>
      dispatch(facetActions.saveResetFacets(resetFacets)),
  };
};

FacetSelectionMenu.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxResultsCount: PropTypes.number,
  defaultFacetData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,
  reduxFacetSelectedKeys: PropTypes.array,

  saveVehicleData: PropTypes.func,
  savePageFrom: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  saveFacetSelectedKeys: PropTypes.func,
  saveSearchTerm: PropTypes.func,
  saveOrderby: PropTypes.func,
  saveDialogOpen: PropTypes.func,
  saveSelectedFacet: PropTypes.func,
  saveResetFacets: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetSelectionMenu);
