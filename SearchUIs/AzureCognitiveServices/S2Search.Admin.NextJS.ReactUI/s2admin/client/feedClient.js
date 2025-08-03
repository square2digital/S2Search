import {
  handleResponse,
  handleError,
  apiOptions,
  apiOptionsWithData,
  apiOptionsWithFormData,
} from "../pages/api/apiUtils";

export async function getCredentialsBySearchIndexId(customerId, searchIndexId) {
  const url = `/api/feed/getCredentialsBySearchIndexId?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function getLatestFeedConfigurationBySearchIndexId(searchIndexId) {
  const url = `/api/feed/getLatestFeedConfigurationBySearchIndexId?searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function saveFeedConfiguration(feedRequest) {
  const url = `/api/feed/saveFeedConfiguration?searchIndexId=${feedRequest.searchIndexId}`;
  const postMethod = apiOptionsWithData("POST", feedRequest);
  return await fetch(url, postMethod).then(handleResponse).catch(handleError);
}

export async function updatePassword(customerId, searchIndexId, data) {
  const url = `/api/feed/updatePassword?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  const postMethod = apiOptionsWithData("PUT", data);
  return await fetch(url, postMethod).then(handleResponse).catch(handleError);
}

export async function uploadFile(customerId, searchIndexId, feedFile) {
  const url = `/api/feed/uploadFile?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptionsWithFormData("POST", feedFile))
    .then(handleResponse)
    .catch(handleError);
}
