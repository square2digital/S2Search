import React, { useEffect, useState, forwardRef } from "react";
import { connect } from "react-redux";
import FormGroup from "@mui/material/FormGroup";
import TextField from "@mui/material/TextField";
import { updateConfig } from "../../client/configClient";
import { makeStyles } from "@mui/styles";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import PropTypes from "prop-types";
import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";

const Alert = forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

const useStyles = makeStyles((theme) => ({
  button: {
    display: "relative",
    marginTop: theme.spacing(1.5),
    marginLeft: theme.spacing(1.5),
  },
}));

const PlaceholderText = (props) => {
  const classes = useStyles();
  const [placeholderText, setPlaceholderText] = useState("");
  const [currentlySavedValue, setCurrentlySavedValue] = useState("");
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [snackOpen, setSnackOpen] = useState(false);
  const [snackMessage, setSnackMessage] = useState("");

  useEffect((props) => {
    if (props) {
      setPlaceholderText(props.value);
      setCurrentlySavedValue(props.value);
    }
  }, []);

  useEffect(() => {
    if (placeholderText === currentlySavedValue) {
      setCurrentlySavedValue(placeholderText);
      setButtonDisabled(true);
    } else {
      setButtonDisabled(false);
    }
  }, [placeholderText]);

  const handleChange = (event) => {
    if (event.target.value !== undefined) {
      setPlaceholderText(event.target.value);
    }
  };

  const handleClose = (event, reason) => {
    setSnackOpen(false);
  };

  const saveConfig = () => {
    updateConfig({
      SearchConfigurationMappingId: props.searchConfigurationMappingId,
      SeachConfigurationOptionId: props.seachConfigurationOptionId,
      SearchIndexId: props.reduxSelectedSearchIndex.id,
      Value: placeholderText,
    }).then(function () {
      setButtonDisabled(true);
      setSnackOpen(true);
      setSnackMessage(`${props.description} updated successfully`);
    });
  };

  return (
    <Box container display="flex">
      <Box flexGrow={1}>
        <FormGroup aria-label="position">
          <TextField
            color="primary"
            label={props.description}
            id={props.id}
            data-configMappingId={props.searchConfigurationMappingId}
            data-configOptionId={props.seachConfigurationOptionId}
            value={placeholderText !== "" ? placeholderText : props.value}
            variant="outlined"
            onChange={handleChange}
          />
          <br />
        </FormGroup>
      </Box>
      <Box>
        <Button
          className={classes.button}
          variant="contained"
          color="primary"
          disabled={buttonDisabled}
          onClick={saveConfig}>
          Update
        </Button>
      </Box>
      <Snackbar
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        open={snackOpen}
        autoHideDuration={4000}
        onClose={handleClose}>
        <Alert onClose={handleClose} severity="success" sx={{ width: "100%" }}>
          {snackMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchIndexes: reduxState.searchIndexes,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

PlaceholderText.propTypes = {
  id: PropTypes.string,
  description: PropTypes.string,
  searchConfigurationMappingId: PropTypes.string,
  seachConfigurationOptionId: PropTypes.string,
  value: PropTypes.string,
  reduxSelectedSearchIndex: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
  friendlyName: PropTypes.array,
};

export default connect(mapStateToProps)(PlaceholderText);
