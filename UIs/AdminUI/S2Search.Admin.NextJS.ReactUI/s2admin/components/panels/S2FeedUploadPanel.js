import React from "react";
import { connect } from "react-redux";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import { uploadFile } from "../../client/feedClient";
import PropTypes from "prop-types";
import { makeStyles } from "@mui/styles";
import { toast } from "react-toastify";
import { getDefaultPanelStyles } from "../../styles/common";

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: theme.spacing(1),
    textAlign: "center",
    color: theme.palette.text.secondary,
    whiteSpace: "nowrap",
    marginBottom: theme.spacing(1),
  },
  divider: {
    margin: theme.spacing(2, 0),
  },
  content: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing(3),
  },
}));

const S2FeedUploadPanel = (props) => {
  const classes = useStyles();

  const onFileUpload = (event) => {
    let formData = new FormData();
    formData.append("feedFile", event.target.files[0]);

    uploadFile(
      props.reduxCustomer.customerId,
      props.reduxSelectedSearchIndex.id,
      formData
    ).then(function () {
      toast.success(`File uploaded`);
    });
  };

  return (
    <main className={classes.content}>
      <form encType="multipart/form-data">
        <Typography component="div" style={{ getDefaultPanelStyles }}>
          <p>
            <b>Upload Vehicle Feed</b> - You can use this interface to upload a
            vehicle feed - this will override the existing search data
          </p>

          <Button
            variant="contained"
            component="label"
            color="primary"
            onChange={onFileUpload}>
            Upload File
            <input type="file" hidden />
          </Button>
        </Typography>
      </form>
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxLoading: reduxState.apiCallsInProgress > 0,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
    reduxCustomer: reduxState.customer,
  };
};

S2FeedUploadPanel.propTypes = {
  reduxLoading: PropTypes.bool,
  reduxSelectedSearchIndex: PropTypes.object,
  reduxCustomer: PropTypes.object,
};

export default connect(mapStateToProps)(S2FeedUploadPanel);
