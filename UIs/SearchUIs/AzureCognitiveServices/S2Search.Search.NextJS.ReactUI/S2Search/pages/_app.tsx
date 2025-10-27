import type { AppProps } from 'next/app';
import '@/styles/globals.css';
// Optimized font loading - only load weights we actually use
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import CssBaseline from '@mui/material/CssBaseline';
import Head from 'next/head';
import { Provider as ReduxProvider } from 'react-redux';
import { store } from '../store';
import { ThemeProvider } from '@mui/material/styles';
import { DefaultTheme } from '../common/Constants';
import { createAppTheme } from '../common/theme';

export default function App({ Component, pageProps }: AppProps) {
  const theme = createAppTheme({
    primaryColor: DefaultTheme.primaryHexColour,
    secondaryColor: DefaultTheme.secondaryHexColour,
  });

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
        <ReduxProvider store={store}>
          <Component {...pageProps} />
        </ReduxProvider>
      </ThemeProvider>
    </>
  );
}
