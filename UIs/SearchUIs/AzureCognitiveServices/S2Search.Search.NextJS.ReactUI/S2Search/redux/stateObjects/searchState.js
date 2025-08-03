import { DefaultPageSize, DefaultPageNumber } from "../../common/Constants";
import {
  getQueryStringSearchTerm,
  getQueryStringOrderBy,
} from "../../common/functions/QueryStringFunctions";

export default {
  searchTerm: getQueryStringSearchTerm(),
  searchCount: 0,
  totalDocumentCount: 0,
  vehicleData: [],
  orderBy: getQueryStringOrderBy(),
  pageNumber: DefaultPageNumber,
  networkError: false,
  previousRequest: {},

  filters: "",
  pageSize: DefaultPageSize,
};
