import React from "react";
import Overview from "../components/layout/Overview";
import S2ThemePanel from "../components/panels/S2ThemePanel";

export default function theme() {
  return (
    <Overview panelComponent={<S2ThemePanel title={"S2 Search - Theme"} />} />
  );
}
