import React from "react";

function DeleteMaterial (props)
{
    const handleMaterialId = async () => 
        {
          const url = `https://localhost:7055/api/Materials
          ?id=${props.materialId}`
      
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
            <button className="btn btn-danger" onClick={handleMaterialId}>Delete</button>
        </div>
    )
}

export default DeleteMaterial
