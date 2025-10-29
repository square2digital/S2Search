import { handleResponse, handleError, apiOptions } from "../apiUtils";

export async function getNotificationsBySearchIndexId(
  customerId,
  searchIndexId,
  page,
  pageSize
) {
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_CUSTOMERS_URL}`;
  const url = `${baseUrl}/${customerId}/searchindex/${searchIndexId}/notifications?page=${page}&pageSize=${pageSize}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const customerId = req.query.customerId;
    const searchIndexId = req.query.searchIndexId;
    const page = req.query.page;
    const pageSize = req.query.pageSize;

    getNotificationsBySearchIndexId(
      customerId,
      searchIndexId,
      page,
      pageSize
    ).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
