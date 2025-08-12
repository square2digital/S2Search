import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Checkbox from '@mui/material/Checkbox';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import {
  FormatStringOrNumeric,
  FormatLongStrings,
} from '../../../common/functions/SharedFunctions';
import { FacetToParseAsNumeric, DefaultTheme } from '../../../common/Constants';
import { useTheme } from '@mui/material/styles';
import { setSearchTerm } from '../../../store/slices/searchSlice';
import { setFacetSelectors } from '../../../store/slices/facetSlice';

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
      return '1 Vehicle';
    }
    return `${count.toLocaleString()} Vehicles`;
  };

  return (
    <Card
      onClick={FacetOnClick}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
        border: '1px solid',
        borderColor: checked ? 'primary.main' : 'grey.200',
        backgroundColor: checked ? 'primary.50' : 'white',
        boxShadow: checked
          ? '0 4px 12px rgba(0, 0, 0, 0.15)'
          : '0 2px 8px rgba(0, 0, 0, 0.1)',
        '&:hover': {
          transform: 'translateY(-2px)',
          boxShadow: checked
            ? '0 8px 25px rgba(0, 0, 0, 0.15)'
            : '0 4px 20px rgba(0, 0, 0, 0.12)',
          borderColor: checked ? 'primary.dark' : 'grey.300',
        },
        position: 'relative',
        overflow: 'visible',
      }}
    >
      <CardContent
        sx={{
          p: 3,
          pb: '16px !important',
          position: 'relative',
        }}
      >
        <Box
          sx={{
            display: 'flex',
            alignItems: 'flex-start',
            gap: 2,
          }}
        >
          <Checkbox
            checked={checked}
            onChange={FacetOnClick}
            name={checkboxFacetNameAsString}
            sx={{
              p: 0,
              '&.Mui-checked': {
                color: 'primary.main',
              },
            }}
          />

          <Box sx={{ flex: 1, minWidth: 0 }}>
            <Typography
              variant="body1"
              sx={{
                fontWeight: checked ? 600 : 500,
                color: checked ? 'primary.main' : 'text.primary',
                mb: 1,
                lineHeight: 1.4,
                wordBreak: 'break-word',
              }}
            >
              {props.facet.facetDisplayText}
            </Typography>

            <Chip
              label={renderVehicleCount(props.facet.count)}
              size="small"
              variant={checked ? 'filled' : 'outlined'}
              sx={{
                fontSize: '0.75rem',
                height: 24,
                backgroundColor: checked ? 'primary.main' : 'transparent',
                color: checked ? 'white' : 'text.secondary',
                borderColor: checked ? 'primary.main' : 'grey.300',
                '& .MuiChip-label': {
                  px: 1.5,
                  fontWeight: 500,
                },
              }}
            />
          </Box>
        </Box>

        {checked && (
          <Box
            sx={{
              position: 'absolute',
              top: 8,
              right: 8,
              width: 8,
              height: 8,
              borderRadius: '50%',
              backgroundColor: 'primary.main',
            }}
          />
        )}
      </CardContent>
    </Card>
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
