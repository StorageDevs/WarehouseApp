import React, { useState } from "react";

const InventoryForm = ({ onAddInventory }) => {
  const [formData, setFormData] = useState({
    materialId: "",
    materialNumber: "",
    materialDescription: "",
    locationId: "",
    locationName: "",
    inventoryQuantity: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (onAddInventory) {
      onAddInventory({
        id: Date.now(),
        ...formData,
      });
    }
    setFormData({ materialId: "", materialNumber: "", materialDescription: "", locationId: "", locationName: "", inventoryQuantity: "" });
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
      <input type="number" name="materialId" placeholder="materialId" value={formData.materialId} onChange={handleChange} required />
      <input type="number" name="materialNumber" placeholder="materialNumber" value={formData.materialNumber} onChange={handleChange} required />
      <input type="text" name="materialDescription" placeholder="materialDescription" value={formData.materialDescription} onChange={handleChange} required />
      <input type="number" name="locationId" placeholder="locationId" value={formData.locationId} onChange={handleChange} required />
      <input type="text" name="locationName" placeholder="locationName" value={formData.locationName} onChange={handleChange} required />
      <input type="number" name="inventoryQuantity" placeholder="inventoryQuantity" value={formData.inventoryQuantity} onChange={handleChange} required />
    </form>
  );
};

export default InventoryForm;