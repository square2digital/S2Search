import React from "react";
import { Select, MenuItem, Box } from "@mui/material";
import { PropTypes } from "prop-types";

const TimeframeDropdown = (props) => {
  return (
    <Box>
      <Select
        labelId="timeframe-select-label"
        id="timeframe-select"
        variant="outlined"
        value={props.periodDays}
        onChange={props.setTimeframe}>
        <MenuItem value={-7}>Last 7 Days</MenuItem>
        <MenuItem value={-14}>Last 14 Days</MenuItem>
        <MenuItem value={-30}>Last 30 Days</MenuItem>
        <MenuItem value={-60}>Last 60 Days</MenuItem>
      </Select>
    </Box>
  );
};

TimeframeDropdown.propTypes = {
  periodDays: PropTypes.number,
  setTimeframe: PropTypes.func,
};

export default TimeframeDropdown;
