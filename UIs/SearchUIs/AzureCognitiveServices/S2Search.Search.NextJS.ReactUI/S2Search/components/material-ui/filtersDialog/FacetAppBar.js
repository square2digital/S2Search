import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Box from '@mui/material/Box';
import { setDialogOpen } from '../../../store/slices/uiSlice';
import Button from '@mui/material/Button';
import { resetFacets } from '../../../store/slices/facetSlice';
import { DefaultTheme } from '../../../common/Constants';
import { useTheme } from '@mui/material/styles';

const FacetAppBar = props => {
  const theme = useTheme();

  const handleClose = () => {
    props.saveDialogOpen(false);
  };

  const handleReset = () => {
    props.saveResetFacets();
  };

  return (
    <AppBar
      position="fixed"
      elevation={0}
      sx={{
        background: props.reduxNavBarColour || DefaultTheme.navBarHexColour,
        borderBottom: '1px solid rgba(255, 255, 255, 0.12)',
        zIndex: theme.zIndex.drawer + 1,
      }}
    >
      <Toolbar
        sx={{
          justifyContent: 'space-between',
          minHeight: '64px !important',
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <IconButton
            edge="start"
            color="inherit"
            onClick={handleClose}
            aria-label="close"
            sx={{
              '&:hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.1)',
              },
            }}
          >
            <CloseIcon />
          </IconButton>
          <Typography
            variant="h6"
            sx={{
              fontWeight: 600,
              color: 'white',
              letterSpacing: '0.5px',
            }}
          >
            Filters
          </Typography>
        </Box>

        <Box sx={{ display: 'flex', gap: 1 }}>
          <Button
            variant="outlined"
            onClick={handleReset}
            sx={{
              color: 'white',
              borderColor: 'rgba(255, 255, 255, 0.5)',
              textTransform: 'none',
              fontWeight: 500,
              px: 3,
              '&:hover': {
                borderColor: 'white',
                backgroundColor: 'rgba(255, 255, 255, 0.1)',
              },
            }}
          >
            Reset All
          </Button>
          <Button
            variant="outlined"
            onClick={handleClose}
            sx={{
              color: 'white',
              borderColor: 'rgba(255, 255, 255, 0.5)',
              textTransform: 'none',
              fontWeight: 500,
              px: 3,
              '&:hover': {
                borderColor: 'white',
                backgroundColor: 'rgba(255, 255, 255, 0.1)',
              },
            }}
          >
            Show Results
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
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
    saveResetFacets: () => dispatch(resetFacets()),
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
