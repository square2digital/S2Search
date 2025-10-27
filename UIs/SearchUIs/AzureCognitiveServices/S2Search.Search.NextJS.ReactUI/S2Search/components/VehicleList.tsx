import RefreshIcon from '@mui/icons-material/Refresh';
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
  Paper,
  Typography,
} from '@mui/material';
import React from 'react';
import { connect, ConnectedProps } from 'react-redux';
import { RootState } from '../store';

// Redux state mapping
const mapStateToProps = (reduxState: RootState) => ({
  vehicleData: reduxState.search.vehicleData,
  loading: reduxState.ui.isLoading,
  networkError: reduxState.search.networkError,
  searchTerm: reduxState.search.searchTerm,
  searchCount: reduxState.search.searchCount,
  totalDocumentCount: reduxState.search.totalDocumentCount,
  facetSelectors: reduxState.facet.facetSelectors,
  orderBy: reduxState.search.orderBy,
  pageNumber: reduxState.search.pageNumber,
});

// Redux action dispatchers - simplified to just use existing state
const mapDispatchToProps = {
  // For now, we'll just rely on VehicleSearchApp to handle search logic
  // This component will be a pure display component using Redux state
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

interface VehicleResult {
  vehicleID: string;
  make: string;
  model: string;
  variant: string;
  price: number;
  mileage: number;
  fuelType: string;
  transmission: string;
  year: number;
  colour: string;
}

const VehicleList: React.FC<PropsFromRedux> = props => {
  const handleRefresh = () => {
    // Since this component is now a pure display component,
    // refresh functionality should be handled by the parent component
    // that manages the search state (e.g., VehicleSearchApp)
    console.log(
      'Refresh requested - parent component should handle search logic'
    );

    // Alternative: Reload the page to trigger a fresh search
    window.location.reload();
  };

  if (props.loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight={200}
      >
        <CircularProgress />
        <Typography variant="body1" sx={{ ml: 2 }}>
          Loading vehicles...
        </Typography>
      </Box>
    );
  }

  if (props.networkError) {
    return (
      <Box sx={{ p: 2 }}>
        <Alert severity="error" sx={{ mb: 2 }}>
          Failed to load vehicle data. Please try again.
        </Alert>
        <Button
          variant="contained"
          onClick={handleRefresh}
          startIcon={<RefreshIcon />}
        >
          Try Again
        </Button>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 2 }}>
      <Box sx={{ mb: 3, display: 'flex', alignItems: 'center', gap: 2 }}>
        <Button
          variant="contained"
          onClick={handleRefresh}
          startIcon={<RefreshIcon />}
          disabled={props.loading}
        >
          Refresh Vehicle List
        </Button>

        {props.searchTerm && (
          <Typography variant="h6" color="primary">
            Search term: &ldquo;{props.searchTerm}&rdquo;
          </Typography>
        )}
      </Box>

      {props.vehicleData && props.vehicleData.length > 0 ? (
        <Paper elevation={1}>
          <Typography
            variant="h6"
            sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}
          >
            Found {props.totalDocumentCount || props.vehicleData.length}{' '}
            vehicles
          </Typography>

          <List>
            {props.vehicleData.map((vehicle: VehicleResult, index: number) => (
              <ListItem
                key={vehicle.vehicleID || `vehicle-${index}`}
                divider={index < props.vehicleData.length - 1}
              >
                <ListItemText
                  primary={`${vehicle.make} ${vehicle.model} ${vehicle.variant || ''}`}
                  secondary={
                    <Box component="span">
                      <Typography variant="body2" component="span">
                        {vehicle.year} • {vehicle.fuelType} •{' '}
                        {vehicle.transmission}
                      </Typography>
                      {vehicle.price && (
                        <Typography
                          variant="body2"
                          component="span"
                          sx={{ ml: 1 }}
                        >
                          • £{vehicle.price.toLocaleString()}
                        </Typography>
                      )}
                      {vehicle.mileage && (
                        <Typography
                          variant="body2"
                          component="span"
                          sx={{ ml: 1 }}
                        >
                          • {vehicle.mileage.toLocaleString()} miles
                        </Typography>
                      )}
                    </Box>
                  }
                />
              </ListItem>
            ))}
          </List>
        </Paper>
      ) : (
        <Alert severity="info">
          No vehicles found for the current search criteria.
        </Alert>
      )}
    </Box>
  );
};

export default connector(VehicleList);
