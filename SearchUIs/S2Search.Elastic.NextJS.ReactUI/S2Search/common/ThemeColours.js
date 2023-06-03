import ThemeAPI from "../pages/api/helper/ThemeAPI";
import { DefaultTheme } from "./Constants";

export const ThemeColours = () => {
  return ThemeAPI().then(function (axiosThemeResponse) {
    if (axiosThemeResponse) {
      if (axiosThemeResponse.status === undefined) {
        return GetJSONObj(
          DefaultTheme.primaryHexColour,
          DefaultTheme.secondaryHexColour
        );
      } else {
        return GetJSONObj(
          axiosThemeResponse.data.primaryHexColour,
          axiosThemeResponse.data.secondaryHexColour
        );
      }
    }

    return GetJSONObj(
      DefaultTheme.primaryHexColour,
      DefaultTheme.secondaryHexColour
    );
  });
};

const GetJSONObj = (primaryHexColour, secondaryHexColour) => {
  return {
    palette: {
      primary: {
        main: primaryHexColour,
      },
      secondary: {
        main: secondaryHexColour,
      },
    },
    overrides: {
      MuiToolbar: {
        regular: {
          paddingLeft: "10px",
          paddingRight: "10px",
        },
      },
    },
  };
};

export default ThemeColours;
