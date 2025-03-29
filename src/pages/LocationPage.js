import React from 'react';
import GetLocation from '../components/Location/GetLocation';

const LocationPage = () => {
  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Locations</h3>
      <GetLocation />
    </div>
  );
};

export default LocationPage;