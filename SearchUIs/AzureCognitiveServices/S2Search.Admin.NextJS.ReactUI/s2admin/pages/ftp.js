import React from "react";
import Overview from "../components/layout/Overview";
import S2FtpDetailsPanel from "../components/panels/S2FtpDetailsPanel";

export default function ftp() {
  return (
    <Overview
      panelComponent={
        <S2FtpDetailsPanel title={"S2 Search - FTP Vehicle Data"} />
      }
    />
  );
}
