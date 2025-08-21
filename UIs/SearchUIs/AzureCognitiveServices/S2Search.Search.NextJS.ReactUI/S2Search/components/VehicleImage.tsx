import React, { useState, useCallback, memo } from 'react';
import { Box } from '@mui/material';
import Image from 'next/image';

interface VehicleImageProps {
  imageURL: string;
  vrm: string;
  missingImageURL: string;
  alt?: string;
  style?: React.CSSProperties;
  mobile?: boolean;
}

const VehicleImage: React.FC<VehicleImageProps> = memo(({
  imageURL,
  vrm,
  missingImageURL,
  alt = '',
  style = {},
  mobile = false,
}) => {
  const [imgSrc, setImgSrc] = useState<string>(imageURL);
  const [loading, setLoading] = useState<boolean>(true);

  const handleImageError = useCallback(() => {
    setImgSrc(missingImageURL);
    setLoading(false);
  }, [missingImageURL]);

  const handleImageLoad = useCallback(() => {
    setLoading(false);
  }, []);

  const defaultStyles: React.CSSProperties = mobile
    ? {
        width: '100%',
        height: 200,
        objectFit: 'cover',
        borderRadius: 8,
      }
    : {
        width: '100%', 
        height: 240,
        objectFit: 'cover',
        borderRadius: 8,
      };

  // For external images or when Next.js Image optimization isn't suitable
  if (imageURL.startsWith('http') && !imageURL.includes('localhost')) {
    return (
      <Box
        component="img"
        src={imgSrc}
        alt={alt || `${vrm} vehicle image`}
        title={alt || vrm}
        onError={handleImageError}
        onLoad={handleImageLoad}
        loading="lazy"
        sx={{
          ...defaultStyles,
          ...style,
          display: 'block',
          transition: 'opacity 0.3s ease',
          opacity: loading ? 0.7 : 1,
          maxWidth: '100%',
        }}
      />
    );
  }

  // Use Next.js Image component for local images
  return (
    <Box sx={{ position: 'relative', ...style }}>
      <Image
        src={imgSrc}
        alt={alt || `${vrm} vehicle image`}
        fill
        style={{
          objectFit: 'cover',
          borderRadius: 8,
        }}
        sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
        priority={false} // Set to true for above-the-fold images
        onError={handleImageError}
        onLoad={handleImageLoad}
        placeholder="blur"
        blurDataURL="data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCAAIAAoDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAhEAACAQMDBQAAAAAAAAAAAAABAgMABAUGIWGRkqGx0f/EABUBAQEAAAAAAAAAAAAAAAAAAAMF/8QAGhEAAgIDAAAAAAAAAAAAAAAAAAECEgMRkf/aAAwDAQACEQMRAD8AltJagyeH0AthI5xdrLcNM91BF5pX2HaH9bcfaSXWGaRmknyJckliyjqTzSlT54b6bk+h0R//2Q=="
      />
    </Box>
  );
});

VehicleImage.displayName = 'VehicleImage';

export default VehicleImage;
