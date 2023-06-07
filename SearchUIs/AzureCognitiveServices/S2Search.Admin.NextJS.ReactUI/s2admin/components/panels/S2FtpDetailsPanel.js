import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import PropTypes from "prop-types";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Grid from "@mui/material/Grid";
import Divider from "@mui/material/Divider";
import { grey } from "@mui/material/colors";
import { getCredentialsBySearchIndexId } from "../../client/feedClient";
import { homePageRedirectOnNullSearchIndex } from "../utilities/helpers";

const S2FtpDetailsPanel = (props) => {
  const [configuration, setConfiguration] = useState({
    searchIndexId: "",
    sftpEndpoint: "",
    username: "",
    createdDate: "",
    modifiedDate: "",
  });

  const useStyles = makeStyles((theme) => ({
    content: {
      flexGrow: 1,
      backgroundColor: theme.palette.background.default,
      padding: theme.spacing(3),
    },
    gridContainer: {
      marginTop: theme.spacing(1),
    },
    verticleDivider: {
      border: `2px solid ${grey[100]}`,
      marginLeft: theme.spacing(2),
      marginRight: theme.spacing(2),
    },
    horizontalTopDivider: {
      border: `1px solid ${grey[300]}`,
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(3),
    },
    horizontalDivider: {
      border: `1px solid ${grey[100]}`,
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(2),
    },
  }));

  const classes = useStyles();

  useEffect(() => {
    homePageRedirectOnNullSearchIndex(props.reduxSelectedSearchIndex.id);

    getCredentialsBySearchIndexId(
      props.reduxCustomer.customerId,
      props.reduxSelectedSearchIndex.id
    ).then(function (config) {
      setConfiguration(config.result);
    });
  }, [props.reduxSelectedSearchIndex]);

  return (
    <main className={classes.content}>
      <h2>Manage FTP</h2>

      <Typography>
        Your FTP details are below. Use these credentials to confirm your
        systems to send your CSV vehicle feed. If you wish to create a new
        password contact us
        <br />
      </Typography>
      <Divider className={classes.horizontalTopDivider} />
      <div>
        <Grid container alignItems="center" className={classes.gridContainer}>
          <Typography variant="overline">FTP URL</Typography>
          <Divider
            orientation="vertical"
            flexItem
            className={classes.verticleDivider}
          />
          <TextField
            id="outlined-basic"
            label="FTP URL Address"
            variant="outlined"
            style={{ width: "70%" }}
            value={configuration.sftpEndpoint}
            disabled
          />
        </Grid>
        <Divider className={classes.horizontalDivider} />
        <Grid container alignItems="center" className={classes.gridContainer}>
          <Typography variant="overline">Username</Typography>
          <Divider
            orientation="vertical"
            flexItem
            className={classes.verticleDivider}
          />
          <TextField
            id="outlined-basic"
            label="FTP Credentials - Username"
            variant="outlined"
            style={{ width: "70%" }}
            value={configuration.username}
            disabled
          />
        </Grid>
        <Divider className={classes.horizontalDivider} />
        <Grid container alignItems="center" className={classes.gridContainer}>
          <Typography variant="overline">Password</Typography>
          <Divider
            orientation="vertical"
            flexItem
            className={classes.verticleDivider}
          />
          <Typography variant="caption">
            Please contact us if you want to reset your password.
          </Typography>
        </Grid>
        <Divider className={classes.horizontalDivider} />
      </div>
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
    reduxCustomer: reduxState.customer,
  };
};

S2FtpDetailsPanel.propTypes = {
  reduxSelectedSearchIndex: PropTypes.object,
  children: PropTypes.node,
  index: PropTypes.any.isRequired,
  value: PropTypes.any.isRequired,
};

export default connect(mapStateToProps)(S2FtpDetailsPanel);
