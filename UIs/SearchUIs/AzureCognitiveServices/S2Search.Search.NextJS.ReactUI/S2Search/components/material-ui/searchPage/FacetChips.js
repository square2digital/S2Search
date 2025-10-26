import React from 'react';
import PropTypes from 'prop-types';
import Chip from '@mui/material/Chip';
import Paper from '@mui/material/Paper';
import { connect } from 'react-redux';
import {
  setFacetSelectors,
  setFacetChipDeleted,
} from '../../../store/slices/facetSlice';

// Modern styles object
const styles = {
  root: theme => ({
    display: 'flex',
    justifyContent: 'left',
    flexWrap: 'wrap',
    listStyle: 'none',
    marginTop: theme.spacing(1.5),
    marginLeft: '-40px',
  }),
  chip: {
    paddingRight: '10px',
    paddingTop: '5px',
  },
};

const FacetChips = props => {
  // Removed problematic useEffect that was causing infinite loops
  // The facet selectors are already managed by the parent component

  const handleDelete = facetChipToDelete => () => {
    const updatedArray = props.reduxFacetSelectors.filter(
      facet => facet.facetDisplayText !== facetChipToDelete.facetDisplayText
    );

    props.saveFacetSelectors(updatedArray);
    props.saveFacetChipDeleted(props.reduxFacetChipDeleted + 1);
  };

  return (
    <Paper elevation={0} component="ul" sx={styles.root}>
      {props.reduxFacetSelectors.map((data, index) => {
        return (
          <li key={index} style={styles.chip}>
            <Chip
              key={index}
              clickable
              label={data.facetDisplayText}
              onDelete={handleDelete(data)}
              color="secondary"
            />
          </li>
        );
      })}
    </Paper>
  );
};

const mapStateToProps = reduxState => {
  return {
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetChipDeleted: reduxState.facet.facetChipDeleted,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveFacetSelectors: facetSelectorArray =>
      dispatch(setFacetSelectors(facetSelectorArray)),
    saveFacetChipDeleted: facetChipDeleted =>
      dispatch(setFacetChipDeleted(facetChipDeleted)),
  };
};

FacetChips.propTypes = {
  reduxFacetSelectors: PropTypes.array,
  saveFacetSelectors: PropTypes.func,
  saveFacetChipDeleted: PropTypes.func,
  reduxFacetChipDeleted: PropTypes.number,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetChips);
