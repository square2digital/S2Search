import * as actionTypes from "./actionTypes";

const searchActions = {
  saveSearchTerm: function (searchTerm) {
    return { type: actionTypes.SEARCH_TERM, searchTerm: searchTerm };
  },
  saveSearchCount: function (searchCount) {
    return { type: actionTypes.SEARCH_COUNT, searchCount: searchCount };
  },
  saveTotalDocumentCount: function (totalDocumentCount) {
    return {
      type: actionTypes.TOTAL_DOCUMENT_COUNT,
      totalDocumentCount: totalDocumentCount,
    };
  },
  saveVehicleData: function (vehicleData) {
    return { type: actionTypes.VEHICLE_DATA, vehicleData: vehicleData };
  },
  saveOrderby: function (orderBy) {
    return { type: actionTypes.ORDER_BY, orderBy: orderBy };
  },
  saveSortOrder: function (sortOrder) {
    return { type: actionTypes.SORT_ORDER, sortOrder: sortOrder };
  },
  savePageFrom: function (pageFrom) {
    return { type: actionTypes.PAGE_FROM, pageFrom: pageFrom };
  },
  saveNetworkError: function (networkError) {
    return { type: actionTypes.NETWORK_ERROR, networkError: networkError };
  },
  savePreviousRequest: function (previousRequest) {
    return {
      type: actionTypes.PREVIOUS_REQUEST,
      previousRequest: previousRequest,
    };
  },
};

export default searchActions;
