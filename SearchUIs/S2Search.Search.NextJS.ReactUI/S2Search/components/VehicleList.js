import React, { useState, useEffect } from "react";
import Button from "@mui/material/Button";
import { AxiosGet } from "../pages/api/helper/AxiosAPICall";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";

const VehicleList = () => {
  const [vehicleData, setVehicleData] = useState([]);
  const [buttonClickCount, setButtonClickCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(async () => {
    getVehicleData();
  }, [buttonClickCount]);

  const buttonClick = () => {
    const count = buttonClickCount + 1;
    setButtonClickCount(count);
  };

  const getRandomInt = (max) => {
    return Math.floor(Math.random() * max);
  };

  const getURL = () => {
    const arr = [
      "bmw",
      "audi",
      "porsche",
      "bentley",
      "red",
      "yellow",
      "black",
      "volvo",
      "ford",
      "audi rs",
      "white",
      "suv",
      "convert",
    ];
    let index = getRandomInt(arr.length);
    setSearchTerm(arr[index]);
    return `/api/search?searchTerm=${arr[index]}&filters=&orderBy=&pageNumber=0&pageSize=24&numberOfExistingResults=0&callingHost=localhost:3000`;
  };

  const getVehicleData = async () => {
    const axiosResponse = await AxiosGet(getURL());

    if (axiosResponse.status === 200) {
      setVehicleData(axiosResponse.data);
    }
  };

  if (!vehicleData.results) return <div>Loading...</div>;

  return (
    <Box>
      <Button variant="contained" onClick={buttonClick}>
        Update vehicle List
      </Button>

      <Typography variant="h6" gutterBottom component="div">
        seatch term - {searchTerm}
      </Typography>

      <ul>
        {vehicleData.results.map((vehicle) => (
          <li>
            <p>{`${vehicle.make} ${vehicle.model} ${vehicle.variant}`}</p>
          </li>
        ))}
      </ul>
    </Box>
  );
};

export default VehicleList;
