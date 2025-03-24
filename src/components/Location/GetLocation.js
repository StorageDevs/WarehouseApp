import React, { useState } from "react";

const LocationForm = ({ onAddLocation }) => {
  const [formData, setFormData] = useState({
    id: "",
    name: "",
    description: "",
    capacity: ""
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (onAddLocation) {
      onAddLocation({
        id: Date.now(),
        ...formData,
        capacity: parseInt(formData.capacity, 10),
      });
    }
    setFormData({ id: "", name: "", description: "", capacity: "" });
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
      <input type="number" name="id" placeholder="ID" value={formData.id} onChange={handleChange} required />
      <input type="text" name="name" placeholder="Name" value={formData.name} onChange={handleChange} required />
      <input type="text" name="description" placeholder="Description" value={formData.description} onChange={handleChange} required />
      <input type="number" name="capacity" placeholder="Capacity" value={formData.capacity} onChange={handleChange} required />
      <button type="submit">Add title</button>
    </form>
  );
};

export default LocationForm;