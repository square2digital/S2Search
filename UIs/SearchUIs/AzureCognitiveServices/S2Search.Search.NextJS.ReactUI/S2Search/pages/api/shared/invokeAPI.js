const invokeAPI = async (
  req,
  res,
  type,
  configType,
  addApiKeyHeader,
  cancellation
) => {
  const {
    SearchAPIDomain,
    AutoCompleteURL,
    DocumentCountURL,
    SearchAPIEndpoint,
    FacetsAPIEndpoint,
  } = require('../../../common/Constants');
  const { FormatCallingHost } = require('./apiFunctions/apiHelpers');
  const setResponse = require('./response/invovationContext/setResponse');
  const axiosAPICall = require('../helper/AxiosAPICall');

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
      `Bad Request - missing required query string - "callingHost"`,
      400
    );
  } else {
    let url = '';
    let axiosResponse = {};
    const callingHost = FormatCallingHost(req.headers.host);

    switch (type) {
      // *****************
      // Search Endpoints
      // *****************

      case 'search':
        url = `${SearchAPIDomain}${SearchAPIEndpoint}`;
        axiosResponse = await axiosAPICall.AxiosGetWithQueryString(
          req,
          url,
          addApiKeyHeader,
          cancellation
        );
        break;

      case 'facets':
        url = `${SearchAPIDomain}${FacetsAPIEndpoint}`;
        axiosResponse = await axiosAPICall.AxiosGetWithQueryString(
          req,
          url,
          addApiKeyHeader
        );
        break;

      case 'documentCount':
        url = `${SearchAPIDomain}${SearchAPIEndpoint}${DocumentCountURL}?callingHost=${callingHost}`;
        axiosResponse = await axiosAPICall.AxiosGet(url, addApiKeyHeader);
        break;

      case 'autoSuggest':
        url = `${SearchAPIDomain}${SearchAPIEndpoint}${AutoCompleteURL}?SearchTerm=${req.query.SearchTerm}&callingHost=${callingHost}`;
        axiosResponse = await axiosAPICall.AxiosGet(
          url,
          addApiKeyHeader,
          cancellation
        );
        break;

      // *****************
      // configuration Endpoints
      // *****************
      case 'configuration':
        url = `${SearchAPIDomain}/api/configuration/${configType}/${callingHost}`;
        axiosResponse = await axiosAPICall.AxiosGet(url, addApiKeyHeader);
        break;

      case 'theme':
        url = `${SearchAPIDomain}/api/configuration/${configType}/${callingHost}`;
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
