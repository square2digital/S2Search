import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import componentActions from '../../../redux/actions/componentActions';
import Button from '@mui/material/Button';
import facetActions from '../../../redux/actions/facetActions';
import { DefaultTheme } from '../../../common/Constants';
import { createTheme, ThemeProvider } from '@mui/material/styles';

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
  const handleClose = () => {
    props.saveDialogOpen(false);
  };

  const theme = createTheme({
    palette: {
      primary: {
        main:
          props.reduxPrimaryColour ||
          DefaultTheme.primaryHexColour ||
          '#616161',
      },
      secondary: {
        main:
          props.reduxSecondaryColour ||
          DefaultTheme.secondaryHexColour ||
          '#303f9f',
      },
    },
  });

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
          <ThemeProvider theme={theme}>
            <Button
              size="small"
              variant="contained"
              color="secondary"
              onClick={handleClose}
            >
              Show Results
            </Button>
          </ThemeProvider>
        </Toolbar>
      </AppBar>
    </div>
  );
};

const mapStateToProps = reduxState => {
  return {
    searchCount: reduxState.searchReducer.searchCount,
    reduxNavBarColour: reduxState.themeReducer.navBarColour,
    reduxPrimaryColour: reduxState.themeReducer.primaryColour,
    reduxSecondaryColour: reduxState.themeReducer.secondaryColour,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveDialogOpen: dialogOpen =>
      dispatch(componentActions.saveDialogOpen(dialogOpen)),
    saveResetFacets: resetFacets =>
      dispatch(facetActions.saveResetFacets(resetFacets)),
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
