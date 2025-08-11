import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Fab from '@mui/material/Fab';
import NavigationIcon from '@mui/icons-material/Navigation';
import Tooltip from '@mui/material/Tooltip';
import UseWindowSize from '../../../common/hooks/UseWindowSize';
import { DefaultTheme } from '../../../common/Constants';
import Zoom from '@mui/material/Zoom';
import { useTheme } from '@mui/material/styles';

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
  const theme = useTheme();

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

  return (
    <div>
      <Zoom in={scrollPosition > buttonPositionTrigger}>
        <Tooltip title="Back to Top" aria-label="Back to Top">
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
          </Tooltip>
        </Zoom>
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
