import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import Select from '@mui/material/Select';
import React from 'react';
import { GetOrderByData } from '../../../../common/Constants';
import { useAppDispatch, useAppSelector } from '../../../../store/hooks';
import { setOrderBy } from '../../../../store/slices/searchSlice';

// Inline styles object (converted from makeStyles)
const styles = {
  formControl: {
    margin: 0,
    minWidth: 120,
  },
};

interface OrderByOption {
  value: string;
  display: string;
}

const OrderByDropdown: React.FC = () => {
  const [orderBy, setLocalOrderBy] = React.useState<string>('');
  const [open, setOpen] = React.useState<boolean>(false);

  const dispatch = useAppDispatch();
  const reduxOrderBy = useAppSelector(state => state.search.orderBy);

  const GenerateDropdown = (): React.ReactElement[] => {
    const dropdownArray: React.ReactElement[] = [];

    if (GetOrderByData) {
      for (const order of GetOrderByData as OrderByOption[]) {
        dropdownArray.push(
          <MenuItem key={order.value} value={order.value}>
            {order.display}
          </MenuItem>
        );
      }
    }

    return dropdownArray;
  };

  const handleChange = (event: any): void => {
    setLocalOrderBy(event.target.value);
    dispatch(setOrderBy(event.target.value));
  };

  const handleClose = (): void => {
    setOpen(false);
  };

  const handleOpen = (): void => {
    setOpen(true);
  };

  return (
    <div>
      <FormControl variant="outlined" style={styles.formControl}>
        <InputLabel htmlFor="demo-customized-select-native" color="secondary">
          Order By
        </InputLabel>
        <Select
          open={open}
          color="secondary"
          onClose={handleClose}
          onOpen={handleOpen}
          value={orderBy}
          renderValue={selected => reduxOrderBy !== selected}
          onChange={handleChange}
          label="Order By"
        >
          {GenerateDropdown()}
        </Select>
      </FormControl>
    </div>
  );
};

export default OrderByDropdown;
