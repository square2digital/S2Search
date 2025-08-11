import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Fab from '@mui/material/Fab';
import NavigationIcon from '@mui/icons-material/Navigation';
import Tooltip, { tooltipClasses } from '@mui/material/Tooltip';
import UseWindowSize from '../../../common/hooks/UseWindowSize';
import { DefaultTheme } from '../../../common/Constants';
import Zoom from '@mui/material/Zoom';
import { createTheme, ThemeProvider, styled } from '@mui/material/styles';

const LightTooltip = styled(({ className, ...props }) => (
  <Tooltip {...props} classes={{ popper: className }} />
))(({ theme }) => ({
  [`& .${tooltipClasses.tooltip}`]: {
    backgroundColor: theme.palette.common.white,
    color: 'rgba(0, 0, 0, 0.87)',
    boxShadow: theme.shadows[1],
    fontSize: 11,
  },
}));

const setSize = width => {
  if (width < 600) {
    return 'small';
  }
  if (width > 600 && width < 960) {
    return 'medium';
  }

  return 'large';
};

const handleClick = () => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

const buttonPositionTrigger = 650;

const FloatingTopButton = props => {
  const [width, height] = UseWindowSize();
  const [scrollPosition, setScrollPosition] = useState(0);

  useEffect(() => {
    window.addEventListener('scroll', handleScroll, { passive: true });

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  const handleScroll = () => {
    const position = window.pageYOffset;
    setScrollPosition(position);
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
    <div>
      <ThemeProvider theme={theme}>
        <Zoom in={scrollPosition > buttonPositionTrigger}>
          <LightTooltip title="Back to Top" aria-label="Back to Top">
            <Fab
              size={setSize(width)}
              color="secondary"
              aria-label="add"
              sx={{
                margin: 1,
                position: 'fixed',
                bottom: 2,
                right: 3,
                zIndex: 999,
              }}
              onClick={handleClick}
            >
              <NavigationIcon />
            </Fab>
          </LightTooltip>
        </Zoom>
      </ThemeProvider>
    </div>
  );
};

const mapStateToProps = reduxState => {
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
