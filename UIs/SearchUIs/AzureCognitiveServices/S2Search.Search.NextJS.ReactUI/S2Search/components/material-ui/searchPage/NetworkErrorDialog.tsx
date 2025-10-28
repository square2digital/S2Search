import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Backdrop from '@mui/material/Backdrop';
import RefreshIcon from '@mui/icons-material/Refresh';
import { useAppDispatch, useAppSelector } from '../../../store/hooks';
import { setNetworkError } from '../../../store/slices/searchSlice';

const NetworkErrorDialog: React.FC = () => {
  const dispatch = useAppDispatch();
  const reduxNetworkError = useAppSelector(state => state.search.networkError);
  const [open, setOpen] = useState(reduxNetworkError);

  useEffect(() => {
    setOpen(reduxNetworkError);
  }, [reduxNetworkError]);

  const refreshPage = (): void => {
    dispatch(setNetworkError(false));
    window.location.reload();
  };

  const handleClose = (): void => {
    dispatch(setNetworkError(false));
    setOpen(false);
  };

  return (
    <div>
      <Backdrop
        sx={theme => ({
          zIndex: theme.zIndex.drawer + 1,
          color: '#fff',
        })}
        open={open}
      >
        <Dialog
          open={open}
          onClose={handleClose}
          aria-labelledby="alert-dialog-title"
          aria-describedby="alert-dialog-description"
        >
          <DialogTitle id="alert-dialog-title">
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
              startIcon={<RefreshIcon />}
            >
              Refresh
            </Button>
          </DialogActions>
        </Dialog>
      </Backdrop>
    </div>
  );
};

export default NetworkErrorDialog;