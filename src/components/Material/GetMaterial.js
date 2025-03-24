import React, { useState } from "react";

const MaterialForm = ({ onAddMaterial }) => {
  const [formData, setFormData] = useState({
    id: "",
    name: "",
    description: "",
    quantity: "",
    unit: "",
    location: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (onAddMaterial) {
      onAddMaterial({
        id: Date.now(),
        ...formData,
        quantity: parseInt(formData.quantity, 10),
      });
    }
    setFormData({ id: "",name: "", description: "", quantity: "", unit: "", location: "" });
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
      <input type="number" name="id" placeholder="ID" value={formData.id} onChange={handleChange} required />
      <input type="text" name="name" placeholder="Name" value={formData.name} onChange={handleChange} required />
      <input type="text" name="description" placeholder="Description" value={formData.description} onChange={handleChange} required />
      <input type="number" name="quantity" placeholder="Quantity" value={formData.quantity} onChange={handleChange} required />
      <input type="text" name="unit" placeholder="Unit" value={formData.unit} onChange={handleChange} required />
      <input type="text" name="location" placeholder="Location" value={formData.location} onChange={handleChange} required />
      <button type="submit">Add title</button>
    </form>
  );
};

export default MaterialForm;
