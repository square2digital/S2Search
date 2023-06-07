import React, { useEffect } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import MenuItem from "@mui/material/MenuItem";
import PropTypes from "prop-types";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import FormHelperText from "@mui/material/FormHelperText";
import { toast } from "react-toastify";
import { setCustomerDetails } from "../../redux/actions/customerActions";
import { setSearchIndexes } from "../../redux/actions/searchIndexesActions";
import { setSelectedSearchIndexValue } from "../../redux/actions/selectedSearchIndexActions";
import { useMsal } from "@azure/msal-react";
import { setSelectedValue } from "../../components/common/sharedFunctions";

const useStyles = makeStyles((theme) => ({
  formControl: {
    maxWidth: "90%",
    marginLeft: theme.spacing(1),
  },
}));

const SearchIndexList = (props) => {
  const classes = useStyles();
  const { accounts } = useMsal();

  useEffect(() => {
    if (props.reduxSearchIndexes.length === 0) {
      props.setSearchIndexes(accounts[0].localAccountId).catch(() => {
        toast.error(
          "Error getting search index data - please refresh this page"
        );
      });
    }
  }, [props.reduxSelectedSearchIndex.id, props.reduxSearchIndexes.length]);

  const handleChange = (event) => {
    props.setSelectedSearchIndexValue(
      setSelectedValue(event.target.value, event.currentTarget.innerText)
    );
  };

  const renderValue = () => {
    if (props.reduxSearchIndexes.length > 0) {
      return (
        <>
          <Select
            value={props.reduxSelectedSearchIndex.id}
            onChange={handleChange}>
            {buildSearchIndexSelectMenuItems()}
          </Select>
        </>
      );
    }
  };

  const buildSearchIndexSelectMenuItems = () => {
    let dropdownArray = [];

    for (const searchIndex of props.reduxSearchIndexes) {
      dropdownArray.push(
        <MenuItem value={searchIndex.searchIndexId}>
          {searchIndex.friendlyName}
        </MenuItem>
      );
    }

    return dropdownArray;
  };

  return (
    <FormControl className={classes.formControl}>
      {renderValue()}
      <FormHelperText>Select your search to manage settings</FormHelperText>
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

SearchIndexList.propTypes = {
  reduxLoading: PropTypes.bool,
  reduxCustomer: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
  reduxSelectedSearchIndex: PropTypes.object,

  setCustomerDetails: PropTypes.func.isRequired,
  setSearchIndexes: PropTypes.func.isRequired,
  setSelectedSearchIndexValue: PropTypes.func.isRequired,
};

export default connect(mapStateToProps, mapDispatchToProps)(SearchIndexList);
