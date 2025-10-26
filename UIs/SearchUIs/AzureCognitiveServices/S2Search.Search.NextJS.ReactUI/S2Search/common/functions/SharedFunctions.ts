interface SearchRequest {
  facets?: string[];
  filters?: string[];
  orderBy: string;
  pageNumber: number;
  searchTerm: string;
}

interface FacetData {
  value: string;
  [key: string]: any;
}

export const GenerateFacetArray = (stateArray: string[], facetKey: string): string | string[] => {
  if (facetKey === '' || typeof facetKey === 'undefined') {
    return [];
  } else {
    if (!stateArray.includes(facetKey)) {
      return facetKey;
    } else {
      const removeFacetArray = stateArray.filter(function (item) {
        return item !== facetKey;
      });

      return removeFacetArray;
    }
  }
};

export const FormatLongStrings = (input: string, maxLength: number): string => {
  return input.length > maxLength
    ? `${input.substring(0, maxLength - 3)}...`
    : input;
};

export const DoesValueExistInArray = (array: any[], value: any): boolean => {
  return array.includes(value);
};

export const RemoveSpacesAndSetToLower = (str: string): string => {
  return str.replace(/ /g, '').toLocaleLowerCase();
};

export const IsNumeric = (str: string): boolean => {
  return /^-?\d+$/.test(str);
};

export const ConvertStringToBoolean = (str: string): boolean => {
  return str === 'true';
};

export const FormatStringOrNumeric = (str: string): number | string => {
  return IsNumeric(str) === true ? Number.parseInt(str, 10) : `'${str}'`;
};

export const GenerateUniqueID = (): string => {
  return Math.random().toString(36).substring(2) + Date.now().toString(36);
};

export const GetFacetData = (facetJSONData: FacetData[], facetName: string): FacetData | undefined => {
  if (facetJSONData) {
    for (const facet of facetJSONData) {
      if (facet.value === facetName) {
        return facet;
      }
    }
  }

  return undefined;
};

/// *************************
/// Properties Omitted -
/// *************************
// - NumberOfExistingResults
// - pageSize
export const IsPreviousRequestDataTheSame = (
  newRequest: SearchRequest, 
  reduxRequest: SearchRequest | null
): boolean => {
  if (!reduxRequest || Object.keys(reduxRequest).length === 0) {
    return false;
  }

  // Check both facets and filters properties for backward compatibility
  // Some requests use 'facets' (newer frontend) and some use 'filters' (Redux state/legacy)
  const newRequestFacets = newRequest.facets || newRequest.filters || [];
  const reduxRequestFacets = reduxRequest.facets || reduxRequest.filters || [];

  if (JSON.stringify(newRequestFacets) != JSON.stringify(reduxRequestFacets)) {
    LogRequests(
      newRequest,
      reduxRequest,
      `compareRequestData is false - facets/filters are different`
    );
    return false;
  }

  if (newRequest.orderBy !== reduxRequest.orderBy) {
    LogRequests(
      newRequest,
      reduxRequest,
      `compareRequestData is false - orderBy are different`
    );
    return false;
  }

  if (newRequest.pageNumber !== reduxRequest.pageNumber) {
    LogRequests(
      newRequest,
      reduxRequest,
      `compareRequestData is false - pageNumber are different`
    );
    return false;
  }

  if (newRequest.searchTerm.trim() !== reduxRequest.searchTerm.trim()) {
    LogRequests(
      newRequest,
      reduxRequest,
      `compareRequestData is false - searchTerm are different`
    );
    return false;
  }

  return true;
};

const LogRequests = (
  newRequest: SearchRequest, 
  reduxRequest: SearchRequest, 
  logString: string
): void => {
  if (process.env.NODE_ENV !== 'production') {
    // eslint-disable-next-line no-console
    console.log(logString);
    // eslint-disable-next-line no-console
    console.dir(newRequest);
    // eslint-disable-next-line no-console
    console.dir(reduxRequest);
  }
};

export const LogString = (str: string): void => {
  if (process.env.NODE_ENV !== 'production') {
    // eslint-disable-next-line no-console
    console.log(str);
  }
};

/// *************************
/// Properties Omitted -
/// *************************
// - NumberOfExistingResults
// - pageSize
export const IsRequestReOrderBy = (
  newRequest: SearchRequest, 
  reduxRequest: SearchRequest | null
): boolean => {
  if (!reduxRequest) {
    return false;
  }

  // Check both facets and filters properties for backward compatibility
  const newRequestFacets = newRequest.facets || newRequest.filters || [];
  const reduxRequestFacets = reduxRequest.facets || reduxRequest.filters || [];

  if (
    newRequest.orderBy !== reduxRequest.orderBy &&
    JSON.stringify(newRequestFacets) === JSON.stringify(reduxRequestFacets) &&
    newRequest.pageNumber === reduxRequest.pageNumber &&
    newRequest.searchTerm.trim() === reduxRequest.searchTerm.trim()
  ) {
    // eslint-disable-next-line no-console
    console.log(`request is just for a OrderBy of results`);
    return true;
  } else {
    return false;
  }
};