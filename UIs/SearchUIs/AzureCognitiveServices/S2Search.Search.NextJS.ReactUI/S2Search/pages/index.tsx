import ResetFacets from '@/common/functions/ResetFacets';
import VehicleSearchApp from '@/components/VehicleSearchApp';
import Container from '@mui/material/Container';
import Head from 'next/head';
import React from 'react';

const Home: React.FC = () => {
  return (
    <>
      <Head>
        <title>S2 Search - Next.js 16 & React 19</title>
      </Head>
      <Container disableGutters maxWidth={false}>
        <VehicleSearchApp />
        <ResetFacets />
      </Container>
    </>
  );
};

export default Home;
