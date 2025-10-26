import React from 'react';
import Dialog from '@mui/material/Dialog';
import Slide from '@mui/material/Slide';
import Box from '@mui/material/Box';
import { connect, ConnectedProps } from 'react-redux';
import { setDialogOpen } from '../../../store/slices/uiSlice';
import FacetAppBar from '../filtersDialog/FacetAppBar';
import FacetSelectionMenu from '../filtersDialog/FacetSelectionMenu';
import FacetSelectionList from '../filtersDialog/FacetSelectorList';
import { LogDetails } from '../../../helpers/LogDetails';
import { RootState } from '../../../store';

// *********************************************************************************************************************
// ** - WARNING
// ** this has to be outside of the component - adding inside will overlay with transparemcy so nothing can be selected
// *********************************************************************************************************************
const Transition = React.forwardRef(function Transition(
  props: any,
  ref: React.Ref<unknown>
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

const FacetFullScreenDialog: React.FC<ConnectedProps<typeof connector>> = (
  props
) => {
  const handleClose = (): void => {
    props.saveDialogOpen(false);
  };

  return (
    <Box>
      <LogDetails logData={props} enable={false} />
      <Dialog
        fullScreen
        open={props.reduxDialogOpen}
        onClose={handleClose}
        TransitionComponent={Transition}
        sx={{
          '& .MuiDialog-paper': {
            backgroundColor: '#f8fafc',
          },
        }}
      >
        <Box
          sx={{
            display: 'flex',
            minHeight: '100vh',
            backgroundColor: '#f8fafc',
          }}
        >
          <FacetAppBar />
          <FacetSelectionMenu />
          <FacetSelectionList />
        </Box>
      </Dialog>
    </Box>
  );
};

const mapStateToProps = (reduxState: RootState) => {
  return {
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    defaultFacetData: reduxState.facet.defaultFacetData,
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveDialogOpen: (dialogOpen: boolean) => dispatch(setDialogOpen(dialogOpen)),
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);

export default connector(FacetFullScreenDialog);