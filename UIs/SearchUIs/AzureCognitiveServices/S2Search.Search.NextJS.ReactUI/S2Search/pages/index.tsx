import React from 'react';
import Head from 'next/head';
import Layout from '@/components/Layout';
import VehicleSearchApp from '@/components/VehicleSearchApp';
import { Provider as ReduxProvider } from 'react-redux';
import { store } from '../store';
import { ThemeProvider } from '@mui/material/styles';
import { DefaultTheme } from '../common/Constants';
import { createAppTheme } from '../common/theme';

const Home: React.FC = () => {
  const theme = createAppTheme({
    primaryColor: DefaultTheme.primaryHexColour,
    secondaryColor: DefaultTheme.secondaryHexColour,
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
        <Layout>
          <VehicleSearchApp />
        </Layout>
      </ReduxProvider>
    </ThemeProvider>
  );
};

export default Home;
