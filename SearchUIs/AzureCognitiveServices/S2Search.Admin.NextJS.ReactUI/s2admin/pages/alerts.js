import React from "react";
import Overview from "../components/layout/Overview";
import S2AlertsPanel from "../components/panels/S2AlertsPanel";

export default function alerts() {
  return (
    <Overview panelComponent={<S2AlertsPanel title={"S2 Search - Alerts"} />} />
  );
}
