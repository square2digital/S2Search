import {
  handleResponse,
  handleError,
  apiOptions,
  apiOptionsWithData,
} from "../pages/api/apiUtils";

export async function getThemeByID(customerId, searchIndexId, themeId) {
  const url = `/api/theme/getThemeByID?customerId=${customerId}&searchIndexId=${searchIndexId}&theme=${themeId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function getThemeBySearchIndexId(customerId, searchIndexId) {
  const url = `/api/theme/getThemeBySearchIndexIdcustomerId?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function getThemesByCustomerId(customerId, searchIndexId) {
  const url = `/api/theme/getThemesByCustomerId?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function updateTheme(customerId, searchIndexId, data) {
  const url = `/api/theme/updateTheme?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  const putMethod = apiOptionsWithData("PUT", data);
  return await fetch(url, putMethod).then(handleResponse).catch(handleError);
}
