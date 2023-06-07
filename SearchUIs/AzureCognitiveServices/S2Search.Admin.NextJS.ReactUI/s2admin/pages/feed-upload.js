import React from "react";
import Overview from "../components/layout/Overview";
import S2FeedUploadPanel from "../components/panels/S2FeedUploadPanel";

export default function feedUpload() {
  return (
    <Overview
      panelComponent={<S2FeedUploadPanel title={"S2 Search - Upload Feed"} />}
    />
  );
}
