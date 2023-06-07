import React from "react";
import { makeStyles } from "@mui/styles";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";

const useStyles = makeStyles((theme) => ({
  spinner: {
    textAlign: "center",
    color: theme.palette.text.secondary,
  },
}));

const Spinner = () => {
  const classes = useStyles();

  return (
    <div>
      <Grid container>
        <Grid item xs={12}>
          <div className={classes.spinner}>
            <CircularProgress />
          </div>
        </Grid>
      </Grid>
    </div>
  );
};

export default Spinner;
