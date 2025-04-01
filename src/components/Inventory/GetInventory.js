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
              width: 300,
              height: "auto",
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
              position: "relative",
            }}
            key={inventory.inventoryId}
          >
          <div className="card-body">Number: {inventory.materialNumber}<br />
          Description: {inventory.materialDescription}<br />
          From: {inventory.locationFrom}<br />
          To: {inventory.locationTo}<br />
          Quantity: {inventory.transferQuantity}<br />
          User: {inventory.user}<br />
          Time: {inventory.transactionTime}</div>
          </div>
        ))}
      </div>
    </div>
    );
}

export default GetInventory