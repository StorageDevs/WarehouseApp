import React, { useState } from 'react';

function AddNewTransaction({ closeForm }) {
  const [transactionData, setTransactionData] = useState({
    materialNumber: "",
    transactionFromLocationName: "",
    transactionToLocationName: "",
    transactedQty: "",
    transferBy: "",
    transferDate: "",

  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setTransactionData({ ...transactionData, [name]: value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const url = "https://localhost:7055/api/Transactions";

    const request = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(transactionData),
    });

    if (!request.ok) {
      console.log("Error");
      return;
    }

    console.log("Transaction added successfully");
    closeForm();
  };

  return (
    <div className="add-form">
      <h3>Add New Transaction</h3>
      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
        <label>Transaction Id:</label>
        <input type="number" name="transactionId" value={transactionData.transactionId} onChange={handleChange} className="form-control" />
        
        <label>Mat. Number:</label>
        <input type="number" name="materialNumber" value={transactionData.materialNumber} onChange={handleChange} className="form-control" />

        <label>From (Location):</label>
        <input type="text" name="transactionFromLocationName" value={transactionData.transactionFromLocationName} onChange={handleChange} className="form-control" />

        <label>To (Location):</label>
        <input type="text" name="transactionToLocationName" value={transactionData.transactionToLocationName} onChange={handleChange} className="form-control" />

        <label>Quantity:</label>
        <input type="number" name="transactedQty" value={transactionData.transactedQty} onChange={handleChange} className="form-control" />

        <label>User:</label>
        <input type="text" name="transferBy" value={transactionData.transferBy} onChange={handleChange} className="form-control" />
        
        <label>Date:</label>
        <input type="text" name="transferDate" value={transactionData.transferDate} onChange={handleChange} className="form-control" />

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
            max-width: 300px;
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

export default AddNewTransaction;
