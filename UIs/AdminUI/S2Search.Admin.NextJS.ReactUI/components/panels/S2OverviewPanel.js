import React, { useEffect } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { setSelectedSearchIndexValue } from "../../redux/actions/selectedSearchIndexActions";
import S2InsightsPanel from "./S2InsightsPanel";

const S2OverviewPanel = (props) => {
  useEffect(() => {
    document.title = props.title;
  }, []);

  return <S2InsightsPanel />;
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

const mapDispatchToProps = {
  setSelectedSearchIndexValue,
};

S2OverviewPanel.propTypes = {
  user: PropTypes.object,
  reduxSelectedSearchIndex: PropTypes.object,
  setSelectedSearchIndexValue: PropTypes.func.isRequired,
};

export default connect(mapStateToProps, mapDispatchToProps)(S2OverviewPanel);
