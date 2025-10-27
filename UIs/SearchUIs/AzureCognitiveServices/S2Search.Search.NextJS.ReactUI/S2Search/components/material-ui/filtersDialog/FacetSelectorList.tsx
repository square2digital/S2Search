import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import React, { useCallback, useEffect, useState } from 'react';
import { connect, ConnectedProps } from 'react-redux';
import { StaticFacets } from '../../../common/Constants';
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

      // ****************
      // facets To Load - default or from Search Results?
      // ****************
      // on the first load or if reduxFacetSelectors is empty we need to load the defaultFacets.
      // when a facet is selected, from that point the facets returned from search will be displayed

      let facetsToLoad: any[] = [];
      if (props.reduxSearchTerm) {
        facetsToLoad = props.reduxFacetData;
      } else if (
        props.reduxFacetSelectors.length === 0 &&
        StaticFacets.includes(facetKeyName)
      ) {
        facetsToLoad = props.reduxDefaultFacetData;
      } else {
        if (
          isSelectFacetMenuAlreadySelected(
            props.reduxFacetSelectors,
            facetKeyName
          )
        ) {
          facetsToLoad = getDefaultFacetsWithSelections(
            facetKeyName,
            props.reduxDefaultFacetData as any,
            props.reduxFacetSelectors
          );
        } else {
          facetsToLoad = props.reduxFacetData;
        }
      }

      facetData = facetsToLoad.filter(x => x.facetKey === facetKeyName);

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
