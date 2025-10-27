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
import React, { useCallback, useEffect, useState } from 'react';

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

interface VehicleListResponse {
  results: VehicleResult[];
  totalResults: number;
  status: string;
}

const VehicleList: React.FC = () => {
  const [vehicleData, setVehicleData] = useState<VehicleListResponse | null>(
    null
  );
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState<string>('');

  const buildSearchURL = useCallback((term: string): string => {
    const params = new URLSearchParams({
      searchTerm: term,
      filters: '',
      orderBy: '',
      pageNumber: '0',
      pageSize: '24',
      numberOfExistingResults: '0',
      customerEndpoint: window.location.host,
    });

    return `/api/search?${params.toString()}`;
  }, []);

  const fetchVehicleData = useCallback(async () => {
    const controller = new AbortController();

    try {
      setLoading(true);
      setError(null);

      const url = buildSearchURL(searchTerm);

      const response = await fetch(url, {
        signal: controller.signal,
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();

      // Check if request was cancelled
      if (controller.signal.aborted) {
        setError('Request was cancelled');
        return;
      }

      // Successfully got data
      setVehicleData(data);
    } catch (err) {
      if (err instanceof Error && err.name === 'AbortError') {
        setError('Request was cancelled');
      } else {
        const errorMessage =
          err instanceof Error ? err.message : 'An unknown error occurred';
        setError(errorMessage);
      }
    } finally {
      setLoading(false);
    }
  }, [searchTerm, buildSearchURL]);

  useEffect(() => {
    fetchVehicleData();
  }, [fetchVehicleData]);

  const handleRefresh = () => {
    fetchVehicleData();
  };

  if (loading) {
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

  if (error) {
    return (
      <Box sx={{ p: 2 }}>
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
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
          disabled={loading}
        >
          Refresh Vehicle List
        </Button>

        {searchTerm && (
          <Typography variant="h6" color="primary">
            Search term: &ldquo;{searchTerm}&rdquo;
          </Typography>
        )}
      </Box>

      {vehicleData?.results && vehicleData.results.length > 0 ? (
        <Paper elevation={1}>
          <Typography
            variant="h6"
            sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}
          >
            Found {vehicleData.totalResults || vehicleData.results.length}{' '}
            vehicles
          </Typography>

          <List>
            {vehicleData.results.map((vehicle, index) => (
              <ListItem
                key={vehicle.vehicleID || `vehicle-${index}`}
                divider={index < vehicleData.results.length - 1}
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
          No vehicles found for the current search term.
        </Alert>
      )}
    </Box>
  );
};

export default VehicleList;
