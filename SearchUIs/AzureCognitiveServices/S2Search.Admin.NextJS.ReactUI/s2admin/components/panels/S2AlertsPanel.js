import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import PropTypes from "prop-types";
import AlertTable from "../alerts/AlertTable";
import { getNotificationsBySearchIndexId } from "../../client/notificationClient";
import { homePageRedirectOnNullSearchIndex } from "../utilities/helpers";

const useStyles = makeStyles((theme) => ({
  content: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing(3),
  },
}));

const S2AlertsPanel = (props) => {
  const classes = useStyles();
  const [notificationData, setNotificationData] = useState([]);

  useEffect(() => {
    homePageRedirectOnNullSearchIndex(props.reduxSelectedSearchIndex.id);

    getNotificationsBySearchIndexId(
      props.reduxCustomer.customerId,
      props.reduxSelectedSearchIndex.id,
      1,
      25
    ).then(function (data) {
      setNotificationData(data.result.results);
    });
  }, [props.reduxSelectedSearchIndex.id, notificationData.length]);

  return (
    <main className={classes.content}>
      <Typography>
        <p>
          <b>Search Instance Alerts</b> - you can view your search instance
          alerts below
        </p>
        <Divider className={classes.divider} />
      </Typography>

      {notificationData.length > 0 ? (
        <AlertTable notificationData={notificationData} />
      ) : (
        <p>No Alerts</p>
      )}
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
    reduxCustomer: reduxState.customer,
  };
};

S2AlertsPanel.propTypes = {
  user: PropTypes.object,
  reduxSelectedSearchIndex: PropTypes.object,
};

export default connect(mapStateToProps)(S2AlertsPanel);
