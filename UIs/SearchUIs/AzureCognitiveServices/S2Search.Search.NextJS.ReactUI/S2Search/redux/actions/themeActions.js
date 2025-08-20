import * as actionTypes from './actionTypes';

const componentActions = {
  savePrimaryColour: function (primaryColour) {
    return {
      type: actionTypes.PRIMARY_COLOUR,
      primaryColour: primaryColour,
    };
  },
  saveSecondaryColour: function (secondaryColour) {
    return {
      type: actionTypes.SECONDARY_COLOUR,
      secondaryColour: secondaryColour,
    };
  },
  saveNavBarColour: function (navBarColour) {
    return {
      type: actionTypes.NAV_BAR_COLOUR,
      navBarColour: navBarColour,
    };
  },
  saveLogoURL: function (logoURL) {
    return {
      type: actionTypes.LOGO_URL,
      logoURL: logoURL,
    };
  },
  saveMissingImageURL: function (missingImageURL) {
    return {
      type: actionTypes.MISSING_IMAGE_URL,
      missingImageURL: missingImageURL,
    };
  },
};

export default componentActions;
