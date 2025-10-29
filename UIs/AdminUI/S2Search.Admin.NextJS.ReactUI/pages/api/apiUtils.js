export async function handleResponse(response) {
  return response.json();
}

// In a real app, would likely call an error logging service.
export function handleError(error) {
  // eslint-disable-next-line no-console
  console.error("API call failed. " + error);
}

function jsonContentTypeHeader() {
  return {
    "Content-type": "application/json; charset=UTF-8",
  };
}

export function apiOptions(method) {
  return {
    method: `${method}`,
    headers: {
      ...jsonContentTypeHeader(),
    },
  };
}

export function apiOptionsWithData(method, data) {
  return {
    method: `${method}`,
    headers: {
      ...jsonContentTypeHeader(),
    },
    body: JSON.stringify(data),
  };
}

export function apiOptionsWithFormData(method, formData) {
  return {
    method: `${method}`,
    body: formData,
  };
}
