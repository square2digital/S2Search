export const GenerateFacetArray = (stateArray, facetKey) => {
  if (facetKey === '' || typeof facetKey === 'undefined') {
    return [];
  } else {
    if (!stateArray.includes(facetKey)) {
      return facetKey;
    } else {
      let removeFacetArray = stateArray.filter(function (item) {
        return item !== facetKey;
      });

      return removeFacetArray;
    }
  }
};

export const FormatLongStrings = (input, maxLength) => {
  return input.length > maxLength
    ? `${input.substring(0, maxLength - 3)}...`
    : input;
};

export const DoesValueExistInArray = (array, value) => {
  return array.includes(value);
};

export const RemoveSpacesAndSetToLower = str => {
  return str.replace(/ /g, '').toLocaleLowerCase();
};

export const IsNumeric = str => {
  return /^-?\d+$/.test(str);
};

export const ConvertStringToBoolean = str => {
  return str === 'true';
};

export const FormatStringOrNumeric = str => {
  return IsNumeric(str) === true ? Number.parseInt(str, 0) : `'${str}'`;
};

export const GenerateUniqueID = () => {
  return Math.random().toString(36).substring(2) + Date.now().toString(36);
};

export const GetFacetData = (facetJSONData, facetName) => {
  if (facetJSONData) {
    for (let facet of facetJSONData) {
      if (facet.value === facetName) {
        return facet;
      }
    }
  }

  return [];
};

/// *************************
/// Properties Omitted -
/// *************************
// - NumberOfExistingResults
// - pageSize
export const IsPreviousRequestDataTheSame = (newRequest, reduxRequest) => {
  if (!reduxRequest || Object.keys(reduxRequest).length === 0) {
    return false;
  }

  if (
    JSON.stringify(newRequest.facets) != JSON.stringify(reduxRequest.facets)
  ) {
    LogRequests(
      newRequest,
      reduxRequest,
      `compareRequestData is false - facets are different`
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

const LogRequests = (newRequest, reduxRequest, logString) => {
  if (process.env.NODE_ENV !== 'production') {
    console.log(logString);
    console.dir(newRequest);
    console.dir(reduxRequest);
  }
};

export const LogString = str => {
  if (process.env.NODE_ENV !== 'production') {
    console.log(str);
  }
};

/// *************************
/// Properties Omitted -
/// *************************
// - NumberOfExistingResults
// - pageSize
export const IsRequestReOrderBy = (newRequest, reduxRequest) => {
  if (!reduxRequest) {
    return false;
  }

  if (
    newRequest.orderBy !== reduxRequest.orderBy &&
    JSON.stringify(newRequest.filters) ===
      JSON.stringify(reduxRequest.filters) &&
    newRequest.pageNumber === reduxRequest.pageNumber &&
    newRequest.searchTerm.trim() === reduxRequest.searchTerm.trim()
  ) {
    console.log(`request is just for a OrderBy of results`);
    return true;
  } else {
    return false;
  }
};
