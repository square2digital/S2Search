import React, { useEffect } from "react";
import PropTypes from "prop-types";
import { makeStyles } from "@mui/styles";
import Chip from "@mui/material/Chip";
import Paper from "@mui/material/Paper";
import { connect } from "react-redux";
import facetActions from "../../../redux/actions/facetActions";
import { createTheme, ThemeProvider } from "@mui/material/styles";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    justifyContent: "left",
    flexWrap: "wrap",
    listStyle: "none",
    marginTop: theme.spacing(1.5),
    marginLeft: "-40px",
  },
  chip: {
    paddingRight: "10px",
    paddingTop: "5px",
  },
}));

const FacetChips = (props) => {
  const classes = useStyles();

  useEffect(() => {
    reduxFacetSelectors(props);
  }, [props.reduxFacetSelectors]);

  const reduxFacetSelectors = (props) => {
    if (props.reduxFacetSelectors.length > 0) {
      props.saveFacetSelectors(props.reduxFacetSelectors);
    }
  };

  const handleDelete = (facetChipToDelete) => () => {
    let updatedArray = props.reduxFacetSelectors.filter(
      (facet) => facet.facetDisplayText !== facetChipToDelete.facetDisplayText
    );

    props.saveFacetSelectors(updatedArray);
    props.saveFacetChipDeleted(props.reduxFacetChipDeleted + 1);
  };

  const theme = createTheme({
    palette: {
      primary: {
        main: props.reduxPrimaryColour,
      },
      secondary: {
        main: props.reduxSecondaryColour,
      },
    },
  });

  return (
    <Paper elevation={0} component="ul" className={classes.root}>
      {props.reduxFacetSelectors.map((data, index) => {
        return (
          <li key={index} className={classes.chip}>
            <ThemeProvider theme={theme}>
              <Chip
                key={index}
                clickable
                label={data.facetDisplayText}
                onDelete={handleDelete(data)}
                color="secondary"
              />
            </ThemeProvider>
          </li>
        );
      })}
    </Paper>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxFacetSelectors: reduxState.facetReducer.facetSelectors,
    reduxFacetChipDeleted: reduxState.facetReducer.facetChipDeleted,
    reduxPrimaryColour: reduxState.themeReducer.primaryColour,
    reduxSecondaryColour: reduxState.themeReducer.secondaryColour,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveFacetSelectors: (facetSelectorArray) =>
      dispatch(facetActions.saveFacetSelectors(facetSelectorArray)),
    saveFacetChipDeleted: (facetChipDeleted) =>
      dispatch(facetActions.saveFacetChipDeleted(facetChipDeleted)),
  };
};

FacetChips.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  saveFacetSelectors: PropTypes.func,
  saveFacetChipDeleted: PropTypes.func,
  reduxFacetChipDeleted: PropTypes.number,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetChips);
