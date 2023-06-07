import { handleResponse, handleError, apiOptions } from "../apiUtils";

export async function getCustomerSearchIndexes(userid) {
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/api/customers/${userid}`;
  const url = `${baseUrl}/searchIndexes`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
}

export default function index(req, res) {
  try {
    const userid = req.query.userid;
    getCustomerSearchIndexes(userid).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
