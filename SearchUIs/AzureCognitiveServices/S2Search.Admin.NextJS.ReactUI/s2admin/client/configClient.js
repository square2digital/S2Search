import {
  handleResponse,
  handleError,
  apiOptions,
  apiOptionsWithData,
} from "../pages/api/apiUtils";

export async function getConfig(searchIndexId) {
  const url = `/api/config/getConfig?searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function updateConfig(data) {
  const url = `/api/config/updateConfig`;
  const putMethod = apiOptionsWithData("PUT", data);
  return await fetch(url, putMethod).then(handleResponse).catch(handleError);
}
