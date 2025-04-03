import React, { useState } from 'react';

function AddNewLocation({ closeForm, addLocation }) {
  const [locationData, setLocationData] = useState(
    {
      locationId: "",
      locationName: "",
      locationDescription: "",
      locationCapacity: "",
    });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setLocationData({ ...locationData, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const url = "https://localhost:7055/api/Locations";

    const request = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(locationData),
    });

    if (!request.ok) {
      console.log("Error");
      return;
    }

    const newLocation = await request.json();
    addLocation(newLocation);

    console.log("Location added successfully");
    closeForm();
  };

  return (
    <div className="add-form">
      <h3>Add New Location</h3>
      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
          <label>ID:</label>
            <input type="number" id="id" name="locationId" value={locationData.locationId} onChange={handleChange} className='form-control' placeholder='Location Id'/>
          <label>Name:</label>
            <input type="text" id="name" name="locationName" value={locationData.locationName} onChange={handleChange} className='form-control' placeholder='Location Name'/>
          <label>Description:</label>
            <input type="text" id="description" name="locationDescription" value={locationData.locationDescription} onChange={handleChange} className='form-control' placeholder='Location Description'/>
          <label>Capacity:</label>
            <input type="number" id="capacity" name="locationCapacity" value={locationData.locationCapacity} onChange={handleChange} className='form-control' placeholder='Location Capacity'/>

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
            max-width: 400px;
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

export default AddNewLocation;
