import { handleResponse, handleError, apiOptions } from "../apiUtils";
const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_SEARCHINDEX_URL}`;

export async function getLatestFeedConfigurationBySearchIndexId(searchIndexId) {
  const url = `${baseUrl}/${searchIndexId}/feeds/latest`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const searchIndexId = req.query.searchIndexId;
    getLatestFeedConfigurationBySearchIndexId(searchIndexId).then(function (
      data
    ) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
