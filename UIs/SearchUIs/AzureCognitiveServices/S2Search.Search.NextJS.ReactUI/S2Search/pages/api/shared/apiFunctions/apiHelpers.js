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
  let errorMessage = 'An unknown API error occurred.';
  let statusCode = 500; // Default to a generic server error code

  // 1. Check for a standard HTTP error response (server responded with a 4xx/5xx code)
  if (data && data.response) {
    statusCode = data.response.status;
    errorMessage = `HTTP Status Error: ${data.response.status} - ${data.response.statusText || 'No Status Text'}`;
  }
  // 2. Check for a network or request error (like ECONNREFUSED)
  else if (data && data.code) {
    // Axios network errors (ECONNREFUSED, ENOTFOUND, etc.)
    statusCode = 503; // Use 503 Service Unavailable for connectivity issues
    errorMessage = `Network Error: ${data.code} - Cannot connect to backend service.`;
  }
  // 3. Fallback for generic errors (e.g., if data is not an Axios error, but a simple Error object)
  else if (data && data.message) {
    errorMessage = data.message;
  }
  // 4. Fallback for null/undefined 'data' (this handles your original 'else' case)
  else {
    errorMessage = 'GenerateAPIError - data is null or undefined';
    statusCode = 500;
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
