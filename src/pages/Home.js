import React, { useState, useEffect } from "react";

const HomePage = () => {
  const [logoutMessage, setLogoutMessage] = useState("");
  const [showMessage, setShowMessage] = useState(false);

  useEffect(() => {
    const wasLoggedOut = localStorage.getItem("logoutSuccess");
    if (wasLoggedOut) {
      
      setShowMessage(true);
      localStorage.removeItem("logoutSuccess");
      setLogoutMessage("Successful logout.");

      const timer = setTimeout(() => {
        setShowMessage(false); 
      }, 500);

      const removeTimer = setTimeout(() => {
        setLogoutMessage("");
      }, 3500);
  
      return () => {
        clearTimeout(timer);
        clearTimeout(removeTimer);
      };
    }
  }, []);

  return (
    <div style={styles.container}>
      <video autoPlay loop muted style={styles.videoBackground}>
        <source src="/video2.mp4" type="video/mp4" />
        Your browser does not support the video tag.
      </video>
      
      <div style={{ padding: "20px" }}>
      {logoutMessage && (
        <div
          style={{
            position: "absolute",
            top: "30px",
            right: "30px",
            backgroundColor: "rgba(83, 154, 235, 0.9)",
            color: "white",
            padding: "10px 20px",
            borderRadius: "5px",
            boxShadow: "0 2px 6px rgba(0,0,0,0.1)",
            fontWeight: "bold",
            zIndex: 1000,
            opacity: showMessage ? 1 : 0,
            transition: "opacity 1s ease-in-out",
          }}
        >
          {logoutMessage}
        </div>
      )}
      <div style={styles.card}>
        <h1>Welcome to the StorageDevs Warehouse Management System</h1>
        <br />
        <p>
        This platform allows you to efficiently manage your inventory. Use the "Material" and "Location" tabs to add, modify, or delete items. Check the "Inventory" tab to see the current stock. The "Transaction" tab helps you track all inventory movements.

        If you have a user account, please log in using the "Login" tab. 

        If you don’t have a user account yet, please go to the "Register" tab to create one.
        </p>
        <p>
          For further assistance, navigate through the options in the top menu.
        </p>
      </div>
    </div>
    </div>
  );
};

const styles = {
  container: {
    position: "relative",
    display: "flex",
    height: "100vh",
    justifyContent: "center",
    alignItems: "center",
    overflow: "hidden",
  },
  videoBackground: {
    position: "absolute",
    top: "50%",
    left: "50%",
    width: "100%",
    height: "100%",
    objectFit: "cover",
    transform: "translate(-50%, -50%)",
    zIndex: "-1",
    yIndex: "0",
  },
  card: {
    backgroundColor: "rgba(65, 126, 196, 0.9)",
    padding: "80px",
    borderRadius: "12px",
    boxShadow: "0 4px 15px rgba(0, 0, 0, 0.2)",
    textAlign: "center",
    maxWidth: "800px",
    width: "100%",
    zIndex: "1",
  },
};

export default HomePage;
