import { MsalProvider } from "@azure/msal-react";
import { msalInstance, getRedirectURI } from "../services/identity/msal";
import { ThemeProvider } from "@mui/styles";
import { createTheme } from "@mui/material/styles";
import configureStore from "../redux/configureStore";
import { Provider as ReduxProvider } from "react-redux";
import React, { useEffect, useState } from "react";
import "../index.css";

const theme = createTheme({});
const store = configureStore();

const MyApp = ({ Component }) => {
  const [endpoint, setEndpoint] = useState("");

  useEffect(() => {
    setEndpoint(getRedirectURI(window));
  }, [endpoint]);

  return endpoint === "" ? (
    <></>
  ) : (
    <MsalProvider instance={msalInstance(endpoint)}>
      <ThemeProvider theme={theme}>
        <ReduxProvider store={store}>
          <Component />
        </ReduxProvider>
      </ThemeProvider>
    </MsalProvider>
  );
};

export default MyApp;
