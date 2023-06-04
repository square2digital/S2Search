import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import BlueRadio from "../custom/BlueRadio";
import FormControl from "@mui/material/FormControl";
import { toast } from "react-toastify";
import { setCustomerDetails } from "../../redux/actions/customerActions";
import { setSearchIndexes } from "../../redux/actions/searchIndexesActions";
import { setSelectedSearchIndexValue } from "../../redux/actions/selectedSearchIndexActions";
import { getUserData } from "../../services/identity/msal";
import PropTypes from "prop-types";
import { getCustomerSearchIndexes } from "../../client/customerClient";

import Spinner from "../common/Spinner";

const useStyles = makeStyles((theme) => ({
  formControl: {
    marginLeft: theme.spacing(2),
  },
}));

const setSelectedValue = (key, value) => {
  return {
    key: key,
    value: value,
  };
};

const SearchIndexRadioList = (props) => {
  const classes = useStyles();
  const [value, setValue] = useState("");

  useEffect(() => {
    if (!props.reduxSelectedSearchIndex.id) {
      let customerId = getUserData().localAccountId;
      props.setCustomerDetails(customerId);
      props.setSearchIndexes(customerId).catch(() => {
        toast.error(
          "Error getting search index data - please refresh this page"
        );
      });

      getCustomerSearchIndexes(customerId).then(function (data) {
        if (data.result.searchIndexes.length === 1) {
          setValue(data.result.searchIndexes[0].searchIndexId);
        }
      });
    } else {
      setValue(props.reduxSelectedSearchIndex.id);
    }
  }, [props.reduxSelectedSearchIndex]);

  const setUpData = (searchIndexId, friendlyName) => {
    props.setSelectedSearchIndexValue(
      setSelectedValue(searchIndexId, friendlyName)
    );
  };

  const handleChange = (event) => {
    console.log(` ${event.target.value} `);

    setValue(event.target.value);
    props.setSelectedSearchIndexValue(
      setUpData(
        event.target.getAttribute("searchIndexId"),
        event.target.getAttribute("friendlyName")
      )
    );

    setValue(event.target.value);
  };

  const buildSearchIndexSelectMenuItems = () => {
    let dropdownArray = [];

    let orderedByIndexName = props.reduxSearchIndexes.sort((a, b) =>
      a.friendlyName.localeCompare(b.friendlyName)
    );

    for (const searchIndex of orderedByIndexName) {
      dropdownArray.push(
        <FormControlLabel
          value={`${searchIndex.searchIndexId}`}
          control={
            <BlueRadio
              inputProps={{
                searchIndexId: `${searchIndex.searchIndexId}`,
                friendlyName: `${searchIndex.friendlyName}`,
              }}
            />
          }
          label={searchIndex.friendlyName}
        />
      );
    }

    return dropdownArray;
  };

  return props.reduxSearchIndexes.length === 0 ? (
    <Spinner />
  ) : (
    <FormControl
      color="primary"
      component="fieldset"
      className={classes.formControl}>
      <RadioGroup
        aria-label="Search Instance"
        name="Search"
        value={value}
        onChange={handleChange}>
        {buildSearchIndexSelectMenuItems()}
      </RadioGroup>
    </FormControl>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxLoading: reduxState.apiCallsInProgress > 0,
    reduxCustomer: reduxState.customer,
    reduxSearchIndexes: reduxState.searchIndexes,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

const mapDispatchToProps = {
  setCustomerDetails,
  setSearchIndexes,
  setSelectedSearchIndexValue,
};

SearchIndexRadioList.propTypes = {
  reduxLoading: PropTypes.bool,
  reduxCustomer: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
  reduxSelectedSearchIndex: PropTypes.array,

  setCustomerDetails: PropTypes.func.isRequired,
  setSearchIndexes: PropTypes.func.isRequired,
  setSelectedSearchIndexValue: PropTypes.func.isRequired,
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(SearchIndexRadioList);
