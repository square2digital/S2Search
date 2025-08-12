const ConfigAPI = async hostParam => {
  let URL = `/api/configuration`;
  if (hostParam) {
    URL += `?host=${encodeURIComponent(hostParam)}`;
  }

  try {
    const response = await fetch(URL, {
      method: 'GET',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
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
    console.log(`error on ConfigAPI - url ${URL}`, error);
    return {
      error: error.message,
      isError: true,
    };
  }
};

export default ConfigAPI;
