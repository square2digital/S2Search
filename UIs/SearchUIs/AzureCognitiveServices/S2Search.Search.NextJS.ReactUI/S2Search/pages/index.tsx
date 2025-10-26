import Head from 'next/head';
import { GetServerSideProps } from 'next';
import App from '../components/App';
import { Provider as ReduxProvider } from 'react-redux';
import { store } from '../store';
import { ThemeProvider } from '@mui/material/styles';
import { DefaultTheme } from '../common/Constants';
import ThemeColours from '../common/ThemeColours';
import { createAppTheme } from '../common/theme';

interface ThemeData {
  palette: {
    primary: {
      main: string;
    };
    secondary: {
      main: string;
    };
  };
  overrides?: any;
}

interface HomeProps {
  data?: ThemeData;
  primaryHexColour?: string;
  secondaryHexColour?: string;
}

export const getServerSideProps: GetServerSideProps<HomeProps> = async ({ req }) => {
  const data = await ThemeColours(req);

  if (!data) {
    return {
      props: {
        primaryHexColour: DefaultTheme.primaryHexColour,
        secondaryHexColour: DefaultTheme.secondaryHexColour,
      },
    };
  }

  // Ensure all required values are defined with fallbacks
  const primaryColor = data.palette?.primary?.main ?? DefaultTheme.primaryHexColour;
  const secondaryColor = data.palette?.secondary?.main ?? DefaultTheme.secondaryHexColour;

  // Type assertion to ensure they are strings (we know they are after nullish coalescing)
  const themeData: ThemeData = {
    palette: {
      primary: {
        main: primaryColor as string,
      },
      secondary: {
        main: secondaryColor as string,
      },
    },
    overrides: data.overrides,
  };

  return {
    props: {
      data: themeData,
    },
  };
};

const Home: React.FC<HomeProps> = (props) => {
  // Create theme using the modern theme factory
  const primaryColor = props.data?.palette?.primary?.main || props.primaryHexColour || DefaultTheme.primaryHexColour;
  const secondaryColor = props.data?.palette?.secondary?.main || props.secondaryHexColour || DefaultTheme.secondaryHexColour;
  
  const theme = createAppTheme({
    primaryColor,
    secondaryColor,
  });

  return (
    <ThemeProvider theme={theme}>
      <Head>
        <title>S2 Search</title>
        <meta
          name="description"
          content="S2 Search - Modern search interface"
        />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <ReduxProvider store={store}>
        <App />
      </ReduxProvider>
    </ThemeProvider>
  );
};

export default Home;