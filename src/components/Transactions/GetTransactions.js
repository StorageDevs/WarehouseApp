import React, { useEffect, useState } from 'react'

function GetTransactions() {
  const url = "https://localhost:7055/api/Transactions"
  const [transactionData, setTransactionData] = useState([]);

  useEffect(() => 
  {
    (async () => 
      {
      const request = await fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        }
      })
      
      if(!request.ok)
      {
        console.log("Error")
        return
      }

      const response = await request.json();
      setTransactionData(response.result);
      console.log(response.message);
      })()
    }, []);

    return (
      <div>
        <br></br>
        <h2 style={{ textAlign: "center", marginBottom: "20px", padding: "10px" }}>Transactions</h2>
      <div style={{ display: "flex", flexWrap: "wrap", gap: "10px", justifyContent: "left"}}>
        {transactionData.map((transaction) => (
          <div
            className="card"
            style={{
              width: 450,
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
            }}
            key={transaction.transactionId}
          >
          <div className="card-body">Transaction Id: {transaction.transactionId}</div>
          <div className="card-body">Mat. Number: {transaction.materialNumber}</div>
          <div className="card-body">Mat. Description: {transaction.materialDescription}</div>
          <div className="card-body">From: {transaction.transferFrom}</div>
          <div className="card-body">To: {transaction.transferTo}</div>
          <div className="card-body">Quantity: {transaction.transferedQuantity}</div>
          <div className="card-body">By: {transaction.transferBy}</div>
          <div className="card-body">Date: {transaction.transferDate}</div>

          </div>
        ))}
      </div>
    </div>
    );
}

export default GetTransactions