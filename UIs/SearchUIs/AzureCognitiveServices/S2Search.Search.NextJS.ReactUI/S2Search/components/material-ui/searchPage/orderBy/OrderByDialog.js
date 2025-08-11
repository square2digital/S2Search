import React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import OrderByRadios from './OrderByRadios';
import FormControl from '@mui/material/FormControl';

const OrderByDialog = () => {
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  return (
    <div>
      <Button
        size="small"
        color="secondary"
        variant="contained"
        onClick={handleClickOpen}
      >
        Sort
      </Button>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Order Results</DialogTitle>
        <DialogContent>
          <form
            style={{
              display: 'flex',
              flexWrap: 'wrap',
            }}
          >
            <FormControl
              sx={{
                margin: 1,
                minWidth: 120,
              }}
            >
              <OrderByRadios />
            </FormControl>
          </form>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default OrderByDialog;
