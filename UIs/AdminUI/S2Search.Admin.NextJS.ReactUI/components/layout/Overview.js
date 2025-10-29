import React from "react";
import Box from "@mui/material/Box";
import CssBaseline from "@mui/material/CssBaseline";
import S2SideBar from "./S2SideBar";
import PropTypes from "prop-types";

const Overview = (props) => {
  return (
    <>
      <Box sx={{ display: "flex" }}>
        <CssBaseline />
        <S2SideBar />
        {props.panelComponent}
      </Box>
    </>
  );
};

Overview.propTypes = {
  panelComponent: PropTypes.object,
  user: PropTypes.object,
};

export default Overview;
