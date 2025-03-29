import React, { useEffect, useState } from 'react'

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

    return (
    <div>
      <br></br>
        <h2 style={{ textAlign: "center", marginBottom: "20px", padding: "10px" }}>Locations</h2>
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px" }}>
        {locationData.map((location) => (
          <div
            className="card"
            style={{
              width: 450,
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
            }}
            key={location.locationId}
          >
            <div className="card-body">ID: {location.locationId}</div>
            <div className="card-body">Name: {location.locationName}</div>
            <div className="card-body">Description: {location.locationDescription}</div>
            <div className="card-body">Capacity: {location.locationCapacity}</div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default GetLocation