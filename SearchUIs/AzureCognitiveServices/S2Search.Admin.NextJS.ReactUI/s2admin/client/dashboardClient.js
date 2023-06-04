import { handleResponse, handleError, apiOptions } from "../pages/api/apiUtils";

export async function getDashboardSummaryItems(userid) {
  const url = `/api/dashboard/getDashboardSummaryItems?userid=${userid}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}
