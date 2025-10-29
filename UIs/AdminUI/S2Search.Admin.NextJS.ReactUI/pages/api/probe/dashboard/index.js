import { handleResponse, handleError, apiOptions } from "../../apiUtils";

const getDashboardSummaryItems = async () => {
  const userid = "e7099bf6-79a5-4174-b671-f766a2d438e6";
  const baseUrl = `${process.env.NEXT_PUBLIC_S2_CUSTOMER_RESOURCE_ENDPOINT}/api/customers/${userid}/dashboard`;
  const url = `${baseUrl}/summary`;
  return await fetch(url, apiOptions("GET"))
    .then(handleResponse)
    .catch(handleError);
};

export default function index(req, res) {
  try {
    getDashboardSummaryItems().then(function (data) {
      res.status(200).json(data);
    });
  } catch (e) {
    console.log(e);
    res.status(500).json(e);
  }
}
