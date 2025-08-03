import http from "k6/http";
import { URLSearchParams } from "https://jslib.k6.io/url/1.0.0/index.js";

export const options = {
  vus: 1,
  // duration: "5s",
};

// const apiBaseUrl = __ENV.API_BASE_URL;
const apiBaseUrl = "https://localhost:5001";
const searchCallingHost = "s2search.co.uk";

export default function () {
  const searchParams = new URLSearchParams([
    ["searchTerm", "BMW"],
    ["filters", ""],
    ["orderBy", "price desc"],
    ["pageNumber", "0"],
    ["pageSize", "24"],
    ["numberOfExistingResults", "0"],
    ["callingHost", searchCallingHost],
  ]);

  http.get(`${apiBaseUrl}/v1/search?${searchParams.toString()}`);
  // http.get(`${apiBaseUrl}/api/status}`);
}
