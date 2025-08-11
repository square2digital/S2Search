import Head from 'next/head';
import App from '../components/App';
import { Provider as ReduxProvider } from 'react-redux';
import configureStore from '../redux/configureStore';
import { ThemeProvider } from '@mui/material/styles';
import { DefaultTheme } from '../common/Constants';
import ThemeColours from '../common/ThemeColours';
import { createAppTheme } from '../common/theme';

export async function getServerSideProps() {
  const data = await ThemeColours();

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

const Home = props => {
  const reduxStore = configureStore();

  // Create theme using the modern theme factory
  const theme = createAppTheme(
    props.data?.palette?.primary?.main || DefaultTheme.primaryHexColour,
    props.data?.palette?.secondary?.main || DefaultTheme.secondaryHexColour
  );

  return (
    <ThemeProvider theme={theme}>
      <Head>
        <title>S2 Search</title>
        <meta
          name="description"
          content="S2 Search - Modern search interface"
        />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta name="emotion-insertion-point" content="" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <ReduxProvider store={reduxStore}>
        <App />
      </ReduxProvider>
    </ThemeProvider>
  );
};

export default Home;
