import React from "react";
import PropTypes from "prop-types";
import VehicleCard from "./VehicleCard";
import Grid from "@mui/material/Grid";
import { connect } from "react-redux";

const VehicleCardList = (props) => {
  return (
    <Grid container>
      {props.vehicleData.map((vehicleData, i) => (
        <VehicleCard
          key={i}
          missingImageURL={props.missingImageURL}
          {...vehicleData}
        />
      ))}
    </Grid>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    vehicleData: reduxState.searchReducer.vehicleData,
    missingImageURL: reduxState.themeReducer.missingImageURL,
  };
};

VehicleCardList.propTypes = {
  vehicleData: PropTypes.array,
  missingImageURL: PropTypes.string,
};

export default connect(mapStateToProps)(VehicleCardList);
