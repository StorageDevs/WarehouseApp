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
              width: 200,
              height: "auto",
              margin: 10,
              padding: 10,
              border: "5px solid #ccc",
              borderRadius: "8px",
              boxShadow: "2px 2px 10px rgba(0,0,0,0.1)",
              position: "relative",
            }}
            key={transaction.transactionId}
          >
          <div className="card-body">Transaction Id: {transaction.transactionId}
          Mat. Number: {transaction.materialNumber}
          Mat. Description: {transaction.materialDescription}
          From: {transaction.transferFrom}
          To: {transaction.transferTo}
          Quantity: {transaction.transferedQuantity}
          By: {transaction.transferBy}
          Date: {transaction.transferDate}</div>
          </div>
        ))}
      </div>
    </div>
    );
}

export default GetTransactions