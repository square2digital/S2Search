import React from 'react';
import CheckIcon from '@mui/icons-material/Check';
import { RemoveSpacesAndSetToLower } from './SharedFunctions';

// Interface definitions
interface FacetSelector {
  facetKey: string;
  facetDisplayText: string;
  luceneExpression: string;
  checked: boolean;
  enabled?: boolean;
}

interface FacetItem {
  facetDisplayText: string;
  checked?: boolean;
  value?: string | number;
}

interface DefaultFacet {
  facetKey: string;
  facetItems: FacetItem[];
}

export const setSelectedFacetButton = (
  facetName: string, 
  reduxFacetSelectors: FacetSelector[]
): React.ReactElement | undefined => {
  if (isFacetKeyButtonSelected(facetName, reduxFacetSelectors)) {
    return <CheckIcon style={{ color: 'green' }} />;
  }
  return undefined;
};

export const isFacetKeyButtonSelected = (
  facetName: string, 
  reduxFacetSelectors: FacetSelector[]
): boolean => {
  if (reduxFacetSelectors && reduxFacetSelectors.length > 0) {
    return isFacetSelected(facetName, reduxFacetSelectors);
  }
  return false;
};

const isFacetSelected = (
  facetName: string, 
  reduxFacetSelectors: FacetSelector[]
): boolean => {
  const facetSelector = reduxFacetSelectors.filter(
    facet =>
      RemoveSpacesAndSetToLower(facet.facetKey) ===
        RemoveSpacesAndSetToLower(facetName) && facet.checked === true
  );

  return facetSelector.length > 0;
};

export const isAnyFacetSelected = (reduxFacetSelectors: FacetSelector[]): boolean => {
  let facetSelected = false;

  reduxFacetSelectors.filter(facet => {
    if (facet.enabled === true) {
      facetSelected = true;
    }
  });

  return facetSelected;
};

export const getSelectedFacets = (reduxFacetSelectors: FacetSelector[]): string[] => {
  const requestFilters: string[] = [];

  if (reduxFacetSelectors !== undefined) {
    for (const item of reduxFacetSelectors) {
      if (item.checked) {
        requestFilters.push(item.luceneExpression);
      }
    }
  }

  return requestFilters;
};

export const getDefaultFacetsWithSelections = (
  facetKeyName: string,
  reduxDefaultFacets: DefaultFacet[],
  reduxFacetSelectors: FacetSelector[]
): DefaultFacet[] => {
  // Create a deep copy of the defaultFacets to avoid mutating the original objects
  const defaultFacetsCopy = reduxDefaultFacets.map(facet => ({
    ...facet,
    facetItems: facet.facetItems.map(item => ({ ...item }))
  }));
  
  const defaultFacetByKey = defaultFacetsCopy.filter(
    x => x.facetKey === facetKeyName
  );

  for (const selectedFacetData of reduxFacetSelectors) {
    if (selectedFacetData.facetKey === defaultFacetByKey[0]?.facetKey) {
      for (const facets of defaultFacetByKey[0].facetItems) {
        if (facets.facetDisplayText === selectedFacetData.facetDisplayText) {
          facets.checked = true;
          break;
        }
      }
    }
  }

  return defaultFacetsCopy;
};

export const isSelectFacetMenuAlreadySelected = (
  reduxFacetSelectors: FacetSelector[],
  facetKeyName: string
): boolean => {
  const filter = reduxFacetSelectors.filter(x => x.facetKey === facetKeyName);

  return filter.length > 0;
};