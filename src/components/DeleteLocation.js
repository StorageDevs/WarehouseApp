import React from "react";

function DeleteLocation (props)
{
    const handleLocationId = async () => 
        {
          const url = `http://localhost:7055/api/Locations
          ?id=${props.locationId}`
      
          const request = await fetch(url, {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
            },
          });
          if(!request.ok)
          {
             console.log("Error")
             return
          }
          const response = await request.json();
          props.handleCount();
          console.log(response.message);
        }

    return (
        <div>
            <button className="btn btn-danger" onClick={handleLocationId}>Delete</button>
        </div>
    )
}

export default DeleteLocation