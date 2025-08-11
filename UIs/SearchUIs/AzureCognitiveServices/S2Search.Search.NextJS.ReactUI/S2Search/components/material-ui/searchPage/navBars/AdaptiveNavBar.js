import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import {
  setResetFacets,
  setFacetSelectors,
} from '../../../../store/slices/facetSlice';
import { setDialogOpen } from '../../../../store/slices/uiSlice';
import Typography from '@mui/material/Typography';
import { DefaultTheme, MobileMaxWidth } from '../../../../common/Constants';
import Button from '@mui/material/Button';
import OrderByDialog from '../orderBy/OrderByDialog';
import Tooltip from '@mui/material/Tooltip';
import Box from '@mui/material/Box';
import {
  setSearchTerm,
  setVehicleData,
  setPageNumber,
} from '../../../../store/slices/searchSlice';
import DriveEtaIcon from '@mui/icons-material/DriveEta';
import Badge from '@mui/material/Badge';
import SearchBar from '../../../../components/material-ui/searchPage/searchBars/SearchBar';
import AutoSuggest from '../searchBars/AutoSuggest';
import { useTheme } from '@mui/material/styles';
import UseWindowSize from '../../../../common/hooks/UseWindowSize';

const s2logoWidth = 57;

// Inline styles object (converted from makeStyles)
const styles = {
  root: {
    flexGrow: 1,
  },
  paper: {
    margin: 'auto',
  },
  image: {
    width: 128,
    height: 128,
  },
  img: {
    margin: 'auto',
    display: 'block',
    maxWidth: '100%',
    maxHeight: '100%',
  },
  searchBarRoot: {
    padding: '2px 4px',
    display: 'flex',
  },
  input: {
    marginLeft: 8, // theme.spacing(1) = 8px
    flex: 1,
  },
  iconButton: {
    padding: 10,
  },
  divider: {
    height: 28,
    margin: 4,
  },
  menuButton: {
    marginRight: 16, // theme.spacing(2) = 16px
  },
  firstBadgeContainer: {
    marginLeft: 2, // theme.spacing(0.25) = 2px
  },
  badgeContainer: {
    marginLeft: 2, // theme.spacing(0.25) = 2px
  },
  topMargin: {
    marginTop: 18, // theme.spacing(2.25) = 18px
  },
  resultsText: {
    fontSize: '0.875rem',
  },
};

const openDialog = (open, props) => event => {
  if (
    event.type === 'keydown' &&
    (event.key === 'Tab' || event.key === 'Shift')
  ) {
    return;
  } else {
    props.saveDialogOpen(open);
  }
};

const AdaptiveNavBar = props => {
  const [windowWidth, windowHeight] = UseWindowSize();
  const theme = useTheme();

  const resetFilters = () => {
    props.saveResetFacets(true);
  };

  const desktopNavBar = () => {
    return (
      <div
        style={{
          position: 'fixed',
          top: 0,
          right: 0,
          width: '100%',
          background: props.reduxNavBarColour
            ? props.reduxNavBarColour
            : DefaultTheme.navBarHexColour,
          zIndex: 1,
        }}
      >
        <div style={styles.root}>
          <Box style={styles.paper}>
            <Box display="flex">
              <Box p={1} style={{ textAlign: 'left' }}>
                <img
                  data-href="/"
                  onClick={resetFilters}
                  src={
                    props.reduxLogoURL
                      ? props.reduxLogoURL
                      : DefaultTheme.logoURL
                  }
                  width={s2logoWidth}
                  style={{ position: 'relative', top: 3 }}
                />
              </Box>

              {props.reduxHideIconVehicleCounts ? (
                <></>
              ) : (
                <>
                  <Box p={1} style={{ textAlign: 'left' }}>
                    <Tooltip
                      placement="bottom"
                      title="Number of vehicles in this search"
                      aria-label="Number of vehicles in this search"
                    >
                      <div
                        style={{
                          ...styles.topMargin,
                          ...styles.firstBadgeContainer,
                          ...styles.badgeContainer,
                        }}
                      >
                        <Typography variant="body2" style={styles.resultsText}>
                          <Badge
                            anchorOrigin={{
                              vertical: 'top',
                              horizontal: 'left',
                            }}
                            badgeContent={props.reduxVehicleData.length}
                            color="secondary"
                          >
                            <DriveEtaIcon />
                          </Badge>
                        </Typography>
                      </div>
                    </Tooltip>
                  </Box>
                  <Box p={1} style={{ textAlign: 'left' }}>
                    <Tooltip
                      placement="bottom"
                      title="Total Number of Vehicles"
                      aria-label="Total Number of Vehicles"
                    >
                      <div
                        style={{
                          ...styles.topMargin,
                          ...styles.badgeContainer,
                        }}
                      >
                        <Typography variant="body2" style={styles.resultsText}>
                          <Badge
                            anchorOrigin={{
                              vertical: 'top',
                              horizontal: 'left',
                            }}
                            badgeContent={props.reduxTotalDocumentCount}
                            max={99999}
                            color="secondary"
                          >
                            <DriveEtaIcon />
                          </Badge>
                        </Typography>
                      </div>
                    </Tooltip>
                  </Box>
                </>
              )}

              <Box
                flexGrow={1}
                style={{
                  textAlign: 'left',
                  marginTop: '15px',
                  paddingRight: '15px',
                }}
              >
                <Box style={{ textAlign: 'left' }}>
                  {props.autoCompleteSearchBar === true ? (
                    <AutoSuggest placeholderText="Start typing to search..." />
                  ) : (
                    <SearchBar placeholderText="Start typing to search..." />
                  )}
                </Box>
              </Box>
              <Box>
                <Box
                  display="flex"
                  flexDirection="row-reverse"
                  style={{ marginTop: 25 }}
                >
                  <Box>
                    <Box style={{ textAlign: 'right' }}>
                      <Button
                        size="small"
                        onClick={openDialog(true, props)}
                        color="secondary"
                        variant="contained"
                        style={{ marginRight: '10px' }}
                      >
                        Filter
                      </Button>
                    </Box>
                  </Box>
                  <Box style={{ position: 'relative', right: 5 }}>
                    <OrderByDialog />
                  </Box>
                </Box>
              </Box>
            </Box>
          </Box>
        </div>
      </div>
    );
  };

  const mobileNavBar = () => {
    return (
      <div
        style={{
          position: 'fixed',
          top: 0,
          right: 0,
          width: '100%',
          minWidth: '260px',
          background: props.reduxNavBarColour
            ? props.reduxNavBarColour
            : DefaultTheme.navBarHexColour,
          zIndex: 1,
        }}
      >
        <Box
          display="flex"
          p={1}
          style={{ flexGrow: '1', justifyContent: 'space-between' }}
        >
          <Box>
            <img
              data-href="/"
              onClick={resetFilters}
              src={
                props.reduxLogoURL ? props.reduxLogoURL : DefaultTheme.logoURL
              }
              width={s2logoWidth}
              style={{ position: 'relative', top: 3 }}
            />
          </Box>
          <Box>
            <Tooltip
              placement="bottom"
              title="Number of vehicles in this search"
              aria-label="Number of vehicles in this search"
            >
              <div
                style={{
                  ...styles.firstBadgeContainer,
                  ...styles.badgeContainer,
                  position: 'relative',
                  top: 8,
                  right: 2,
                }}
              >
                <Typography variant="body2" style={styles.resultsText}>
                  <Badge
                    anchorOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                    }}
                    badgeContent={props.reduxVehicleData.length}
                    color="secondary"
                  >
                    <DriveEtaIcon />
                  </Badge>
                </Typography>
              </div>
            </Tooltip>
            <Tooltip
              placement="bottom"
              title="Total Number of Vehicles"
              aria-label="Total Number of Vehicles"
            >
              <div
                style={{
                  ...styles.badgeContainer,
                  position: 'relative',
                  top: 15,
                  right: 2,
                }}
              >
                <Typography variant="body2" style={styles.resultsText}>
                  <Badge
                    anchorOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                    }}
                    badgeContent={props.reduxTotalDocumentCount}
                    max={99999}
                    color="secondary"
                  >
                    <DriveEtaIcon />
                  </Badge>
                </Typography>
              </div>
            </Tooltip>
          </Box>
          <Box
            flexGrow={1}
            style={{
              textAlign: 'left',
              paddingLeft: '5px',
              marginLeft: '15px',
              marginRight: '8px',
              paddingBottom: '5px',
              paddingTop: '7px',
            }}
          >
            <Box style={{ textAlign: 'left' }}>
              {props.autoCompleteSearchBar === true ? (
                <AutoSuggest placeholderText="Search..." />
              ) : (
                <SearchBar placeholderText="Search..." />
              )}
            </Box>
          </Box>
          <Box style={{ position: 'relative', bottom: 2, left: 3 }}>
            <Button
              size="small"
              onClick={openDialog(true, props)}
              color="secondary"
              variant="contained"
            >
              Filter
            </Button>
            <div style={{ position: 'relative', top: 4 }}>
              <OrderByDialog />
            </div>
          </Box>
        </Box>
      </div>
    );
  };

  return windowWidth < MobileMaxWidth ? mobileNavBar() : desktopNavBar();
};

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxSearchCount: reduxState.search.searchCount,
    reduxTotalDocumentCount: reduxState.search.totalDocumentCount,
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxLoading: reduxState.ui.isLoading,
    reduxVehicleData: reduxState.search.vehicleData,
    reduxNavBarColour: reduxState.theme.navBarColour,
    reduxLogoURL: reduxState.theme.logoURL,

    reduxConfigData: reduxState.config.configData,
    reduxEnableAutoComplete: reduxState.config.enableAutoComplete,
    reduxHideIconVehicleCounts: reduxState.config.hideIconVehicleCounts,
    reduxPlaceholderText: reduxState.config.placeholderText,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveDialogOpen: open => dispatch(setDialogOpen(open)),
    saveResetFacets: resetFacets => dispatch(setResetFacets(resetFacets)),
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveVehicleData: () => dispatch(setVehicleData([])),
    savePageNumber: () => dispatch(setPageNumber(0)),
    saveFacetSelectors: () => dispatch(setFacetSelectors([])),
  };
};

AdaptiveNavBar.propTypes = {
  reduxSearchTerm: PropTypes.string,
  reduxFacetSelectors: PropTypes.array,
  reduxDialogOpen: PropTypes.bool,
  reduxLoading: PropTypes.bool,
  saveResetFacets: PropTypes.func,
  reduxVehicleData: PropTypes.array,
  reduxSearchCount: PropTypes.number,
  reduxTotalDocumentCount: PropTypes.number,
  reduxNavBarColour: PropTypes.string,
  reduxLogoURL: PropTypes.string,
  saveSearchTerm: PropTypes.func,
  saveVehicleData: PropTypes.func,
  savePageNumber: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  autoCompleteSearchBar: PropTypes.bool,
  reduxPrimaryColour: PropTypes.string,
  reduxSecondaryColour: PropTypes.string,
};

export default connect(mapStateToProps, mapDispatchToProps)(AdaptiveNavBar);
