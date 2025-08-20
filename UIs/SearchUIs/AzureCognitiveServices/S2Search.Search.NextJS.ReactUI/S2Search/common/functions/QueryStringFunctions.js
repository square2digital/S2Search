export const getQueryStringSearchTerm = () => {
  return getString('searchterm');
};

export const getQueryStringOrderBy = () => {
  return getString('orderby');
};

const getString = propertyName => {
  const params = getQueryStringParams();

  const value = params[propertyName];

  if (value !== undefined && value !== null) {
    return params[propertyName];
  }

  return '';
};

export const getQueryStringFacetsSelectors = () => {
  const params = getQueryStringParams();

  if (params['facetselectors']) {
    const jsonString = params['facetselectors'];
    const object = JSON.parse(jsonString);

    return object;
  }

  return [];
};

export const removeFullQueryString = () => {
  const newurl = window.location.href.split('?')[0];
  window.history.pushState({ path: newurl }, '', newurl);
};

export const insertQueryStringParam = (key, value) => {
  if (history.pushState) {
    let currentUrlWithOutHash =
      window.location.origin +
      window.location.pathname +
      window.location.search;
    let hash = window.location.hash;

    currentUrlWithOutHash = removeURLParameter(currentUrlWithOutHash, key);

    let queryStart;
    if (currentUrlWithOutHash.indexOf('?') !== -1) {
      queryStart = '&';
    } else {
      queryStart = '?';
    }

    let newurl = '';

    if (value !== '') {
      newurl = currentUrlWithOutHash + queryStart + key + '=' + value + hash;
      window.history.pushState({ path: newurl }, '', newurl);
    } else {
      newurl = currentUrlWithOutHash;
      window.history.pushState({ path: newurl }, '', newurl);
    }
  }
};

const getQueryStringParams = () => {
  if (typeof window !== 'undefined') {
    // Client-side-only code
    const urlSearchParams = new URLSearchParams(window.location.search);
    const params = Object.fromEntries(urlSearchParams.entries());

    return params;
  }

  return '';
};

const removeURLParameter = (url, parameter) => {
  let urlParts = url.split('?');

  if (urlParts.length >= 2) {
    let urlBase = urlParts.shift();
    let queryString = urlParts.join('?');
    let prefix = encodeURIComponent(parameter) + '=';
    let parts = queryString.split(/[&;]/g);

    for (let i = parts.length; i-- > 0; ) {
      if (parts[i].lastIndexOf(prefix, 0) !== -1) {
        parts.splice(i, 1);
      }
    }

    url = urlBase + '?' + parts.join('&');
  }

  return url;
};
