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
        headers: {
          'Content-Type': 'application/json',
          "Authorization": `Bearer ${localStorage.getItem("jwt")}`
      },
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

  const handleAddLocation = (newLocation) => {
    setLocationData((prevData) => [...prevData, newLocation]);
    setShowAddForm(false);
  };

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
              <AddNewLocation closeForm={closeAddForm} addLocation={handleAddLocation}/>
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
            overflow-y: auto;
            max-height: 100vh;
            scrollbar-width: none;
            -ms-overflow-style: none;
          }
          
          .container::-webkit-scrollbar {
          display: none;
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
            justify-content: center;
            overflow-y: auto;
            max-height: 80vh; 
            padding: 10px;
            scrollbar-width: none;
          }

          .card-container::-webkit-scrollbar {
          display: none;
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
