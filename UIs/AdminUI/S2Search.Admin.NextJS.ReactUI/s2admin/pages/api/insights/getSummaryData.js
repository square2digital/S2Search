import { handleResponse, handleError, apiOptions } from "../apiUtils";
import { getUserData } from "../../../services/identity/msal";

const getSummaryData = async (customerId, searchIndexId) => {
  searchIndexId;

  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/api/customers/${customerId}`;

  const url = `${baseUrl}/searchIndex/${searchIndexId}/searchinsights/summary`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
};

export default function index(req, res) {
  try {
    const searchIndexId = req.query.searchIndexId;
    const customerId = req.query.customerId;

    getSummaryData(customerId, searchIndexId).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
