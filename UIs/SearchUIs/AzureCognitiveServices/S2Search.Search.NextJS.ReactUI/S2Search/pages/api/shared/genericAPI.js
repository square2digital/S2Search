export const genericAPI = async (
  req,
  res,
  type,
  configType,
  addApiKeyHeader,
  cancellation
) => {
  const { invokeAPI } = require('../shared/invokeAPI');
  const { GenerateAPIError } = require('../shared/apiFunctions/apiHelpers');

  try {
    const axiosResponse = await invokeAPI(
      req,
      res,
      type,
      configType,
      addApiKeyHeader,
      cancellation
    );

    if (axiosResponse) {
      // Check if this is a successful response
      if (axiosResponse.status === 200) {
        res.statusCode = axiosResponse.status;
        res.end(JSON.stringify(axiosResponse.data));
      }
      // Check if this is an axios error with a response
      else if (axiosResponse.response && axiosResponse.response.status) {
        console.log(`error - ${axiosResponse.message || 'API Error'}`);
        res.statusCode = axiosResponse.response.status;
        res.end(GenerateAPIError(axiosResponse.response));
      }
      // Handle axios errors without response (like network errors, SSL errors)
      else if (axiosResponse.code) {
        console.log(`error - ${axiosResponse.message || axiosResponse.code}`);
        res.statusCode = 500;
        res.end(
          JSON.stringify({
            error: axiosResponse.message || axiosResponse.code,
            details: `Network or connection error for ${type} API call`,
          })
        );
      }
      // Handle other types of errors
      else {
        console.log(`error - Unexpected response format`, axiosResponse);
        res.statusCode = 500;
        res.end(
          JSON.stringify({
            error: 'Unexpected response format',
            details: `Error calling ${type} API`,
          })
        );
      }
    } else {
      res.statusCode = 500;
      res.end(
        `error calling API for ${type} - axiosResponse is null or undefined`
      );
    }
  } catch (error) {
    console.log(`error - ${error.message}`);
    res.statusCode = 500;
    res.end(
      JSON.stringify({
        error: error.message,
        details: `Exception occurred calling ${type} API`,
      })
    );
  }
};
