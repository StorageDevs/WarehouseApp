import React, { useState } from 'react';

function AddNewMaterial({ closeForm, addMaterial }) {
  const [materialData, setMaterialData] = useState({
    materialNumber: "",
    materialDescription: "",
    unit: "",
    priceUnit: ""
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setMaterialData({ ...materialData, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const url = "https://localhost:7055/api/Materials";

    const request = await fetch(url, {
      method: "POST",
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem("jwt")}`
    },
      body: JSON.stringify(materialData),
    });

    if (!request.ok) {
      console.log("Error");
      return;
    }

    const newMaterial = await request.json();
    addMaterial(newMaterial);

    console.log("Material added successfully");
    closeForm();
  };

  return (
    <div className="add-form">
      <h3>Add New Material</h3>
      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
        <label>Material Number:</label>
        <input type="number" name="materialNumber" value={materialData.materialNumber} onChange={handleChange} className="form-control" />

        <label>Description:</label>
        <input type="text" name="materialDescription" value={materialData.materialDescription} onChange={handleChange} className="form-control" />

        <label>Unit:</label>
        <input type="text" name="unit" value={materialData.unit} onChange={handleChange} className="form-control" />

        <label>Price/Unit:</label>
        <input type="number" name="priceUnit" value={materialData.priceUnit} onChange={handleChange} className="form-control" />

        <button type="submit" className="btn btn-success">Submit</button>
        <button type="button" onClick={closeForm} className="btn btn-secondary">Cancel</button>
      </form>

      <style>
        {`
          .add-form {
            background: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.2);
            width: 90%;
            max-width: 350px;
          }

          @media (max-width: 600px) {
            .add-form {
              width: 95%;
            }
          }
        `}
      </style>
    </div>
  );
}

export default AddNewMaterial;
