import { combineReducers } from "redux";
import facetReducer from "./facetReducer";
import searchReducer from "./searchReducer";
import componentReducer from "./componentReducer";
import themeReducer from "./themeReducer";
import configReducer from "./configReducer";

const rootReducer = combineReducers({
  facetReducer,
  searchReducer,
  componentReducer,
  themeReducer,
  configReducer,
});

export default rootReducer;
