import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import Box from "@mui/material/Box";
import { getDefaultPanelStyles } from "../../styles/common";
import PropTypes from "prop-types";
import { getConfig } from "../../client/configClient";
import CheckBoxConfig from "../configuration/CheckBoxConfig";
import PlaceholderText from "../configuration/PlaceholderText";
import { homePageRedirectOnNullSearchIndex } from "../utilities/helpers";

const useStyles = makeStyles((theme) => ({
  divider: {
    margin: theme.spacing(2, 0),
  },
  content: {
    padding: theme.spacing(3),
  },
}));

const S2ConfigurationPanel = (props) => {
  const classes = useStyles();
  const [config, setConfig] = useState([]);

  useEffect(() => {
    homePageRedirectOnNullSearchIndex(props.reduxSelectedSearchIndex.id);

    document.title = props.title;
    if (props.reduxSelectedSearchIndex.id) {
      getConfig(props.reduxSelectedSearchIndex.id).then(function (data) {
        if (data && !data.isError) {
          if (data.result.length > 0) {
            setConfig([...data.result]);
          }
        }
      });
    }
    setConfig([]);
  }, [props.reduxSelectedSearchIndex]);

  const generateConfigSwitches = () => {
    let configElements = [];

    if (config.length > 0) {
      for (let data of config) {
        if (data.dataType === "Bool") {
          configElements.push(
            <CheckBoxConfig
              id={data.searchConfigurationMappingId}
              searchConfigurationMappingId={data.searchConfigurationMappingId}
              seachConfigurationOptionId={data.seachConfigurationOptionId}
              description={data.description}
              friendlyName={data.friendlyName}
              value={data.value === "true"}
            />
          );
        }
      }
    }

    return configElements;
  };

  const generateConfigTextFields = () => {
    let configElements = [];

    if (config.length > 0) {
      for (let data of config) {
        if (data.dataType === "String") {
          if (data.orderIndex) {
            const config = (
              <PlaceholderText
                color="primary"
                id={data.searchConfigurationMappingId}
                searchConfigurationMappingId={data.searchConfigurationMappingId}
                seachConfigurationOptionId={data.seachConfigurationOptionId}
                description={data.description}
                friendlyName={data.friendlyName}
                value={data.value}
              />
            );

            configElements.push(config);
          }
        }
      }
    }

    return configElements;
  };

  return (
    <main className={classes.content}>
      <Typography component="div" style={{ getDefaultPanelStyles }}>
        <h2>Configuration</h2>
        <p>
          Configure your search instance and enable and disable the features
          that work for you. Simply update a configuration value and then
          refresh your search instance to view the change.
        </p>
      </Typography>
      <Box>
        <Divider className={classes.divider} />
        {generateConfigSwitches()}
        <Divider className={classes.divider} />
        <br />
        {generateConfigTextFields()}
      </Box>
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchIndexes: reduxState.searchIndexes,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

S2ConfigurationPanel.propTypes = {
  reduxSelectedSearchIndex: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
};

export default connect(mapStateToProps)(S2ConfigurationPanel);
