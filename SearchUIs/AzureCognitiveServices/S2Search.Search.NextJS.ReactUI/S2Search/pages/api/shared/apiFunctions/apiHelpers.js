export const FormatCallingHost = (callingHost) => {
  if (callingHost.includes("www.")) {
    callingHost = callingHost.replace(/www./, "");
  }
  if (callingHost.includes("https://")) {
    callingHost = callingHost.replace(/https:\/\//, "");
  }
  if (callingHost.includes("http://")) {
    callingHost = callingHost.replace(/https:\/\//, "");
  }

  // remove any trailing slash
  if (callingHost.charAt(callingHost.length - 1) == "/") {
    callingHost = callingHost.substr(0, callingHost.length - 1);
  }

  return callingHost;
};

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
