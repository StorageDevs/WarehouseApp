import React, { useEffect, useState } from 'react'
import DeleteMaterial from './DeleteMaterial';
import UpdateMaterial from './UpdateMaterial';
import AddNewMaterial from './AddNewMaterial';

function GetAllMaterial(props) {
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

    const handleDelete = (id) => {
      setMaterialData(materialData.filter(location => location.locationId !== id));
    };
  
    const handleUpdate = (id) => {
      setMaterialData(materialData.filter(location => location.locationId !== id));
    };

    return (
    <div>
      <br></br>
        <h2 style={{ textAlign: "center", marginBottom: "20px", padding: "10px" }}>Materials</h2>
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px" }}>
        {materialData.map((material) => (
          <div
            className="card"
            style={{
              width: 200,
              height: "auto",
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
              position: "relative",
            }}
            key={material.materialId}
          >
            <div className="card-body">Material Number: {material.materialNumber}<br></br>
            Description: {material.materialDescription}<br></br>
            Unit: {material.unit}<br></br>
            Priceunit: {material.priceUnit}</div>
            <DeleteMaterial materialId={material.materialId} handleDelete={handleDelete} />
            <UpdateMaterial materialId={material.materialId} handleUpdate={handleUpdate} />
          </div>
        ))}
      </div>
      <AddNewMaterial handleCount={handleUpdate} />
    </div>
  );
}

export default GetAllMaterial