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
      <div>
        <br></br>
        <h2 style={{ textAlign: "center", marginBottom: "20px", padding: "10px" }}>Inventory</h2>
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px", justifyContent: "left"}}>
        {inventoryData.map((inventory) => (
          <div
            className="card"
            style={{
              width: 450,
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
            }}
            key={inventory.inventoryId}
          >
          <div className="card-body">Number: {inventory.materialNumber}</div>
          <div className="card-body">Description: {inventory.materialDescription}</div>
          <div className="card-body">From: {inventory.locationFrom}</div>
          <div className="card-body">To: {inventory.locationTo}</div>
          <div className="card-body">Quantity: {inventory.transferQuantity}</div>
          <div className="card-body">User: {inventory.user}</div>
          <div className="card-body">Time: {inventory.transactionTime}</div>
          </div>
        ))}
      </div>
    </div>
    );
}

export default GetInventory