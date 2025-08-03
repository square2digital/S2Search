import http from "k6/http";
import { URLSearchParams } from "https://jslib.k6.io/url/1.0.0/index.js";

export const options = {
  vus: 100,
  duration: "5s",
};

// const apiBaseUrl = __ENV.API_BASE_URL;
const apiBaseUrl = "https://localhost:5001";
const searchCallingHost = "s2search.co.uk";

export default function () {
  //documentCount
  const documentCountEndpoint = "v1/totalDocumentCount";
  const documentCountParams = new URLSearchParams([
    ["callingHost", searchCallingHost],
  ]);

  const documentCountRequestUrl = `${documentCountEndpoint}?${documentCountParams.toString()}`;

  //search
  const searchEndpoint = "v1/search";
  const searchParams = new URLSearchParams([
    ["searchTerm", ""],
    ["filters", ""],
    ["orderBy", ""],
    ["pageNumber", "0"],
    ["pageSize", "24"],
    ["numberOfExistingResults", "0"],
    ["callingHost", searchCallingHost],
  ]);

  const searchRequestUrl = `${searchEndpoint}?${searchParams.toString()}`;

  //autocomplete
  const autocompleteEndpoint = "v1/autoSuggest";
  const autocompleteParams = new URLSearchParams([
    ["searchTerm", "BMW"],
    ["callingHost", searchCallingHost],
  ]);

  const autocompleteRequestUrl = `${autocompleteEndpoint}?${autocompleteParams.toString()}`;

  //   http.batch([
  //     ["GET", documentCountRequestUrl],
  //     ["GET", searchRequestUrl],
  //     ["GET", autocompleteRequestUrl],
  //   ]);

  http.batch([
    ["GET", `${apiBaseUrl}/api/status`],
    ["GET", `${apiBaseUrl}/api/status`],
    ["GET", `${apiBaseUrl}/api/status`],
  ]);

  // http.get(`${apiBaseUrl}/v1/search?${searchParams.toString()}`);
  // http.get(`${apiBaseUrl}/api/status}`);
}
