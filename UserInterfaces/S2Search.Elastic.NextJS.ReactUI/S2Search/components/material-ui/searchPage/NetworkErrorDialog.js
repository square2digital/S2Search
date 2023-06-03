import React, { useState } from "react";
import PropTypes from "prop-types";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import Backdrop from "@mui/material/Backdrop";
import { makeStyles } from "@mui/styles";
import RefreshIcon from "@mui/icons-material/Refresh";
import searchActions from "../../../redux/actions/searchActions";
import { connect } from "react-redux";

const useStyles = makeStyles((theme) => ({
  backdrop: {
    zIndex: theme.zIndex.drawer + 1,
    color: "#fff",
  },
}));

const NetworkErrorDialog = (props) => {
  const classes = useStyles();
  const [open, setOpen] = useState(props.reduxNetworkError);

  const refreshPage = () => {
    props.saveNetworkError(false);
    window.location.reload();
  };

  const handleClose = () => {
    props.saveNetworkError(false);
    setOpen(false);
  };

  return (
    <div>
      <Backdrop className={classes.backdrop} open={open}>
        <Dialog
          open={open}
          onClose={handleClose}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description">
          <DialogTitle id="alert-dialog-title" onClose={handleClose}>
            Connectivity Issue
          </DialogTitle>
          <DialogContent>
            <DialogContentText id="alert-dialog-description">
              Sorry there has been an issue connecting, please refresh and try
              again
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button
              onClick={refreshPage}
              color="secondary"
              size="small"
              startIcon={<RefreshIcon />}>
              Refresh
            </Button>
          </DialogActions>
        </Dialog>
      </Backdrop>
    </div>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxNetworkError: reduxState.searchReducer.networkError,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveNetworkError: (enable) =>
      dispatch(searchActions.saveNetworkError(enable)),
  };
};

NetworkErrorDialog.propTypes = {
  reduxNetworkError: PropTypes.bool,
  saveNetworkError: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(NetworkErrorDialog);
