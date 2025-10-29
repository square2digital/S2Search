import React from "react";
import { PropTypes } from "prop-types";
import { format, parseISO, isValid } from "date-fns";

const CustomTooltip = ({ active, payload, label }) => {
  if (active && payload && payload.length) {
    const isValidDate = isValid(new Date(label));
    const header = isValidDate ? format(parseISO(label), "E do") : label;
    const output = Math.round(payload[0].value);

    return (
      <div className="tooltip">
        <h5>{header}</h5>
        <p>{output}</p>
      </div>
    );
  }
  return null;
};

CustomTooltip.propTypes = {
  active: PropTypes.bool,
  payload: PropTypes.any,
  label: PropTypes.string,
};

export default CustomTooltip;
