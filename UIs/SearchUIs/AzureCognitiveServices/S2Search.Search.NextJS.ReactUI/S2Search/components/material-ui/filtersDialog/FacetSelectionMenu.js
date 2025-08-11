import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Divider from '@mui/material/Divider';
import Drawer from '@mui/material/Drawer';
import RotateLeftIcon from '@mui/icons-material/RotateLeft';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import { setSelectedFacetButton } from '../../../common/functions/FacetFunctions';
import Typography from '@mui/material/Typography';
import { GenerateUniqueID } from '../../../common/functions/SharedFunctions';
import {
  setVehicleData,
  setPageNumber,
  setSearchTerm,
  setOrderBy,
} from '../../../store/slices/searchSlice';
import {
  setFacetSelectors,
  setFacetSelectedKeys,
  resetFacets,
  setSelectedFacet,
} from '../../../store/slices/facetSlice';
import { setDialogOpen } from '../../../store/slices/uiSlice';

const drawerWidth_xs = 143;
const drawerWidth_sm = 180;

const FacetSelectionMenu = props => {
  const resetFilters = () => {
    props.saveResetFacets(true);
  };

  const facetMenuClick = facetKey => {
    props.saveSelectedFacet(facetKey);

    let arr = [...props.reduxFacetSelectedKeys];
    arr.push(facetKey);
    props.saveFacetSelectedKeys(arr);
  };

  return (
    <nav
      sx={{
        width: {
          xs: drawerWidth_xs,
          sm: drawerWidth_sm,
        },
        flexShrink: 0,
      }}
    >
      <Drawer
        sx={{
          '& .MuiDrawer-paper': {
            width: {
              xs: drawerWidth_xs,
              sm: drawerWidth_sm,
            },
            zIndex: 0,
          },
        }}
        variant="permanent"
        open
      >
        <div>
          <div sx={theme => theme.mixins.toolbar} />
          <Divider />
          <List>
            {props.defaultFacetData.map(facet => (
              <React.Fragment key={GenerateUniqueID()}>
                <ListItem
                  button
                  onClick={() => {
                    facetMenuClick(facet.facetKey);
                  }}
                >
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
                  <Typography variant="body2" style={{ fontWeight: 'bold' }}>
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

const mapStateToProps = reduxState => {
  return {
    searchCount: reduxState.search.searchCount,
    reduxResultsCount: reduxState.search.searchCount,
    vehicleData: reduxState.search.vehicleData,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
    defaultFacetData: reduxState.facet.defaultFacetData,
    facetData: reduxState.facet.facetData,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxLoading: reduxState.ui.isLoading,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveVehicleData: vehicleData => dispatch(setVehicleData(vehicleData)),
    savePageNumber: pageNumber => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: resetFacetArray =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveFacetSelectedKeys: facetSelectedKeys =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: orderBy => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: dialogOpen => dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: facet => dispatch(setSelectedFacet(facet)),
    saveResetFacets: resetFacetsFlag => dispatch(resetFacets()),
  };
};

FacetSelectionMenu.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxResultsCount: PropTypes.number,
  defaultFacetData: PropTypes.array,
  reduxSelectedFacet: PropTypes.string,
  reduxFacetSelectedKeys: PropTypes.array,

  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  saveFacetSelectedKeys: PropTypes.func,
  saveSearchTerm: PropTypes.func,
  saveOrderby: PropTypes.func,
  saveDialogOpen: PropTypes.func,
  saveSelectedFacet: PropTypes.func,
  saveResetFacets: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetSelectionMenu);
