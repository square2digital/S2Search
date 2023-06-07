import * as React from "react";
import Alert from "@mui/material/Alert";
import AlertTitle from "@mui/material/AlertTitle";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import Image from "next/image";

import s2logo from "../assets/logos/Square_2_Logo_Colour_Blue.svg";

export default function auth() {
  return (
    <Grid
      container
      spacing={0}
      direction="column"
      alignItems="center"
      justifyContent="center"
      style={{ minHeight: "80vh" }}>
      <Grid>
        <Image
          src={s2logo}
          alt="Square 2 Logo"
          width={175}
          style={{ paddingTop: 10, paddingBottom: 10 }}
        />
      </Grid>
      <Grid>
        <Alert severity="info" style={{ marginTop: 15 }}>
          <AlertTitle>S2 Admin Portal</AlertTitle>
          Loading — <strong>please stand by</strong>
        </Alert>
      </Grid>
    </Grid>
  );
}
