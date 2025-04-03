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
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px", justifyContent: "center"}}>
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
          <div className="card-body">
          <p><strong>Mat. Id:</strong> {inventory.materialId}</p><br />
          <p><strong>Mat. Number:</strong> {inventory.materialNumber}</p><br />
          <p><strong>Description:</strong> {inventory.materialDescription}</p><br />
          <p><strong>Loc. Id:</strong> {inventory.locationId}</p><br />
          <p><strong>Loc. Name:</strong> {inventory.locationName}</p><br />
          <p><strong>Quantity:</strong> {inventory.quantity}</p></div>
          </div>
        ))}
      </div>
      <style>
        {`
          .container {
            max-width: 1200px;
            margin: auto;
            padding: 20px;
            overflow-y: auto;
            max-height: 100vh;
            scrollbar-width: none;
            -ms-overflow-style: none;
          }
          
          .container::-webkit-scrollbar {
           display: none;
          }

          .title {
            text-align: center;
            margin-bottom: 20px;
          }

          .button-container {
            display: flex;
            justify-content: center;
            margin-bottom: 20px;
          }

          .card-container {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            justify-content: center;
            overflow-y: auto;
            max-height: 80vh; 
            padding: 10px;
            scrollbar-width: none;
          }

          .card-container::-webkit-scrollbar {
          display: none;
          }

          .card {
            width: 100%;
            max-width: 300px;
            padding: 15px;
            border: 2px solid #ccc;
            border-radius: 8px;
            box-shadow: 2px 2px 10px rgba(0,0,0,0.1);
            text-align: center;
          }

          @media (max-width: 768px) {
            .card {
              max-width: 90%;
            }
          }

          .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            display: flex;
            align-items: center;
            justify-content: center;
          }
        `}
      </style>
    </div>
    );
}

export default GetInventory