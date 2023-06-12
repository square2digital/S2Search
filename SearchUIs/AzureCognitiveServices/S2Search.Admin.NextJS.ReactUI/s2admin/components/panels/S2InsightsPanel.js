import React, { useEffect, useState, forwardRef } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { BarChart, AreaChart, TileChart } from "../charts/recharts/S2Charts";
import TimeframeDropdown from "./../charts/TimeframeDropdown";
import { makeStyles } from "@mui/styles";
import { Typography, Grid } from "@mui/material";
import { getChartData, getSummaryData } from "../../client/insightsClient";
import { getInterface } from "../../client/interfaceClient";
import Snackbar from "@mui/material/Snackbar";
import MuiAlert from "@mui/material/Alert";
import DateRangeProvider from "../common/DateRangeProvider";
import { getUserData } from "../../services/identity/msal";
import LoadingButtonLink from "../buttons/LoadingButtonLink";

const Alert = forwardRef(function Alert(props, ref) {
  return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
});

const useStyles = makeStyles((theme) => ({
  content: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing(3),
  },
  charts: {
    justifyContent: "center",
  },
}));

const S2InsightsPanel = (props) => {
  const classes = useStyles();
  const [periodDays, setPeriodDays] = useState(-7);
  const [summaryData, setSummaryData] = useState([]);
  const [tilesData, setTilesData] = useState([]);
  const [chart1, setChart1] = useState([]);
  const [chart2, setChart2] = useState([]);
  const [snackOpen, setSnackOpen] = useState(false);
  const [snackMessage, setSnackMessage] = useState("");
  const [alertSeverity, setAlertSeverity] = useState("success");
  const [searchEndpoint, setSearchEndpoint] = useState("");
  const [loading, setLoading] = useState(true);

  const searchIndexId = props.reduxSelectedSearchIndex.id;
  const user = getUserData();

  const handleClose = (event, reason) => {
    setSnackOpen(false);
  };

  useEffect(() => {
    if (searchIndexId === undefined) {
      setSnackOpen(true);
      setAlertSeverity("info");
      setSnackMessage("Loading Search Insights");
    } else {
      const customerId = user.localAccountId;

      getInterface(customerId, searchIndexId).then(function (data) {
        const hasEndpoint = data.result.searchEndpoint.length > 0;

        if (hasEndpoint) {
          setSearchEndpoint(data.result.searchEndpoint);
        }
      });

      if (summaryData.length == 0) {
        getSummaryData(customerId, searchIndexId).then(function (data) {
          setSummaryData(data.result);
          setChart1(data.result.charts[0]);
          setChart2(data.result.charts[1]);
          setTilesData(data.result.tiles);
        });
      } else {
        const { dateFrom, dateTo } = DateRangeProvider(periodDays);

        getChartData(
          customerId,
          searchIndexId,
          "SearchesPerDay",
          dateFrom,
          dateTo
        ).then(function (data) {
          setChart1(data.result);
        });

        getChartData(
          customerId,
          searchIndexId,
          "PopularMakes",
          dateFrom,
          dateTo
        ).then(function (data) {
          setChart2(data.result);
        });
      }
      setLoading(false);
    }
  }, [searchIndexId, periodDays, summaryData]);

  const handleTimeframeChange = (event) => {
    setPeriodDays(event.target.value);
  };

  return (
    <main className={classes.content}>
      <Grid container spacing={2}>
        <Grid item container xs={12}>
          <Grid item xs={12}>
            <Typography>
              <h2>Search Insights</h2>
              <LoadingButtonLink
                displayText={searchEndpoint}
                placeholderText="Domain Pending"
                href={`https://${searchEndpoint}`}
                colour={searchEndpoint.length > 0 ? "inherit" : "warning"}
                loading={loading}
              />
            </Typography>
          </Grid>
        </Grid>

        <Grid item container xs={12} spacing={3}>
          {tilesData.map((tile) => (
            <Grid item key={tile.title}>
              <TileChart
                title={tile.title}
                count={tile.count}
                percentageChange={tile.previousPeriodPercentageChange}
                percentageChangePeriod={tile.previousPeriod}
              />
            </Grid>
          ))}
        </Grid>
        <Grid item container>
          <Grid item container xs={9}>
            <Grid item xs={10} />
            <Grid item xs={2}>
              <TimeframeDropdown
                periodDays={periodDays}
                setTimeframe={handleTimeframeChange}
              />
            </Grid>
          </Grid>
          <Grid item container xs={12}>
            <AreaChart
              data={chart1.data}
              title={chart1.title}
              chartLayout="horizontal"
              areaKey="count"
              xKey="date"
              yKey="count"
            />
          </Grid>
          <Grid item container xs={12}>
            <BarChart
              data={chart2.data}
              title={chart2.title}
              chartLayout="vertical"
              barKey="count"
              xKey="count"
              yKey="dataPoint"
            />
          </Grid>
        </Grid>
      </Grid>

      <Snackbar
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        open={snackOpen}
        autoHideDuration={4000}
        onClose={handleClose}
      >
        <Alert
          onClose={handleClose}
          severity={alertSeverity}
          sx={{ width: "100%" }}
        >
          {snackMessage}
        </Alert>
      </Snackbar>
    </main>
  );
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

S2InsightsPanel.propTypes = {
  user: PropTypes.object,
  reduxSelectedSearchIndex: PropTypes.object,
};

export default connect(mapStateToProps)(S2InsightsPanel);
