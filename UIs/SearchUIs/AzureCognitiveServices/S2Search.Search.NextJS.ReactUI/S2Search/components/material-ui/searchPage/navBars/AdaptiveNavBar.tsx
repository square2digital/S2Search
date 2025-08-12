import * as React from 'react';
import {
  Box,
  Button,
  Badge,
  Typography,
  Tooltip,
} from '@mui/material';
import DriveEtaIcon from '@mui/icons-material/DriveEta';
import { useAppSelector, useAppDispatch } from '../../../../store/hooks';
import {
  selectVehicleData,
  selectTotalDocumentCount,
} from '../../../../store/selectors/searchSelectors';
import {
  selectLogoURL,
  selectNavBarColour,
} from '../../../../store/selectors/themeSelectors';
import {
  selectEnableAutoComplete,
  selectHideIconVehicleCounts,
} from '../../../../store/selectors';
import {
  setResetFacets,
  setFacetSelectors,
} from '../../../../store/slices/facetSlice';
import { setDialogOpen } from '../../../../store/slices/uiSlice';
import {
  setSearchTerm,
  setVehicleData,
  setPageNumber,
} from '../../../../store/slices/searchSlice';
import SearchBar from '../searchBars/SearchBar';
import AutoSuggest from '../searchBars/AutoSuggest';
import OrderByDialog from '../orderBy/OrderByDialog';
import { DefaultTheme, MobileMaxWidth } from '../../../../common/Constants';
import { useWindowSize } from '../../../../hooks/useWindowSize';

// Types
interface AdaptiveNavBarProps {
  autoCompleteSearchBar?: boolean;
}

const s2logoWidth = 57;

// Inline styles object (converted from makeStyles)
const styles = {
  root: {
    flexGrow: 1,
  },
  paper: {
    margin: 'auto',
  },
  firstBadgeContainer: {
    marginLeft: 2,
  },
  badgeContainer: {
    marginLeft: 2,
  },
  topMargin: {
    marginTop: 18,
  },
  resultsText: {
    fontSize: '0.875rem',
  },
};

const AdaptiveNavBar: React.FC<AdaptiveNavBarProps> = ({
  autoCompleteSearchBar = false,
}) => {
  const { width: windowWidth } = useWindowSize();
  const dispatch = useAppDispatch();

  // Redux selectors
  const vehicleData = useAppSelector(selectVehicleData);
  const totalDocumentCount = useAppSelector(selectTotalDocumentCount);
  const logoURL = useAppSelector(selectLogoURL);
  const navBarColour = useAppSelector(selectNavBarColour);
  const enableAutoComplete = useAppSelector(selectEnableAutoComplete);
  const hideIconVehicleCounts = useAppSelector(selectHideIconVehicleCounts);

  const resetFilters = () => {
    dispatch(setResetFacets(true));
    dispatch(setVehicleData([]));
    dispatch(setPageNumber(0));
    dispatch(setFacetSelectors([]));
    dispatch(setSearchTerm(''));
  };

  const openDialog = () => {
    dispatch(setDialogOpen(true));
  };

  const desktopNavBar = () => {
    return (
      <Box
        sx={{
          position: 'fixed',
          top: 0,
          left: 0,
          right: 0,
          width: '100%',
          backgroundColor: navBarColour || DefaultTheme.navBarHexColour || '#1976d2',
          zIndex: 1000,
          minHeight: '64px',
          display: 'flex',
          alignItems: 'center',
          px: 2,
          py: 1,
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', width: '100%' }}>
          {/* Logo */}
          <Box sx={{ mr: 2 }}>
            <Box
              component="img"
              onClick={resetFilters}
              src={logoURL || DefaultTheme.logoURL}
              width={s2logoWidth}
              sx={{ cursor: 'pointer', display: 'block' }}
              alt="Logo"
            />
          </Box>

          {/* Vehicle Count Badges */}
          {!hideIconVehicleCounts && (
            <>
              <Box sx={{ mr: 2 }}>
                <Tooltip
                  placement="bottom"
                  title="Number of vehicles in this search"
                  aria-label="Number of vehicles in this search"
                >
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Typography variant="body2" sx={{ fontSize: '0.875rem' }}>
                      <Badge
                        anchorOrigin={{
                          vertical: 'top',
                          horizontal: 'left',
                        }}
                        badgeContent={vehicleData.length}
                        color="secondary"
                      >
                        <DriveEtaIcon />
                      </Badge>
                    </Typography>
                  </Box>
                </Tooltip>
              </Box>
              <Box sx={{ mr: 2 }}>
                <Tooltip
                  placement="bottom"
                  title="Total Number of Vehicles"
                  aria-label="Total Number of Vehicles"
                >
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Typography variant="body2" sx={{ fontSize: '0.875rem' }}>
                      <Badge
                        anchorOrigin={{
                          vertical: 'top',
                          horizontal: 'left',
                        }}
                        badgeContent={totalDocumentCount}
                        max={99999}
                        color="secondary"
                      >
                        <DriveEtaIcon />
                      </Badge>
                    </Typography>
                  </Box>
                </Tooltip>
              </Box>
            </>
          )}

          {/* Search Bar */}
          <Box sx={{ flexGrow: 1, mx: 2 }}>
            {autoCompleteSearchBar || enableAutoComplete ? (
              <AutoSuggest placeholderText="Start typing to search..." />
            ) : (
              <SearchBar placeholderText="Start typing to search..." />
            )}
          </Box>

          {/* Action Buttons */}
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <OrderByDialog />
            <Button
              size="small"
              onClick={openDialog}
              color="secondary"
              variant="contained"
            >
              Filter
            </Button>
          </Box>
        </Box>
      </Box>
    );
  };

  const mobileNavBar = () => {
    return (
      <Box
        sx={{
          position: 'fixed',
          top: 0,
          left: 0,
          right: 0,
          width: '100%',
          backgroundColor: navBarColour || DefaultTheme.navBarHexColour || '#1976d2',
          zIndex: 1000,
          minHeight: '64px',
          px: 1,
          py: 1,
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', width: '100%', gap: 1 }}>
          {/* Logo */}
          <Box>
            <Box
              component="img"
              onClick={resetFilters}
              src={logoURL || DefaultTheme.logoURL}
              width={s2logoWidth}
              sx={{ cursor: 'pointer', display: 'block' }}
              alt="Logo"
            />
          </Box>

          {/* Vehicle Count Badges - Mobile */}
          {!hideIconVehicleCounts && (
            <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', mr: 1 }}>
              <Tooltip
                placement="bottom"
                title="Number of vehicles in this search"
                aria-label="Number of vehicles in this search"
              >
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 0.5 }}>
                  <Typography variant="body2" sx={{ fontSize: '0.75rem' }}>
                    <Badge
                      anchorOrigin={{
                        vertical: 'top',
                        horizontal: 'right',
                      }}
                      badgeContent={vehicleData.length}
                      color="secondary"
                    >
                      <DriveEtaIcon fontSize="small" />
                    </Badge>
                  </Typography>
                </Box>
              </Tooltip>
              <Tooltip
                placement="bottom"
                title="Total Number of Vehicles"
                aria-label="Total Number of Vehicles"
              >
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                  <Typography variant="body2" sx={{ fontSize: '0.75rem' }}>
                    <Badge
                      anchorOrigin={{
                        vertical: 'top',
                        horizontal: 'right',
                      }}
                      badgeContent={totalDocumentCount}
                      max={99999}
                      color="secondary"
                    >
                      <DriveEtaIcon fontSize="small" />
                    </Badge>
                  </Typography>
                </Box>
              </Tooltip>
            </Box>
          )}

          {/* Search Bar - Mobile */}
          <Box sx={{ flexGrow: 1, mx: 1 }}>
            {autoCompleteSearchBar || enableAutoComplete ? (
              <AutoSuggest placeholderText="Search..." />
            ) : (
              <SearchBar placeholderText="Search..." />
            )}
          </Box>

          {/* Action Buttons - Mobile */}
          <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 0.5 }}>
            <Button
              size="small"
              onClick={openDialog}
              color="secondary"
              variant="contained"
              sx={{ minWidth: 'auto', fontSize: '0.75rem' }}
            >
              Filter
            </Button>
            <OrderByDialog />
          </Box>
        </Box>
      </Box>
    );
  };

  return windowWidth < MobileMaxWidth ? mobileNavBar() : desktopNavBar();
};

export default AdaptiveNavBar;
