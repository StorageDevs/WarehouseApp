import React, { useEffect, useState } from 'react'

function GetInventory() {
  const url = "https://localhost:7055/api/Inventories"
  const [inventoryData, setInventoryData] = useState([]);

  useEffect(() => 
  {
    (async () => 
      {
      const request = await fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        }
      })
      
      if(!request.ok)
      {
        console.log("Error")
        return
      }

      const response = await request.json();
      setInventoryData(response.result);
      console.log(response.message);
      })()
    }, []);

    return (
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px" }}>
        {inventoryData.map((inventory) => (
          <div
            className="card"
            style={{
              width: 200,
              margin: 10,
              padding: 10,
              border: "2px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
            }}
            key={inventory.inventoryId}
          >
          <div className="card-body">Number: {inventory.materialNumber}</div>
          <div className="card-body">Description: {inventory.materialDescription}</div>
          <div className="card-body">LocationFrom: {inventory.locationFrom}</div>
          <div className="card-body">LocationTo: {inventory.locationTo}</div>
          <div className="card-body">Quantity: {inventory.transferQuantity}</div>
          <div className="card-body">User: {inventory.user}</div>
          <div className="card-body">Time: {inventory.transactionTime}</div>
          </div>
        ))}
      </div>
    );
}

export default GetInventory