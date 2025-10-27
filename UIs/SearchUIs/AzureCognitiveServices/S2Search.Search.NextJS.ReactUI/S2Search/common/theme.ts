import { createTheme } from '@mui/material/styles';
import { ThemeColors } from '../types/colourTypes';
import { DefaultTheme } from './Constants';

// Create a theme with custom colors and component overrides
export const createAppTheme = (colors?: ThemeColors) => {
  // Use provided colors, fallback to DefaultTheme, then to hardcoded defaults
  const primaryColor = colors?.primaryColor || DefaultTheme.primaryHexColour || '#1976d2';
  const secondaryColor = colors?.secondaryColor || DefaultTheme.secondaryHexColour || '#dc004e';

  return createTheme({
    palette: {
      primary: {
        main: primaryColor,
      },
      secondary: {
        main: secondaryColor,
      },
    },
    components: {
      // Custom scrollbar styling
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            scrollbarColor: '#6b6b6b #2b2b2b',
            '&::-webkit-scrollbar, & *::-webkit-scrollbar': {
              backgroundColor: '#f5f5f5',
              width: 8,
            },
            '&::-webkit-scrollbar-thumb, & *::-webkit-scrollbar-thumb': {
              borderRadius: 8,
              backgroundColor: '#d4d4d4',
              minHeight: 24,
              border: '2px solid #f5f5f5',
            },
            '&::-webkit-scrollbar-thumb:hover, & *::-webkit-scrollbar-thumb:hover':
              {
                backgroundColor: '#959595',
              },
          },
        },
      },
      // Modern button styling
      MuiButton: {
        styleOverrides: {
          root: {
            textTransform: 'none', // Disable uppercase transform
            fontWeight: 500,
            boxShadow: 'none',
            '&:hover': {
              boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
            },
          },
          containedPrimary: {
            '&:hover': {
              boxShadow: '0 4px 8px rgba(0,0,0,0.15)',
            },
          },
        },
      },
      // Enhanced card styling
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: 12,
            boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
            '&:hover': {
              boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
            },
            transition: 'box-shadow 0.2s ease-in-out',
          },
        },
      },
    },
  });
};
