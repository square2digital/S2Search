import { handleResponse, handleError, apiOptions } from "../apiUtils";

const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/api/customers`;

export async function getChartData(
  customerId,
  searchIndexId,
  datasetName,
  dateFrom,
  dateTo
) {
  const url = `${baseUrl}/${customerId}/searchIndex/${searchIndexId}/searchinsights/chart/${datasetName}?dateFrom=${dateFrom}&dateTo=${dateTo}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const customerId = req.query.customerId;
    const searchIndexId = req.query.searchIndexId;
    const datasetName = req.query.datasetName;
    const dateFrom = req.query.dateFrom;
    const dateTo = req.query.dateTo;

    getChartData(customerId, searchIndexId, datasetName, dateFrom, dateTo).then(
      function (data) {
        res.status(200).json(data);
      }
    );
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
