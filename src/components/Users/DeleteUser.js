import React from "react";

function DeleteUser (props)
{
    const handleUserId = async () => 
        {
          const url = `https://localhost:7188/auth/DeleteUser`
      
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
          props.handleDelete(props.userId);
          console.log(response.message);
        }

    return (
      <div>
      <button
        className="btn btn-danger"
        onClick={handleUserId}
        style={
          {
            position: "absolute",
            top: "0px",
            right: "5px",
            backgroundColor: "transparent",
            border: "none",
            fontSize: "20px",
            cursor: "pointer",
            color: "red",
          }
        }
      >
        x
      </button>
    </div>
    )
}

export default DeleteUser
