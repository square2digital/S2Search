import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { setDialogOpen } from '../../../store/slices/uiSlice';
import Button from '@mui/material/Button';
import { resetFacets } from '../../../store/slices/facetSlice';
import { DefaultTheme } from '../../../common/Constants';
import { useTheme } from '@mui/material/styles';

const styles = {
  root: {
    display: 'flex',
  },
  menuButton: theme => ({
    marginRight: theme.spacing(1),
    [theme.breakpoints.up('sm')]: {},
  }),
  // necessary for content to be below app bar
  toolbar: theme => theme.mixins.toolbar,
};

const FacetAppBar = props => {
  const theme = useTheme();

  const handleClose = () => {
    props.saveDialogOpen(false);
  };

  return (
    <div style={styles.root}>
      <AppBar
        style={{
          background: props.reduxNavBarColour
            ? props.reduxNavBarColour
            : DefaultTheme.navBarHexColour,
        }}
      >
        <Toolbar>
          <Typography
            variant="subtitle1"
            onClick={handleClose}
            style={{ flex: 1 }}
          ></Typography>
          <Button
            size="small"
            variant="contained"
            color="secondary"
            onClick={handleClose}
          >
            Show Results
          </Button>
        </Toolbar>
      </AppBar>
    </div>
  );
};

const mapStateToProps = reduxState => {
  return {
    searchCount: reduxState.search.searchCount,
    reduxNavBarColour: reduxState.theme.navBarColour,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveDialogOpen: dialogOpen => dispatch(setDialogOpen(dialogOpen)),
    saveResetFacets: resetFacetsFlag => dispatch(resetFacets()),
  };
};

FacetAppBar.propTypes = {
  searchCount: PropTypes.number,
  vehicleData: PropTypes.array,
  saveVehicleData: PropTypes.func,
  saveDialogOpen: PropTypes.func,
  saveResetFacets: PropTypes.func,
  reduxNavBarColour: PropTypes.string,
  reduxPrimaryColour: PropTypes.string,
  reduxSecondaryColour: PropTypes.string,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetAppBar);
