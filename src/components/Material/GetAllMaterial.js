import React, { useEffect, useState } from 'react'

function GetAllMaterial() {
  const url = "https://localhost:7055/api/Materials"
  const [materialData, setMaterialData] = useState([]);

  useEffect(() => 
  {
    (async () => 
      {
      const request = await fetch(url, {
       
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
      setMaterialData(response.result);
      console.log(response.message);
      })()
    }, []);

    return (
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px" }}>
        {materialData.map((material) => (
          <div
            className="card"
            style={{
              width: 200,
              margin: 10,
              padding: 10,
              border: "1px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
            }}
            key={material.materialId}
          >
            <div className="card-body">Material Number: {material.materialNumber}</div>
            <div className="card-body">Description: {material.materialDescription}</div>
            <div className="card-body">Unit: {material.unit}</div>
            <div className="card-body">Priceunit: {material.priceUnit}</div>
          </div>
        ))}
      </div>
    );
}

export default GetAllMaterial