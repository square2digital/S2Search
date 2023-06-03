import * as actionTypes from "../actions/actionTypes";
import initialSearchState from "../../redux/stateObjects/searchState";

const searchReducer = (state = initialSearchState, action) => {
  //debugger;
  switch (action.type) {
    case actionTypes.SEARCH_TERM:
      return {
        ...state,
        searchTerm: action.searchTerm,
      };
    case actionTypes.SEARCH_COUNT:
      return {
        ...state,
        searchCount: action.searchCount,
      };
    case actionTypes.TOTAL_DOCUMENT_COUNT:
      return {
        ...state,
        totalDocumentCount: action.totalDocumentCount,
      };
    case actionTypes.VEHICLE_DATA:
      return {
        ...state,
        vehicleData: action.vehicleData,
      };
    case actionTypes.ORDER_BY:
      return {
        ...state,
        orderBy: action.orderBy,
      };
    case actionTypes.SORT_ORDER:
      return {
        ...state,
        sortOrder: action.sortOrder,
      };
    case actionTypes.PAGE_FROM:
      return {
        ...state,
        pageFrom: action.pageFrom,
      };
    case actionTypes.NETWORK_ERROR:
      return {
        ...state,
        networkError: action.networkError,
      };
    case actionTypes.PREVIOUS_REQUEST:
      return {
        ...state,
        previousRequest: action.previousRequest,
      };
    default:
      return state;
  }
};

export default searchReducer;
