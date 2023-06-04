import { handleResponse, handleError, apiOptions } from "../apiUtils";

export async function getConfig(searchIndexId) {
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_CUSTOMERS_URL}`;
  const url = `${baseUrl}/config/${searchIndexId}`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const searchIndexId = req.query.searchIndexId;

    getConfig(searchIndexId).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
