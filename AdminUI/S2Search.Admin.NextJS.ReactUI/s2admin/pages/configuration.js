import React from "react";
import Overview from "../components/layout/Overview";
import S2ConfigurationPanel from "../components/panels/S2ConfigurationPanel";

export default function configuration() {
  return (
    <Overview
      panelComponent={
        <S2ConfigurationPanel title={"S2 Search - Configuration"} />
      }
    />
  );
}
