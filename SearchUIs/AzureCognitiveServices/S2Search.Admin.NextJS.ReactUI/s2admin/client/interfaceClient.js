import { handleResponse, handleError, apiOptions } from "../pages/api/apiUtils";

export async function getInterface(customerId, searchIndexId) {
  const url = `/api/interface/getInterface?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}
