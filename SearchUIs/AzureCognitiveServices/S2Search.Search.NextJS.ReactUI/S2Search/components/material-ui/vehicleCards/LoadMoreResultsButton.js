import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import Button from "@mui/material/Button";
import { makeStyles } from "@mui/styles";
import CloudDownloadIcon from "@mui/icons-material/CloudDownload";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import searchActions from "../../../redux/actions/searchActions";
import { connect } from "react-redux";
import { MobileMaxWidth } from "../../../common/Constants";

const useStyles = makeStyles((theme) => ({
  button: {
    margin: theme.spacing(5),
    color: theme.palette.primary,
  },
}));

const LoadMoreResultsButton = (props) => {
  const classes = useStyles();

  const [windowWidth, setwindowWidth] = useState(0);

  useEffect(() => {
    const updateWindowDimensions = () => {
      setwindowWidth(window.innerWidth);
    };

    window.addEventListener("resize", updateWindowDimensions);

    return () => window.removeEventListener("resize", updateWindowDimensions);
  }, [windowWidth]);

  const toggleLoadMore = () => {
    props.savePageNumber(props.reduxPageNumber + 1);
  };

  const getLoadMoreButtonStyle = (width) => {
    if (width > 0 && width <= MobileMaxWidth) {
      return {
        fontSize: 16,
        width: 175,
        borderColor: props.reduxPrimaryColour,
      };
    }

    return {
      fontSize: 18,
      width: 250,
      borderColor: props.reduxPrimaryColour,
    };
  };

  const showLoadMoreButton = () => {
    if (props.reduxVehicleData.length === 0) {
      return false;
    }

    if (props.reduxSearchCount === props.reduxVehicleData.length) {
      return false;
    }

    return true;
  };

  return (
    <div>
      {showLoadMoreButton() === true ? (
        <Button
          onClick={toggleLoadMore}
          size="large"
          variant="outlined"
          className={classes.button}
          style={getLoadMoreButtonStyle(windowWidth)}
          disableElevation
          startIcon={
            props.reduxLoading ? (
              <CloudUploadIcon color="disabled" />
            ) : (
              <CloudDownloadIcon style={{ color: props.reduxPrimaryColour }} />
            )
          }>
          {props.reduxLoading ? <>Loading...</> : <>Load more</>}
        </Button>
      ) : (
        <></>
      )}
    </div>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchCount: reduxState.searchReducer.searchCount,
    reduxPageNumber: reduxState.searchReducer.pageNumber,
    reduxVehicleData: reduxState.searchReducer.vehicleData,
    reduxLoading: reduxState.componentReducer.loading,
    reduxNavBarColour: reduxState.themeReducer.navBarColour,
    reduxPrimaryColour: reduxState.themeReducer.primaryColour,
    reduxSecondaryColour: reduxState.themeReducer.secondaryColour,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    savePageNumber: (pageNumber) =>
      dispatch(searchActions.savePageNumber(pageNumber)),
  };
};

LoadMoreResultsButton.propTypes = {
  reduxSearchCount: PropTypes.number,
  reduxPageNumber: PropTypes.number,
  reduxVehicleData: PropTypes.array,

  savePageNumber: PropTypes.func,
  reduxLoading: PropTypes.bool,
  reduxNavBarColour: PropTypes.string,
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LoadMoreResultsButton);
