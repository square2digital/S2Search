import React from "react";
import {
  AuthenticatedTemplate,
  UnauthenticatedTemplate,
  useIsAuthenticated,
} from "@azure/msal-react";
import { getUserData, signIn } from "../services/identity/msal";
import Overview from "../components/layout/Overview";
import S2OverviewPanel from "../components/panels/S2OverviewPanel";
import BuildReduxData from "../components/configuration/BuildReduxData";

const RedirectToB2C = () => {
  const isAuthenticated = useIsAuthenticated();

  if (!isAuthenticated) {
    signIn();
  }

  return null;
};

const RenderStartPanel = () => {
  return (
    <Overview
      user={getUserData}
      panelComponent={
        <S2OverviewPanel
          user={getUserData}
          title={"S2 Search - Admin Portal"}
        />
      }
    />
  );
};

const Index = () => {
  return (
    <div>
      <AuthenticatedTemplate>
        <BuildReduxData />
        <RenderStartPanel />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <RedirectToB2C />
      </UnauthenticatedTemplate>
    </div>
  );
};

export default Index;
