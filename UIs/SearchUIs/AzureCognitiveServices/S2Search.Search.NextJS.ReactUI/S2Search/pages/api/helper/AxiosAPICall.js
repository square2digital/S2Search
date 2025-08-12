const axios = require('axios');
const https = require('https');

// Create an HTTPS agent that ignores SSL certificate errors for development
const httpsAgent = new https.Agent({
  rejectUnauthorized: false, // Only use this in development!
});

const configWithApiKey = () => {
  const key = getApiSubscriptionKey();

  return {
    headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
      'Ocp-Apim-Subscription-Key': key,
    },
    httpsAgent: httpsAgent, // Add HTTPS agent to ignore self-signed certificates
  };
};

const configNoApiKey = {
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
  httpsAgent: httpsAgent, // Add HTTPS agent to ignore self-signed certificates
};

const cancelRequest = configHeaders => {
  const source = axios.CancelToken.source();
  const cancelToken = source.token;
  let axiosConfig = {};

  // To cancel the request, call `cancel()` on the `axios.CancelToken.source()`.
  source.cancel('axios cancellation from token');

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
    return '';
  } else {
    return ApiKey;
  }
};

const getSearchQueryParams = request => {
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
      console.log(`error on AxiosPost - url ${url}`, error);
      return {
        code: error.code || 'UNKNOWN_ERROR',
        message: error.message,
        response: error.response,
        isError: true,
      };
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
        console.log('Request canceled', error.message);
        return {
          code: 'REQUEST_CANCELLED',
          message: 'Request was cancelled',
          cancelled: true,
        };
      } else {
        console.log(`error on axiosCall - url ${url}`, error, configHeaders);

        // Return a structured error object that genericAPI can handle
        return {
          code: error.code || 'UNKNOWN_ERROR',
          message: error.message,
          response: error.response,
          isError: true,
        };
      }
    });
};

exports.AxiosPost = AxiosPost;
exports.AxiosGet = AxiosGet;
exports.AxiosGetWithQueryString = AxiosGetWithQueryString;
