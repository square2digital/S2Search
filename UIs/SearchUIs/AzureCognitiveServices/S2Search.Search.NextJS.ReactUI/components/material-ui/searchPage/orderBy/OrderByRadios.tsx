import FormControl from '@mui/material/FormControl';
import FormControlLabel from '@mui/material/FormControlLabel';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';
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

const OrderByRadios: React.FC = () => {
  const dispatch = useAppDispatch();
  const reduxOrderBy = useAppSelector(state => state.search.orderBy);

  const GenerateRadios = (): React.ReactElement[] => {
    const dropdownArray: React.ReactElement[] = [];

    if (GetOrderByData) {
      let index = 0;
      for (const order of GetOrderByData as OrderByOption[]) {
        dropdownArray.push(
          <FormControlLabel
            key={index}
            value={order.value}
            control={<Radio checked={order.value === reduxOrderBy} />}
            label={order.display}
          />
        );
        index++;
      }
    }

    return dropdownArray;
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    dispatch(setOrderBy(event.target.value));
  };

  return (
    <div>
      <FormControl variant="outlined" style={styles.formControl}>
        <RadioGroup
          color="secondary"
          name="Order By"
          value={reduxOrderBy}
          onChange={handleChange}
        >
          {GenerateRadios()}
        </RadioGroup>
      </FormControl>
    </div>
  );
};

export default OrderByRadios;
