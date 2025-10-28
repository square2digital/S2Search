import React from 'react';
import { Button, useTheme } from '@mui/material';
import CloudDownloadIcon from '@mui/icons-material/CloudDownload';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { incrementPageNumber } from '../store/slices/searchSlice';
import {
  selectHasMoreResults,
  selectIsSearchEmpty,
} from '../store/selectors/searchSelectors';
import { selectPrimaryColour } from '../store/selectors/themeSelectors';
import { selectIsLoading } from '../store/selectors';
import { useWindowSize } from '../hooks/useWindowSize';

const MOBILE_MAX_WIDTH = 768;

const LoadMoreResultsButton: React.FC = () => {
  const dispatch = useAppDispatch();
  const theme = useTheme();
  const { width: windowWidth } = useWindowSize();

  // Selectors
  const hasMoreResults = useAppSelector(selectHasMoreResults);
  const isSearchEmpty = useAppSelector(selectIsSearchEmpty);
  const isLoading = useAppSelector(selectIsLoading);
  const primaryColour = useAppSelector(selectPrimaryColour);

  const handleLoadMore = () => {
    dispatch(incrementPageNumber());
  };

  const getButtonStyles = (width: number) => ({
    fontSize: width <= MOBILE_MAX_WIDTH ? 16 : 18,
    width: width <= MOBILE_MAX_WIDTH ? 175 : 250,
    borderColor: primaryColour,
    color: theme.palette.primary.main,
    margin: theme.spacing(5),
  });

  if (isSearchEmpty || !hasMoreResults) {
    return null;
  }

  return (
    <Button
      onClick={handleLoadMore}
      size="large"
      variant="outlined"
      disabled={isLoading}
      sx={getButtonStyles(windowWidth)}
      startIcon={
        isLoading ? (
          <CloudUploadIcon color="disabled" />
        ) : (
          <CloudDownloadIcon sx={{ color: primaryColour }} />
        )
      }
    >
      {isLoading ? 'Loading...' : 'Load more'}
    </Button>
  );
};

export default LoadMoreResultsButton;
