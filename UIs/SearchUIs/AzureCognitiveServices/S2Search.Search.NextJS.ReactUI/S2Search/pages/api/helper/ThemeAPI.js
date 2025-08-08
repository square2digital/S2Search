const ThemeAPI = async (req, res) => {
  // Check if we're running on the server side (SSR) or client side
  if (typeof window === "undefined") {
    // Server-side: Call the API handler directly
    const { DefaultTheme } = require("../../../common/Constants");
    const { invokeAPI } = require("../shared/invokeAPI");

    try {
      // Create a mock request object if not provided
      const mockReq = req || {
        headers: { host: "localhost:2997" },
        query: {},
      };
      const mockRes = res || {};

      const axiosResponse = await invokeAPI(
        mockReq,
        mockRes,
        "theme",
        "theme",
        true
      );

      if (axiosResponse && axiosResponse.status === 200) {
        return {
          status: 200,
          data: axiosResponse.data,
        };
      } else {
        // Return default theme on error
        return {
          status: 200,
          data: DefaultTheme,
        };
      }
    } catch (error) {
      console.log(`error on ThemeAPI (server-side)`, error);
      const { DefaultTheme } = require("../../../common/Constants");
      return {
        status: 200,
        data: DefaultTheme,
      };
    }
  } else {
    // Client-side: Use fetch to call the API route
    const URL = "/api/theme";

    try {
      const response = await fetch(URL, {
        method: "GET",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        const data = await response.json();
        return {
          status: response.status,
          data: data,
        };
      } else {
        return {
          status: response.status,
          error: `HTTP error! status: ${response.status}`,
        };
      }
    } catch (error) {
      console.log(`error on ThemeAPI - url ${URL}`, error);
      return {
        error: error.message,
        isError: true,
      };
    }
  }
};

export default ThemeAPI;
