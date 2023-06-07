import * as actionTypes from "../actions/actionTypes";
import initialConfigState from "../stateObjects/configState";

const configReducer = (state = initialConfigState, action) => {
  switch (action.type) {
    case actionTypes.CONFIG_DATA: {
      return {
        ...state,
        configData: action.configData,
      };
    }
    case actionTypes.ENABLE_AUTO_COMPLETE: {
      return {
        ...state,
        enableAutoComplete: action.enableAutoComplete,
      };
    }
    case actionTypes.HIDE_ICON_VEHICLE_COUNTS: {
      return {
        ...state,
        hideIconVehicleCounts: action.hideIconVehicleCounts,
      };
    }
    case actionTypes.PLACEHOLDER_TEXT: {
      return {
        ...state,
        placeholderText: action.placeholderText,
      };
    }
    default:
      return state;
  }
};

export default configReducer;
