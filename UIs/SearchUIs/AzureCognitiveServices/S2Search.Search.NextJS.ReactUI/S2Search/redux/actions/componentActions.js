import * as actionTypes from './actionTypes';

const componentActions = {
  saveDialogOpen: function (open) {
    return {
      type: actionTypes.FULL_SCREEN_DIALOG,
      dialogOpen: open,
    };
  },
  saveSelectedFacet: function (selectedFacet) {
    return { type: actionTypes.SELECTED_FACET, selectedFacet: selectedFacet };
  },
  saveLoading: function (apiLoading) {
    return {
      type: actionTypes.LOADING,
      loading: apiLoading,
    };
  },
  saveCancellationToken: function (enable) {
    return {
      type: actionTypes.CANCELLATION_TOKEN,
      enableToken: enable,
    };
  },
};

export default componentActions;
