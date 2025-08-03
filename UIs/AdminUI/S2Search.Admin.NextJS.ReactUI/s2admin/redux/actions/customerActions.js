import * as types from "./actionTypes";
import * as customerApi from "../../client/customerClient";
import { beginApiCall, apiCallError } from "./apiStatusActions";

export function setCustomerDetails(userid) {
  return function (dispatch) {
    dispatch(beginApiCall());
    return customerApi
      .getCustomerSearchIndexes(userid)
      .then((customerData) => {
        dispatch(loadCustomerDetailsSuccess(customerData.result.customer));
      })
      .catch((error) => {
        dispatch(apiCallError(error));
        throw error;
      });
  };
}

function loadCustomerDetailsSuccess(customerDetails) {
  return {
    type: types.LOAD_CUSTOMERDETAILS_SUCCESS,
    customer: customerDetails,
  };
}
