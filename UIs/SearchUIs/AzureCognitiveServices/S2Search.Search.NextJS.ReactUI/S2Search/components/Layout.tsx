import React from 'react';
import VehicleSearchApp from './VehicleSearchApp';
import Container from '@mui/material/Container';
import ResetFacets from '../common/functions/ResetFacets';

interface LayoutProps {
  children?: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <Container disableGutters maxWidth={false}>
        {children || <VehicleSearchApp />}
        <ResetFacets />
      </Container>
    </>
  );
};

export default Layout;
