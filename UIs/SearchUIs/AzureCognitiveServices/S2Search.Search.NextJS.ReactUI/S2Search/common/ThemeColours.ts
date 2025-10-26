import { DefaultTheme } from './Constants';
import { ThemeColors } from '../types/theme/colours';

export const ThemeColours = async (req?: any) => {
  try {
    // If we're on the server side (getServerSideProps), use the API client directly
    if (typeof window === 'undefined') {
      if (req) {
        const { apiClient } = await import('../lib/api/client');
        const host = req.headers?.host || 'localhost:2997';
        const response = await apiClient.getTheme(host);
        
        if (response.success && response.data) {
          return GetJSONObj({
            primaryColor: response.data.primaryHexColour || DefaultTheme.primaryHexColour,
            secondaryColor: response.data.secondaryHexColour || DefaultTheme.secondaryHexColour,
          });
        }
      }
      // If no request object on server, return default theme
      return GetJSONObj({
        primaryColor: DefaultTheme.primaryHexColour,
        secondaryColor: DefaultTheme.secondaryHexColour,
      });
    } else {
      // If we're on the client side, use fetch
      const response = await fetch('/api/theme');
      const themeData = await response.json();
      
      if (response.ok && themeData) {
        return GetJSONObj({
          primaryColor: themeData.primaryHexColour || DefaultTheme.primaryHexColour,
          secondaryColor: themeData.secondaryHexColour || DefaultTheme.secondaryHexColour,
        });
      }
    }
  } catch (error) {
    console.error('Failed to fetch theme:', error);
  }

  // Fallback to default theme
  return GetJSONObj({
    primaryColor: DefaultTheme.primaryHexColour,
    secondaryColor: DefaultTheme.secondaryHexColour,
  });
};

const GetJSONObj = (colors: ThemeColors) => {
  return {
    palette: {
      primary: {
        main: colors.primaryColor,
      },
      secondary: {
        main: colors.secondaryColor,
      },
    },
    overrides: {
      MuiToolbar: {
        regular: {
          paddingLeft: '10px',
          paddingRight: '10px',
        },
      },
    },
  };
};

export default ThemeColours;
