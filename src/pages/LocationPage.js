import React from 'react';
import LocationList from '../components/LocationList';

const LocationPage = () => {
  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Locations</h3>
      <LocationList />
    </div>
  );
};

export default LocationPage;