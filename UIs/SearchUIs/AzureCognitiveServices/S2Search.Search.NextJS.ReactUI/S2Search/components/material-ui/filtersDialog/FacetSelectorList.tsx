import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import React, { useCallback, useEffect, useState } from 'react';
import { connect, ConnectedProps } from 'react-redux';
import {
  getDefaultFacetsWithSelections,
  isSelectFacetMenuAlreadySelected,
} from '../../../common/functions/FacetFunctions';
import { RootState } from '../../../store';
import {
  setFacetSelectors,
  setSelectedFacet,
} from '../../../store/slices/facetSlice';
import {
  setOrderBy,
  setPageNumber,
  setSearchTerm,
  setVehicleData,
} from '../../../store/slices/searchSlice';
import { setDialogOpen } from '../../../store/slices/uiSlice';
import FacetSelector from '../filtersDialog/FacetSelector';

// Re-export interfaces from FacetFunctions for consistency
interface FacetItem {
  facetDisplayText: string;
  selected?: boolean;
  checked?: boolean;
  value?: string | number;
  count?: number;
  type?: string;
  from?: number;
  to?: number;
  [key: string]: any;
}

interface FacetStateData {
  facetKey?: string;
  facetKeyDisplayName?: string;
  facetItems?: FacetItem[];
  enabled?: boolean;
  [key: string]: any;
}

const FacetSelectionList: React.FC<
  ConnectedProps<typeof connector>
> = props => {
  const [facetState, setfacetState] = useState<FacetStateData>({});

  const handleChecked = useCallback(
    (theFacet: FacetStateData): FacetStateData => {
      let theFacetUpdated = theFacet;
      const updatedFacetItems: FacetItem[] = [];

      if (theFacet.facetItems) {
        theFacet.facetItems.map(facetItem => {
          if (
            props.reduxFacetSelectors.some(
              f => f.facetDisplayText === facetItem.facetDisplayText
            )
          ) {
            updatedFacetItems.push({ ...facetItem, selected: true });
          } else {
            updatedFacetItems.push({ ...facetItem, selected: false });
          }
        });

        if (updatedFacetItems.length > 0) {
          theFacetUpdated = {
            ...theFacetUpdated,
            facetItems: updatedFacetItems,
          };
        }
      }

      return theFacetUpdated;
    },
    [props.reduxFacetSelectors]
  );

  const generateFacetSelectors = useCallback(
    (facetKeyName: string): void => {
      let theFacet: FacetStateData = {};
      let currentFacet: any = {};
      let facetData: any[] = [];
      const selectedFacetData: any[] = [];

      let facetsToLoad: any[] = [];

      // Fixed logic for multi-selection support:
      // Always use defaultFacetData for displaying all available facet options
      // This prevents filtered API responses from hiding unselected options

      // Use localStorage backup if Redux state is empty (for persistence)
      let defaultFacetDataToUse = props.reduxDefaultFacetData;
      if (defaultFacetDataToUse.length === 0 && typeof window !== 'undefined') {
        const storedFacetData = localStorage.getItem('originalFacetData');
        if (storedFacetData) {
          try {
            defaultFacetDataToUse = JSON.parse(storedFacetData);
          } catch (error) {
            console.warn('Failed to parse stored facet data:', error);
          }
        }
      }

      if (
        isSelectFacetMenuAlreadySelected(
          props.reduxFacetSelectors,
          facetKeyName
        )
      ) {
        // Merge selections with default facets to show checked state
        facetsToLoad = getDefaultFacetsWithSelections(
          facetKeyName,
          defaultFacetDataToUse as any,
          props.reduxFacetSelectors
        );
      } else {
        // No selections yet - show all default facets
        facetsToLoad = defaultFacetDataToUse;
      }

      // Note: We no longer use reduxFacetData for display to preserve all options
      // This allows multiple selections within the same filter category

      facetData = facetsToLoad.filter(x => x.facetKey === facetKeyName);

      console.log('DEBUG - Filtered facetData for', facetKeyName, ':', {
        facetsToLoadKeys: facetsToLoad.map(f => f.facetKey),
        filteredCount: facetData.length,
        targetKey: facetKeyName,
        firstFacetItems: facetData[0]?.facetItems?.length || 0,
      });

      currentFacet = facetData[0];

      theFacet = { ...currentFacet, enabled: false };

      // check if the facet is enabled
      if (props.reduxFacetData.length > 0) {
        if (selectedFacetData.length > 0) {
          theFacet = { ...currentFacet, enabled: selectedFacetData[0].enabled };
        }
      } else {
        theFacet = { ...currentFacet, enabled: false };
      }

      if (props.reduxFacetSelectors.length > 0) {
        const theFacetUpdated = handleChecked(theFacet);
        setfacetState(theFacetUpdated);
      } else {
        setfacetState(theFacet);
      }
    },
    [
      props.reduxSearchTerm,
      props.reduxFacetData,
      props.reduxFacetSelectors,
      props.reduxDefaultFacetData,
      handleChecked,
    ]
  );

  useEffect(() => {
    if (props.reduxSelectedFacet) {
      generateFacetSelectors(props.reduxSelectedFacet);
    }
  }, [props.reduxSelectedFacet, generateFacetSelectors]);

  return (
    <Box
      component="main"
      sx={{
        flexGrow: 1,
        ml: '280px',
        mt: '64px',
        p: 4,
        backgroundColor: '#f8fafc',
        minHeight: 'calc(100vh - 64px)',
      }}
    >
      {facetState.facetItems !== undefined ? (
        <>
          <Box sx={{ mb: 4 }}>
            <Typography
              variant="h4"
              sx={{
                fontWeight: 700,
                color: 'text.primary',
                mb: 1,
              }}
            >
              {facetState.facetKeyDisplayName || 'Select Options'}
            </Typography>
            <Typography
              variant="body1"
              sx={{
                color: 'text.secondary',
                mb: 3,
              }}
            >
              Choose from the options below to filter your results
            </Typography>
          </Box>

          <Box
            sx={{
              display: 'grid',
              gridTemplateColumns: {
                xs: 'repeat(1, 1fr)',
                sm: 'repeat(2, 1fr)',
                md: 'repeat(3, 1fr)',
                lg: 'repeat(4, 1fr)',
              },
              gap: 3,
              maxWidth: '100%',
            }}
          >
            {facetState.facetItems.map((facetSelectorItem, index) => (
              <Box key={`${index}-${facetSelectorItem.facetDisplayText}`}>
                <FacetSelector
                  facet={facetSelectorItem}
                  selectedFacet={facetState.facetKey}
                  isChecked={facetSelectorItem.selected}
                />
              </Box>
            ))}
          </Box>
        </>
      ) : (
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: '400px',
            textAlign: 'center',
          }}
        >
          <Typography
            variant="h5"
            sx={{
              fontWeight: 600,
              color: 'text.secondary',
              mb: 2,
            }}
          >
            Select a filter category
          </Typography>
          <Typography
            variant="body1"
            sx={{
              color: 'text.secondary',
            }}
          >
            Choose a category from the left menu to view filter options
          </Typography>
        </Box>
      )}
    </Box>
  );
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveVehicleData: (vehicleData: any[]) =>
      dispatch(setVehicleData(vehicleData)),
    savePageNumber: (pageNumber: number) => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: (resetFacetArray: any[]) =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveSearchTerm: (searchTerm: string) => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: (orderBy: string) => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: (dialogOpen: boolean) =>
      dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: (facetName: string) =>
      dispatch(setSelectedFacet(facetName)),
  };
};

const mapStateToProps = (reduxState: RootState) => {
  return {
    reduxSearchTerm: reduxState.search.searchTerm,
    reduxSearchCount: reduxState.search.searchCount,
    reduxVehicleData: reduxState.search.vehicleData,
    reduxOrderBy: reduxState.search.orderBy,
    reduxPageNumber: reduxState.search.pageNumber,
    reduxNetworkError: reduxState.search.networkError,
    reduxPreviousRequest: reduxState.search.previousRequest,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxDefaultFacetData: reduxState.facet.defaultFacetData,
    reduxFacetData: reduxState.facet.facetData,
    reduxDialogOpen: reduxState.ui.isDialogOpen,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);

export default connector(FacetSelectionList);
