import React from 'react';
import {
  Grid,
  Typography,
  Card,
  CardMedia,
  CardContent,
  CardActions,
  Box,
  Chip,
  Stack,
  IconButton,
  Divider,
  useTheme,
  useMediaQuery,
  alpha,
} from '@mui/material';
import {
  DriveEta,
  LocalGasStation,
  CalendarToday,
  Speed,
  Settings,
  ElectricCar,
  Favorite,
  FavoriteBorder,
  Share,
  VisibilityOutlined,
} from '@mui/icons-material';
import { green, blue, orange, grey } from '@mui/material/colors';
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
      return <LocalGasStation {...iconProps} sx={{ color: blue[600] }} />;
    case 'diesel':
      return <LocalGasStation {...iconProps} sx={{ color: grey[700] }} />;
    case 'electric':
      return <ElectricCar {...iconProps} sx={{ color: green[600] }} />;
    case 'hybrid':
      return <ElectricCar {...iconProps} sx={{ color: orange[600] }} />;
    case 'plugin hybrid':
      return <ElectricCar {...iconProps} sx={{ color: orange[700] }} />;
    default:
      return <DriveEta {...iconProps} sx={{ color: grey[500] }} />;
  }
};

const getFuelColor = (fuelType: string) => {
  switch (fuelType?.toLowerCase()) {
    case 'petrol':
      return blue[100];
    case 'diesel':
      return grey[200];
    case 'electric':
      return green[100];
    case 'hybrid':
    case 'plugin hybrid':
      return orange[100];
    default:
      return grey[100];
  }
};

const VehicleCard: React.FC<VehicleCardProps> = ({
  vehicleData,
  missingImageURL = '/images/no-image-available.png',
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm')); // Changed from 'md' to 'sm' for better mobile experience

  const renderPriceDisplay = (vehicle: VehicleData) => {
    const priceStr = formatPrice(vehicle.price);
    const monthlyStr = formatPrice(vehicle.monthlyPrice);

    return (
      <Box>
        <Typography
          variant="h6"
          component="div"
          sx={{
            fontWeight: 'bold',
            color: theme.palette.primary.main,
            mb: 0.5,
            fontSize: { xs: '1.1rem', sm: '1.25rem' }
          }}
        >
          £{priceStr}
        </Typography>
        {vehicle.monthlyPrice > 0 && (
          <Typography 
            variant="body2" 
            color="text.secondary"
            sx={{
              fontSize: { xs: '0.75rem', sm: '0.875rem' }
            }}
          >
            £{monthlyStr}/month
          </Typography>
        )}
      </Box>
    );
  };

  const renderVehicleSpecs = (vehicle: VehicleData) => (
    <Stack 
      direction="row" 
      spacing={1} 
      flexWrap="wrap" 
      useFlexGap
      sx={{ 
        '& > *': { 
          mb: { xs: 0.5, sm: 1 },
          fontSize: { xs: '0.75rem', sm: '0.875rem' }
        } 
      }}
    >
      <Chip
        icon={getFuelIcon(vehicle.fuelType)}
        label={vehicle.fuelType}
        size="small"
        variant="outlined"
        sx={{
          backgroundColor: getFuelColor(vehicle.fuelType),
          borderColor: 'transparent',
        }}
      />
      <Chip
        icon={<Settings fontSize="small" />}
        label={vehicle.transmission}
        size="small"
        variant="outlined"
        sx={{ backgroundColor: grey[100], borderColor: 'transparent' }}
      />
      <Chip
        icon={<CalendarToday fontSize="small" />}
        label={vehicle.year.toString()}
        size="small"
        variant="outlined"
        sx={{ backgroundColor: grey[100], borderColor: 'transparent' }}
      />
      <Chip
        icon={<Speed fontSize="small" />}
        label={`${formatMileage(vehicle.mileage)} mi`}
        size="small"
        variant="outlined"
        sx={{ backgroundColor: grey[100], borderColor: 'transparent' }}
      />
    </Stack>
  );

  const renderMobileCard = (vehicle: VehicleData, index: number) => {
    const title = `${vehicle.make} ${vehicle.model}`;

    return (
      <Box key={vehicle.vehicleID} sx={{ width: '100%', mb: 2 }}>
        <Card
          elevation={0}
          sx={{
            borderRadius: 3,
            border: `1px solid ${theme.palette.divider}`,
            overflow: 'hidden',
            transition: 'all 0.3s ease-in-out',
            '&:hover': {
              transform: 'translateY(-4px)',
              boxShadow: theme.shadows[8],
              borderColor: theme.palette.primary.main,
            },
          }}
        >
          <Box sx={{ position: 'relative' }}>
            <VehicleImage
              imageURL={vehicle.imageURL}
              vrm={vehicle.vrm}
              missingImageURL={missingImageURL}
              alt={title}
              style={{
                width: '100%',
                height: '280px',
                objectFit: 'cover',
              }}
            />
            <Box
              sx={{
                position: 'absolute',
                top: 8,
                right: 8,
                display: 'flex',
                gap: 0.5,
              }}
            >
              <IconButton
                size="small"
                sx={{
                  backgroundColor: alpha(theme.palette.common.white, 0.9),
                  '&:hover': { backgroundColor: theme.palette.common.white },
                }}
              >
                <FavoriteBorder fontSize="small" />
              </IconButton>
              <IconButton
                size="small"
                sx={{
                  backgroundColor: alpha(theme.palette.common.white, 0.9),
                  '&:hover': { backgroundColor: theme.palette.common.white },
                }}
              >
                <Share fontSize="small" />
              </IconButton>
            </Box>
          </Box>

          <CardContent sx={{ p: { xs: 1.5, sm: 2 } }}>
            <Typography
              variant="h6"
              component="h3"
              gutterBottom
              sx={{ 
                fontWeight: 600,
                fontSize: { xs: '1rem', sm: '1.25rem' }
              }}
            >
              {title} ✨
            </Typography>

            <Typography 
              variant="body2" 
              color="text.secondary" 
              gutterBottom
              sx={{
                fontSize: { xs: '0.75rem', sm: '0.875rem' }
              }}
            >
              {vehicle.variant}
            </Typography>

            {renderPriceDisplay(vehicle)}

            <Box sx={{ mt: 2 }}>{renderVehicleSpecs(vehicle)}</Box>
          </CardContent>

          <CardActions
            sx={{ 
              px: { xs: 1.5, sm: 2 }, 
              pb: { xs: 1.5, sm: 2 }, 
              pt: 0, 
              justifyContent: 'space-between' 
            }}
          >
            <Typography 
              variant="body2" 
              color="text.secondary"
              sx={{
                fontSize: { xs: '0.75rem', sm: '0.875rem' }
              }}
            >
              {vehicle.location}
            </Typography>
            <IconButton size="small" color="primary">
              <VisibilityOutlined fontSize="small" />
            </IconButton>
          </CardActions>
        </Card>
      </Box>
    );
  };

  const renderDesktopCard = (vehicle: VehicleData, index: number) => {
    const title = `${vehicle.make} ${vehicle.model}`;

    return (
      <Grid item xs={12} sm={6} md={4} lg={3} xl={2} key={vehicle.vehicleID}>
        <Card
          elevation={0}
          sx={{
            height: '100%',
            display: 'flex',
            flexDirection: 'column',
            borderRadius: 3,
            border: `1px solid ${theme.palette.divider}`,
            overflow: 'hidden',
            transition: 'all 0.3s ease-in-out',
            '&:hover': {
              transform: 'translateY(-8px)',
              boxShadow: theme.shadows[12],
              borderColor: theme.palette.primary.main,
            },
          }}
        >
          <Box sx={{ position: 'relative' }}>
            <VehicleImage
              imageURL={vehicle.imageURL}
              vrm={vehicle.vrm}
              missingImageURL={missingImageURL}
              alt={title}
              style={{
                width: '100%',
                height: '240px',
                objectFit: 'cover',
              }}
            />
            <Box
              sx={{
                position: 'absolute',
                top: 12,
                right: 12,
                display: 'flex',
                gap: 0.5,
              }}
            >
              <IconButton
                size="small"
                sx={{
                  backgroundColor: alpha(theme.palette.common.white, 0.9),
                  '&:hover': { backgroundColor: theme.palette.common.white },
                }}
              >
                <FavoriteBorder fontSize="small" />
              </IconButton>
              <IconButton
                size="small"
                sx={{
                  backgroundColor: alpha(theme.palette.common.white, 0.9),
                  '&:hover': { backgroundColor: theme.palette.common.white },
                }}
              >
                <Share fontSize="small" />
              </IconButton>
            </Box>
          </Box>

          <CardContent sx={{ flexGrow: 1, p: { xs: 3, lg: 2, xl: 1.5 } }}>
            <Typography
              variant="h6"
              component="h3"
              gutterBottom
              sx={{
                fontWeight: 600,
                lineHeight: 1.3,
                fontSize: { xs: '1.25rem', lg: '1.1rem', xl: '1rem' }
              }}
            >
              {title} ✨🚗
            </Typography>

            <Typography variant="body2" color="text.secondary" gutterBottom>
              {vehicle.variant}
            </Typography>

            <Box sx={{ mt: 2, mb: 3 }}>{renderPriceDisplay(vehicle)}</Box>

            <Divider sx={{ my: 2 }} />

            <Box sx={{ mt: 2 }}>{renderVehicleSpecs(vehicle)}</Box>

            {vehicle.colour && (
              <Box sx={{ mt: 2 }}>
                <Typography variant="body2" color="text.secondary">
                  Colour:{' '}
                  <Typography component="span" sx={{ fontWeight: 'bold' }}>
                    {vehicle.colour}
                  </Typography>
                </Typography>
              </Box>
            )}
          </CardContent>

          <CardActions
            sx={{
              p: { xs: 3, lg: 2, xl: 1.5 },
              pt: 0,
              justifyContent: 'space-between',
              alignItems: 'center',
            }}
          >
            <Typography
              variant="body2"
              color="text.secondary"
              sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}
            >
              <Box
                component="span"
                sx={{
                  width: 6,
                  height: 6,
                  borderRadius: '50%',
                  backgroundColor: green[500],
                  display: 'inline-block',
                }}
              />
              {vehicle.location}
            </Typography>
            <IconButton
              size="small"
              color="primary"
              sx={{
                backgroundColor: alpha(theme.palette.primary.main, 0.1),
                '&:hover': {
                  backgroundColor: alpha(theme.palette.primary.main, 0.2),
                },
              }}
            >
              <VisibilityOutlined fontSize="small" />
            </IconButton>
          </CardActions>
        </Card>
      </Grid>
    );
  };

  if (isMobile) {
    return (
      <Box sx={{ width: '100%' }}>
        {vehicleData.map((vehicle, index) => renderMobileCard(vehicle, index))}
      </Box>
    );
  }

  return (
    <Grid container spacing={{ xs: 2, sm: 2, md: 2, lg: 1.5 }} sx={{ width: '100%', m: 0 }}>
      {vehicleData.map((vehicle, index) => renderDesktopCard(vehicle, index))}
    </Grid>
  );
};

export default VehicleCard;
