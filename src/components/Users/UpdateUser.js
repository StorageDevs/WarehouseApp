import React, { useState, useEffect } from "react";

function UpdateUser({ selectedUser, handleUpdate, closeEdit }) {
  const [updatedUser, setUpdatedUser] = useState(selectedUser || {});

  useEffect(() => {
    setUpdatedUser(selectedUser || {});
  }, [selectedUser]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setUpdatedUser({ ...updatedUser, [name]: value });
  };

  const handleUserData = async () => {
    const url = `https://localhost:7055/api/Users/${updatedUser.userId}`;

    const request = await fetch(url, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(updatedUser),
    });

    if (!request.ok) {
      console.log("Error updating user");
      return;
    }

    console.log("User updated successfully");
    handleUpdate(updatedUser.userId, updatedUser);
    closeEdit();
  };

  return (
    <div className="edit-overlay">
      <div className="edit-form">
        <h3>Edit User</h3>
        <label>User ID:</label>
        <input type="number" name="userId" value={updatedUser.userId || ''} onChange={handleChange} className="form-control" />

        <label>Username:</label>
        <input type="text" name="userName" value={updatedUser.userName || ''} onChange={handleChange} className="form-control" />

        <label>Password:</label>
        <input type="text" name="password" value={updatedUser.password || ''} onChange={handleChange} className="form-control" />

        <label>Salt:</label>
        <input type="text" name="salt" value={updatedUser.salt || ''} onChange={handleChange} className="form-control" />

        <label>Hash:</label>
        <input type="text" name="hash" value={updatedUser.hash || ''} onChange={handleChange} className="form-control" />

        <label>Role:</label>
        <input type="number" name="role" value={updatedUser.role || ''} onChange={handleChange} className="form-control" />

        <button onClick={handleUserData} className="btn btn-success">Save</button>
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

export default UpdateUser;
