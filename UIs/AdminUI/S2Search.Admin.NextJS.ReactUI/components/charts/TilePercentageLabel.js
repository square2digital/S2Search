import React from "react";
import { PropTypes } from "prop-types";

const TilePercentageLabel = (props) => {
  const isIncrease = props.percentageChange > 0;
  const isZero = props.percentageChange === 0;
  const colour = isIncrease ? "green" : isZero ? "lightgrey" : "crimson";
  const backgroundColour = isIncrease
    ? "lightgreen"
    : isZero
    ? "grey"
    : "#F58484";

  return (
    <span
      style={{
        color: colour,
        fontWeight: 600,
        backgroundColor: backgroundColour,
        padding: "2px",
        borderRadius: "3px",
        marginRight: "8px",
      }}
    >
      {props.percentageChange}%
    </span>
  );
};

TilePercentageLabel.propTypes = {
  percentageChange: PropTypes.number,
};

export default TilePercentageLabel;
