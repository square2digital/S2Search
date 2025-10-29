import { handleResponse, handleError, apiOptionsWithData } from "../apiUtils";
const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_SEARCHINDEX_URL}`;

export async function saveFeedConfiguration(feedRequest) {
  const url = `${baseUrl}/${feedRequest.searchIndexId}/feeds`;
  const postMethod = apiOptionsWithData("POST", feedRequest);
  return await fetch(url, postMethod).then(handleResponse).catch(handleError);
}

export default function index(req, res) {
  try {
    const feedRequest = req.body.feedRequest;
    saveFeedConfiguration(feedRequest).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
