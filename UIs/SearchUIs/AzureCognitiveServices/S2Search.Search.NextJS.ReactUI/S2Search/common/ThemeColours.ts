import ThemeAPI from '../pages/api/helper/ThemeAPI';
import { DefaultTheme } from './Constants';
import { ThemeColors } from '../types/theme/colours';

export const ThemeColours = () => {
  return ThemeAPI().then(function (axiosThemeResponse) {
    if (axiosThemeResponse) {
      if (axiosThemeResponse.status === undefined) {
        return GetJSONObj({
          primaryColor: DefaultTheme.primaryHexColour,
          secondaryColor: DefaultTheme.secondaryHexColour,
        });
      } else {
        return GetJSONObj({
          primaryColor: axiosThemeResponse.data.primaryHexColour,
          secondaryColor: axiosThemeResponse.data.secondaryHexColour,
        });
      }
    }

    return GetJSONObj({
      primaryColor: DefaultTheme.primaryHexColour,
      secondaryColor: DefaultTheme.secondaryHexColour,
    });
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
