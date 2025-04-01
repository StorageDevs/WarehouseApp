import React, { useEffect, useState } from 'react'
import DeleteLocation from './DeleteLocation';
import UpdateLocation from './UpdateLocation';
import AddNewLocation from './AddNewLocation';

function GetLocation() {
  const url = "https://localhost:7055/api/Locations"
  const [locationData, setLocationData] = useState([]);
  
  useEffect(() => 
  {
    (async () => 
      {
      const request = await fetch(url, {
        method: "GET",
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
      setLocationData(response.result);
      console.log(response.message);
      })()
    }, []);

    const handleDelete = (id) => {
      setLocationData(locationData.filter(location => location.locationId !== id));
    };
  
    const handleUpdate = (id) => {
      setLocationData(locationData.filter(location => location.locationId !== id));
    };

    return (
    <div>
      <h2 style={{ textAlign: "center", marginBottom: "20px", padding: "10px" }}>Locations</h2>
      <br></br>
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px" }}>
        {locationData.map((location) => (
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
              position: 'relative',
            }}
            key={location.locationId}
          >
            <div className="card-body">ID: {location.locationId}<br />
            Name: {location.locationName}<br />
            Description: {location.locationDescription}<br />
            Capacity: {location.locationCapacity}</div>
            <DeleteLocation locationId={location.locationId} handleDelete={handleDelete} />
            <UpdateLocation locationId={location.locationId} handleUpdate={handleUpdate} />
          </div>
        ))}
      </div>
      <AddNewLocation handleCount={handleUpdate} />
    </div>
  );
}

export default GetLocation