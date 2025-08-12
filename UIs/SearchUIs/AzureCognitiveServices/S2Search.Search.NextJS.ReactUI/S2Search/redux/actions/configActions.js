import * as actionTypes from './actionTypes';

const configActions = {
  saveConfigData: function (configData) {
    return {
      type: actionTypes.CONFIG_DATA,
      configData: configData,
    };
  },
  saveEnableAutoComplete: function (enableAutoComplete) {
    return {
      type: actionTypes.ENABLE_AUTO_COMPLETE,
      enableAutoComplete: enableAutoComplete,
    };
  },
  saveHideIconVehicleCounts: function (hideIconVehicleCounts) {
    return {
      type: actionTypes.HIDE_ICON_VEHICLE_COUNTS,
      hideIconVehicleCounts: hideIconVehicleCounts,
    };
  },
  savePlaceholderArray: function (placeholderArray) {
    return {
      type: actionTypes.PLACEHOLDER_TEXT,
      placeholderText: placeholderArray,
    };
  },
};

export default configActions;
