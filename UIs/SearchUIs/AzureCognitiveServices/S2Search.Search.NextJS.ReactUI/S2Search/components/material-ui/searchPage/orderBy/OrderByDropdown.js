import React from 'react';
import { useTheme } from '@mui/material/styles';
import PropTypes from 'prop-types';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import searchActions from '../../../../redux/actions/searchActions';
import { GetOrderByData } from '../../../../common/Constants';
import { connect } from 'react-redux';

// Inline styles object (converted from makeStyles)
const styles = {
  formControl: {
    margin: 0, // theme.spacing(0)
    minWidth: 120,
  },
};

const OrderByDropdown = props => {
  const theme = useTheme();
  const [orderBy, setOrderBy] = React.useState('');
  const [open, setOpen] = React.useState(false);

  const GenerateDropdown = () => {
    const dropdownArray = [];

    if (GetOrderByData) {
      for (let order of GetOrderByData) {
        dropdownArray.push(
          <MenuItem value={order.value}>{`${order.display}`}</MenuItem>
        );
      }
    }

    return dropdownArray;
  };

  const handleChange = event => {
    setOrderBy(event.target.value);
    props.saveOrderby(event.target.value);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleOpen = () => {
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
          renderValue={selected => props.reduxOrderBy !== selected}
          onChange={handleChange}
          label="Order By"
        >
          {GenerateDropdown()}
        </Select>
      </FormControl>
    </div>
  );
};

const mapStateToProps = reduxState => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxOrderBy: reduxState.searchReducer.orderBy,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveOrderby: orderBy => dispatch(searchActions.saveOrderby(orderBy)),
  };
};

OrderByDropdown.propTypes = {
  reduxOrderBy: PropTypes.string,
  saveOrderby: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(OrderByDropdown);
