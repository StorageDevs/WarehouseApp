import React, { useState, useEffect } from "react";

function UpdateMaterial({ selectedMaterial, handleUpdate, closeEdit }) {
  const [updatedMaterial, setUpdatedMaterial] = useState(selectedMaterial || {});

  useEffect(() => {
    setUpdatedMaterial(selectedMaterial || {});
  }, [selectedMaterial]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setUpdatedMaterial({ ...updatedMaterial, [name]: value });
  };

  const handleMaterialData = async () => {
    const url = `https://localhost:7055/api/Materials/${updatedMaterial.materialId}`;

    const request = await fetch(url, {
      method: "PUT",
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem("jwt")}`
    },
      body: JSON.stringify(updatedMaterial),
    });

    if (!request.ok) {
      console.log("Error updating material");
      return;
    }

    console.log("Material updated successfully");
    handleUpdate(updatedMaterial.materialId, updatedMaterial);
    closeEdit();
  };

  return (
    <div className="edit-overlay">
      <div className="edit-form">
        <h3>Edit Material</h3>
        <label>Material Number:</label>
        <input type="number" name="materialNumber" value={updatedMaterial.materialNumber || ''} onChange={handleChange} className="form-control" />

        <label>Description:</label>
        <input type="text" name="materialDescription" value={updatedMaterial.materialDescription || ''} onChange={handleChange} className="form-control" />

        <label>Unit:</label>
        <input type="text" name="unit" value={updatedMaterial.unit || ''} onChange={handleChange} className="form-control" />

        <label>Price/Unit:</label>
        <input type="number" name="priceUnit" value={updatedMaterial.priceUnit || ''} onChange={handleChange} className="form-control" />

        <button onClick={handleMaterialData} className="btn btn-success">Save</button>
        <button onClick={closeEdit} className="btn btn-secondary" style={{ marginLeft: "10px" }}>Cancel</button>
      </div>

      <style>
        {`
          .edit-overlay {
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

          .edit-form {
            background: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.2);
            width: 90%;
            max-width: 400px;
          }

          @media (max-width: 600px) {
            .edit-form {
              width: 95%;
            }
          }
        `}
      </style>
    </div>
  );
}

export default UpdateMaterial;

