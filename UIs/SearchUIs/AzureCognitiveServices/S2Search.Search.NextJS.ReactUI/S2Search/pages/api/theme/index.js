const { GenerateAPIError } = require('../shared/apiFunctions/apiHelpers');
const { DefaultTheme } = require('../../../common/Constants');

export default async function handler(req, res) {
  const { invokeAPI } = require('../shared/invokeAPI');
  const axiosResponse = await invokeAPI(req, res, 'theme', 'theme', true);

  if (axiosResponse) {
    if (axiosResponse.status === 200) {
      res.statusCode = axiosResponse.status;
      res.end(JSON.stringify(axiosResponse.data));
    } else {
      res.statusCode = 200;
      const errors = GenerateAPIError(axiosResponse.response);
      console.log(
        `error calling Theme API - ErrorMessage: ${errors.errorMessage} StatusCode: ${errors.statusCode}`
      );
      res.end(JSON.stringify(DefaultTheme));
    }
  } else {
    res.statusCode = 200;
    console.log(`error calling Theme API - axiosResponse is null or undefined`);
    res.end(JSON.stringify(DefaultTheme));
  }
}
