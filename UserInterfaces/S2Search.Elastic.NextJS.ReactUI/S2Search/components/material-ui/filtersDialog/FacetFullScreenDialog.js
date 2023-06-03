import React from "react";
import PropTypes from "prop-types";
import Dialog from "@mui/material/Dialog";
import Slide from "@mui/material/Slide";
import { connect } from "react-redux";
import componentActions from "../../../redux/actions/componentActions";
import { makeStyles } from "@mui/styles";
import FacetAppBar from "../filtersDialog/FacetAppBar";
import FacetSelectionMenu from "../filtersDialog/FacetSelectionMenu";
import FacetSelectionList from "../filtersDialog/FacetSelectorList";
import { LogDetails } from "../../../helpers/LogDetails";

const useStyles = makeStyles(() => ({
  root: {
    display: "flex",
  },
}));

// *********************************************************************************************************************
// ** - WARNING
// ** this has to be outside of the component - adding inside will overlay with transparemcy so nothing can be selected
// *********************************************************************************************************************
const Transition = React.forwardRef(function Transition(props, ref) {
  return <Slide direction="up" ref={ref} {...props} />;
});

const FacetFullScreenDialog = (props) => {
  const classes = useStyles();

  const handleClose = () => {
    props.saveDialogOpen(false);
  };

  return (
    <div>
      <LogDetails logData={props} enable={false} />
      <Dialog
        fullScreen
        open={props.reduxDialogOpen}
        onClose={handleClose}
        TransitionComponent={Transition}>
        <div className={classes.root}>
          <FacetAppBar />
          <FacetSelectionMenu />
          <FacetSelectionList />
        </div>
      </Dialog>
    </div>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxDialogOpen: reduxState.componentReducer.dialogOpen,
    defaultFacetData: reduxState.facetReducer.defaultFacetData,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveDialogOpen: (dialogOpen) =>
      dispatch(componentActions.saveDialogOpen(dialogOpen)),
  };
};

FacetFullScreenDialog.propTypes = {
  reduxDialogOpen: PropTypes.bool,
  saveDialogOpen: PropTypes.func,
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(FacetFullScreenDialog);
