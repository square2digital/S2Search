import React from "react";
import { makeStyles } from "@mui/styles";
import { Card, CardContent, Typography } from "@mui/material";
import PropTypes from "prop-types";
import TilePercentageLabel from "../TilePercentageLabel";

const useStyles = makeStyles({
  content: {
    width: 200,
    height: 200,
    maxWidth: 200,
    maxHeight: 200,
  },
  title: {
    fontSize: 14,
    maxHeight: 10,
  },
});

const S2TileChart = ({
  title,
  count,
  percentageChange,
  percentageChangePeriod,
}) => {
  const classes = useStyles();

  return (
    <Card className={classes.content} elevation={2}>
      <CardContent>
        <Typography
          className={classes.title}
          color="textSecondary"
          gutterBottom>
          {title}
        </Typography>
      </CardContent>

      <CardContent>
        <br />
        <Typography variant="h4">
          {count.toLocaleString(navigator.language)}
        </Typography>
        <br />
        <Typography variant="h7">
          <TilePercentageLabel percentageChange={percentageChange} />
          <Typography variant="caption">{percentageChangePeriod}</Typography>
        </Typography>
      </CardContent>
    </Card>
  );
};

S2TileChart.propTypes = {
  title: PropTypes.string,
  count: PropTypes.number,
  percentageChange: PropTypes.number,
  percentageChangePeriod: PropTypes.string,
};

export default S2TileChart;
