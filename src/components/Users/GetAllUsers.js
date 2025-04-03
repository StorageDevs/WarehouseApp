import React, { useEffect, useState } from "react";
import DeleteUser from "./DeleteUser";
import UpdateUser from "./UpdateUser";
import AddNewUser from "./AddNewUser";

function GetAllUser() {
  const url = "https://localhost:7055/api/Users";
  const [userData, setUserData] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
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
      setUserData(response);
      console.log(response);
    })();
  }, []);

  const handleAddUser = (newUser) => {
    setUserData((prevData) => [...prevData, newUser]);
    setShowAddForm(false); 
  };

  const handleUpdate = (id, updatedUser) => {
    setUserData(userData.map(user => user.userId === id ? updatedUser : user));
    setSelectedUser(null);
    setIsEditing(false);
  };

  const handleEditClick = (user) => {
    setSelectedUser(user);
    setIsEditing(true);
  };

  const handleAddNewUser = () => {
    setShowAddForm(true); 
  };

  const closeAddForm = () => {
    setShowAddForm(false); 
  };

  return (
    <div className="container">
      {isEditing ? (
        <UpdateUser
          selectedUser={selectedUser}
          handleUpdate={handleUpdate}
          closeEdit={() => setIsEditing(false)}
        />
      ) : (
        <>
          {showAddForm ? (
            <div className="overlay">
              <AddNewUser closeForm={closeAddForm} addUser={handleAddUser}/>
            </div>
          ) : (
            <>
              <h2 className="title">Users</h2>
              <div className="button-container">
                <button onClick={handleAddNewUser} className="btn btn-primary">
                  Add New User
                </button>
              </div>
              <div className="card-container">
                {userData.map((user) => (
                  <div className="card" key={user.userId}>
                    <div className="card-body">
                      <p><strong>UserId:</strong> {user.userId}</p>
                      <p><strong>Username:</strong> {user.userName}</p>
                      <p><strong>Password:</strong> {user.password}</p>
                      <p><strong>Salt:</strong> {user.salt}</p>
                      <p><strong>Hash:</strong> {user.hash}</p>
                      <p><strong>Role:</strong> {user.role}</p>
                    </div>
                    <DeleteUser userId={user.userId} handleDelete={(id) => setUserData(userData.filter(u => u.userId !== id))} />
                    <button onClick={() => handleEditClick(user)} className="btn btn-warning">Modify</button>
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

export default GetAllUser;
