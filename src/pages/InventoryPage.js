import React from 'react';
import GetInventory from '../Inventory/GetInventory';

const InventoryPage = () => {
  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Inventory</h3>
      <GetInventory />
  </div>
  );
};

export default InventoryPage;