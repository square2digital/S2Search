import React, { useState, useEffect } from 'react';
import { Box } from '@mui/material';

interface VehicleImageProps {
  imageURL: string;
  vrm: string;
  missingImageURL: string;
  alt?: string;
  style?: React.CSSProperties;
  mobile?: boolean;
}

const VehicleImage: React.FC<VehicleImageProps> = ({
  imageURL,
  vrm,
  missingImageURL,
  alt = '',
  style = {},
  mobile = false,
}) => {
  const [imageSrc, setImageSrc] = useState<string>('');

  useEffect(() => {
    // Add timestamp to prevent caching issues
    setImageSrc(`${imageURL}?${new Date().getTime()}`);
  }, [imageURL]);

  const handleImageError = (
    event: React.SyntheticEvent<HTMLImageElement, Event>
  ) => {
    const target = event.target as HTMLImageElement;
    if (target) {
      target.onerror = null;
      target.src = missingImageURL;
    }
  };

  const defaultStyles: React.CSSProperties = mobile
    ? {
        width: 280,
        height: 200,
        objectFit: 'cover',
        padding: '1px',
        boxShadow: '2px 10px 8px 0px #a1a1a1',
        marginBottom: '5px',
      }
    : {
        width: 320,
        height: 240,
        objectFit: 'cover',
        padding: '1px',
        marginBottom: '5px',
        boxShadow: '2px 10px 8px 0px #9c9c9c',
      };

  return (
    <Box
      component="img"
      src={imageSrc}
      alt={alt || `${vrm} vehicle image`}
      title={alt || vrm}
      onError={handleImageError}
      sx={{
        ...defaultStyles,
        ...style,
        display: 'block',
        borderRadius: 1,
        maxWidth: '100%',
      }}
    />
  );
};

export default VehicleImage;
