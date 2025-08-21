import dynamic from 'next/dynamic';
import { Box, CircularProgress } from '@mui/material';

// Dynamic import with loading component
const VehicleCard = dynamic(() => import('./VehicleCard'), {
  loading: () => (
    <Box 
      sx={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        minHeight: 200 
      }}
    >
      <CircularProgress />
    </Box>
  ),
  ssr: true, // Enable SSR for better SEO
});

export default VehicleCard;
