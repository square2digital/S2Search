import http from "k6/http";
import { sleep } from "k6";

// import {
//   jUnit,
//   textSummary,
// } from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

export const options = {
  vus: 500,
  duration: "90s",
};

// const ApiBaseUrl = __ENV.API_BASE_URL;
const ApiBaseUrl =
  "https://demo.square2digital.com/api/search?searchTerm=&filters=&orderBy=&pageNumber=0&pageSize=24&numberOfExistingResults=0&callingHost=demo.square2digital.com";

export default function () {
  http.get(`${ApiBaseUrl}`);
  sleep(2.5);
}

// export function handleSummary(data) {
//   const filepath = `./k6-result.xml`;
//   return {
//     stdout: textSummary(data, { indent: " ", enableColors: true }),
//     "./loadtest-results.xml": jUnit(data),
//   };
// }
