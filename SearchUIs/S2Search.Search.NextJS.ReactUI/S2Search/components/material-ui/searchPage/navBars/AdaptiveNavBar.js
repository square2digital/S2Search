import React from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import facetActions from "../../../../redux/actions/facetActions";
import ComponentActions from "../../../../redux/actions/componentActions";
import { withStyles, makeStyles } from "@mui/styles";
import Typography from "@mui/material/Typography";
import { DefaultTheme, MobileMaxWidth } from "../../../../common/Constants";
import Button from "@mui/material/Button";
import OrderByDialog from "../orderBy/OrderByDialog";
import Tooltip from "@mui/material/Tooltip";
import Box from "@mui/material/Box";
import searchActions from "../../../../redux/actions/searchActions";
import DriveEtaIcon from "@mui/icons-material/DriveEta";
import Badge from "@mui/material/Badge";
import SearchBar from "../../../../components/material-ui/searchPage/searchBars/SearchBar";
import AutoSuggest from "../searchBars/AutoSuggest";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import UseWindowSize from "../../../../common/hooks/UseWindowSize";

const s2logoWidth = 57;

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
    margin: "auto",
  },
  image: {
    width: 128,
    height: 128,
  },
  img: {
    margin: "auto",
    display: "block",
    maxWidth: "100%",
    maxHeight: "100%",
  },
  searchBarRoot: {
    padding: "2px 4px",
    display: "flex",
  },
  input: {
    marginLeft: theme.spacing(1),
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
    marginRight: theme.spacing(2),
  },
  firstBadgeContainer: {
    [theme.breakpoints.up("xs")]: {
      marginLeft: theme.spacing(0.25),
    },
  },
  badgeContainer: {
    [theme.breakpoints.up("xs")]: {
      marginLeft: theme.spacing(0.25),
    },
  },
  topMargin: {
    marginTop: theme.spacing(2.25),
  },
}));

const LightTooltip = withStyles((theme) => ({
  tooltip: {
    backgroundColor: theme.palette.common.white,
    color: "rgba(0, 0, 0, 0.87)",
    boxShadow: theme.shadows[1],
    fontSize: 11,
  },
}))(Tooltip);

const openDialog = (open, props) => (event) => {
  if (
    event.type === "keydown" &&
    (event.key === "Tab" || event.key === "Shift")
  ) {
    return;
  } else {
    props.saveDialogOpen(open);
  }
};

const AdaptiveNavBar = (props) => {
  const classes = useStyles();
  const [windowWidth, windowHeight] = UseWindowSize();

  const resetFilters = () => {
    props.saveResetFacets(true);
  };

  const theme = createTheme({
    palette: {
      primary: {
        main: props.reduxPrimaryColour,
      },
      secondary: {
        main: props.reduxSecondaryColour,
      },
    },
  });

  const desktopNavBar = () => {
    return (
      <ThemeProvider theme={theme}>
        <div
          style={{
            position: "fixed",
            top: 0,
            right: 0,
            width: "100%",
            background: props.reduxNavBarColour
              ? props.reduxNavBarColour
              : DefaultTheme.navBarHexColour,
            zIndex: 1,
          }}>
          <div className={classes.root}>
            <Box className={classes.paper}>
              <Box container display="flex">
                <Box p={1} style={{ textAlign: "left" }}>
                  <img
                    data-href="/"
                    onClick={resetFilters}
                    src={
                      props.reduxLogoURL
                        ? props.reduxLogoURL
                        : DefaultTheme.logoURL
                    }
                    width={s2logoWidth}
                    style={{ position: "relative", top: 3 }}
                  />
                </Box>

                {props.reduxHideIconVehicleCounts ? (
                  <></>
                ) : (
                  <>
                    <Box p={1} item style={{ textAlign: "left" }}>
                      <LightTooltip
                        placement="bottom"
                        title="Number of vehicles in this search"
                        aria-label="Number of vehicles in this search">
                        <div
                          className={`${classes.topMargin} ${classes.firstBadgeContainer} ${classes.badgeContainer}`}>
                          <Typography
                            variant="body2"
                            className={classes.resultsText}>
                            <Badge
                              anchorOrigin={{
                                vertical: "top",
                                horizontal: "left",
                              }}
                              badgeContent={props.reduxVehicleData.length}
                              color="secondary">
                              <DriveEtaIcon />
                            </Badge>
                          </Typography>
                        </div>
                      </LightTooltip>
                    </Box>
                    <Box p={1} style={{ textAlign: "left" }}>
                      <LightTooltip
                        placement="bottom"
                        title="Total Number of Vehicles"
                        aria-label="Total Number of Vehicles">
                        <div
                          className={`${classes.topMargin} ${classes.badgeContainer}`}>
                          <Typography
                            variant="body2"
                            className={classes.resultsText}>
                            <Badge
                              anchorOrigin={{
                                vertical: "top",
                                horizontal: "left",
                              }}
                              badgeContent={props.reduxTotalDocumentCount}
                              max={99999}
                              color="secondary">
                              <DriveEtaIcon />
                            </Badge>
                          </Typography>
                        </div>
                      </LightTooltip>
                    </Box>
                  </>
                )}

                <Box
                  flexGrow={1}
                  style={{
                    textAlign: "left",
                    marginTop: "15px",
                    paddingRight: "15px",
                  }}>
                  <Box style={{ textAlign: "left" }}>
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
                    style={{ marginTop: 25 }}>
                    <Box>
                      <Box style={{ textAlign: "right" }}>
                        <Button
                          size="small"
                          onClick={openDialog(true, props)}
                          color="secondary"
                          variant="contained"
                          style={{ marginRight: "10px" }}>
                          Filter
                        </Button>
                      </Box>
                    </Box>
                    <Box style={{ position: "relative", right: 5 }}>
                      <OrderByDialog />
                    </Box>
                  </Box>
                </Box>
              </Box>
            </Box>
          </div>
        </div>
      </ThemeProvider>
    );
  };

  const mobileNavBar = () => {
    return (
      <ThemeProvider theme={theme}>
        <div
          style={{
            position: "fixed",
            top: 0,
            right: 0,
            width: "100%",
            minWidth: "260px",
            background: props.reduxNavBarColour
              ? props.reduxNavBarColour
              : DefaultTheme.navBarHexColour,
            zIndex: 1,
          }}>
          <Box
            display="flex"
            p={1}
            style={{ flexGrow: "1", justifyContent: "space-between" }}>
            <Box>
              <img
                data-href="/"
                onClick={resetFilters}
                src={
                  props.reduxLogoURL ? props.reduxLogoURL : DefaultTheme.logoURL
                }
                width={s2logoWidth}
                style={{ position: "relative", top: 3 }}
              />
            </Box>
            <Box>
              <LightTooltip
                placement="bottom"
                title="Number of vehicles in this search"
                aria-label="Number of vehicles in this search">
                <div
                  className={`${classes.firstBadgeContainer} ${classes.badgeContainer}`}
                  style={{ position: "relative", top: 8, right: 2 }}>
                  <Typography variant="body2" className={classes.resultsText}>
                    <Badge
                      anchorOrigin={{
                        vertical: "top",
                        horizontal: "right",
                      }}
                      badgeContent={props.reduxVehicleData.length}
                      color="secondary">
                      <DriveEtaIcon />
                    </Badge>
                  </Typography>
                </div>
              </LightTooltip>
              <LightTooltip
                placement="bottom"
                title="Total Number of Vehicles"
                aria-label="Total Number of Vehicles">
                <div
                  className={`${classes.badgeContainer}`}
                  style={{ position: "relative", top: 15, right: 2 }}>
                  <Typography variant="body2" className={classes.resultsText}>
                    <Badge
                      anchorOrigin={{
                        vertical: "top",
                        horizontal: "right",
                      }}
                      badgeContent={props.reduxTotalDocumentCount}
                      max={99999}
                      color="secondary">
                      <DriveEtaIcon />
                    </Badge>
                  </Typography>
                </div>
              </LightTooltip>
            </Box>
            <Box
              flexGrow={1}
              style={{
                textAlign: "left",
                paddingLeft: "5px",
                marginLeft: "15px",
                marginRight: "8px",
                paddingBottom: "5px",
                paddingTop: "7px",
              }}>
              <Box style={{ textAlign: "left" }}>
                {props.autoCompleteSearchBar === true ? (
                  <AutoSuggest placeholderText="Search..." />
                ) : (
                  <SearchBar placeholderText="Search..." />
                )}
              </Box>
            </Box>
            <Box style={{ position: "relative", bottom: 2, left: 3 }}>
              <Button
                size="small"
                onClick={openDialog(true, props)}
                color="secondary"
                variant="contained">
                Filter
              </Button>
              <div style={{ position: "relative", top: 4 }}>
                <OrderByDialog />
              </div>
            </Box>
          </Box>
        </div>
      </ThemeProvider>
    );
  };

  return windowWidth < MobileMaxWidth ? mobileNavBar() : desktopNavBar();
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxSearchCount: reduxState.searchReducer.searchCount,
    reduxTotalDocumentCount: reduxState.searchReducer.totalDocumentCount,
    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxDialogOpen: reduxState.componentReducer.dialogOpen,
    reduxLoading: reduxState.componentReducer.loading,
    reduxVehicleData: reduxState.searchReducer.vehicleData,
    reduxNavBarColour: reduxState.themeReducer.navBarColour,
    reduxLogoURL: reduxState.themeReducer.logoURL,

    reduxConfigData: reduxState.configReducer.configData,
    reduxEnableAutoComplete: reduxState.configReducer.enableAutoComplete,
    reduxHideIconVehicleCounts: reduxState.configReducer.hideIconVehicleCounts,
    reduxPlaceholderText: reduxState.configReducer.placeholderText,
    reduxPrimaryColour: reduxState.themeReducer.primaryColour,
    reduxSecondaryColour: reduxState.themeReducer.secondaryColour,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveDialogOpen: (open) => dispatch(ComponentActions.saveDialogOpen(open)),
    saveResetFacets: (resetFacets) =>
      dispatch(facetActions.saveResetFacets(resetFacets)),
    saveSearchTerm: (searchTerm) =>
      dispatch(searchActions.saveSearchTerm(searchTerm)),
    saveVehicleData: () => dispatch(searchActions.saveVehicleData([])),
    savePageNumber: () => dispatch(searchActions.savePageNumber(0)),
    saveFacetSelectors: () => dispatch(facetActions.saveFacetSelectors([])),
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
