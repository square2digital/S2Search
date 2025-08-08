// Helper function to build query string from request object
const buildQueryString = (request) => {
  const params = new URLSearchParams();

  if (request.query) {
    // If request has a query property, use those values
    Object.keys(request.query).forEach((key) => {
      if (request.query[key] !== undefined) {
        params.append(key, request.query[key]);
      }
    });
  } else {
    // Otherwise use direct properties
    if (request.searchTerm !== undefined)
      params.append("searchTerm", request.searchTerm);
    if (request.filters !== undefined)
      params.append("filters", request.filters);
    if (request.orderBy !== undefined)
      params.append("orderBy", request.orderBy);
    if (request.pageNumber !== undefined)
      params.append("pageNumber", request.pageNumber);
    if (request.pageSize !== undefined)
      params.append("pageSize", request.pageSize);
    if (request.numberOfExistingResults !== undefined)
      params.append("numberOfExistingResults", request.numberOfExistingResults);
    if (request.callingHost !== undefined)
      params.append("callingHost", request.callingHost);
  }

  return params.toString();
};

// Helper function to make fetch calls
const fetchAPI = async (url, options = {}) => {
  try {
    const response = await fetch(url, {
      method: "GET",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      ...options,
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
    console.log(`error on API call - url ${url}`, error);
    return {
      error: error.message,
      isError: true,
    };
  }
};

export const SearchAPI = async (request, cancellationToken) => {
  const queryString = buildQueryString(request);
  const url = `/api/search?${queryString}`;

  // Note: For now, ignoring cancellationToken as fetch doesn't have the same cancellation setup
  // You can implement AbortController if needed
  return await fetchAPI(url);
};

export const SearchAndFacetsAPI = async (request, cancellationToken) => {
  const queryString = buildQueryString(request);
  const url = `/api/search?${queryString}`;

  return await fetchAPI(url);
};

export const AutoSuggestAPI = async (
  searchTerm,
  callingHost,
  cancellationToken
) => {
  const url = `/api/autoSuggest?SearchTerm=${encodeURIComponent(
    searchTerm
  )}&callingHost=${encodeURIComponent(callingHost)}`;
  return await fetchAPI(url);
};

export const DocumentCountAPI = async (callingHost) => {
  const url = `/api/documentCount?callingHost=${encodeURIComponent(
    callingHost
  )}`;
  return await fetchAPI(url);
};

const setCancellationToken = (cancellationToken) => {
  if (process.env.NEXT_PUBLIC_ENABLE_CANCELATION_TOKEN === "false") {
    return false;
  }

  console.log(
    `NEXT_PUBLIC_ENABLE_CANCELATION_TOKEN = ${process.env.NEXT_PUBLIC_ENABLE_CANCELATION_TOKEN}`
  );

  return cancellationToken;
};
