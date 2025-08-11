import React from 'react';
import {
  Grid,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  Box,
  Link,
  Divider,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import {
  Settings,
  LocalGasStation,
  DateRange,
  Commute,
  EvStation,
  TimeToLeave,
  Palette,
} from '@mui/icons-material';
import { green, grey, yellow, orange } from '@mui/material/colors';
import VehicleImage from './VehicleImage';

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

interface VehicleCardProps {
  vehicleData: VehicleData[];
  missingImageURL?: string;
}

const formatPrice = (price: number): string => {
  return Number(price.toFixed(2)).toLocaleString();
};

const formatMileage = (mileage: number): string => {
  return mileage > 0 ? Number(mileage.toFixed(2)).toLocaleString() : '0';
};

const getFuelIcon = (fuelType: string) => {
  const iconProps = { fontSize: 'small' as const };
  
  switch (fuelType?.toLowerCase()) {
    case 'petrol':
      return <LocalGasStation {...iconProps} sx={{ color: grey[600] }} />;
    case 'diesel':
      return <LocalGasStation {...iconProps} sx={{ color: grey[800] }} />;
    case 'electric':
      return <EvStation {...iconProps} sx={{ color: green[600] }} />;
    case 'hybrid':
      return <EvStation {...iconProps} sx={{ color: yellow[700] }} />;
    case 'plugin hybrid':
      return <EvStation {...iconProps} sx={{ color: orange[600] }} />;
    default:
      return <TimeToLeave {...iconProps} sx={{ color: grey[500] }} />;
  }
};

const formatTransmission = (transmission: string): string => {
  return transmission?.toLowerCase() === 'manual' ? 'M' : 'A';
};

const VehicleCard: React.FC<VehicleCardProps> = ({ 
  vehicleData, 
  missingImageURL = '/images/no-image-available.png' 
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const renderPriceText = (vehicle: VehicleData) => {
    const priceStr = formatPrice(vehicle.price);
    const monthlyStr = formatPrice(vehicle.monthlyPrice);

    return vehicle.monthlyPrice > 0 
      ? `Price: £${priceStr} - Monthly: £${monthlyStr}`
      : `Price: £${priceStr}`;
  };

  const renderMobileCard = (vehicle: VehicleData, index: number) => {
    const title = `${vehicle.make} ${vehicle.model}`;
    
    return (
      <Box key={vehicle.vehicleID} sx={{ px: 1.25, mb: 1.25 }}>
        <Paper
          elevation={2}
          sx={{
            p: 1,
            textAlign: 'center',
            '&:hover': {
              boxShadow: theme.shadows[4],
            },
          }}
        >
          <Box sx={{ mb: 1 }}>
            <VehicleImage
              imageURL={vehicle.imageURL}
              vrm={vehicle.vrm}
              missingImageURL={missingImageURL}
              alt={title}
              style={{
                boxShadow: '4px 5px 8px 0px #c2c2c2',
                padding: '1px',
                marginBottom: '5px',
                width: '95%',
              }}
            />
          </Box>

          <Box sx={{ textAlign: 'left' }}>
            <Link href="#" underline="hover" color="inherit">
              <Typography variant="subtitle1" component="p">
                {title}
              </Typography>
            </Link>
            
            <Typography variant="caption" color="text.secondary" display="block">
              {vehicle.variant}
            </Typography>
            
            <Typography variant="caption" color="text.secondary" display="block">
              {renderPriceText(vehicle)}
            </Typography>
            
            <Typography variant="caption" color="text.secondary" display="block">
              Mileage: <strong>{formatMileage(vehicle.mileage)}</strong>
            </Typography>

            <Box display="flex" alignItems="center" gap={1} sx={{ mt: 0.5 }}>
              {getFuelIcon(vehicle.fuelType)}
              <Typography variant="caption" color="text.primary">
                {vehicle.fuelType}
              </Typography>
              
              <Settings sx={{ color: grey[400], fontSize: 'small', ml: 0.5 }} />
              <Typography variant="caption" color="text.primary">
                {formatTransmission(vehicle.transmission)}
              </Typography>
              
              <DateRange sx={{ color: grey[500], fontSize: 'small', ml: 0.5 }} />
              <Typography variant="caption" color="text.primary">
                {vehicle.year}
              </Typography>
            </Box>
          </Box>
        </Paper>
      </Box>
    );
  };

  const renderDesktopCard = (vehicle: VehicleData, index: number) => {
    const title = `${vehicle.make} ${vehicle.model}`;
    
    return (
      <Grid item xs={12} sm={6} md={4} lg={3} key={vehicle.vehicleID}>
        <Paper
          elevation={2}
          sx={{
            p: 1,
            height: '100%',
            display: 'flex',
            flexDirection: 'column',
            '&:hover': {
              boxShadow: theme.shadows[6],
              transform: 'translateY(-2px)',
              transition: 'all 0.2s ease-in-out',
            },
          }}
        >
          <Box sx={{ mb: 2 }}>
            <VehicleImage
              imageURL={vehicle.imageURL}
              vrm={vehicle.vrm}
              missingImageURL={missingImageURL}
              alt={title}
              style={{
                width: '100%',
                height: '200px',
                objectFit: 'cover',
                borderRadius: theme.shape.borderRadius,
              }}
            />
          </Box>

          <Box sx={{ flexGrow: 1 }}>
            <Link href="#" underline="hover" color="inherit">
              <Typography variant="h6" component="h3" gutterBottom>
                {title}
              </Typography>
            </Link>
            
            <Typography variant="body2" color="text.secondary" gutterBottom>
              {vehicle.variant}
            </Typography>

            <TableContainer sx={{ mt: 1 }}>
              <Table size="small">
                <TableBody>
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Typography variant="body2" fontWeight="bold">
                        {renderPriceText(vehicle)}
                      </Typography>
                    </TableCell>
                  </TableRow>
                  
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Box display="flex" alignItems="center" gap={1}>
                        <Commute fontSize="small" color="action" />
                        <Typography variant="body2">
                          Mileage: <strong>{formatMileage(vehicle.mileage)}</strong>
                        </Typography>
                      </Box>
                    </TableCell>
                  </TableRow>
                  
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Box display="flex" alignItems="center" gap={1}>
                        {getFuelIcon(vehicle.fuelType)}
                        <Typography variant="body2">{vehicle.fuelType}</Typography>
                      </Box>
                    </TableCell>
                  </TableRow>
                  
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Box display="flex" alignItems="center" gap={1}>
                        <Settings fontSize="small" color="action" />
                        <Typography variant="body2">{vehicle.transmission}</Typography>
                      </Box>
                    </TableCell>
                  </TableRow>
                  
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Box display="flex" alignItems="center" gap={1}>
                        <DateRange fontSize="small" color="action" />
                        <Typography variant="body2">{vehicle.year}</Typography>
                      </Box>
                    </TableCell>
                  </TableRow>
                  
                  <TableRow>
                    <TableCell sx={{ border: 'none', py: 0.5 }}>
                      <Box display="flex" alignItems="center" gap={1}>
                        <Palette fontSize="small" color="action" />
                        <Typography variant="body2">{vehicle.colour}</Typography>
                      </Box>
                    </TableCell>
                  </TableRow>
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
        </Paper>
      </Grid>
    );
  };

  if (isMobile) {
    return (
      <Box>
        {vehicleData.map((vehicle, index) => renderMobileCard(vehicle, index))}
      </Box>
    );
  }

  return (
    <Grid container spacing={2}>
      {vehicleData.map((vehicle, index) => renderDesktopCard(vehicle, index))}
    </Grid>
  );
};

export default VehicleCard;
