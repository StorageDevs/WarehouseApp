import React, { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import * as THREE from "three";
import NET from "vanta/src/vanta.net";
import axios from "axios";

const RegisterPage = () => {
  const [userName, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const navigate = useNavigate();
  const vantaRef = useRef(null);

  useEffect(() => {
    const vantaEffect = NET({
      el: vantaRef.current,
      THREE,
      color: 0x00b894,
      backgroundColor: 0x2d3436,
      points: 12.0,
      maxDistance: 20.0,
      spacing: 18.0,
    });

    return () => {
      if (vantaEffect) vantaEffect.destroy();
    };
  }, []);

  const handleRegister = async () => {
    try {
      const response = await axios.post("https://localhost:7188/auth/Register", {
        userName,
        password,
        fullName,
        email,
      });
      const token = response.data.token;
      localStorage.setItem("jwt", token);
      setSuccess("Registration successful! Redirecting to login...");
      setTimeout(() => navigate("/login"), 2000);
      setError("");
      navigate("/home");
    } catch (error) {
      setError("Registration failed. Username might be taken.");
      setSuccess("");
    }
  }; 

  /* const handleRegister = async () =>{
        try {
            const token = localStorage.getItem('jwt');
            if(!token) {
                throw new Error('Not found JWT token!');
            }
            const response = await axios.post("https://localhost:7188/auth/Register", {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setSuccess(response.data);
        }
        catch(error) {
            setError('Registration failed. Username might be taken.');
        }
    }
    handleRegister();
    */

  return (
    <div ref={vantaRef} style={styles.vantaBackground}>
      <div style={styles.loginContainer}>
        <h1 style={styles.title}>StorageDevs WH Registration</h1>
        {error && <p style={styles.error}>{error}</p>}
        {success && <p style={styles.success}>{success}</p>}
        <div style={styles.inputGroup}>
          <label style={styles.label}>Username:</label>
          <input
            type="text"
            placeholder="Choose a username"
            value={userName}
            onChange={(e) => setUsername(e.target.value)}
            style={styles.input}
          />
        </div>
        <div style={styles.inputGroup}>
          <label style={styles.label}>Password:</label>
          <input
            type="password"
            placeholder="Choose a password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={styles.input}
          />
        </div>
        <div style={styles.inputGroup}>
          <label style={styles.label}>Full name:</label>
          <input
            type="text"
            placeholder="Full name"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            style={styles.input}
          />
        </div>
        <div style={styles.inputGroup}>
          <label style={styles.label}>Email:</label>
          <input
            type="text"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            style={styles.input}
          />
        </div>
        <button onClick={handleRegister} style={styles.button}>
          Register
        </button>
        <button
            onClick={() => navigate("/home")}
            style={{ ...styles.button, backgroundColor: "#6c757d", marginTop: "10px" }}
        >
            Cancel
        </button>
      </div>
    </div>
  );
};

const styles = {
  vantaBackground: {
    height: "100vh",
    width: "100vw",
    position: "fixed",
    top: 0,
    left: 0,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  loginContainer: {
    backgroundColor: "rgba(255, 255, 255, 0.9)",
    padding: "30px",
    borderRadius: "12px",
    boxShadow: "0 4px 10px rgba(0, 0, 0, 0.3)",
    textAlign: "center",
    width: "360px",
  },
  title: {
    color: "#00b894",
    marginBottom: "25px",
  },
  error: {
    color: "red",
    marginBottom: "15px",
  },
  success: {
    color: "green",
    marginBottom: "15px",
  },
  inputGroup: {
    marginBottom: "10px",
    width: "100%",
  },
  label: {
    display: "block",
    fontWeight: "bold",
    marginBottom: "5px",
    color: "#333",
  },
  input: {
    width: "100%",
    padding: "10px",
    borderRadius: "5px",
    border: "1px solid #ccc",
    fontSize: "16px",
  },
  button: {
    backgroundColor: "#00b894",
    color: "white",
    border: "none",
    padding: "10px 15px",
    borderRadius: "5px",
    cursor: "pointer",
    fontWeight: "bold",
    fontSize: "16px",
    width: "100%",
  },
};

export default RegisterPage;
