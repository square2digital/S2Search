import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Checkbox from '@mui/material/Checkbox';
import {
  FormatStringOrNumeric,
  FormatLongStrings,
} from '../../../common/functions/SharedFunctions';
import { FacetToParseAsNumeric, DefaultTheme } from '../../../common/Constants';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { useTheme } from '@mui/material/styles';
import { setSearchTerm } from '../../../store/slices/searchSlice';
import { setFacetSelectors } from '../../../store/slices/facetSlice';

const facetWidth_xs = 180;
const facetWidth_sm = 180;

const FacetSelector = props => {
  const theme = useTheme();
  const [checked, setChecked] = useState(props.isChecked);

  const checkboxFacetNameAsString = `${props.facet.facetDisplayText}`;
  const formattedFacetName = FormatLongStrings(
    `${props.facet.facetDisplayText}`,
    20
  );

  const FacetOnClick = () => {
    let checkedValue = !checked;
    setChecked(checkedValue);
    if (checkedValue === false) {
      handleDelete(props.facet);
    }
    buildFacetSelectors(checkedValue);
  };

  const handleDelete = facetToDelete => {
    let facetSelectorArray = [...props.reduxFacetSelectors];
    let forDeletion = [facetToDelete.value];

    if (facetSelectorArray.length > 0) {
      facetSelectorArray = facetSelectorArray.filter(
        item => !forDeletion.includes(item.facetDisplayText)
      );

      props.saveFacetSelectors(facetSelectorArray);
    } else {
      props.saveFacetSelectors([]);
    }
  };

  const buildFacetSelectors = isChecked => {
    let selectedFacetData = {
      facetKey: props.selectedFacet,
      facetDisplayText: props.facet.facetDisplayText,
      luceneExpression: '',
      checked: isChecked,
    };

    if (props.facet.type === 'Range') {
      selectedFacetData.luceneExpression = `${
        props.selectedFacet
      } ge ${FormatStringOrNumeric(props.facet.from)} and ${
        props.selectedFacet
      } le ${FormatStringOrNumeric(props.facet.to)}`;
    } else {
      if (FacetToParseAsNumeric.includes(selectedFacetData.facetKey)) {
        selectedFacetData.luceneExpression = `${
          props.selectedFacet
        } eq ${FormatStringOrNumeric(props.facet.value)}`;
      } else {
        selectedFacetData.luceneExpression = `${props.selectedFacet} eq '${props.facet.value}'`;
      }
    }

    if (!isChecked) {
      let copyArr = [...props.reduxFacetSelectors];
      copyArr = copyArr.filter(
        x => x.luceneExpression !== selectedFacetData.luceneExpression
      );
      props.saveFacetSelectors([...copyArr]);
    } else {
      if (
        props.reduxFacetSelectors == [] ||
        props.reduxFacetSelectors.length === 0
      ) {
        props.saveFacetSelectors([selectedFacetData]);
        return;
      }

      if (
        props.reduxFacetSelectors.some(
          x => x.luceneExpression === selectedFacetData.luceneExpression
        )
      ) {
        // facet is in redux - update it
        let index = props.reduxFacetSelectors.findIndex(
          x => x.luceneExpression === selectedFacetData.luceneExpression
        );

        let facetSelectorArray = [...props.reduxFacetSelectors];
        facetSelectorArray[index] = selectedFacetData;

        props.saveFacetSelectors(facetSelectorArray);
      } else {
        // facet not in redux - add it
        let reduxFacetSelectorsArray = [...props.reduxFacetSelectors];
        reduxFacetSelectorsArray.push(selectedFacetData);
        props.saveFacetSelectors(reduxFacetSelectorsArray);
      }
    }
  };

  const renderVehicleCount = count => {
    if (count === 1) {
      return `${count} Vehicle`;
    }

    return `${count} Vehicles`;
  };

  return (
    <>
      <List
        sx={{
          width: {
            xs: facetWidth_xs,
            sm: facetWidth_sm,
          },
          margin: 0.5,
          backgroundColor: '#ffffff',
          cursor: 'pointer',
          '&:hover': {
            backgroundColor: '#f2f2f2',
            boxShadow: '2px 2px 10px #efefef',
          },
          boxShadow: '5px 5px 10px #efefef',
        }}
        onClick={FacetOnClick}
      >
        <ListItem
          sx={{
            padding: 0,
            lineHeight: 0,
            marginTop: 0,
          }}
        >
          <Checkbox
            onChange={FacetOnClick}
            name={`${checkboxFacetNameAsString}`}
            checked={checked}
          />

          <ListItemText
            primary={formattedFacetName}
            secondary={<>{renderVehicleCount(props.facet.count)}</>}
          />
        </ListItem>
      </List>
    </>
  );
};

const mapStateToProps = reduxState => {
  return {
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxResultsCount: reduxState.search.searchCount,
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    saveSearchTerm: searchTerm => dispatch(setSearchTerm(searchTerm)),
    saveFacetSelectors: facetSelectors =>
      dispatch(setFacetSelectors(facetSelectors)),
  };
};

FacetSelector.propTypes = {
  reduxDialogOpen: PropTypes.bool,
  reduxResultsCount: PropTypes.number,
  reduxFacetSelectors: PropTypes.array,
  buttonLabel: PropTypes.string,
  dialogLabel: PropTypes.string,

  facet: PropTypes.object,
  selectedFacet: PropTypes.string,
  isChecked: PropTypes.bool,

  saveSearchTerm: PropTypes.func,
  saveFacetSelectors: PropTypes.func,
  reduxPrimaryColour: PropTypes.string,
  reduxSecondaryColour: PropTypes.string,
};

export default connect(mapStateToProps, mapDispatchToProps)(FacetSelector);
