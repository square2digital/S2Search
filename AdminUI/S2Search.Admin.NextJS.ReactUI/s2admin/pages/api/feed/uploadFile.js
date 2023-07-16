import {
  handleResponse,
  handleError,
  apiOptionsWithFormData,
} from "../apiUtils";
const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_CUSTOMERS_URL}`;

export async function uploadFile(customerId, searchIndexId, feedFile) {
  const url = `${baseUrl}/${customerId}/searchindex/${searchIndexId}/feeds/upload`;
  console.log(`CRAPI URL - ${url}`);
  return await fetch(url, apiOptionsWithFormData("POST", feedFile))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const customerId = req.query.customerId;
    const searchIndexId = req.query.searchIndexId;
    const feedFile = req.body;

    uploadFile(customerId, searchIndexId, feedFile).then(function (data) {
      if (data.isError) {
        res.status(500).json(data);
      } else {
        res.status(200).json(data);
      }
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
