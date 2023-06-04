import { handleResponse, handleError, apiOptions } from "../apiUtils";

const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/api/customers`;

export async function getSummaryData(customerId, searchIndexId) {
  const url = `${baseUrl}/${customerId}/searchIndex/${searchIndexId}/searchinsights/summary`;
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
  const url = `${baseUrl}/searchIndex/${searchIndexId}/searchinsights/tile/count?dateFrom=${dateFrom}&dateTo=${dateTo}&includePreviousPeriod=${includePreviousPeriod}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const searchIndexId = req.query.searchIndexId;
    const dateFrom = req.query.dateFrom;
    const dateTo = req.query.dateTo;
    const includePreviousPeriod = req.query.includePreviousPeriod;

    getTileData(searchIndexId, dateFrom, dateTo, includePreviousPeriod).then(
      function (data) {
        res.status(200).json(data);
      }
    );
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
