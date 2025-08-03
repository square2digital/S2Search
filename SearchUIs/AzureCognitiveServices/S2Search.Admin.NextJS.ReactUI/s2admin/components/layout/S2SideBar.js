import React from "react";
import { useMsal } from "@azure/msal-react";

// material UI Components
import { useRouter } from "next/router";
import Drawer from "@mui/material/Drawer";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Grid from "@mui/material/Grid";
import Divider from "@mui/material/Divider";
import Image from "next/image";

// Icons
import DynamicFeedIcon from "@mui/icons-material/DynamicFeed";
import SettingsIcon from "@mui/icons-material/Settings";
import ColorLensIcon from "@mui/icons-material/ColorLens";
import ExitToAppIcon from "@mui/icons-material/ExitToApp";
import SettingsOverscanIcon from "@mui/icons-material/SettingsOverscan";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import PublishIcon from "@mui/icons-material/Publish";
import BarChartIcon from "@mui/icons-material/BarChart";

// logo
import s2logo from "../../assets/logos/Square_2_Logo_Colour_Blue.svg";
import SearchIndexRadioList from "../searchIndexes/SearchIndexRadioList";

const drawerWidth = 210;

const S2SideBar = () => {
  const router = useRouter();
  const { instance } = useMsal();

  const changePage = (url) => {
    router.push(url, url, { shallow: true });
  };

  return (
    <>
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          [`& .MuiDrawer-paper`]: {
            width: drawerWidth,
            boxSizing: "border-box",
          },
        }}>
        <Grid
          container
          spacing={0}
          direction="column"
          alignItems="center"
          justify="center">
          <Grid item xs={12}>
            <Image
              src={s2logo}
              data-href="/"
              alt="Square 2 Logo"
              width={drawerWidth - 125}
              style={{ paddingTop: 10, paddingBottom: 10 }}
            />
          </Grid>
        </Grid>

        <Divider />
        <ListItem></ListItem>

        <SearchIndexRadioList />

        <ListItem></ListItem>
        <Divider />

        <List component="nav">
          <ListItem button onClick={() => changePage("/")}>
            <ListItemIcon>
              <SettingsOverscanIcon />
            </ListItemIcon>
            <ListItemText primary="Overview" />
          </ListItem>
          <ListItem button onClick={() => changePage("/configuration")}>
            <ListItemIcon>
              <SettingsIcon />
            </ListItemIcon>
            <ListItemText primary="Configuration" />
          </ListItem>
          <ListItem button onClick={() => changePage("/ftp")}>
            <ListItemIcon>
              <DynamicFeedIcon />
            </ListItemIcon>
            <ListItemText primary="FTP" />
          </ListItem>
          <ListItem button onClick={() => changePage("/feed-upload")}>
            <ListItemIcon>
              <PublishIcon />
            </ListItemIcon>
            <ListItemText primary="Feed Upload" />
          </ListItem>
          <ListItem button onClick={() => changePage("/theme")}>
            <ListItemIcon>
              <ColorLensIcon />
            </ListItemIcon>
            <ListItemText primary="Theme" />
          </ListItem>
          <ListItem button onClick={() => changePage("/alerts")}>
            <ListItemIcon>
              <NotificationsNoneIcon />
            </ListItemIcon>
            <ListItemText primary="Alerts" />
          </ListItem>
        </List>
        <ListItem></ListItem>
        <Divider />
        <ListItem></ListItem>

        {instance !== null ? (
          <>
            <ListItem button onClick={() => instance.logout()}>
              <ListItemIcon>
                <ExitToAppIcon />
              </ListItemIcon>
              <ListItemText primary="Sign Out" />
            </ListItem>
          </>
        ) : (
          <></>
        )}
      </Drawer>
    </>
  );
};

export default S2SideBar;
