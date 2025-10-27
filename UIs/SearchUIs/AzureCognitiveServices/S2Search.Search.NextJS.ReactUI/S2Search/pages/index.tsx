import React from 'react';
import Head from 'next/head';
import VehicleSearchApp from '@/components/VehicleSearchApp';
import ResetFacets from '@/common/functions/ResetFacets';
import Container from '@mui/material/Container';

const Home: React.FC = () => {
  return (
    <>
      <Head>
        <title>S2 Search</title>
      </Head>
      <Container disableGutters maxWidth={false}>
        <VehicleSearchApp />
        <ResetFacets />
      </Container>
    </>
  );
};

export default Home;
