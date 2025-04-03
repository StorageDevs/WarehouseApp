import React from "react";

const HomePage = () => {
  return (
    <div style={styles.container}>
      <video autoPlay loop muted style={styles.videoBackground}>
        <source src="/video2.mp4" type="video/mp4" />
        Your browser does not support the video tag.
      </video>

      <div style={styles.card}>
        <h1>Welcome to the StorageDevs Warehouse Management System</h1>
        <br />
        <p>
        This platform allows you to efficiently manage your inventory. Use the "Material" and "Location" tabs to add, modify, or delete items. Check the "Inventory" tab to see the current stock. The "Transaction" tab helps you track all inventory movements.

        If you are logged in as an admin, you will also have access to the "Users" tab, where you can manage user accounts.

        If you don’t have a user account yet, please go to the "Register" tab to create one.
        </p>
        <p>
          For further assistance, navigate through the options in the top menu.
        </p>
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
