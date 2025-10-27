import Head from 'next/head';
import { GetServerSideProps } from 'next';
import App from '../components/App';
import { Provider as ReduxProvider } from 'react-redux';
import { store } from '../store';
import { ThemeProvider } from '@mui/material/styles';
import { DefaultTheme } from '../common/Constants';
import ThemeColours from '../common/ThemeColours';
import { createAppTheme } from '../common/theme';

interface HomeProps {
  primaryColor?: string;
  secondaryColor?: string;
}

export const getServerSideProps: GetServerSideProps<HomeProps> = async ({ req }) => {
  const themeData = await ThemeColours(req);

  return {
    props: {
      primaryColor: themeData?.palette?.primary?.main || DefaultTheme.primaryHexColour,
      secondaryColor: themeData?.palette?.secondary?.main || DefaultTheme.secondaryHexColour,
    },
  };
};

const Home: React.FC<HomeProps> = ({ primaryColor, secondaryColor }) => {
  const theme = createAppTheme({
    primaryColor: primaryColor || DefaultTheme.primaryHexColour,
    secondaryColor: secondaryColor || DefaultTheme.secondaryHexColour,
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