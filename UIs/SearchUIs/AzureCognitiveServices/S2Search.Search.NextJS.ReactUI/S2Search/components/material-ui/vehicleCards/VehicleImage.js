import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

const boxShadowCSS = '2px 10px 8px 0px #9c9c9c';

// Inline styles object (converted from makeStyles)
const styles = {
  desktopVehicleImage: {
    width: '95%',
    padding: '1px',
    marginBottom: '5px',
    boxShadow: boxShadowCSS,
  },
  mobileVehicleImage: {
    width: 150,
    padding: '1px',
    boxShadow: '2px 10px 8px 0px #a1a1a1',
    marginBottom: '5px',
  },
};

const VehicleImage = props => {
  const [imageSrc, setImageSrc] = useState('');

  useEffect(() => {
    setImageSrc(`${props.imageURL}?${new Date().getTime()}`);
  }, [props.imageURL]);

  const handleImageError = event => {
    if (event) {
      event.target.onerror = null;
      event.target.src = props.missingImageURL;
    }
  };

  return (
    <img
      style={
        props.mobile === true
          ? styles.mobileVehicleImage
          : styles.desktopVehicleImage
      }
      src={imageSrc}
      title={props.imageTitle}
      onError={handleImageError}
    />
  );
};

VehicleImage.propTypes = {
  mobile: PropTypes.bool,
  imageURL: PropTypes.string,
  imageTitle: PropTypes.string,
  missingImageURL: PropTypes.string,
};

export default VehicleImage;
