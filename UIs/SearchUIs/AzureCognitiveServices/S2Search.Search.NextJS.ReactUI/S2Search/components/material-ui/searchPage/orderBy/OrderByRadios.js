import React from 'react';
import { useTheme } from '@mui/material/styles';
import PropTypes from 'prop-types';
import FormControl from '@mui/material/FormControl';
import FormControlLabel from '@mui/material/FormControlLabel';
import searchActions from '../../../../redux/actions/searchActions';
import { GetOrderByData } from '../../../../common/Constants';
import { connect } from 'react-redux';
import Radio from '@mui/material/Radio';
import RadioGroup from '@mui/material/RadioGroup';

// Inline styles object (converted from makeStyles)
const styles = {
  formControl: {
    margin: 0, // theme.spacing(0)
    minWidth: 120,
  },
};

const OrderByRadios = props => {
  const theme = useTheme();
  const [orderBy, setOrderBy] = React.useState('');

  const GenerateRadios = () => {
    const dropdownArray = [];

    if (GetOrderByData) {
      let index = 0;
      for (let order of GetOrderByData) {
        dropdownArray.push(
          <FormControlLabel
            key={index}
            value={order.value}
            control={<Radio checked={order.value === props.reduxOrderBy} />}
            label={order.display}
          />
        );
        index++;
      }
    }

    return dropdownArray;
  };

  const handleChange = event => {
    setOrderBy(event.target.value);
    props.saveOrderby(event.target.value);
  };

  return (
    <div>
      <FormControl variant="outlined" style={styles.formControl}>
        <RadioGroup
          color="secondary"
          label="Order By"
          name="Order By"
          value={orderBy}
          onChange={handleChange}
        >
          {GenerateRadios()}
        </RadioGroup>
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

OrderByRadios.propTypes = {
  reduxOrderBy: PropTypes.string,
  saveOrderby: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(OrderByRadios);
