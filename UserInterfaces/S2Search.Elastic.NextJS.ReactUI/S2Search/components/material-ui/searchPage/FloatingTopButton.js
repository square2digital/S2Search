import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import Fab from "@mui/material/Fab";
import NavigationIcon from "@mui/icons-material/Navigation";
import { withStyles, makeStyles } from "@mui/styles";
import Tooltip from "@mui/material/Tooltip";
import UseWindowSize from "../../../common/hooks/UseWindowSize";
import Zoom from "@mui/material/Zoom";
import { createTheme, ThemeProvider } from "@mui/material/styles";

const useStyles = makeStyles((theme) => ({
  floatPosition: {
    margin: theme.spacing(1),
    position: "fixed",

    [theme.breakpoints.height > 500]: {
      top: "90%",
    },

    bottom: theme.spacing(2),
    right: theme.spacing(3),
    zIndex: 999,
  },
  extendedIcon: {
    marginRight: theme.spacing(1),
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

const setSize = (width) => {
  if (width < 600) {
    return "small";
  }
  if (width > 600 && width < 960) {
    return "medium";
  }

  return "large";
};

const handleClick = () => {
  window.scrollTo({ top: 0, behavior: "smooth" });
};

const buttonPositionTrigger = 650;

const FloatingTopButton = (props) => {
  const classes = useStyles();
  const [width, height] = UseWindowSize();
  const [scrollPosition, setScrollPosition] = useState(0);

  useEffect(() => {
    window.addEventListener("scroll", handleScroll, { passive: true });

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  const handleScroll = () => {
    const position = window.pageYOffset;
    setScrollPosition(position);
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

  return (
    <div>
      <ThemeProvider theme={theme}>
        <Zoom in={scrollPosition > buttonPositionTrigger}>
          <LightTooltip title="Back to Top" aria-label="Back to Top">
            <Fab
              size={setSize(width)}
              color="secondary"
              aria-label="add"
              className={[classes.floatPosition, classes.extendedIcon]}
              onClick={handleClick}>
              <NavigationIcon />
            </Fab>
          </LightTooltip>
        </Zoom>
      </ThemeProvider>
    </div>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxPrimaryColour: reduxState.themeReducer.primaryColour,
    reduxSecondaryColour: reduxState.themeReducer.secondaryColour,
  };
};

FloatingTopButton.propTypes = {
  reduxPrimaryColour: PropTypes.string,
  reduxSecondaryColour: PropTypes.string,
};

export default connect(mapStateToProps)(FloatingTopButton);
