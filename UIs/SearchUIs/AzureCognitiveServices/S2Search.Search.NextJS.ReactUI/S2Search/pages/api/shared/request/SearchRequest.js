import {
  DefaultPageNumber,
  DefaultPageSize,
} from '../../../../common/Constants';

class SearchRequest {
  constructor(
    searchTerm,
    filters,
    orderBy,
    pageNumber,
    pageSize,
    numberOfExistingResults,
    callingHost
  ) {
    if (searchTerm === undefined || searchTerm === null || searchTerm === ' ') {
      this.searchTerm = '';
    } else {
      this.searchTerm = searchTerm;
    }

    if (filters === undefined || filters === null) {
      this.filters = [];
    } else {
      this.filters = filters;
    }

    if (orderBy === undefined || orderBy === null) {
      this.orderBy = ['price desc'];
    } else {
      this.orderBy = orderBy;
    }

    if (pageNumber === undefined || pageNumber === null) {
      this.pageNumber = DefaultPageNumber;
    } else {
      this.pageNumber = pageNumber;
    }

    if (pageSize === undefined || pageSize === null) {
      this.pageSize = DefaultPageSize;
    } else {
      this.pageSize = pageSize;
    }

    if (
      numberOfExistingResults === undefined ||
      numberOfExistingResults === null ||
      filters.length == 1
    ) {
      this.numberOfExistingResults = pageSize;
    } else {
      this.numberOfExistingResults = numberOfExistingResults;
    }

    if (callingHost === undefined || callingHost === null) {
      this.callingHost = [];
    } else {
      this.callingHost = callingHost;
    }
  }

  // Convert SearchRequest instance to a plain object for Redux serialization
  toPlainObject() {
    return {
      searchTerm: this.searchTerm,
      filters: this.filters,
      orderBy: this.orderBy,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      numberOfExistingResults: this.numberOfExistingResults,
      callingHost: this.callingHost,
    };
  }

  // Create SearchRequest instance from plain object
  static fromPlainObject(obj) {
    if (!obj) return null;
    return new SearchRequest(
      obj.searchTerm,
      obj.filters,
      obj.orderBy,
      obj.pageNumber,
      obj.pageSize,
      obj.numberOfExistingResults,
      obj.callingHost
    );
  }
}

export default SearchRequest;
