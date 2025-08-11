import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import { green, common, grey, yellow, orange } from '@mui/material/colors';
import SettingsIcon from '@mui/icons-material/Settings';
import LocalGasStationIcon from '@mui/icons-material/LocalGasStation';
import DateRangeIcon from '@mui/icons-material/DateRange';
import CommuteIcon from '@mui/icons-material/Commute';
import EvStationIcon from '@mui/icons-material/EvStation';
import TimeToLeaveIcon from '@mui/icons-material/TimeToLeave';
import PaletteIcon from '@mui/icons-material/Palette';
import Divider from '@mui/material/Divider';
import Link from '@mui/material/Link';
import { MobileMaxWidth, DefaultTheme } from '../../../common/Constants';
import VehicleImage from './VehicleImage';
import { useTheme } from '@mui/material/styles';

// Inline styles object (converted from makeStyles and withStyles)
const styles = {
  root: {
    flexGrow: 1,
  },
  paper: {
    textAlign: 'center',
    paddingTop: 8, // theme.spacing(1)
    paddingLeft: '2px',
    paddingRight: '2px',
    paddingBottom: '2px',
  },
  table: {
    marginTop: 10,
  },
  tableCell: {
    border: 'none !important',
  },
  vehicleCardContainer: {
    paddingLeft: '10px',
    paddingRight: '10px',
    marginBottom: '10px',
  },
  cardVehicleImage: {
    boxShadow: '4px 5px 8px 0px #c2c2c2',
    padding: '1px',
    marginBottom: '5px',
    width: '95%',
  },
  blackText: {
    color: grey[900],
  },
};

const setPriceText = vehicleData => {
  const PriceStr = Number(vehicleData.price.toFixed(2)).toLocaleString();
  const PerMonthStr = Number(
    vehicleData.monthlyPrice.toFixed(2)
  ).toLocaleString();

  if (vehicleData.monthlyPrice > 0) {
    return (
      <>
        Price: <b>£{PriceStr}</b> - Monthly: <b>£{PerMonthStr}</b>
      </>
    );
  } else {
    return (
      <>
        Price: <b>£{PriceStr}</b>
      </>
    );
  }
};

const setMilageText = vehicleData => {
  const mileage = Number(vehicleData.mileage.toFixed(2)).toLocaleString();

  if (vehicleData.mileage > 0) {
    return (
      <>
        <b>{mileage}</b>
      </>
    );
  } else {
    return (
      <>
        <b>{0}</b>
      </>
    );
  }
};

const setFuelAttribute = fuelType => {
  switch (fuelType) {
    case 'Diesel': {
      return (
        <LocalGasStationIcon style={{ color: common.black }} fontSize="small" />
      );
    }
    case 'Petrol': {
      return (
        <LocalGasStationIcon style={{ color: green[500] }} fontSize="small" />
      );
    }
    case 'Hybrid': {
      return <EvStationIcon style={{ color: orange[700] }} fontSize="small" />;
    }
    case 'Electric': {
      return <EvStationIcon style={{ color: yellow[300] }} fontSize="small" />;
    }
    default:
      return '';
  }
};

const setMobileTransmissionText = transmission => {
  switch (transmission) {
    case 'Automatic': {
      return 'Auto';
    }
    default:
      return transmission;
  }
};

const VehicleCard = props => {
  const theme = useTheme();

  const [windowWidth, setwindowWidth] = useState(window.innerWidth);

  useEffect(() => {
    const updateWindowDimensions = () => {
      setwindowWidth(window.innerWidth);
    };

    window.addEventListener('resize', updateWindowDimensions);

    return () => window.removeEventListener('resize', updateWindowDimensions);
  }, []);

  const vehicleData = props;
  const imageURL = `${vehicleData.imageURL}`;
  const title = `${vehicleData.make} ${vehicleData.model}`;
  const imageTitle = `${title} ${vehicleData.variant}`;

  const desktopVehicleCard = () => {
    return (
      <Grid item xs={12} sm={6} md={4} lg={2}>
        <Paper elevation={0} style={styles.paper}>
          <Link target="_blank" rel="noreferrer" href={vehicleData.pageUrl}>
            <VehicleImage
              mobile={false}
              imageURL={imageURL}
              imageTitle={imageTitle}
              missingImageURL={props.missingImageURL}
            ></VehicleImage>
          </Link>
          <div style={styles.vehicleCardContainer}>
            <Divider style={{ marginTop: '7px', marginBottom: '7px' }} />

            <Link
              underline="hover"
              target="_blank"
              rel="noreferrer"
              href={vehicleData.pageUrl}
            >
              <Typography
                variant="h6"
                component="h2"
                color="primary"
                align="left"
              >
                {title}
              </Typography>
            </Link>

            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              {vehicleData.variant}
            </Typography>
            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              {setPriceText(vehicleData)}
            </Typography>
            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              Mileage: <b>{setMilageText(vehicleData)}</b> - Location:{' '}
              <b>{vehicleData.location}</b>
            </Typography>

            <TableContainer style={{ overflow: 'hidden' }}>
              <Table
                style={styles.table}
                size="small"
                padding="none"
                aria-label="simple table"
              >
                <TableBody>
                  <TableRow>
                    <TableCell style={styles.tableCell}>
                      <Box
                        display="flex"
                        flexDirection="row"
                        overflow="hidden"
                        bgcolor="background.paper"
                      >
                        <Box>
                          <SettingsIcon
                            style={{ color: grey[500] }}
                            fontSize="small"
                          />
                        </Box>
                        <Box>
                          <Typography
                            variant="caption"
                            color="textSecondary"
                            component="p"
                            style={styles.blackText}
                          >
                            {vehicleData.transmission}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                    <TableCell align="right" style={styles.tableCell}>
                      <Box
                        display="flex"
                        flexDirection="row"
                        overflow="hidden"
                        bgcolor="background.paper"
                      >
                        <Box>{setFuelAttribute(vehicleData.fuelType)}</Box>
                        <Box>
                          <Typography
                            variant="caption"
                            color="textSecondary"
                            component="p"
                            style={styles.blackText}
                          >
                            {vehicleData.fuelType}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell style={styles.tableCell}>
                      <Box
                        display="flex"
                        flexDirection="row"
                        overflow="hidden"
                        bgcolor="background.paper"
                      >
                        <Box>
                          <DateRangeIcon
                            style={{ color: grey[500] }}
                            fontSize="small"
                          />
                        </Box>
                        <Box>
                          <Typography
                            variant="caption"
                            color="textSecondary"
                            component="p"
                            style={styles.blackText}
                          >
                            {vehicleData.year}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                    <TableCell align="right" style={styles.tableCell}>
                      <Box display="flex">
                        <Box>
                          <CommuteIcon
                            style={{ color: grey[500] }}
                            fontSize="small"
                          />
                        </Box>
                        <Box>
                          <Typography
                            variant="caption"
                            color="textSecondary"
                            component="p"
                            style={styles.blackText}
                          >
                            {vehicleData.bodyStyle}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell style={styles.tableCell}>
                      <Box
                        display="flex"
                        flexDirection="row"
                        overflow="hidden"
                        bgcolor="background.paper"
                      >
                        <Box>
                          <TimeToLeaveIcon
                            style={{ color: grey[500] }}
                            fontSize="small"
                          />
                        </Box>
                        <Box>
                          <Typography
                            variant="caption"
                            color="textSecondary"
                            component="p"
                            style={styles.blackText}
                          >
                            {`${(
                              Number(vehicleData.engineSize) / Number(1000)
                            ).toFixed(1)} Ltr`}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                    <TableCell align="right" style={styles.tableCell}>
                      <Box display="flex">
                        <Box>
                          <PaletteIcon
                            style={{ color: grey[500] }}
                            fontSize="small"
                          />
                        </Box>
                        <Box>
                          <Typography
                            variant="caption"
                            component="p"
                            style={styles.blackText}
                          >
                            {vehicleData.colour}
                          </Typography>
                        </Box>
                      </Box>
                    </TableCell>
                  </TableRow>
                </TableBody>
              </Table>
            </TableContainer>
          </div>
        </Paper>
      </Grid>
    );
  };

  const mobileVehicleCard = () => {
    return (
      <Box display="flex" style={{ marginTop: 20 }}>
        <Box>
          <Link target="_blank" rel="noreferrer" href={vehicleData.pageUrl}>
            <VehicleImage
              mobile={true}
              imageURL={imageURL}
              imageTitle={imageTitle}
              missingImageURL={props.missingImageURL}
            ></VehicleImage>
          </Link>
        </Box>
        <Box>
          <div
            style={{
              paddingLeft: '10px',
              paddingRight: '10px',
              marginBottom: '0px',
              position: 'relative',
              bottom: 5,
              zIndex: -1,
            }}
          >
            <Link
              underline="hover"
              target="_blank"
              rel="noreferrer"
              href={vehicleData.pageUrl}
            >
              <Typography
                variant="subtitle1"
                color="textSecondary"
                component="p"
                align="left"
              >
                {title}
              </Typography>
            </Link>
            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              {vehicleData.variant}
            </Typography>

            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              {setPriceText(vehicleData)}
            </Typography>
            <Typography
              variant="caption"
              color="textSecondary"
              component="p"
              align="left"
            >
              Mileage: <b>{setMilageText(vehicleData)}</b>
            </Typography>

            <Box display="flex" style={{ position: 'relative', top: 5 }}>
              <Box>{setFuelAttribute(vehicleData.fuelType)}</Box>
              <Box>
                <Typography
                  variant="caption"
                  color="textSecondary"
                  component="p"
                  style={styles.blackText}
                >
                  {vehicleData.fuelType}
                </Typography>
              </Box>
              <Box style={{ marginLeft: 5 }}>
                <SettingsIcon style={{ color: grey[400] }} fontSize="small" />
              </Box>
              <Box>
                <Typography
                  variant="caption"
                  color="textSecondary"
                  component="p"
                  style={styles.blackText}
                >
                  {setMobileTransmissionText(vehicleData.transmission)}
                </Typography>
              </Box>
              <Box style={{ marginLeft: 5 }}>
                <DateRangeIcon style={{ color: grey[500] }} fontSize="small" />
              </Box>
              <Box>
                <Typography
                  variant="caption"
                  color="textSecondary"
                  component="p"
                  style={styles.blackText}
                >
                  {vehicleData.year}
                </Typography>
              </Box>
            </Box>
          </div>
        </Box>
      </Box>
    );
  };

  return windowWidth < MobileMaxWidth
    ? mobileVehicleCard()
    : desktopVehicleCard();
};

VehicleCard.propTypes = {
  vehicleData: PropTypes.arrayOf(
    PropTypes.shape({
      vehicleID: PropTypes.string,
      make: PropTypes.string,
      model: PropTypes.string,
      variant: PropTypes.string,
      location: PropTypes.string,
      price: PropTypes.number,
      monthlyPrice: PropTypes.number,
      mileage: PropTypes.number,
      fuelType: PropTypes.string,
      transmission: PropTypes.string,
      doors: PropTypes.number,
      engineSize: PropTypes.number,
      bodyStyle: PropTypes.string,
      colour: PropTypes.string,
      year: PropTypes.number,
      description: PropTypes.string,
      manufactureColour: PropTypes.string,
      vrm: PropTypes.string,
      imageURL: PropTypes.string,
    })
  ),
  missingImageURL: PropTypes.string,
};

const mapStateToProps = reduxState => {
  return {
    reduxPrimaryColour: reduxState.theme.primaryColour,
    reduxSecondaryColour: reduxState.theme.secondaryColour,
  };
};

VehicleCard.propTypes = {
  reduxPrimaryColour: PropTypes.string,
  reduxSecondaryColour: PropTypes.string,
};

export default connect(mapStateToProps)(VehicleCard);
