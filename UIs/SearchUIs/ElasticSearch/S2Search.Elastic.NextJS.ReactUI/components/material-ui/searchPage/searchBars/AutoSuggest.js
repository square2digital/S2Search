﻿import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { AutoSuggestAPI } from "../../../../pages/api/helper/SearchAPI";
import searchActions from "../../../../redux/actions/searchActions";
import facetActions from "../../../../redux/actions/facetActions";
import InputBase from "@mui/material/InputBase";
import Autocomplete from "@mui/material/Autocomplete";
import Popper from "@mui/material/Popper";
import parse from "autosuggest-highlight/parse";
import match from "autosuggest-highlight/match";
import Paper from "@mui/material/Paper";
import IconButton from "@mui/material/IconButton";
import Divider from "@mui/material/Divider";
import RotateLeftIcon from "@mui/icons-material/RotateLeft";
import { MobileMaxWidth } from "../../../../common/Constants";
import {
  checkForEnter,
  generatePlaceholder,
  resetFilters,
  disableResetFiltersButton,
  updateSearchTerm,
} from "./searchBarSharedFunctions";

export const AutoSuggest = (props) => {
  const [options, setOptions] = useState([]);
  const [showDropdown, setShowDropdown] = useState(false);
  const [windowWidth, setwindowWidth] = useState(window.innerWidth);

  useEffect(() => {
    window.addEventListener("resize", updateWindowDimensions);
    window.removeEventListener("resize", updateWindowDimensions);
  }, []);

  useEffect(() => {
    if (props.reduxSearchTerm) {
      if (props.reduxSearchTerm.length >= 2) {
        AutoSuggestAPI(
          props.reduxSearchTerm,
          S2SearchIndex,
          props.reduxCancellationToken
        ).then(function (axiosSuggestionsResponse) {
          if (axiosSuggestionsResponse) {
            if (axiosSuggestionsResponse.status === 200) {
              let suggestions = axiosSuggestionsResponse.data;
              suggestions.shift();
              setOptions(suggestions);
            } else {
              setOptions([]);
            }
          }
        });
      } else {
        setOptions([]);
      }
    }

    setShowDropdown(options.length > 0);
  }, [props.reduxSearchTerm]);

  const updateWindowDimensions = () => {
    setwindowWidth(window.innerWidth);
  };

  const updateSearch = (event, value) => {
    updateSearchTerm(value, props);
  };

  const reset = () => {
    resetFilters(props);
  };

  const saveOnChange = (event) => {
    updateSearchTerm(event.target.value, props);
  };

  const disableResetButton = () => {
    return disableResetFiltersButton(props);
  };

  const PopperOverride = function (props) {
    const styles = () => ({
      popper: {
        width: "fit-content",
      },
    });

    if (MobileMaxWidth > windowWidth) {
      return (
        <Popper {...props} style={styles.popper} placement="bottom-start" />
      );
    } else {
      return <Popper {...props} />;
    }
  };

  return (
    <Autocomplete
      PopperComponent={PopperOverride}
      id="auto-complete-suggestions"
      freeSolo
      disableClearable
      forcePopupIcon={showDropdown}
      options={options}
      onInputChange={updateSearch}
      value={props.reduxSearchTerm}
      renderOption={(renderProps, option) => {
        const matches = match(option, props.reduxSearchTerm);
        const parts = parse(option, matches);

        return (
          <div {...renderProps}>
            <span>
              {parts.map((part, index) => (
                <span
                  key={index}
                  style={{
                    fontWeight: !part.highlight ? 700 : 400,
                  }}>
                  {part.text.replace(/ /g, "\u00a0")}
                </span>
              ))}
            </span>
          </div>
        );
      }}
      renderInput={(params) => {
        const { ...rest } = params;
        return (
          <>
            <Paper
              component="form"
              style={{
                padding: "2px 4px",
                display: "flex",
                height: 45,
              }}>
              <InputBase
                {...params.InputProps}
                {...rest}
                style={{
                  marginLeft: 1,
                  flex: 1,
                }}
                placeholder={generatePlaceholder(
                  props,
                  windowWidth,
                  MobileMaxWidth
                )}
                variant="outlined"
                onChange={saveOnChange}
                onKeyPress={checkForEnter}
                value={props.reduxSearchTerm}
              />
              <Divider
                style={{
                  height: 28,
                  margin: 4,
                }}
                orientation="vertical"
              />
              <IconButton
                style={{ paddingLeft: 2, paddingRight: 2 }}
                color="primary"
                onClick={reset}
                disabled={disableResetButton()}
                aria-label="directions">
                <RotateLeftIcon />
              </IconButton>
            </Paper>
          </>
        );
      }}
    />
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxConfigPlaceholders: reduxState.configReducer.placeholderText,
    reduxCancellationToken: reduxState.componentReducer.enableToken,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveSearchTerm: (searchTerm) =>
      dispatch(searchActions.saveSearchTerm(searchTerm)),
    saveResetFacets: (resetFacets) =>
      dispatch(facetActions.saveResetFacets(resetFacets)),
    saveVehicleData: () => dispatch(searchActions.saveVehicleData([])),
    savePageFrom: () => dispatch(searchActions.savePageFrom(0)),
    saveFacetSelectors: () => dispatch(facetActions.saveFacetSelectors([])),
    saveCancellationToken: (enable) =>
      dispatch(componentActions.saveCancellationToken(enable)),
  };
};

AutoSuggest.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  reduxSearchTerm: PropTypes.string,
  saveSearchTerm: PropTypes.func,
  saveResetFacets: PropTypes.func,
  saveVehicleData: PropTypes.func,
  savePageFrom: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  placeholderText: PropTypes.string,
  reduxConfigPlaceholders: PropTypes.array,
  reduxCancellationToken: PropTypes.bool,
  saveCancellationToken: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(AutoSuggest);
