import React from "react";
import VehicleSearchApp from "./VehicleSearchApp";
import Container from "@mui/material/Container";
import ResetFacets from "../common/functions/ResetFacets";

const Layout = () => {
  return (
    <>
      <Container disableGutters maxWidth={false}>
        <VehicleSearchApp />
        <ResetFacets />
      </Container>
    </>
  );
};

export default Layout;
