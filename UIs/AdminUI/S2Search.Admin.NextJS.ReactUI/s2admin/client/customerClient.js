import { handleResponse, handleError, apiOptions } from "../pages/api/apiUtils";

export async function getCustomerSearchIndexes(userid) {
  const url = `/api/customer/getCustomerSearchIndexes?userid=${userid}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}
