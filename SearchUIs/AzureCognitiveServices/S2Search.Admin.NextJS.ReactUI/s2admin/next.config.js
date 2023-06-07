module.exports = {
  reactStrictMode: true,
  serverRuntimeConfig: {
    // Will only be available on the server side
    S2_CUSTOMER_RESOURCE_ENDPOINT: "http://customerresource-api-service",
  },
};
