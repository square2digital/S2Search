interface FacetSelector {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
}

export const getQueryStringSearchTerm = (): string => {
  return getString('searchterm');
};

export const getQueryStringOrderBy = (): string => {
  return getString('orderby');
};

const getString = (propertyName: string): string => {
  const params = getQueryStringParams();

  const value = params[propertyName];

  if (value !== undefined && value !== null) {
    return params[propertyName];
  }

  return '';
};

export const getQueryStringFacetsSelectors = (): FacetSelector[] => {
  const params = getQueryStringParams();

  if (params['facetselectors']) {
    const jsonString = params['facetselectors'];
    try {
      const object = JSON.parse(jsonString);
      return object;
    } catch (error) {
      console.warn('Failed to parse facetselectors from query string:', error);
      return [];
    }
  }

  return [];
};

export const removeFullQueryString = (): void => {
  if (typeof window !== 'undefined') {
    const newurl = window.location.href.split('?')[0];
    window.history.pushState({ path: newurl }, '', newurl);
  }
};

export const insertQueryStringParam = (key: string, value: string): void => {
  if (typeof window !== 'undefined' && window.history && window.history.pushState) {
    let currentUrlWithOutHash =
      window.location.origin +
      window.location.pathname +
      window.location.search;
    const hash = window.location.hash;

    currentUrlWithOutHash = removeURLParameter(currentUrlWithOutHash, key);

    let queryStart: string;
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

const getQueryStringParams = (): Record<string, string> => {
  if (typeof window !== 'undefined') {
    // Client-side-only code
    const urlSearchParams = new URLSearchParams(window.location.search);
    const params = Object.fromEntries(urlSearchParams.entries());

    return params;
  }

  return {};
};

const removeURLParameter = (url: string, parameter: string): string => {
  const urlParts = url.split('?');

  if (urlParts.length >= 2) {
    const urlBase = urlParts.shift();
    const queryString = urlParts.join('?');
    const prefix = encodeURIComponent(parameter) + '=';
    const parts = queryString.split(/[&;]/g);

    for (let i = parts.length; i-- > 0; ) {
      if (parts[i].lastIndexOf(prefix, 0) !== -1) {
        parts.splice(i, 1);
      }
    }

    url = urlBase + '?' + parts.join('&');
  }

  return url;
};