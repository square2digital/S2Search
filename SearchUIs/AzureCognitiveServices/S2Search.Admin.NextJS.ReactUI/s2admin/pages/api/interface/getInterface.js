import { handleResponse, handleError, apiOptions } from "../apiUtils";

export async function getInterface(customerId, searchIndexId) {
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_CUSTOMERS_URL}/${customerId}`;
  const url = `${baseUrl}/searchindex/${searchIndexId}/searchinterfaces/latest`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const customerId = req.query.customerId;
    const searchIndexId = req.query.searchIndexId;

    getInterface(customerId, searchIndexId).then(function (data) {
      const responseJson = {
        isError: data.isError,
        message: data.message,
        result: {
          searchEndpoint: data.result.searchEndpoint,
          logoURL: data.result.logoURL,
        },
      };
      res.status(200).json(responseJson);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
