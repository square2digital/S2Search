import React, { useEffect, useState, forwardRef } from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";
import PropTypes from "prop-types";
import TextField from "@mui/material/TextField";
import { updateTheme, getThemesByCustomerId } from "../../client/themeClient";
import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";
import Grid from "@mui/material/Grid";
import { getDefaultPanelStyles } from "../../styles/common";
import { setSelectedSearchIndexValue } from "../../redux/actions/selectedSearchIndexActions";
import Spinner from "../common/Spinner";
import { homePageRedirectOnNullSearchIndex } from "../utilities/helpers";

const Alert = forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

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
  form: {
    "& .MuiTextField-root": {
      margin: theme.spacing(1),
      width: "100%",
    },
  },
  saveButton: {
    marginTop: theme.spacing(1),
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
  },
  colourPickerLabel: {
    backgroundColor: "black",
    height: "32px",
    width: "32px",
    display: "block",
    marginTop: theme.spacing(2),
    border: "1px solid #404040",
  },
  colourPickerInput: {
    visibility: "hidden",
  },
}));

const S2ThemePanel = (props) => {
  const classes = useStyles();
  const [themeId, setThemeId] = useState("");
  const [primaryHexColour, setPrimaryHexColour] = useState("");
  const [primaryHexColourValid, setPrimaryHexColourValid] = useState(true);
  const [secondaryHexColour, setSecondaryHexColour] = useState("");
  const [secondaryHexColourValid, setSecondaryHexColourValid] = useState(true);
  const [navBarHexColour, setNavBarHexColour] = useState("");
  const [navBarHexColourValid, setNavBarHexColourValid] = useState(true);
  const [logoURL, setLogoURL] = useState("");
  const [logoURLValid, setlogoURLValid] = useState(true);
  const [missingImageURL, setMissingImageURL] = useState("");
  const [missingImageURLValid, setMissingImageURLValid] = useState(true);
  const [IsformValid, setIsformValid] = useState(true);
  const [snackOpen, setSnackOpen] = useState(false);
  const [snackMessage, setSnackMessage] = useState("");
  const [alertSeverity, setAlertSeverity] = useState("success");
  const hexColourRegEx = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
  const UrlRegEx =
    /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/gi;

  useEffect(() => {
    document.title = props.title;

    homePageRedirectOnNullSearchIndex(props.reduxSelectedSearchIndex.id);

    getCustomerThemes(
      props.reduxCustomer.customerId,
      props.reduxSelectedSearchIndex.id
    );

    configureColourPicker("primaryHexColourPickerInput");
    configureColourPicker("secondaryHexColourPickerInput");
    configureColourPicker("navBarHexColourPickerInput");
  }, []);

  useEffect(() => {
    homePageRedirectOnNullSearchIndex(props.reduxSelectedSearchIndex.id);

    let formValid =
      primaryHexColourValid &&
      secondaryHexColourValid &&
      navBarHexColourValid &&
      logoURLValid &&
      missingImageURLValid;

    setIsformValid(formValid);

    configureColourPicker("primaryHexColourPickerInput");
    configureColourPicker("secondaryHexColourPickerInput");
    configureColourPicker("navBarHexColourPickerInput");
  }, [
    primaryHexColourValid,
    secondaryHexColourValid,
    navBarHexColourValid,
    logoURLValid,
    missingImageURL,
    props.reduxSelectedSearchIndex.id,
  ]);

  const configureColourPicker = (colorPickerID) => {
    let colorInput = document.getElementById(colorPickerID);

    colorInput.addEventListener("input", () => {
      document.getElementById(colorPickerID).innerHTML = colorInput.value;
      switch (colorPickerID) {
        case "primaryHexColourPickerInput":
          setPrimaryHexColour(colorInput.value);
          break;
        case "secondaryHexColourPickerInput":
          setSecondaryHexColour(colorInput.value);
          break;
        case "navBarHexColourPickerInput":
          setNavBarHexColour(colorInput.value);
          break;
      }
    });
  };

  const getCustomerThemes = async (customerId, searchIndex) => {
    getThemesByCustomerId(customerId, searchIndex).then(function (data) {
      if (data) {
        let themes = data.result.themes;

        for (let theme of themes) {
          if (theme.searchIndexId === props.reduxSelectedSearchIndex.id) {
            setThemeId(theme.themeId);
            setPrimaryHexColour(theme.primaryHexColour);
            setPrimaryHexColourValid(IsHexColourValid(theme.primaryHexColour));

            setSecondaryHexColour(theme.secondaryHexColour);
            setSecondaryHexColourValid(
              IsHexColourValid(theme.secondaryHexColour)
            );

            setNavBarHexColour(theme.navBarHexColour);
            setNavBarHexColourValid(IsHexColourValid(theme.secondaryHexColour));

            setLogoURL(theme.logoURL);
            setlogoURLValid(IsLogoURLValid(theme.logoURL));

            setMissingImageURL(theme.missingImageURL);
            setMissingImageURLValid(IsLogoURLValid(theme.missingImageURL));
          }
        }

        return themes;
      }
    });
  };

  const updateInputValue = (value, type) => {
    switch (type) {
      case "primary":
        setPrimaryHexColour(value);
        setPrimaryHexColourValid(IsHexColourValid(value));
        break;
      case "secondary":
        setSecondaryHexColour(value);
        setSecondaryHexColourValid(IsHexColourValid(value));
        break;
      case "navbar":
        setNavBarHexColour(value);
        setNavBarHexColourValid(IsHexColourValid(value));
        break;
      case "logourl":
        setLogoURL(value);
        setlogoURLValid(IsLogoURLValid(value));
        break;
      case "missingImageURL":
        setMissingImageURL(value);
        setMissingImageURLValid(IsLogoURLValid(value));
        break;
    }
  };

  const handleClose = (event, reason) => {
    setSnackOpen(false);
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    const formData = Array.from(event.target.elements)
      .filter((el) => el.name)
      .reduce((a, b) => ({ ...a, [b.name]: b.value }), {});

    console.log(formData);

    updateTheme(
      props.reduxCustomer.customerId,
      props.reduxSelectedSearchIndex.id,
      formData
    ).then(function (result) {
      if (result.isError) {
        setSnackOpen(true);
        setAlertSeverity("error");
        setSnackMessage(`An error has occurred - Theme Values not updated`);
      } else {
        setSnackOpen(true);
        setAlertSeverity("success");
        setSnackMessage(`Theme Updated Successfully`);
      }
    });
  };

  const IsHexColourValid = (hexColourTerm) => {
    return new RegExp(hexColourRegEx).test(hexColourTerm);
  };

  const IsLogoURLValid = (logoURL) => {
    return new RegExp(UrlRegEx).test(logoURL);
  };

  return props.reduxCustomer.customerId === undefined ? (
    <Spinner />
  ) : (
    <main className={classes.content}>
      <Typography component="div" style={{ getDefaultPanelStyles }}>
        <h2>Theme</h2>
        <p>
          Set the Hex Values for the Primary, Secondary and Navigation Bar
          colours for your Search User Interface. Use the colour picker or enter
          the Hex colour codes of your choice
        </p>

        <Divider className={classes.divider} />

        <form
          onSubmit={handleSubmit}
          className={classes.form}
          noValidate
          autoComplete="off">
          <div>
            <input type="hidden" name="themeId" id="themeId" value={themeId} />

            <Grid container spacing={3}>
              <Grid item xs={10}>
                <TextField
                  name="primaryHexColour"
                  id="outlined-basic"
                  label="Primary Colour"
                  variant="outlined"
                  onChange={(e) => updateInputValue(e.target.value, "primary")}
                  value={primaryHexColour}
                  error={!primaryHexColourValid}
                />
              </Grid>
              <Grid item xs={2}>
                <label
                  className={classes.colourPickerLabel}
                  style={{ backgroundColor: `${primaryHexColour}` }}>
                  <input
                    type="color"
                    id="primaryHexColourPickerInput"
                    value={primaryHexColour}
                    className={classes.colourPickerInput}></input>
                </label>
              </Grid>
            </Grid>
          </div>
          <div>
            <Grid container spacing={3}>
              <Grid item xs={10}>
                <TextField
                  name="secondaryHexColour"
                  id="outlined-basic"
                  label="Secondary Colour"
                  variant="outlined"
                  onChange={(e) =>
                    updateInputValue(e.target.value, "secondary")
                  }
                  value={secondaryHexColour}
                  error={!secondaryHexColourValid}
                />
              </Grid>
              <Grid item xs={2}>
                <label
                  className={classes.colourPickerLabel}
                  style={{ backgroundColor: `${secondaryHexColour}` }}>
                  <input
                    type="color"
                    id="secondaryHexColourPickerInput"
                    value={secondaryHexColour}
                    className={classes.colourPickerInput}></input>
                </label>
              </Grid>
            </Grid>
          </div>
          <div>
            <Grid container spacing={3}>
              <Grid item xs={10}>
                <TextField
                  name="navBarHexColour"
                  id="outlined-basic"
                  label="NavBar Colour"
                  variant="outlined"
                  onChange={(e) => updateInputValue(e.target.value, "navbar")}
                  value={navBarHexColour}
                  error={!navBarHexColourValid}
                />
              </Grid>
              <Grid item xs={2}>
                <label
                  className={classes.colourPickerLabel}
                  style={{ backgroundColor: `${navBarHexColour}` }}>
                  <input
                    type="color"
                    id="navBarHexColourPickerInput"
                    value={navBarHexColour}
                    className={classes.colourPickerInput}></input>
                </label>
              </Grid>
            </Grid>
          </div>
          <div>
            <Grid container spacing={3}>
              <Grid item xs={10}>
                <TextField
                  name="logoURL"
                  id="outlined-basic"
                  label="Logo URL"
                  variant="outlined"
                  onChange={(e) => updateInputValue(e.target.value, "logourl")}
                  value={logoURL}
                  error={!logoURLValid}
                />
              </Grid>
            </Grid>
          </div>
          <div>
            <Grid container spacing={3}>
              <Grid item xs={10}>
                <TextField
                  name="MissingImageURL"
                  id="outlined-basic"
                  label="Missing Image URL"
                  variant="outlined"
                  onChange={(e) =>
                    updateInputValue(e.target.value, "missingImageURL")
                  }
                  value={missingImageURL}
                  error={!setMissingImageURLValid}
                />
              </Grid>
            </Grid>
          </div>
          <div>
            <Button
              type="submit"
              variant="contained"
              color="primary"
              disabled={!IsformValid}
              className={classes.saveButton}>
              Save Changes
            </Button>
          </div>
        </form>
      </Typography>
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
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchIndexes: reduxState.searchIndexes,
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
    reduxCustomer: reduxState.customer,
  };
};

const mapDispatchToProps = {
  setSelectedSearchIndexValue,
};

S2ThemePanel.propTypes = {
  user: PropTypes.object,
  reduxSelectedSearchIndex: PropTypes.object,
  reduxCustomer: PropTypes.object,
  reduxSearchIndexes: PropTypes.array,
  setSelectedSearchIndexValue: PropTypes.func.isRequired,
};

export default connect(mapStateToProps, mapDispatchToProps)(S2ThemePanel);
