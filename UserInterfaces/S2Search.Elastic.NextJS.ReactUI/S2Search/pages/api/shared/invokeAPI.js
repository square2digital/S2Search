const invokeAPI = async (
  req,
  res,
  type,
  configType,
  addApiKeyHeader,
  cancellation
) => {
  const {
    ClientConfigurationAPIURL,
    AutoCompleteURL,
    SearchAPIDomain,
    SearchAndFacetsAPIURL,
    DocumentCountURL,
    SearchAPIEndpoint,
    FacetsAPIEndpoint,
  } = require("../../../common/Constants");
  const setResponse = require("./response/invovationContext/setResponse");
  const axiosAPICall = require("../helper/AxiosAPICall");

  if (!req.query) {
    return setResponse(
      res,
      `Bad Request - you must pass query string params`,
      400
    );
  }

  if (req.headers.host === undefined) {
    return setResponse(
      res,
      `Bad Request - missing required query string - "index"`,
      400
    );
  } else {
    let url = "";
    let axiosResponse = {};
    const index = req.query.index;

    switch (type) {
      // *****************
      // Search Endpoints
      // *****************

      case "search":
        url = `${SearchAndFacetsAPIURL}${SearchAPIEndpoint}`;
        axiosResponse = await axiosAPICall.AxiosGetWithQueryString(
          req,
          url,
          addApiKeyHeader,
          cancellation
        );
        break;

      case "facets":
        url = `${SearchAndFacetsAPIURL}${FacetsAPIEndpoint}`;
        axiosResponse = await axiosAPICall.AxiosGetWithQueryString(
          req,
          url,
          addApiKeyHeader
        );
        break;

      case "documentCount":
        url = `${SearchAPIDomain}${SearchAPIEndpoint}${DocumentCountURL}?index=${index}`;
        axiosResponse = await axiosAPICall.AxiosGet(url, addApiKeyHeader);
        break;

      case "autoSuggest":
        url = `${SearchAPIDomain}${SearchAPIEndpoint}${AutoCompleteURL}?SearchTerm=${req.query.SearchTerm}&index=${index}`;
        axiosResponse = await axiosAPICall.AxiosGet(
          url,
          addApiKeyHeader,
          cancellation
        );
        break;

      // *****************
      // configuration Endpoints
      // *****************
      case "configuration":
        url = `${ClientConfigurationAPIURL}/api/configuration/${configType}/${index}`;
        axiosResponse = await axiosAPICall.AxiosGet(url, addApiKeyHeader);
        break;

      case "theme":
        url = `${ClientConfigurationAPIURL}/api/configuration/${configType}/${index}`;
        axiosResponse = await axiosAPICall.AxiosGet(url, addApiKeyHeader);
        break;
    }

    console.log(`url - ${url}`);
    console.log(`addApiKeyHeader - ${addApiKeyHeader}`, addApiKeyHeader);
    if (cancellation !== undefined) {
      console.log(`cancellation - ${cancellation}`);
    }

    return axiosResponse;
  }
};

exports.invokeAPI = invokeAPI;
