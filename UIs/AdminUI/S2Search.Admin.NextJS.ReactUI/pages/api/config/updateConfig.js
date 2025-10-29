import { handleResponse, handleError, apiOptionsWithData } from "../apiUtils";

export async function updateConfig(data) {
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_CUSTOMERS_URL}`;
  const url = `${baseUrl}/config/update`;
  const putMethod = apiOptionsWithData("PUT", data);

  return await fetch(url, putMethod).then(handleResponse).catch(handleError);
}

export default function index(req, res) {
  try {
    const data = req.body;

    updateConfig(data).then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
