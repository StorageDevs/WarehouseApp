import React from 'react';
import GetAllMaterial from '../components/Material/GetAllMaterial';

const MaterialsPage = () => {
  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Materials</h3>
      <GetAllMaterial />
    </div>
  );
};

export default MaterialsPage;
