import { removeFullQueryString } from '../../../../common/functions/QueryStringFunctions';

const ForbiddenSearchBarCharacters = '[]{}#~¬$£"%^&@';

export const isCharForbidden = (searchTerm: string): boolean => {
  const char = searchTerm.slice(-1);
  const result = ForbiddenSearchBarCharacters.indexOf(char) > -1;

  return result;
};

interface SearchBarProps {
  reduxSearchTerm: string;
  saveSearchTerm: (term: string) => void;
  saveVehicleData: (data: any[]) => void;
  saveFacetSelectors: (selectors: any[]) => void;
  saveFacetSelectedKeys: (keys: any[]) => void;
  savePageNumber: (page: number) => void;
  saveOrderby: (order: string) => void;
  saveSearchCount: (count: number) => void;
  saveResetFacets: (reset: boolean) => void;
  reduxFacetSelectors: any[];
  searchTerm?: string;
  placeholderText?: string;
}

export const updateSearchTerm = (searchTerm: string, props: SearchBarProps): void => {
  if (searchTerm !== undefined && searchTerm !== props.reduxSearchTerm) {
    props.saveSearchTerm(searchTerm);
  }
};

export const resetFilters = (props: SearchBarProps): void => {
  // Immediately clear all state to prevent timing issues
  props.saveVehicleData([]);
  props.saveSearchTerm('');
  props.saveFacetSelectors([]);
  props.saveFacetSelectedKeys([]);
  props.savePageNumber(0);
  props.saveOrderby('');
  props.saveSearchCount(0);

  // Trigger async reset for dialog closing and other cleanup
  props.saveResetFacets(true);
  removeFullQueryString();
};

export const checkForEnter = (event: React.KeyboardEvent): void => {
  if (event.key === 'Enter') {
    event.preventDefault();
  }
};

export const generatePlaceholder = (
  props: Pick<SearchBarProps, 'searchTerm' | 'placeholderText'>,
  windowWidth: number,
  MobileMaxWidth: number,
  dynamicPlaceholder?: string
): string => {
  if (windowWidth > MobileMaxWidth && !props.searchTerm) {
    return dynamicPlaceholder || 'Search...';
  }

  return props.placeholderText || 'Search...';
};

export const disableResetFiltersButton = (props: Pick<SearchBarProps, 'reduxSearchTerm' | 'reduxFacetSelectors'>): boolean => {
  if (props.reduxSearchTerm !== '') {
    return false;
  }

  if (props.reduxFacetSelectors.length > 0) {
    return false;
  }

  return true;
};