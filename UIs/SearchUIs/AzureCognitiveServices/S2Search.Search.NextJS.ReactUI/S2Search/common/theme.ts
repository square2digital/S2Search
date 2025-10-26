import { createTheme } from '@mui/material/styles';
import { red } from '@mui/material/colors';
import { ThemeColors } from '../types/theme/colours';

// Create a modern theme with improved defaults
export const createAppTheme = (colors: ThemeColors) => {
  return createTheme({
    palette: {
      mode: 'light',
      primary: {
        main: colors.primaryColor || '#1976d2',
        light: '#42a5f5',
        dark: '#1565c0',
        contrastText: '#fff',
      },
      secondary: {
        main: colors.secondaryColor || '#dc004e',
        light: '#ff5983',
        dark: '#9a0036',
        contrastText: '#fff',
      },
      error: {
        main: red.A400,
      },
      background: {
        default: '#fafafa',
        paper: '#fff',
      },
      text: {
        primary: 'rgba(0, 0, 0, 0.87)',
        secondary: 'rgba(0, 0, 0, 0.6)',
      },
    },
    typography: {
      fontFamily: [
        'Roboto',
        '-apple-system',
        'BlinkMacSystemFont',
        '"Segoe UI"',
        '"Helvetica Neue"',
        'Arial',
        'sans-serif',
        '"Apple Color Emoji"',
        '"Segoe UI Emoji"',
        '"Segoe UI Symbol"',
      ].join(','),
      h1: {
        fontSize: '2.125rem',
        fontWeight: 300,
        lineHeight: 1.167,
      },
      h2: {
        fontSize: '1.5rem',
        fontWeight: 400,
        lineHeight: 1.2,
      },
      h3: {
        fontSize: '1.25rem',
        fontWeight: 500,
        lineHeight: 1.167,
      },
      body1: {
        fontSize: '1rem',
        lineHeight: 1.5,
      },
      body2: {
        fontSize: '0.875rem',
        lineHeight: 1.43,
      },
      button: {
        fontSize: '0.875rem',
        fontWeight: 500,
        lineHeight: 1.75,
        textTransform: 'uppercase',
      },
    },
    shape: {
      borderRadius: 8,
    },
    spacing: 8,
    components: {
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
            '&::-webkit-scrollbar-thumb:focus, & *::-webkit-scrollbar-thumb:focus':
              {
                backgroundColor: '#959595',
              },
            '&::-webkit-scrollbar-thumb:active, & *::-webkit-scrollbar-thumb:active':
              {
                backgroundColor: '#959595',
              },
            '&::-webkit-scrollbar-thumb:hover, & *::-webkit-scrollbar-thumb:hover':
              {
                backgroundColor: '#959595',
              },
          },
        },
      },
      MuiButton: {
        styleOverrides: {
          root: {
            borderRadius: 8,
            textTransform: 'none',
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
      MuiChip: {
        styleOverrides: {
          root: {
            borderRadius: 16,
            fontWeight: 500,
          },
        },
      },
      MuiTextField: {
        styleOverrides: {
          root: {
            '& .MuiOutlinedInput-root': {
              borderRadius: 8,
            },
          },
        },
      },
      MuiToolbar: {
        styleOverrides: {
          regular: {
            paddingLeft: '16px',
            paddingRight: '16px',
            '@media (min-width: 600px)': {
              paddingLeft: '24px',
              paddingRight: '24px',
            },
          },
        },
      },
      MuiAppBar: {
        styleOverrides: {
          root: {
            boxShadow: '0 1px 3px rgba(0,0,0,0.12)',
          },
        },
      },
    },
  });
};
