import React from 'react';
import InventoryList from '../components/InventoryList';

const InventoryPage = () => {
  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Inventory</h3>
      <InventoryList />
  </div>
  );
};

export default InventoryPage;