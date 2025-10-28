import React, { useState, useEffect } from 'react';
import Fab from '@mui/material/Fab';
import NavigationIcon from '@mui/icons-material/Navigation';
import Tooltip from '@mui/material/Tooltip';
import UseWindowSize from '../../../common/hooks/UseWindowSize';
import Zoom from '@mui/material/Zoom';

type FabSize = 'small' | 'medium' | 'large';

const setSize = (width: number): FabSize => {
  if (width < 600) {
    return 'small';
  }
  if (width > 600 && width < 960) {
    return 'medium';
  }

  return 'large';
};

const handleClick = (): void => {
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

const buttonPositionTrigger = 650;

const FloatingTopButton: React.FC = () => {
  const [width] = UseWindowSize();
  const [scrollPosition, setScrollPosition] = useState<number>(0);

  useEffect(() => {
    const handleScroll = (): void => {
      const position = window.pageYOffset;
      setScrollPosition(position);
    };

    window.addEventListener('scroll', handleScroll, { passive: true });

    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

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

export default FloatingTopButton;