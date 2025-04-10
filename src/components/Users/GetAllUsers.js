/* import React, { useEffect, useState } from "react";
import DeleteUser from "./DeleteUser";
import { jwtDecode } from "jwt-decode";

function GetAllUser() {
  const url = "https://localhost:7188/auth/GetAllUser";
  const [userData, setUserData] = useState([]);
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("jwt");
    if (token) {
      const decoded = jwtDecode(token);
      const roles = Array.isArray(decoded.role) ? decoded.role : [decoded.role];
      if (roles.includes("admin")) {
        setIsAdmin(true);
        fetchUsers(token);
      }
    }
  }, []);

  const fetchUsers = async (token) => {
    const request = await fetch(url, {
      method: "GET",
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${token}`
      },
    });

    if (!request.ok) {
      console.log("Error");
      return;
    }

    const response = await request.json();
    setUserData(response.result);
  };

  const assignRole = async (userName, roleName) => {
    const token = localStorage.getItem("jwt");
    const response = await fetch("https://localhost:7188/auth/AssignRole", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify({ userName, roleName })
    });

    const data = await response.json();
    alert(data.message);
  };

  const removeRole = async (userName, roleName) => {
    const token = localStorage.getItem("jwt");
    const response = await fetch("https://localhost:7188/auth/RemoveRole", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify({ userName, roleName })
    });

    const data = await response.json();
    alert(data.message);
  };

  if (!isAdmin) {
    return (
      <div style={{ textAlign: "center", marginTop: "50px" }}>
        <h2>Access Denied.</h2>
        <p>Ehhez az oldalhoz csak adminisztrátor férhet hozzá.</p>
      </div>
    );
  }

  return (
    <div className="container">
      <h2 className="title">Users</h2>
      <div className="card-container">
        {userData.map((user) => (
          <div className="card" key={user.userID}>
            <div className="card-body">
              <p><strong>Username:</strong> {user.userName}</p>
              <p><strong>Email:</strong> {user.email}</p>
              <p><strong>Role:</strong> {user.role?.join(", ") || "None"}</p>
            </div>
            <div className="role-buttons">
              <button onClick={() => assignRole(user.userName, "")}>Assign Role</button>
              <button onClick={() => removeRole(user.userName, "")}>Remove Role</button>
            </div>
            <DeleteUser
              userId={user.userID}
              handleDelete={(id) => setUserData(userData.filter(u => u.userID !== id))}
            />
          </div>
        ))}
      </div>
    </div>
  );
}

export default GetAllUser; */

import React, { useEffect, useState } from "react";
import DeleteUser from "./DeleteUser";

function GetAllUser() {
  const url = "https://localhost:7188/auth/GetAllUser";
  const [userData, setUserData] = useState([]);

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
      setUserData(response.result);
      console.log(response);
    })();
  }, []);

  const assignRole = async (userName, roleName) => {
    try {
      const response = await fetch("https://localhost:7188/auth/Assignrole", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${localStorage.getItem("jwt")}`
        },
        body: JSON.stringify({ userName, roleName })
      });

      const data = await response.json();
      alert(data.message);
    } catch (error) {
      console.error("Error assigning role:", error);
    }
  };

  const removeRole = async (userName, roleName) => {
    try {
      const response = await fetch("https://localhost:7188/auth/Removerole", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${localStorage.getItem("jwt")}`
        },
        body: JSON.stringify({ userName, roleName })
      });

      const data = await response.json();
      alert(data.message);
    } catch (error) {
      console.error("Error removing role:", error);
    }
  };

  return (
    <div className="container">
      <>
        <h2 className="title">Users</h2>
        <div className="button-container"></div>
        <div className="card-container">
          {userData.map((user) => (
            <div className="card" key={user.userID}>
              <div className="card-body">
                <p><strong>Username:</strong> {user.userName}</p>
                <p><strong>Email:</strong> {user.email}</p>
                <p><strong>Role:</strong> {user.role?.join(", ") || "None"}</p>
              </div>
              <div className="role-buttons">
                <button onClick={() => assignRole(user.userName, "")}>Assign Role</button>
                <button onClick={() => removeRole(user.userName, "")}>Remove Role</button>
              </div>
              <DeleteUser
                userId={user.userID}
                handleDelete={(id) => setUserData(userData.filter(u => u.userID !== id))}
              />
            </div>
          ))}
        </div>
      </>

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

          .card-container {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            justify-content: center;
            overflow-y: auto;
            max-height: 80vh;
            padding: 10px;
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

          .role-buttons {
            margin: 10px 0;
          }

          .role-buttons button {
            margin: 5px;
            padding: 6px 10px;
            border: none;
            border-radius: 6px;
            background-color: #007bff;
            color: white;
            cursor: pointer;
          }

          .role-buttons button:hover {
            background-color: #0056b3;
          }
        `}
      </style>
    </div>
  );
}

export default GetAllUser;

