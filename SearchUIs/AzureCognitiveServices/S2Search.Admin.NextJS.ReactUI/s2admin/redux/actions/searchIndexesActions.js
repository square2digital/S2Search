import * as types from "./actionTypes";
import * as customerApi from "../../client/customerClient";
import { beginApiCall, apiCallError } from "./apiStatusActions";

export function setSearchIndexes(userid) {
  return function (dispatch) {
    dispatch(beginApiCall());
    return customerApi
      .getCustomerSearchIndexes(userid)
      .then((customerData) => {
        dispatch({
          type: types.LOAD_SEARCHINDEXES_SUCCESS,
          searchIndexes: customerData.result.searchIndexes,
        });
      })
      .catch((error) => {
        dispatch(apiCallError(error));
        throw error;
      });
  };
}
