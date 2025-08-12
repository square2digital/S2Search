import { removeFullQueryString } from '../../../../common/functions/QueryStringFunctions';

const ForbiddenSearchBarCharacters = '[]{}#~¬$£"%^&@';

export const isCharForbidden = searchTerm => {
  const char = searchTerm.slice(-1);
  const result = ForbiddenSearchBarCharacters.indexOf(char) > -1;

  return result;
};

export const updateSearchTerm = (searchTerm, props) => {
  if (searchTerm !== undefined && searchTerm !== props.reduxSearchTerm) {
    props.saveSearchTerm(searchTerm);
  }
};

export const resetFilters = props => {
  props.saveResetFacets(true);
  removeFullQueryString();
};

export const checkForEnter = event => {
  if (event.key === 'Enter') {
    event.preventDefault();
  }
};

export const generatePlaceholder = (props, windowWidth, MobileMaxWidth, dynamicPlaceholder) => {
  if (windowWidth > MobileMaxWidth && !props.searchTerm) {
    return dynamicPlaceholder || 'Search...';
  }

  return props.placeholderText;
};

export const disableResetFiltersButton = props => {
  if (props.reduxSearchTerm !== '') {
    return false;
  }

  if (props.reduxFacetSelectors.length > 0) {
    return false;
  }

  return true;
};
