import React, { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import * as THREE from "three";
import NET from "vanta/src/vanta.net";
import axios from "axios";

const LoginPage = () => {
  const [userName, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const vantaRef = useRef(null);

  useEffect(() => {
    const vantaEffect = NET({
      el: vantaRef.current,
      THREE,
      color: 0x007bff, 
      backgroundColor: 0x0f3460, 
      points: 10.0,
      maxDistance: 18.0,
      spacing: 15.0,
    });

    return () => {
      if (vantaEffect) vantaEffect.destroy();
    };
  }, []);

  const handleLogin = async () => {
    try {
      const response = await axios.post("https://localhost:7188/auth/Login", {
        userName,
        password,
      });
  
      const Token = response.data.token;
  
      if (Token) {
        localStorage.setItem("jwt", Token);
        window.location.href = "/home";
      } else {
        setError("Error during authorization");
      }
    } catch (error) {
      setError("Wrong username or password!!");
    }
  }; 

  return (
    <div ref={vantaRef} style={styles.vantaBackground}>
      <div style={styles.loginContainer}>
        <h1 style={styles.title}>StorageDevs WH Login Panel</h1>
        {error && <p style={styles.error}>{error}</p>}
        <div style={styles.inputGroup}>
          <label style={styles.label}>Username:</label>
          <input
            type="text"
            placeholder="Username"
            value={userName}
            onChange={(e) => setUsername(e.target.value)}
            style={styles.input}
          />
        </div>
        <div style={styles.inputGroup}>
          <label style={styles.label}>Password:</label>
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={styles.input}
          />
        </div>
        <button onClick={handleLogin} style={styles.button}>
          Login
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
    backgroundColor: "rgba(199, 223, 228, 0.84)", 
    padding: "30px",
    borderRadius: "12px",
    boxShadow: "0 4px 10px rgba(24, 22, 22, 0.3)",
    textAlign: "center",
    width: "360px",
  },
  title: {
    color: "#007bff",
    marginBottom: "25px",
  },
  error: {
    color: "red",
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
    padding: "8px",
    borderRadius: "5px",
    border: "1px solid #ccc",
    fontSize: "16px",
  },
  button: {
    backgroundColor: "#007bff",
    color: "white",
    border: "none",
    padding: "10px 15px",
    borderRadius: "8px",
    cursor: "pointer",
    fontWeight: "bold",
    fontSize: "16px",
    width: "100%",
  },
};

export default LoginPage;