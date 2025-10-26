import VehicleList from '../../components/VehicleList';
import Head from 'next/head';

const VehicleTest: React.FC = () => {
  return (
    <div>
      <Head>
        <title>S2 Search - VehicleTest</title>
      </Head>
      <VehicleList />
    </div>
  );
};

export default VehicleTest;