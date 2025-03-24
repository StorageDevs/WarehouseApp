import React from "react";

const HomePage = () => {
  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h1>Welcome to the StorageDevs Warehouse Management System</h1>
        <p>
          This platform allows you to efficiently manage your inventory. Use the
          "Material" and "Location" tab to add, modify, or delete items. Check the "Inventory" tab to see the current stock. The Dashboard serves
          as a quick overview and guide for new users.
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
    display: "flex",
    height: "80vh",
    justifyContent: "center",
    alignItems: "center",
  },
  card: {
    backgroundColor: "rgba(65, 126, 196, 0.9)",
    padding: "80px",
    borderRadius: "12px",
    boxShadow: "0 4px 15px rgba(0, 0, 0, 0.2)",
    textAlign: "center",
    maxWidth: "800px",
    width: "100%",
  },
};

export default HomePage;