import React, { useState } from 'react';

function AddNewUser({ closeForm, addUser}) {
  const [userData, setUserData] = useState({
    userName: "",
    fullName: "",
    password: "",
    email: "",
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setUserData({ ...userData, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const url = "https://localhost:7188/auth/Register";

    const request = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userData),
    });

    if (!request.ok) {
      console.log("Error");
      return;
    }

    const newUser = await request.json();
    addUser(newUser);

    console.log("User added successfully");
    closeForm();
  };

  return (
    <div className="add-form">
      <h3>Add New User</h3>
      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>

        <label>Username:</label>
        <input type="text" name="userName" value={userData.userName} onChange={handleChange} className="form-control" />

        <label>Password:</label>
        <input type="text" name="password" value={userData.password} onChange={handleChange} className="form-control" />

        <label>Full Name:</label>
        <input type="text" name="fullName" value={userData.fullName} onChange={handleChange} className="form-control" />

        <label>Email:</label>
        <input type="email" name="email" value={userData.email} onChange={handleChange} className="form-control" />

        <button type="submit" className="btn btn-success">Submit</button>
        <button type="button" onClick={closeForm} className="btn btn-secondary">Cancel</button>
      </form>

      <style>
        {`
          .add-form {
            background: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.2);
            width: 90%;
            max-width: 400px;
          }

          @media (max-width: 600px) {
            .add-form {
              width: 95%;
            }
          }
        `}
      </style>
    </div>
  );
}

export default AddNewUser;
