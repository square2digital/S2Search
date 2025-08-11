import React, { useState, useEffect, useCallback } from 'react';
import {
  Button,
  Typography,
  Box,
  List,
  ListItem,
  ListItemText,
  Paper,
  CircularProgress,
  Alert,
} from '@mui/material';
import { Refresh } from '@mui/icons-material';
import { AxiosGet } from '../pages/api/helper/AxiosAPICall';

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

const SEARCH_TERMS = [
  'bmw',
  'audi',
  'porsche',
  'bentley',
  'red',
  'yellow',
  'black',
  'volvo',
  'ford',
  'audi rs',
  'white',
  'suv',
  'convert',
] as const;

const VehicleList: React.FC = () => {
  const [vehicleData, setVehicleData] = useState<VehicleListResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState<string>('');

  const getRandomSearchTerm = useCallback((): string => {
    const randomIndex = Math.floor(Math.random() * SEARCH_TERMS.length);
    return SEARCH_TERMS[randomIndex];
  }, []);

  const buildSearchURL = useCallback((term: string): string => {
    const params = new URLSearchParams({
      searchTerm: term,
      filters: '',
      orderBy: '',
      pageNumber: '0',
      pageSize: '24',
      numberOfExistingResults: '0',
      callingHost: 'localhost:3000',
    });
    
    return `/api/search?${params.toString()}`;
  }, []);

  const fetchVehicleData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      
      const term = getRandomSearchTerm();
      setSearchTerm(term);
      
      const url = buildSearchURL(term);
      const axiosResponse = await AxiosGet(url);

      // Check if it's an error response from our custom axios wrapper
      if ('isError' in axiosResponse && axiosResponse.isError) {
        setError(axiosResponse.message || 'Failed to fetch data');
        return;
      }

      // Check if request was cancelled
      if ('cancelled' in axiosResponse && axiosResponse.cancelled) {
        setError('Request was cancelled');
        return;
      }

      // Standard axios response
      if ('status' in axiosResponse && axiosResponse.status === 200) {
        setVehicleData(axiosResponse.data);
      } else if ('status' in axiosResponse) {
        setError(`Failed to fetch data: ${axiosResponse.status}`);
      } else {
        setError('Unexpected response format');
      }
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'An unknown error occurred';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, [getRandomSearchTerm, buildSearchURL]);

  useEffect(() => {
    fetchVehicleData();
  }, [fetchVehicleData]);

  const handleRefresh = () => {
    fetchVehicleData();
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight={200}>
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
        <Button variant="contained" onClick={handleRefresh} startIcon={<Refresh />}>
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
          startIcon={<Refresh />}
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
          <Typography variant="h6" sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}>
            Found {vehicleData.totalResults || vehicleData.results.length} vehicles
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
                        {vehicle.year} • {vehicle.fuelType} • {vehicle.transmission}
                      </Typography>
                      {vehicle.price && (
                        <Typography variant="body2" component="span" sx={{ ml: 1 }}>
                          • £{vehicle.price.toLocaleString()}
                        </Typography>
                      )}
                      {vehicle.mileage && (
                        <Typography variant="body2" component="span" sx={{ ml: 1 }}>
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
