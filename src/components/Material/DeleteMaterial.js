import React from "react";

function DeleteMaterial (props)
{
    const handleMaterialId = async () => 
        {
          const url = `https://localhost:7055/api/Materials/${props.materialId}`
      
          const request = await fetch(url, {
            method: "DELETE",
            headers: {
              'Content-Type': 'application/json',
              "Authorization": `Bearer ${localStorage.getItem("jwt")}`
          },
          });
          if(!request.ok)
          {
             console.log("Error during deletion")
             return
          }
          const response = await request.json();
          props.handleDelete(props.materialId);
          console.log(response.message);
        }

    return (
      <div>
      <button
        className="btn btn-danger"
        onClick={handleMaterialId}
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

export default DeleteMaterial
