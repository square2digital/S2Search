import React from "react";
import { PropTypes } from "prop-types";
import { Link, Button } from "@mui/material";

const LoadingButtonLink = ({
  displayText,
  placeholderText,
  href,
  colour,
  loading,
}) => {
  const showDisplayText = displayText.length > 0;
  return (
    <Link
      href={showDisplayText ? href : null}
      target="_blank"
      rel="noopener"
      underline="none"
    >
      <Button variant="outlined" color={loading ? "info" : colour}>
        {loading
          ? "Loading..."
          : showDisplayText
          ? displayText
          : placeholderText}
      </Button>
    </Link>
  );
};

LoadingButtonLink.propTypes = {
  displayText: PropTypes.string,
  placeholderText: PropTypes.string,
  href: PropTypes.string,
  colour: PropTypes.string,
  loading: PropTypes.bool,
};

export default LoadingButtonLink;
