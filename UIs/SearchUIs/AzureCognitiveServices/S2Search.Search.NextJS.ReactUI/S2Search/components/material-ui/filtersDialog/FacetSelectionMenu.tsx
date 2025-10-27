import RestartAltIcon from '@mui/icons-material/RestartAlt';
import Badge from '@mui/material/Badge';
import Box from '@mui/material/Box';
import Divider from '@mui/material/Divider';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import React from 'react';
import { connect, ConnectedProps } from 'react-redux';
import { GenerateUniqueID } from '../../../common/functions/SharedFunctions';
import { RootState } from '../../../store';
import {
  resetFacets,
  setFacetSelectedKeys,
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

const drawerWidth = 280;

const FacetSelectionMenu: React.FC<
  ConnectedProps<typeof connector>
> = props => {
  const resetFilters = (): void => {
    props.saveResetFacets();
  };

  const facetMenuClick = (facetKey: string): void => {
    props.saveSelectedFacet(facetKey);

    const arr = [...props.reduxFacetSelectedKeys];
    arr.push(facetKey);
    props.saveFacetSelectedKeys(arr);
  };

  const getSelectedCount = (facetKey: string): number => {
    return props.reduxFacetSelectors.filter(
      selector => selector.facetKey === facetKey
    ).length;
  };

  return (
    <nav
      style={{
        width: drawerWidth,
        flexShrink: 0,
        height: '100vh',
        position: 'fixed',
        left: 0,
        top: 64,
        zIndex: 1200,
        borderRight: '1px solid rgba(0, 0, 0, 0.08)',
      }}
    >
      <Paper
        elevation={0}
        sx={{
          width: '100%',
          height: '100%',
          borderRadius: 0,
          backgroundColor: 'white',
          overflow: 'auto',
        }}
      >
        <Box sx={{ p: 3, pb: 2 }}>
          <Typography
            variant="h6"
            sx={{
              fontWeight: 600,
              color: 'text.primary',
              mb: 1,
            }}
          >
            Filter Categories
          </Typography>
          <Typography
            variant="body2"
            sx={{
              color: 'text.secondary',
              mb: 2,
            }}
          >
            Select a category to view options
          </Typography>
        </Box>

        <List sx={{ px: 2 }}>
          {(() => {
            console.log(
              'FacetSelectionMenu - defaultFacetData:',
              props.defaultFacetData
            );
            return null;
          })()}
          {props.defaultFacetData?.map(facet => {
            const selectedCount = getSelectedCount(facet.facetKey);
            const isSelected = props.reduxSelectedFacet === facet.facetKey;

            return (
              <React.Fragment key={GenerateUniqueID()}>
                <ListItem disablePadding sx={{ mb: 1 }}>
                  <ListItemButton
                    onClick={() => facetMenuClick(facet.facetKey)}
                    sx={{
                      borderRadius: 2,
                      py: 1.5,
                      px: 2,
                      backgroundColor: isSelected
                        ? 'primary.50'
                        : 'transparent',
                      border: isSelected
                        ? '1px solid'
                        : '1px solid transparent',
                      borderColor: isSelected ? 'primary.200' : 'transparent',
                      '&:hover': {
                        backgroundColor: isSelected ? 'primary.100' : 'grey.50',
                      },
                      transition: 'all 0.2s ease-in-out',
                    }}
                  >
                    <ListItemText
                      primary={
                        <Typography
                          variant="body1"
                          sx={{
                            fontWeight: isSelected ? 600 : 500,
                            color: isSelected ? 'primary.main' : 'text.primary',
                          }}
                        >
                          {facet.facetKeyDisplayName}
                        </Typography>
                      }
                    />
                    {selectedCount > 0 && (
                      <Badge
                        badgeContent={selectedCount}
                        color="primary"
                        sx={{
                          '& .MuiBadge-badge': {
                            fontSize: '0.75rem',
                            minWidth: 20,
                            height: 20,
                          },
                        }}
                      />
                    )}
                  </ListItemButton>
                </ListItem>
              </React.Fragment>
            );
          })}

          <Divider sx={{ my: 2 }} />

          <ListItem disablePadding>
            <ListItemButton
              onClick={resetFilters}
              sx={{
                borderRadius: 2,
                py: 1.5,
                px: 2,
                border: '1px solid',
                borderColor: 'error.200',
                backgroundColor: 'error.50',
                '&:hover': {
                  backgroundColor: 'error.100',
                  borderColor: 'error.300',
                },
                transition: 'all 0.2s ease-in-out',
              }}
            >
              <ListItemIcon sx={{ minWidth: 40 }}>
                <RestartAltIcon
                  sx={{
                    color: 'error.main',
                    fontSize: 20,
                  }}
                />
              </ListItemIcon>
              <ListItemText
                primary={
                  <Typography
                    variant="body1"
                    sx={{
                      fontWeight: 600,
                      color: 'error.main',
                    }}
                  >
                    Reset All Filters
                  </Typography>
                }
              />
            </ListItemButton>
          </ListItem>
        </List>
      </Paper>
    </nav>
  );
};

const mapStateToProps = (reduxState: RootState) => {
  return {
    searchCount: reduxState.search.searchCount,
    reduxResultsCount: reduxState.search.searchCount,
    vehicleData: reduxState.search.vehicleData,

    reduxFacetSelectors: reduxState.facet.facetSelectors,
    reduxFacetSelectedKeys: reduxState.facet.facetSelectedKeys,
    defaultFacetData: reduxState.facet.defaultFacetData,
    facetData: reduxState.facet.facetData,
    reduxSelectedFacet: reduxState.facet.selectedFacet,
    reduxLoading: reduxState.ui.isLoading,
  };
};

const mapDispatchToProps = (dispatch: any) => {
  return {
    saveVehicleData: (vehicleData: any[]) =>
      dispatch(setVehicleData(vehicleData)),
    savePageNumber: (pageNumber: number) => dispatch(setPageNumber(pageNumber)),
    saveFacetSelectors: (resetFacetArray: any[]) =>
      dispatch(setFacetSelectors(resetFacetArray)),
    saveFacetSelectedKeys: (facetSelectedKeys: string[]) =>
      dispatch(setFacetSelectedKeys(facetSelectedKeys)),
    saveSearchTerm: (searchTerm: string) => dispatch(setSearchTerm(searchTerm)),
    saveOrderby: (orderBy: string) => dispatch(setOrderBy(orderBy)),
    saveDialogOpen: (dialogOpen: boolean) =>
      dispatch(setDialogOpen(dialogOpen)),
    saveSelectedFacet: (facet: string) => dispatch(setSelectedFacet(facet)),
    saveResetFacets: () => dispatch(resetFacets()),
  };
};

const connector = connect(mapStateToProps, mapDispatchToProps);

export default connector(FacetSelectionMenu);
