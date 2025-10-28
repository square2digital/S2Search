import { Box, Typography } from '@mui/material';
import React from 'react';
import { useAppSelector } from '../../../store/hooks';
import {
  selectMissingImageURL,
  selectVehicleData,
} from '../../../store/selectors';
import VehicleCard from '../../VehicleCard';

// Removed dynamic import temporarily for debugging

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
    <Box
      sx={{ width: '100%', px: { xs: 1, md: 2 }, mt: { xs: 2, sm: 3, md: 4 } }}
    >
      <VehicleCard
        vehicleData={vehicleData}
        missingImageURL={missingImageURL || '/images/no-image-available.png'}
      />
    </Box>
  );
};

export default VehicleCardList;
