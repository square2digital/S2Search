import React from 'react';
import Chip from '@mui/material/Chip';
import Paper from '@mui/material/Paper';
import { connect, ConnectedProps } from 'react-redux';
import {
  setFacetSelectors,
  setFacetChipDeleted,
  setFacetSelectedKeys,
} from '../../../store/slices/facetSlice';
import { RootState } from '../../../store';

// Modern styles object
const styles = {
  root: {
    display: 'flex',
    justifyContent: 'left' as const,
    flexWrap: 'wrap' as const,
    listStyle: 'none' as const,
    marginTop: 1.5,
    marginLeft: '-40px',
  },
  chip: {
    paddingRight: '10px',
    paddingTop: '5px',
  },
};

interface FacetData {
  facetDisplayText: string;
  facetKey: string;
  [key: string]: any;
}

const FacetChips: React.FC<ConnectedProps<typeof connector>> = (props) => {
  // Removed problematic useEffect that was causing infinite loops
  // The facet selectors are already managed by the parent component

  const handleDelete = (facetChipToDelete: FacetData) => () => {
    const updatedArray = props.reduxFacetSelectors.filter(
      facet => facet.facetDisplayText !== facetChipToDelete.facetDisplayText
    );

    // Also update facetSelectedKeys by removing the facetKey of the deleted facet
    const updatedSelectedKeys = props.reduxFacetSelectedKeys.filter(
      facetKey => facetKey !== facetChipToDelete.facetKey
    );

    props.saveFacetSelectors(updatedArray);
    props.saveFacetSelectedKeys(updatedSelectedKeys);
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

const mapStateToProps = (reduxState: RootState) => {
  return {
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
    reduxFacetChipDeleted: reduxState.facet.facetChipDeleted,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveFacetSelectors: (facetSelectorArray: any[]) =>
      dispatch(setFacetSelectors(facetSelectorArray)),
    saveFacetSelectedKeys: (facetSelectedKeys: string[]) =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveFacetChipDeleted: (facetChipDeleted: number) =>
      dispatch(setFacetChipDeleted(facetChipDeleted)),
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);

export default connector(FacetChips);