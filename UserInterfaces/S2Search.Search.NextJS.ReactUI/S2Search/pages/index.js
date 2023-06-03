import Head from "next/head";
import App from "../components/App";
import { Provider as ReduxProvider } from "react-redux";
import configureStore from "../redux/configureStore";
import { ThemeProvider } from "@mui/styles";
import { createTheme } from "@mui/material/styles";
import { DefaultTheme } from "../common/Constants";
import ThemeColours from "../common/ThemeColours";

const theme = createTheme({});

export async function getServerSideProps(context) {
  let data = await ThemeColours();

  if (!data) {
    return {
      props: {
        primaryHexColour: DefaultTheme.primaryHexColour,
        secondaryHexColour: DefaultTheme.secondaryHexColour,
      },
    };
  }

  if (data.palette.primary.main === undefined) {
    data.palette.primary.main = DefaultTheme.primaryHexColour;
  }

  if (data.palette.secondary.main === undefined) {
    data.palette.secondary.main = DefaultTheme.secondaryHexColour;
  }

  return {
    props: {
      data,
    },
  };
}

const Home = (props) => {
  const reduxStore = configureStore();

  theme.palette.primary = props.data.palette.primary.main;
  theme.palette.secondary = props.data.palette.secondary.main;

  return (
    <ThemeProvider theme={theme}>
      <Head>
        <title>S2 Search</title>
      </Head>
      <ReduxProvider store={reduxStore}>
        <App />
      </ReduxProvider>
    </ThemeProvider>
  );
};

export default Home;
