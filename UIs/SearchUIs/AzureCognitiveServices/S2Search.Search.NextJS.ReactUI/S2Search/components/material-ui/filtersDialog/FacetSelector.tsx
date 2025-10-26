import React, { useState } from 'react';
import { connect, ConnectedProps } from 'react-redux';
import Checkbox from '@mui/material/Checkbox';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import { FormatStringOrNumeric } from '../../../common/functions/SharedFunctions';
import { FacetToParseAsNumeric } from '../../../common/Constants';
import { setSearchTerm } from '../../../store/slices/searchSlice';
import { setFacetSelectors } from '../../../store/slices/facetSlice';
import { RootState } from '../../../store';

interface FacetSelectorOwnProps {
  facet: {
    facetDisplayText: string;
    value?: string | number;
    count?: number;
    type?: string;
    from?: number;
    to?: number;
    [key: string]: any;
  };
  selectedFacet?: string;
  isChecked?: boolean;
}

type FacetSelectorProps = ConnectedProps<typeof connector> & FacetSelectorOwnProps;

const FacetSelector: React.FC<FacetSelectorProps> = (props) => {
  const [checked, setChecked] = useState<boolean>(props.isChecked || false);

  const checkboxFacetNameAsString = `${props.facet.facetDisplayText}`;

  const FacetOnClick = (): void => {
    const checkedValue = !checked;
    setChecked(checkedValue);
    if (checkedValue === false) {
      handleDelete(props.facet);
    }
    buildFacetSelectors(checkedValue);
  };

  const handleDelete = (facetToDelete: any): void => {
    let facetSelectorArray = [...props.reduxFacetSelectors];
    const forDeletion = [facetToDelete.value];

    if (facetSelectorArray.length > 0) {
      facetSelectorArray = facetSelectorArray.filter(
        item => !forDeletion.includes(item.facetDisplayText)
      );

      props.saveFacetSelectors(facetSelectorArray);
    } else {
      props.saveFacetSelectors([]);
    }
  };

  const buildFacetSelectors = (isChecked: boolean): void => {
    const selectedFacetData = {
      facetKey: props.selectedFacet || '',
      facetDisplayText: props.facet.facetDisplayText,
      luceneExpression: '',
      checked: isChecked,
    };

    if (props.facet.type === 'Range') {
      selectedFacetData.luceneExpression = `${
        props.selectedFacet
      } ge ${FormatStringOrNumeric(String(props.facet.from || 0))} and ${
        props.selectedFacet
      } le ${FormatStringOrNumeric(String(props.facet.to || 0))}`;
    } else {
      if (FacetToParseAsNumeric.includes(selectedFacetData.facetKey)) {
        selectedFacetData.luceneExpression = `${
          props.selectedFacet
        } eq ${FormatStringOrNumeric(String(props.facet.value || 0))}`;
      } else {
        selectedFacetData.luceneExpression = `${props.selectedFacet} eq '${props.facet.value || ''}'`;
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
        const index = props.reduxFacetSelectors.findIndex(
          x => x.luceneExpression === selectedFacetData.luceneExpression
        );

        const facetSelectorArray = [...props.reduxFacetSelectors];
        facetSelectorArray[index] = selectedFacetData;

        props.saveFacetSelectors(facetSelectorArray);
      } else {
        // facet not in redux - add it
        const reduxFacetSelectorsArray = [...props.reduxFacetSelectors];
        reduxFacetSelectorsArray.push(selectedFacetData);
        props.saveFacetSelectors(reduxFacetSelectorsArray);
      }
    }
  };

  const renderVehicleCount = (count?: number): string => {
    if (!count) return '0 Vehicles';
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

const mapStateToProps = (reduxState: RootState) => {
  return {
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxResultsCount: reduxState.search.searchCount,
    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveSearchTerm: (searchTerm: string) => dispatch(setSearchTerm(searchTerm)),
    saveFacetSelectors: (facetSelectors: any[]) =>
      dispatch(setFacetSelectors(facetSelectors)),
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);

export default connector(FacetSelector);