import React from "react";
import { makeStyles } from "@mui/styles";
import PropTypes from "prop-types";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import searchActions from "../../../../redux/actions/searchActions";
import { GetOrderByData } from "../../../../common/Constants";
import { connect } from "react-redux";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";

const useStyles = makeStyles((theme) => ({
  formControl: {
    margin: theme.spacing(0),
    minWidth: 120,
  },
}));

const OrderByRadios = (props) => {
  const classes = useStyles();
  const [orderBy, setOrderBy] = React.useState("");
  const [sortOrder, setSortOrder] = React.useState("");

  const GenerateRadios = () => {
    const dropdownArray = [];

    if (GetOrderByData) {
      let index = 0;
      for (let order of GetOrderByData) {
        dropdownArray.push(
          <FormControlLabel
            key={index}
            value={`${order.value}_${order.sort}`}
            control={
              <Radio
                checked={
                  order.value === props.reduxOrderBy &&
                  order.sort === props.reduxSortOrder
                }
              />
            }
            label={order.display}
          />
        );
        index++;
      }
    }

    return dropdownArray;
  };

  const handleChange = (event) => {
    var fields = event.target.value.split("_");
    var order = fields[0];
    var sort = fields[1];

    setOrderBy(order);
    props.saveOrderby(order);
    setSortOrder(sort);
    props.saveSortOrder(sort);
  };

  return (
    <div>
      <FormControl variant="outlined" className={classes.formControl}>
        <RadioGroup
          color="secondary"
          label="Order By"
          name="Order By"
          value={`${orderBy}_${sortOrder}`}
          onChange={handleChange}>
          {GenerateRadios()}
        </RadioGroup>
      </FormControl>
    </div>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSearchTerm: reduxState.searchReducer.searchTerm,
    reduxOrderBy: reduxState.searchReducer.orderBy,
    reduxSortOrder: reduxState.searchReducer.sortOrder,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    saveOrderby: (orderBy) => dispatch(searchActions.saveOrderby(orderBy)),
    saveSortOrder: (sortOrder) =>
      dispatch(searchActions.saveSortOrder(sortOrder)),
  };
};

OrderByRadios.propTypes = {
  reduxSearchTerm: PropTypes.string,
  reduxOrderBy: PropTypes.string,
  reduxSortOrder: PropTypes.string,
  saveOrderby: PropTypes.func,
  saveSortOrder: PropTypes.func,
};

export default connect(mapStateToProps, mapDispatchToProps)(OrderByRadios);
