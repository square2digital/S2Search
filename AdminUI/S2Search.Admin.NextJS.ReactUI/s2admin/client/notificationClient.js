import { handleResponse, handleError, apiOptions } from "../pages/api/apiUtils";

export async function getNotificationsBySearchIndexId(
  customerId,
  searchIndexId,
  page,
  pageSize
) {
  const url = `/api/notification/getNotificationsBySearchIndexId?customerId=${customerId}&searchIndexId=${searchIndexId}&page=${page}&pageSize=${pageSize}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}
