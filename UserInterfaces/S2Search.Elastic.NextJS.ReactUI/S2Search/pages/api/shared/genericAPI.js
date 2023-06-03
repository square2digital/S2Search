export const genericAPI = async (
  req,
  res,
  type,
  configType,
  addApiKeyHeader,
  cancellation
) => {
  const { invokeAPI } = require("../shared/invokeAPI");
  const { GenerateAPIError } = require("../shared/apiFunctions/apiHelpers");
  const axiosResponse = await invokeAPI(
    req,
    res,
    type,
    configType,
    addApiKeyHeader,
    cancellation
  );

  if (axiosResponse) {
    if (axiosResponse.status === 200) {
      res.statusCode = axiosResponse.status;
      res.end(JSON.stringify(axiosResponse.data));
    } else {
      res.statusCode = axiosResponse.response.status;
      res.end(GenerateAPIError(axiosResponse.response));
    }
  } else {
    res.statusCode = 500;
    res.end(
      `error calling API for ${type} - axiosResponse is null or undefined`
    );
  }
};
