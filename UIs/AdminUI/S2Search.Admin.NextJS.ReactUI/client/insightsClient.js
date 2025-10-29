import { handleResponse, handleError, apiOptions } from "../pages/api/apiUtils";

export async function getSummaryData(customerId, searchIndexId) {
  const url = `/api/insights/getSummaryData?customerId=${customerId}&searchIndexId=${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function getChartData(
  customerId,
  searchIndexId,
  datasetName,
  dateFrom,
  dateTo
) {
  const url = `/api/insights/getChartData?customerId=${customerId}&searchIndexId=${searchIndexId}&datasetName=${datasetName}&dateFrom=${dateFrom}&dateTo=${dateTo}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export async function getTileData(
  searchIndexId,
  dateFrom,
  dateTo,
  includePreviousPeriod
) {
  const url = `/api/insights/getTileData?searchIndexId=${searchIndexId}&dateFrom=${dateFrom}&dateTo=${dateTo}&includePreviousPeriod=${includePreviousPeriod}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}
