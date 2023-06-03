export const S2SearchIndex = process.env.NEXT_PUBLIC_SEARCH_INDEX;
export const SearchAPIDomain = process.env.NEXT_PUBLIC_SEARCH_API_URL;
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

export const InitialSearchTerm = "Initial Search Term";
export const GetOrderByData = [
  {
    name: "Price_High",
    display: "Price - high to low",
    value: "price",
    sort: "desc",
  },
  {
    name: "Price_Low",
    display: "Price - low to high",
    value: "price",
    sort: "asc",
  },
  {
    name: "Monthly_High",
    display: "Monthly - high to low",
    value: "monthlyPrice",
    sort: "desc",
  },
  {
    name: "Monthly_Low",
    display: "Monthly - low to high",
    value: "monthlyPrice",
    sort: "asc",
  },
  {
    name: "Year_High",
    display: "Year - newest first",
    value: "year",
    sort: "desc",
  },
  {
    name: "Year_Low",
    display: "Year - oldest first",
    value: "year",
    sort: "asc",
  },
];

// add facets to this array that you want parsing as strings not numbers
export const FacetToParseAsNumeric = ["year", "doors", "engineSize"];

// this array conatins the facets that will be generated from the default facets
// rather than the dynamic facets returns from search
export const StaticFacets = ["make"];

// global S2 theme
export const DefaultTheme = {
  primaryHexColour: process.env.NEXT_PUBLIC_THEME_PRIMARY_COLOUR,
  secondaryHexColour: process.env.NEXT_PUBLIC_THEME_SECONDARY_COLOUR,
  navBarHexColour: process.env.NEXT_PUBLIC_THEME_NAVBAR_COLOUR,
  logoURL: process.env.NEXT_PUBLIC_THEME_LOGO_URL,
  missingImageURL: process.env.NEXT_PUBLIC_THEME_MISSING_IMAGE_URL,
};

export const DefaultConfigData = [
  {
    key: "EnableAutoComplete",
    value: "false",
  },
];

export const EnableAutoComplete = true;
export const HideIconVehicleCounts = false;

export const DefaultPlaceholderText = [
  {
    key: "PlaceholderText_1",
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_1,
  },
  {
    key: "PlaceholderText_2",
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_2,
  },
  {
    key: "PlaceholderText_3",
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_3,
  },
  {
    key: "PlaceholderText_4",
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_4,
  },
  {
    key: "PlaceholderText_5",
    value: process.env.NEXT_PUBLIC_PLACEHOLDER_TEXT_5,
  },
];
