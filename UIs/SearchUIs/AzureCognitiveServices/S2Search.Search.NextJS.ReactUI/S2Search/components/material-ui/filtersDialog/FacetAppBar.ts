import React from 'react';
// Use useSelector and useDispatch instead of connect for modern Redux
import { useSelector, useDispatch } from 'react-redux';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import { useTheme, Theme } from '@mui/material/styles';

// ACTION IMPORTS (Keep)
import { setDialogOpen } from '../../../store/slices/uiSlice';
import { resetFacets } from '../../../store/slices/facetSlice';
import { DefaultTheme } from '../../../common/Constants';

// 1. DEFINE PROP TYPES USING AN INTERFACE
// The props used here are the ones passed down from connect (if you kept it)
// or the ones you would normally access from the store/dispatch.
interface FacetAppBarProps {
  // These props come from the Redux store via mapStateToProps
  reduxNavBarColour: string;
  reduxPrimaryColour: string;
  reduxSecondaryColour: string;
  searchCount: number; // Added based on mapStateToProps
}

// 2. USE 'useSelector' and 'useDispatch' (Recommended Modern Redux)
const FacetAppBar: React.FC = () => {
  const dispatch = useDispatch();
  const theme: Theme = useTheme();

  // Access Redux state directly with useSelector
  const reduxNavBarColour = useSelector((state: any) => state.theme.navBarColour);
  // Add other needed state properties here if necessary
  // const searchCount = useSelector((state: any) => state.search.searchCount);

  const handleClose = () => {
    // Dispatch actions directly
    dispatch(setDialogOpen(false));
  };

  const handleReset = () => {
    // Dispatch actions directly
    dispatch(resetFacets());
  };

  return (
    <AppBar
      position="fixed"
      elevation={0}
      sx={{
        // Use the selected state
        background: reduxNavBarColour || DefaultTheme.navBarHexColour,
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

// 3. REMOVE Redux connect, mapStateToProps, mapDispatchToProps, and PropTypes

export default FacetAppBar;