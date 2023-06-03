export const GenerateAPIError = (data) => {
  let errorMessage = "";
  let statusCode = data.status;

  if (data) {
    errorMessage = `statusText - ${data.statusText}`;
    statusCode = data.status;
  } else {
    errorMessage = "GenerateAPIError - data is null or undefined";
  }

  console.log(`errorMessage - ${errorMessage}`);
  console.log(`statusCode - ${statusCode}`);

  return {
    errorMessage,
    statusCode,
  };
};
