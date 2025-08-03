import { combineReducers } from "redux";
import apiCallsInProgress from "./reducers/apiStatusReducer";
import customer from "./reducers/customerReducer";
import searchIndexes from "./reducers/searchIndexesReducer";
import selectedSearchIndex from "./reducers/selectedSearchIndexReducer";

const rootReducer = combineReducers({
  apiCallsInProgress,
  customer,
  searchIndexes,
  selectedSearchIndex,
});

export default rootReducer;
