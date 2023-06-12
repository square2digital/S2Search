import React from "react";
import S2BarChart from "./S2BarChart";
import S2AreaChart from "./S2AreaChart";
import S2TileChart from "./S2TileChart";

export const BarChart = (props) => {
  return <S2BarChart {...props} />;
};

export const AreaChart = (props) => {
  return <S2AreaChart {...props} />;
};

export const TileChart = (props) => {
  return <S2TileChart {...props} />;
};
