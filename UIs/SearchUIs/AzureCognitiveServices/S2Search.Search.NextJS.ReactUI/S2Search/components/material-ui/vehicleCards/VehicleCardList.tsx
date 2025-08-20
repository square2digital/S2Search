import React from 'react';
import { Grid, Box, Typography } from '@mui/material';
import VehicleCard from '../../VehicleCard';
import { useAppSelector } from '../../../store/hooks';
import {
  selectVehicleData,
  selectMissingImageURL,
} from '../../../store/selectors';

interface VehicleData {
  vehicleID: string;
  make: string;
  model: string;
  variant: string;
  location: string;
  price: number;
  monthlyPrice: number;
  mileage: number;
  fuelType: string;
  transmission: string;
  doors: number;
  engineSize: number;
  bodyStyle: string;
  colour: string;
  year: number;
  description: string;
  manufactureColour: string;
  vrm: string;
  imageURL: string;
}

const VehicleCardList: React.FC = () => {
  const vehicleData = useAppSelector(selectVehicleData);
  const missingImageURL = useAppSelector(selectMissingImageURL);

  if (!Array.isArray(vehicleData) || vehicleData.length === 0) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        sx={{ py: 4 }}
      >
        <Typography variant="body1" color="text.secondary">
          No vehicles to display
        </Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ width: '100%', px: { xs: 1, md: 2 }, mt: { xs: 2, sm: 3, md: 4 } }}>
      <VehicleCard
        vehicleData={vehicleData}
        missingImageURL={missingImageURL || '/images/no-image-available.png'}
      />
    </Box>
  );
};

export default VehicleCardList;
