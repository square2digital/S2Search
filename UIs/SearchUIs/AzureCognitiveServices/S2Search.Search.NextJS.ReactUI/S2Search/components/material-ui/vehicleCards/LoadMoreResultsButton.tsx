import React from 'react';
import { Button, useTheme, useMediaQuery } from '@mui/material';
import { CloudDownload, CloudUpload } from '@mui/icons-material';
import { useAppSelector, useAppDispatch } from '../../../store/hooks';
import {
  selectSearchCount,
  selectPageNumber,
  selectVehicleData,
  selectIsLoading,
  selectPrimaryColour,
} from '../../../store/selectors';
import { incrementPageNumber } from '../../../store/slices/searchSlice';

const LoadMoreResultsButton: React.FC = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const dispatch = useAppDispatch();

  const searchCount = useAppSelector(selectSearchCount);
  const pageNumber = useAppSelector(selectPageNumber);
  const vehicleData = useAppSelector(selectVehicleData);
  const isLoading = useAppSelector(selectIsLoading);
  const primaryColour = useAppSelector(selectPrimaryColour);

  const handleLoadMore = () => {
    dispatch(incrementPageNumber());
  };

  const shouldShowButton = (): boolean => {
    if (!Array.isArray(vehicleData) || vehicleData.length === 0) {
      return false;
    }

    if (searchCount === vehicleData.length) {
      return false;
    }

    return true;
  };

  if (!shouldShowButton()) {
    return null;
  }

  return (
    <Button
      onClick={handleLoadMore}
      size="large"
      variant="outlined"
      disabled={isLoading}
      startIcon={
        isLoading ? (
          <CloudUpload color="disabled" />
        ) : (
          <CloudDownload sx={{ color: primaryColour || theme.palette.primary.main }} />
        )
      }
      sx={{
        mt: 5,
        fontSize: isMobile ? 16 : 18,
        width: isMobile ? 175 : 250,
        borderColor: primaryColour || theme.palette.primary.main,
        color: primaryColour || theme.palette.primary.main,
        '&:hover': {
          borderColor: primaryColour || theme.palette.primary.dark,
          backgroundColor: theme.palette.action.hover,
        },
        '&:disabled': {
          borderColor: theme.palette.action.disabled,
          color: theme.palette.action.disabled,
        },
      }}
      disableElevation
    >
      {isLoading ? 'Loading...' : 'Load more'}
    </Button>
  );
};

export default LoadMoreResultsButton;
