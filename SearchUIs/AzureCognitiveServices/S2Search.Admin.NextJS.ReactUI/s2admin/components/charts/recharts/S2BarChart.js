/* eslint-disable no-unused-vars */
import React from "react";
import { connect } from "react-redux";
import { makeStyles } from "@mui/styles";
import { Typography, Grid } from "@mui/material/";
import PropTypes from "prop-types";
import {
  ResponsiveContainer,
  BarChart,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  Bar,
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
    justifyContent: "center",
  },
  center: {
    justifyContent: "center",
  },
}));

const S2BarChart = (props) => {
  const classes = useStyles();
  const hasData = props.data && props.data.length;
  const hasHorizontalBars = props.chartLayout == "vertical";
  const chartId = uniqueId("chart");
  const x1Value = hasHorizontalBars ? 1 : 0;
  const y2Value = hasHorizontalBars ? 0 : 1;
  const xAxisType = hasHorizontalBars ? "number" : "category";
  const yAxisType = hasHorizontalBars ? "category" : "number";
  const xAxisTickFormatter = hasHorizontalBars
    ? SimpleAxisTickFormatter
    : CustomAxisTickFormatter;
  const yAxisTickFormatter = hasHorizontalBars
    ? CustomAxisTickFormatter
    : SimpleAxisTickFormatter;

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
            <ResponsiveContainer
              width="75%"
              aspect={props.chartLayout == "vertical" ? 2 : 4}
            >
              <BarChart
                data={props.data}
                layout={props.chartLayout}
                margin={{ left: 50 }}
              >
                <defs>
                  <linearGradient
                    id={`barColour_${chartId}`}
                    x1={x1Value}
                    y1={0}
                    x2={0}
                    y2={y2Value}
                  >
                    <stop offset="0%" stopColor="#2471b7" stopOpacity={0.4} />
                    <stop offset="75%" stopColor="#2471b7" stopOpacity={0.05} />
                  </linearGradient>
                </defs>

                <Bar
                  animationDuration={500}
                  animationEasing="ease-out"
                  dataKey={props.barKey}
                  stroke="#2471b7"
                  fill={`url(#barColour_${chartId})`}
                  background={{ fill: "#eee" }}
                />
                <Tooltip
                  content={<CustomTooltip />}
                  offset={25}
                  cursor={{ opacity: 0.3, stroke: "red", fill: "#fff" }}
                />
                <YAxis
                  dataKey={props.yKey}
                  type={yAxisType}
                  axisLine={false}
                  tickLine={false}
                  tickFormatter={yAxisTickFormatter}
                />
                <XAxis
                  dataKey={props.xKey}
                  type={xAxisType}
                  axisLine={false}
                  tickLine={false}
                  tickFormatter={xAxisTickFormatter}
                />
                <CartesianGrid opacity={0.2} vertical={false} />
              </BarChart>
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

S2BarChart.propTypes = {
  reduxSelectedSearchIndex: PropTypes.object,
  data: PropTypes.array,
  title: PropTypes.string,
  chartLayout: PropTypes.string,
  barKey: PropTypes.string,
  xKey: PropTypes.string,
  yKey: PropTypes.string,
};

export default connect(mapStateToProps)(S2BarChart);
