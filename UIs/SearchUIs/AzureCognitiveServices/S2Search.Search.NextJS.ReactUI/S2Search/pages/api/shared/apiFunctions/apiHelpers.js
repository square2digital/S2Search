const FormatCallingHost = callingHost => {
  // In development mode, use the dev customer endpoint instead of localhost
  if (
    process.env.NODE_ENV === 'development' &&
    callingHost &&
    (callingHost.includes('localhost') || callingHost.includes('127.0.0.1'))
  ) {
    return process.env.NEXT_PUBLIC_DEV_CUSTOMER_ENDPOINT || 'devtest';
  }

  if (callingHost.includes('www.')) {
    callingHost = callingHost.replace(/www./, '');
  }
  if (callingHost.includes('https://')) {
    callingHost = callingHost.replace(/https:\/\//, '');
  }
  if (callingHost.includes('http://')) {
    callingHost = callingHost.replace(/http:\/\//, '');
  }

  // remove any trailing slash
  if (callingHost.charAt(callingHost.length - 1) == '/') {
    callingHost = callingHost.substr(0, callingHost.length - 1);
  }

  return callingHost;
};

const GenerateAPIError = data => {
  let errorMessage = '';
  let statusCode = data.status;

  if (data) {
    errorMessage = `statusText - ${data.statusText}`;
    statusCode = data.status;
  } else {
    errorMessage = 'GenerateAPIError - data is null or undefined';
  }

  console.log(`errorMessage - ${errorMessage}`);
  console.log(`statusCode - ${statusCode}`);

  return {
    errorMessage,
    statusCode,
  };
};

module.exports = {
  FormatCallingHost,
  GenerateAPIError,
};
