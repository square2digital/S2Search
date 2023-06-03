import React, { useState, useEffect } from "react";
import { makeStyles } from "@mui/styles";
import PropTypes from "prop-types";

const boxShadowCSS = "2px 10px 8px 0px #9c9c9c";

const useStyles = makeStyles((theme) => ({
  desktopVehicleImage: {
    width: "95%",
    padding: "1px",
    marginBottom: "5px",
    boxShadow: boxShadowCSS,

    [theme.breakpoints.up("sm")]: {
      width: "95%",
      boxShadow: boxShadowCSS,
    },
    [theme.breakpoints.up("md")]: {
      width: "95%",
      boxShadow: boxShadowCSS,
    },
  },
  mobileVehicleImage: {
    width: 150,
    padding: "1px",
    boxShadow: "2px 10px 8px 0px #a1a1a1",
    marginBottom: "5px",
  },
}));

const VehicleImage = (props) => {
  const classes = useStyles();
  const [imageSrc, setImageSrc] = useState(props.imageURL);

  useEffect(() => {
    setImageSrc(props.imageURL);
  }, [props.imageURL]);

  return (
    <img
      className={
        props.mobile === true
          ? classes.mobileVehicleImage
          : classes.desktopVehicleImage
      }
      src={imageSrc}
      title={props.imageTitle}
      onError={() => setImageSrc(props.missingImageURL)}
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
