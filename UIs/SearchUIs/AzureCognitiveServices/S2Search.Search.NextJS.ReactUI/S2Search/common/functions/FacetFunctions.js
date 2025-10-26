import React from 'react';
import CheckIcon from '@mui/icons-material/Check';
import { RemoveSpacesAndSetToLower } from './SharedFunctions';

export const setSelectedFacetButton = (facetName, reduxFacetSelectors) => {
  if (isFacetKeyButtonSelected(facetName, reduxFacetSelectors)) {
    return <CheckIcon style={{ color: 'green' }} />;
  }
};

export const isFacetKeyButtonSelected = (facetName, reduxFacetSelectors) => {
  if (reduxFacetSelectors && reduxFacetSelectors.length > 0) {
    return isFacetSelected(facetName, reduxFacetSelectors);
  }
};

const isFacetSelected = (facetName, reduxFacetSelectors) => {
  const facetSelector = reduxFacetSelectors.filter(
    facet =>
      RemoveSpacesAndSetToLower(facet.facetKey) ===
        RemoveSpacesAndSetToLower(facetName) && facet.checked === true
  );

  if (facetSelector.length > 0) {
    return true;
  } else {
    return false;
  }
};

export const isAnyFacetSelected = reduxFacetSelectors => {
  let facetSelected = false;

  reduxFacetSelectors.filter(facet => {
    if (facet.enabled === true) {
      facetSelected = true;
    }
  });

  return facetSelected;
};

export const getSelectedFacets = reduxFacetSelectors => {
  const requestFilters = [];

  // DEBUG: Log the facet selectors being processed
  console.log(
    '🔍 getSelectedFacets: Input reduxFacetSelectors:',
    reduxFacetSelectors
  );

  if (reduxFacetSelectors !== undefined) {
    for (let item of reduxFacetSelectors) {
      console.log(
        '🔍 getSelectedFacets: Processing item:',
        item,
        'checked:',
        item.checked
      );
      if (item.checked) {
        requestFilters.push(item.luceneExpression);
        console.log(
          '🔍 getSelectedFacets: Added lucene expression:',
          item.luceneExpression
        );
      }
    }
  }

  console.log('🔍 getSelectedFacets: Final requestFilters:', requestFilters);
  return requestFilters;
};

export const getDefaultFacetsWithSelections = (
  facetKeyName,
  reduxDefaultFacets,
  reduxFacetSelectors
) => {
  const defaultFacetsCopy = [...reduxDefaultFacets];
  const defaultFacetByKey = defaultFacetsCopy.filter(
    x => x.facetKey === facetKeyName
  );

  for (const selectedFacetData of reduxFacetSelectors) {
    if (selectedFacetData.facetKey === defaultFacetByKey.facetKey) {
      for (const facets of defaultFacetByKey.facetItems) {
        if (facets.facetDisplayText === selectedFacetData.facetDisplayText) {
          facets.checked = true;
          break;
        }
      }
    }
  }

  return [...defaultFacetsCopy];
};

export const isSelectFacetMenuAlreadySelected = (
  reduxFacetSelectors,
  facetKeyName
) => {
  const filter = reduxFacetSelectors.filter(x => x.facetKey === facetKeyName);

  if (filter.length === 0) {
    return false;
  }

  return true;
};
