import * as actionTypes from '../actions/actionTypes';
import initialThemeState from '../stateObjects/themeState';

const themeReducer = (state = initialThemeState, action) => {
  switch (action.type) {
    case actionTypes.PRIMARY_COLOUR: {
      return {
        ...state,
        primaryColour: action.primaryColour,
      };
    }
    case actionTypes.SECONDARY_COLOUR: {
      return {
        ...state,
        secondaryColour: action.secondaryColour,
      };
    }
    case actionTypes.NAV_BAR_COLOUR: {
      return {
        ...state,
        navBarColour: action.navBarColour,
      };
    }
    case actionTypes.LOGO_URL: {
      return {
        ...state,
        logoURL: action.logoURL,
      };
    }
    case actionTypes.MISSING_IMAGE_URL: {
      return {
        ...state,
        missingImageURL: action.missingImageURL,
      };
    }
    default:
      return state;
  }
};

export default themeReducer;
