import React from "react";

function DeleteLocation (props)
{
    const handleLocationId = async () => 
        {
          const url = `https://localhost:7055/api/Locations/${props.locationId}`
      
          const request = await fetch(url, {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
            },
          });
          if(!request.ok)
          {
             console.log("Error during deletion")
             return
          }
          const response = await request.json();
          props.handleDelete(props.locationId);
          console.log(response.message);
        }

    return (
      <div>
      <button
        className="btn btn-danger"
        onClick={handleLocationId}
        style={{
          position: "absolute",
          top: "0px",
          right: "5px",
          backgroundColor: "transparent",
          border: "none",
          fontSize: "20px",
          cursor: "pointer",
          color: "red",
        }}
      >
        x
      </button>
    </div>
    )
}

export default DeleteLocation