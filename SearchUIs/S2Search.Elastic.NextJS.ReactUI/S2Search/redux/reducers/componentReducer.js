import * as actionTypes from "../actions/actionTypes";
import initialComponentState from "../../redux/stateObjects/componentState";

const componentReducer = (state = initialComponentState, action) => {
  switch (action.type) {
    case actionTypes.FULL_SCREEN_DIALOG: {
      return { ...state, dialogOpen: action.dialogOpen };
    }
    case actionTypes.LOADING: {
      return { ...state, loading: action.loading };
    }
    case actionTypes.CANCELLATION_TOKEN: {
      return { ...state, enableToken: action.enableToken };
    }
    default:
      return state;
  }
};

export default componentReducer;
