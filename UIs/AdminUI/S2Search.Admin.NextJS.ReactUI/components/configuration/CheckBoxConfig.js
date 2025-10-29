import React, { useEffect, useState, forwardRef } from "react";
import { connect } from "react-redux";
import FormGroup from "@mui/material/FormGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import { updateConfig } from "../../client/configClient";
import Switch from "@mui/material/Switch";
import PropTypes from "prop-types";
import Fade from "@mui/material/Fade";
import Tooltip from "@mui/material/Tooltip";
import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";

const Alert = forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

const CheckBoxConfig = (props) => {
  const [configUpdated, setConfigUpdated] = useState(false);
  const [checked, setChecked] = useState(false);
  const [snackOpen, setSnackOpen] = useState(false);
  const [snackMessage, setSnackMessage] = useState("");
  const [alertSeverity, setAlertSeverity] = useState("success");

  useEffect(() => {
    setChecked(checked);
  }, [checked]);

  useEffect(() => {
    setChecked(props.value);
  }, [props.value]);

  const handleClose = (event, reason) => {
    setSnackOpen(false);
  };

  const saveConfig = (event) => {
    const checkedValue = event.target.checked;
    setChecked(checkedValue);
    updateConfig({
      SearchConfigurationMappingId: props.searchConfigurationMappingId,
      SeachConfigurationOptionId: props.seachConfigurationOptionId,
      SearchIndexId: props.reduxSelectedSearchIndex.id,
      Value: String(checkedValue),
    }).then(function (response) {
      if (response.isError) {
        setAlertSeverity("error");
        setSnackMessage("An error has occurned when saving config");
      } else {
        setConfigUpdated(!configUpdated);
        setSnackOpen(true);
        setAlertSeverity("success");
        setSnackMessage(
          `${props.friendlyName} ${
            checkedValue ? "Enabled" : "Disabled"
          } successfully`
        );
      }
    });
  };

  return (
    <FormGroup aria-label="position" row>
      <Tooltip
        TransitionComponent={Fade}
        TransitionProps={{ timeout: 600 }}
        disableFocusListener
        title={props.description}>
        <FormControlLabel
          value="start"
          id={props.searchConfigurationMappingId}
          control={
            <Switch
              id={props.searchConfigurationMappingId}
              color="primary"
              checked={checked}
              onChange={saveConfig}
            />
          }
          label={props.friendlyName}
          labelPlacement="start"
        />
      </Tooltip>
      <Snackbar
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        open={snackOpen}
        autoHideDuration={4000}
        onClose={handleClose}>
        <Alert
          onClose={handleClose}
          severity={alertSeverity}
          sx={{ width: "100%" }}>
          {snackMessage}
        </Alert>
      </Snackbar>
    </FormGroup>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchIndexes: reduxState.searchIndexes,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

CheckBoxConfig.propTypes = {
  id: PropTypes.string,
  description: PropTypes.string,
  searchConfigurationMappingId: PropTypes.string,
  seachConfigurationOptionId: PropTypes.string,
  value: PropTypes.string,
  reduxSelectedSearchIndex: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
  friendlyName: PropTypes.array,
};

export default connect(mapStateToProps)(CheckBoxConfig);
