/*import React, { useEffect, useState } from 'react'
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

export default GetLocation */

import React, { useEffect, useState } from "react";
import DeleteLocation from "./DeleteLocation";
import UpdateLocation from "./UpdateLocation";
import AddNewLocation from "./AddNewLocation";

function GetLocation() {
  const url = "https://localhost:7055/api/Locations";
  const [locationData, setLocationData] = useState([]);
  const [selectedLocation, setSelectedLocation] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [showAddForm, setShowAddForm] = useState(false); 

  useEffect(() => {
    (async () => {
      const request = await fetch(url, {
        method: "GET",
        headers: { "Content-Type": "application/json" }
      });

      if (!request.ok) {
        console.log("Error");
        return;
      }

      const response = await request.json();
      setLocationData(response.result);
      console.log(response.message);
    })();
  }, []);

  const handleUpdate = (id, updatedLocation) => {
    setLocationData(locationData.map(loc => loc.locationId === id ? updatedLocation : loc));
    setSelectedLocation(null);
    setIsEditing(false);
  };

  const handleEditClick = (location) => {
    setSelectedLocation(location);
    setIsEditing(true);
  };

  const handleAddNewLocation = () => {
    setShowAddForm(true); 
  };

  const closeAddForm = () => {
    setShowAddForm(false); 
  };

  return (
    <div className="container">
      {isEditing ? (
        <UpdateLocation
          selectedLocation={selectedLocation}
          handleUpdate={handleUpdate}
          closeEdit={() => setIsEditing(false)}
        />
      ) : (
        <>
          {showAddForm ? (
            <div className="overlay">
              <AddNewLocation closeForm={closeAddForm} />
            </div>
          ) : (
            <>
              <h2 className="title">Locations</h2>
              <div className="button-container">
                <button onClick={handleAddNewLocation} className="btn btn-primary">
                  Add New Location
                </button>
              </div>
              <div className="card-container">
                {locationData.map((location) => (
                  <div className="card" key={location.locationId}>
                    <div className="card-body">
                      <p><strong>ID:</strong> {location.locationId}</p>
                      <p><strong>Name:</strong> {location.locationName}</p>
                      <p><strong>Description:</strong> {location.locationDescription}</p>
                      <p><strong>Capacity:</strong> {location.locationCapacity}</p>
                    </div>
                    <DeleteLocation locationId={location.locationId} handleDelete={(id) => setLocationData(locationData.filter(l => l.locationId !== id))} />
                    <button onClick={() => handleEditClick(location)} className="btn btn-warning">Modify</button>
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
            justify-content: left;
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

export default GetLocation;
