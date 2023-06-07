import * as types from "../actions/actionTypes";
import initialState from "../initialState";

export default function searchIndexesReducer(
  state = initialState.searchIndexes,
  action
) {
  switch (action.type) {
    case types.LOAD_SEARCHINDEXES_SUCCESS:
      return [...action.searchIndexes];
    default:
      return state;
  }
}
