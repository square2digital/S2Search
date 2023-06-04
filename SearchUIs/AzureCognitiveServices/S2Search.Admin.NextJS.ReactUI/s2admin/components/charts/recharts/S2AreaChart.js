/* eslint-disable no-unused-vars */
import React from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import { Typography, Grid } from "@mui/material/";
import PropTypes from "prop-types";
import {
  ResponsiveContainer,
  AreaChart,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  Area,
} from "recharts";
//import "../../../index.css";
import CustomTooltip from "./CustomTooltip";
import uniqueId from "./../../common/uniqueId";
import {
  CustomAxisTickFormatter,
  SimpleAxisTickFormatter,
} from "./CustomAxisTickFormatters";

const useStyles = makeStyles((theme) => ({
  content: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing(3),
  },
  center: {
    justifyContent: "center",
  },
}));

const S2AreaChart = (props) => {
  const classes = useStyles();
  const hasData = props.data && props.data.length;
  const chartId = uniqueId("chart");

  return (
    <main className={classes.content}>
      {hasData ? (
        <Grid container>
          <Grid item container>
            <Typography>
              <h3>{props.title}</h3>
            </Typography>
          </Grid>
          <Grid item container>
            <ResponsiveContainer width="75%" aspect={4}>
              <AreaChart data={props.data} margin={{ top: 10, left: 50 }}>
                <defs>
                  <linearGradient
                    id={`areaColour_${chartId}`}
                    x1={0}
                    y1={0}
                    x2={0}
                    y2={1}
                  >
                    <stop offset="0%" stopColor="#2471b7" stopOpacity={0.4} />
                    <stop offset="75%" stopColor="#2471b7" stopOpacity={0.05} />
                  </linearGradient>
                </defs>

                <Area
                  animationDuration={500}
                  animationEasing="ease-out"
                  dataKey={props.areaKey}
                  stroke="#2471b7"
                  fill={`url(#areaColour_${chartId})`}
                  activeDot={{ r: 9 }}
                  type="monotone"
                />

                <XAxis
                  dataKey={props.xKey}
                  axisLine={false}
                  tickLine={false}
                  tickFormatter={CustomAxisTickFormatter}
                  tickMargin={10}
                />

                <YAxis
                  dataKey={props.yKey}
                  axisLine={false}
                  tickLine={false}
                  tickCount={5}
                  tickFormatter={SimpleAxisTickFormatter}
                />

                <Tooltip
                  content={<CustomTooltip />}
                  offset={25}
                  stopColor="blue"
                />

                <CartesianGrid opacity={0.4} vertical={false} />
              </AreaChart>
            </ResponsiveContainer>
          </Grid>
        </Grid>
      ) : (
        <Typography>
          <h3>{props.title}</h3>
          <p>No Data Available</p>
        </Typography>
      )}
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

S2AreaChart.propTypes = {
  reduxSelectedSearchIndex: PropTypes.object,
  data: PropTypes.array,
  title: PropTypes.string,
  areaKey: PropTypes.string,
  xKey: PropTypes.string,
  yKey: PropTypes.string,
};

export default connect(mapStateToProps)(S2AreaChart);
