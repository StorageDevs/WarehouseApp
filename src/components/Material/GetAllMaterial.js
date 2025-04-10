import React, { useEffect, useState } from "react";
import DeleteMaterial from "./DeleteMaterial";
import UpdateMaterial from "./UpdateMaterial";
import AddNewMaterial from "./AddNewMaterial";

function GetAllMaterial() {
  const url = "https://localhost:7055/api/Materials";
  const [materialData, setMaterialData] = useState([]);
  const [selectedMaterial, setSelectedMaterial] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [showAddForm, setShowAddForm] = useState(false); 

  useEffect(() => {
    (async () => {
      const request = await fetch(url, {
        method: "GET",
        headers: {
          'Content-Type': 'application/json',
          "Authorization": `Bearer ${localStorage.getItem("jwt")}`
      },
      });

      if (!request.ok) {
        console.log("Error");
        return;
      }

      const response = await request.json();
      setMaterialData(response.result);
      console.log(response.message);
    })();
  }, []);

  const handleAddMaterial = (newMaterial) => {
    setMaterialData((prevData) => [...prevData, newMaterial]);
    setShowAddForm(false);
  };

  const handleUpdate = (id, updatedMaterial) => {
    setMaterialData(materialData.map(mat => mat.materialId === id ? updatedMaterial : mat));
    setSelectedMaterial(null);
    setIsEditing(false);
  };

  const handleEditClick = (material) => {
    setSelectedMaterial(material);
    setIsEditing(true);
  };

  const handleAddNewMaterial = () => {
    setShowAddForm(true); 
  };

  const closeAddForm = () => {
    setShowAddForm(false); 
  };

  return (
    <div className="container">
      {isEditing ? (
        <UpdateMaterial
          selectedMaterial={selectedMaterial}
          handleUpdate={handleUpdate}
          closeEdit={() => setIsEditing(false)}
        />
      ) : (
        <>
          {showAddForm ? (
            <div className="overlay">
              <AddNewMaterial closeForm={closeAddForm} addMaterial={handleAddMaterial}/>
            </div>
          ) : (
            <>
              <h2 className="title">Materials</h2>
              <div className="button-container">
                <button onClick={handleAddNewMaterial} className="btn btn-primary">
                  Add New Material
                </button>
              </div>
              <div className="card-container">
                {materialData.map((material) => (
                  <div className="card" key={material.materialId}>
                    <div className="card-body">
                      <p><strong>Material Number:</strong> {material.materialNumber}</p>
                      <p><strong>Description:</strong> {material.materialDescription}</p>
                      <p><strong>Unit:</strong> {material.unit}</p>
                      <p><strong>Price/unit:</strong> {material.priceUnit}</p>
                    </div>
                    <DeleteMaterial materialId={material.materialId} handleDelete={(id) => setMaterialData(materialData.filter(m => m.materialId !== id))} />
                    <button onClick={() => handleEditClick(material)} className="btn btn-warning">Modify</button>
                  </div>
                ))}
              </div>
            </>
          )}
        </>
      )}

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
              max-width: 100%;
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

export default GetAllMaterial;
