const axios = require("axios");

const configWithApiKey = () => {
  const key = getApiSubscriptionKey();

  return {
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      "Ocp-Apim-Subscription-Key": key,
    },
  };
};

const configNoApiKey = {
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
  },
};

const cancelRequest = (configHeaders) => {
  const source = axios.CancelToken.source();
  const cancelToken = source.token;
  let axiosConfig = {};

  // To cancel the request, call `cancel()` on the `axios.CancelToken.source()`.
  source.cancel("axios cancellation from token");

  if (configHeaders) {
    axiosConfig = { cancelToken, ...configHeaders };
  } else {
    axiosConfig = { cancelToken };
  }

  return axiosConfig;
};

const getApiSubscriptionKey = () => {
  let ApiKey = process.env.NEXT_PUBLIC_OCP_APIM_SUBSCRIPTION_KEY;

  if (ApiKey === undefined) {
    return "";
  } else {
    return ApiKey;
  }
};

const getSearchQueryParams = (request) => {
  if (request.query === undefined) {
    return `?searchTerm=${request.searchTerm}&filters=${request.filters}&orderBy=${request.orderBy}&pageNumber=${request.pageNumber}&pageSize=${request.pageSize}&numberOfExistingResults=${request.numberOfExistingResults}&callingHost=${request.callingHost}`;
  } else {
    return `?searchTerm=${request.query.searchTerm}&filters=${request.query.filters}&orderBy=${request.query.orderBy}&pageNumber=${request.query.pageNumber}&pageSize=${request.query.pageSize}&numberOfExistingResults=${request.query.numberOfExistingResults}&callingHost=${request.query.callingHost}`;
  }
};

const AxiosPost = async (request, url) => {
  const postBody = JSON.stringify(request);

  return await axios
    .post(url, postBody, configWithApiKey())
    .then(function (response) {
      return response;
    })
    .catch(function (error) {
      return error;
    });
};

const AxiosGet = async (url, addApiKeyHeader, cancellation) => {
  return await axiosCall(
    url,
    addApiKeyHeader ? configWithApiKey() : configNoApiKey,
    cancellation
  );
};

const AxiosGetWithQueryString = async (
  queryStrings,
  url,
  addApiKeyHeader,
  cancellation
) => {
  const endpoint = `${url}/${getSearchQueryParams(queryStrings)}`;
  return await axiosCall(
    endpoint,
    addApiKeyHeader ? configWithApiKey() : configNoApiKey,
    cancellation
  );
};

const axiosCall = async (url, configHeaders, cancellation) => {
  let params = configHeaders;
  if (cancellation) {
    params = cancelRequest(configHeaders);
  }

  return await axios
    .get(url, params)
    .then(function (response) {
      return response;
    })
    .catch(function (error) {
      if (axios.isCancel(error)) {
        console.log("Request canceled", error.message);
      } else {
        console.log(`error on axiosCall - url ${url}`, error, configHeaders);
      }
      return error;
    });
};

exports.AxiosPost = AxiosPost;
exports.AxiosGet = AxiosGet;
exports.AxiosGetWithQueryString = AxiosGetWithQueryString;
