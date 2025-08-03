import * as types from "../actions/actionTypes";
import initialState from "../initialState";

export default function selectedSearchIndexReducer(
  state = initialState.searchIndexes,
  action
) {
  switch (action.type) {
    case types.SET_SELECTEDSEARCHINDEXID_SUCCESS:
      return action.selectedSearchIndex;
    default:
      return state;
  }
}
