import { DefaultPageSize, DefaultPageFrom } from "../../common/Constants";
import {
  getQueryStringSearchTerm,
  getQueryStringOrderBy,
  getQueryStringSortOrder,
} from "../../common/functions/QueryStringFunctions";

export default {
  // ****************************
  // SearchDataRequest properties
  // ****************************
  searchTerm: getQueryStringSearchTerm(),
  filters: "",
  orderBy: getQueryStringOrderBy(),
  sortOrder: getQueryStringSortOrder(),
  pageFrom: 0,
  pageSize: DefaultPageSize,
  index: "s2-demo-vehicles",

  // ****************************
  // Possible Legacy properties
  // ****************************
  pageSize: DefaultPageSize,
  searchCount: 0,
  totalDocumentCount: 0,
  vehicleData: [],
  networkError: false,
  previousRequest: {},
};
