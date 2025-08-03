import * as msal from "@azure/msal-browser";
import { msalConfig, loginRequest } from "./maslIdentyConfig";

const instance = new msal.PublicClientApplication(msalConfig);

export const getRedirectURI = (window) => {
  let endpoint = "";
  const auth = "/auth";

  const currentLocation = window.location;
  endpoint = `${currentLocation.protocol}//${currentLocation.hostname}`;

  if (currentLocation.port) {
    endpoint = `${endpoint}:${currentLocation.port}`;
  }

  if (!endpoint.includes(auth)) {
    endpoint = `${endpoint}/auth`;
  }

  return endpoint;
};

export const msalInstance = (redirectUri) => {
  if (redirectUri) {
    let updatedConfig = msalConfig;
    updatedConfig.auth.redirectUri = redirectUri;
    instance = new msal.PublicClientApplication(updatedConfig);
  }

  return instance;
};

export const signIn = () => {
  instance.loginRedirect(loginRequest);
};

export const signOut = () => {
  instance.logoutRedirect(loginRequest);
};

export const getUserData = () => {
  const user = instance.getAllAccounts();
  if (user.length > 0) {
    return user[0];
  }
};
