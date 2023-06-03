import { DefaultPageSize } from "../../../../common/Constants";

class SearchRequest {
  constructor(searchTerm, filters, orderBy, sortOrder, from, pageSize, index) {
    if (searchTerm === undefined || searchTerm === null || searchTerm === " ") {
      this.searchTerm = "";
    } else {
      this.searchTerm = searchTerm;
    }

    if (filters === undefined || filters === null) {
      this.filters = [];
    } else {
      this.filters = filters;
    }

    if (orderBy === undefined || orderBy === null) {
      this.orderBy = "";
    } else {
      this.orderBy = orderBy;
    }

    if (sortOrder === undefined || sortOrder === null) {
      this.sortOrder = "";
    } else {
      this.sortOrder = sortOrder;
    }

    if (from === undefined || from === null) {
      this.from = 0;
    } else {
      this.from = from;
    }

    if (pageSize === undefined || pageSize === null) {
      this.pageSize = DefaultPageSize;
    } else {
      this.pageSize = pageSize;
    }

    if (index === undefined || index === null) {
      this.index = "s2-demo-vehicles";
    } else {
      this.index = index;
    }
  }
}

export default SearchRequest;
