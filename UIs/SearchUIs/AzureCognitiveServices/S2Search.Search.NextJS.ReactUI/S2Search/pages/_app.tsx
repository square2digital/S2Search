import type { AppProps } from 'next/app';
import '@/styles/globals.css';
import CssBaseline from '@mui/material/CssBaseline';
import Head from 'next/head';
import { Provider } from 'react-redux';
import { store } from '../store';
import { ThemeProvider } from '@mui/material/styles';
import { createAppTheme } from '../common/theme';

export default function App({ Component, pageProps }: AppProps) {
  // Create theme with defaults from Constants
  const theme = createAppTheme();

  return (
    <>
      <Head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
        <link rel="icon" href="/favicon.ico" />
        {/* Default meta tags - can be overridden by individual pages */}
        <meta
          name="description"
          content="S2 Search - Modern search interface"
        />
      </Head>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Provider store={store}>
          <Component {...pageProps} />
        </Provider>
      </ThemeProvider>
    </>
  );
}
