import * as types from "./actionTypes";

export function setSelectedSearchIndexValue(selectedValue) {
  return function (dispatch) {
    dispatch(loadSelectSearchIndexSuccess(selectedValue));
  };
}

function loadSelectSearchIndexSuccess(selectedSearchIndex) {
  return {
    type: types.SET_SELECTEDSEARCHINDEXID_SUCCESS,
    selectedSearchIndex: {
      id: selectedSearchIndex.key,
      name: selectedSearchIndex.value,
    },
  };
}
