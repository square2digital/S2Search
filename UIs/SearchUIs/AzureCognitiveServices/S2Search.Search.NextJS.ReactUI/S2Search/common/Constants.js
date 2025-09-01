export const SearchAPIDomain = process.env.NEXT_PUBLIC_SEARCH_API_URL;
export const DefaultPageNumber = Number(
  process.env.NEXT_PUBLIC_DEFAULT_PAGE_NUMBER
);
export const DefaultLoadSpeed = Number(
  process.env.NEXT_PUBLIC_DEFAULT_LOAD_SPEED
);
export const SearchAPIURL = process.env.NEXT_PUBLIC_SEARCH_API_URL;
export const SearchAndFacetsAPIURL =
  process.env.NEXT_PUBLIC_SEARCH_AND_FACETS_URL;
export const SearchAPIEndpoint = process.env.NEXT_PUBLIC_SEARCH_API_ENDPOINT;
export const ClientConfigurationAPIURL =
  process.env.NEXT_PUBLIC_CLIENT_CONFIGIRATION_API_URL;
export const FacetsAPIEndpoint = process.env.NEXT_PUBLIC_FACET_API_ENDPOINT;
export const AutoCompleteURL = process.env.NEXT_PUBLIC_AUTO_COMPLETE_URL;
export const DocumentCountURL = process.env.NEXT_PUBLIC_DOCUMENTS_COUNT_URL;
export const DefaultPageSize = Number(
  process.env.NEXT_PUBLIC_DEFAULT_PAGE_SIZE
);
export const MobileMaxWidth = Number(process.env.NEXT_PUBLIC_MOBILE_MAX_WIDTH);
export const MillisecondsDifference = Number(
  process.env.NEXT_PUBLIC_MILLISECONDS_DIFFERENCE
);

export const InitialSearchTerm = 'Initial Search Term';
export const GetOrderByData = [
  { name: 'Price_High', display: 'Price - high to low', value: 'price desc' },
  { name: 'Price_Low', display: 'Price - low to high', value: 'price asc' },
  {
    name: 'Monthly_High',
    display: 'Monthly - high to low',
    value: 'monthlyPrice desc',
  },
  {
    name: 'Monthly_Low',
    display: 'Monthly - low to high',
    value: 'monthlyPrice asc',
  },
  { name: 'Year_High', display: 'Year - newest first', value: 'year desc' },
  { name: 'Year_Low', display: 'Year - oldest first', value: 'year asc' },
];

// add facets to this array that you want parsing as strings not numbers
export const FacetToParseAsNumeric = ['year', 'doors', 'engineSize'];

// this array conatins the facets that will be generated from the default facets
// rather than the dynamic facets returns from search
export const StaticFacets = ['make'];

// global S2 theme
export const DefaultTheme = {
  primaryHexColour: '#616161',
  secondaryHexColour: '#757575',
  navBarHexColour: '#424242',
  logoURL:
    'https://dpdevstore.blob.core.windows.net/temp/assets/logos/Square_2_Logo_Colour_Blue_White_BG.svg',
  missingImageURL:
    'https://dpdevstore.blob.core.windows.net/temp/assets/image-coming-soon.jpg',
};

export const DefaultConfigData = [
  {
    key: 'EnableAutoComplete',
    value: 'false',
  },
];

export const EnableAutoComplete = true;
export const HideIconVehicleCounts = false;

export const DefaultPlaceholderText = [
  {
    key: 'PlaceholderText_1',
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_1,
  },
  {
    key: 'PlaceholderText_2',
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_2,
  },
  {
    key: 'PlaceholderText_3',
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_3,
  },
  {
    key: 'PlaceholderText_4',
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_4,
  },
  {
    key: 'PlaceholderText_5',
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_5,
  },
];
