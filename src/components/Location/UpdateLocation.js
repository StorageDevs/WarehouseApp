import React, { useState, useEffect } from "react";

function UpdateLocation({ selectedLocation, handleUpdate, closeEdit }) {
  const [updatedLocation, setUpdatedLocation] = useState(selectedLocation || {});

  useEffect(() => {
    setUpdatedLocation(selectedLocation || {});
  }, [selectedLocation]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setUpdatedLocation({ ...updatedLocation, [name]: value });
  };

  const handleLocationData = async () => {
    const url = `https://localhost:7055/api/Locations/${updatedLocation.locationId}`;

    const request = await fetch(url, {
      method: "PUT",
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem("jwt")}`
    },
      body: JSON.stringify(updatedLocation),
    });

    if (!request.ok) {
      console.log("Error updating location");
      return;
    }

    console.log("Location updated successfully");
    handleUpdate(updatedLocation.locationId, updatedLocation);
    closeEdit();
  };

  return (
    <div className="edit-overlay">
      <div className="edit-form">
        <h3>Edit Location</h3>

        <label>Name:</label>
        <input type="text" name="locationName" value={updatedLocation.locationName || ''} onChange={handleChange} className="form-control" />

        <label>Description:</label>
        <input type="text" name="locationDescription" value={updatedLocation.locationDescription || ''} onChange={handleChange} className="form-control" />

        <label>Capacity:</label>
        <input type="number" name="locationCapacity" value={updatedLocation.locationCapacity || ''} onChange={handleChange} className="form-control" />

        <button onClick={handleLocationData} className="btn btn-success">Save</button>
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

export default UpdateLocation;
