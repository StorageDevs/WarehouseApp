import React, { useEffect, useState } from "react";
import AddNewTransaction from "./AddNewTransaction";

function GetAllTransaction() {
  const url = "https://localhost:7055/api/Transactions";
  const [transactionData, setTransactionData] = useState([]);
  const [showAddForm, setShowAddForm] = useState(false);

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
      setTransactionData(response.result);
      console.log(response.message);
    })();
  }, []);
  
  const handleAddTransaction = (newTransaction) => {
    setTransactionData((prevData) => [...prevData, newTransaction]);
    setShowAddForm(false);
  };

  const handleAddNewTransaction = () => {
    setShowAddForm(true);
  };

  const closeAddForm = () => {
    setShowAddForm(false);
  };

  return (
    <div className="container">
      {showAddForm ? (
        <div className="overlay">
          <AddNewTransaction closeForm={closeAddForm} addTransaction={handleAddTransaction}/>
        </div>
      ) : (
        <>
          <h2 className="title">Transactions</h2>
          <div className="button-container">
            <button onClick={handleAddNewTransaction} className="btn btn-primary">
              Add New Transaction
            </button>
          </div>
          <div className="card-container">
            {transactionData.map((transaction) => (
              <div className="card" key={transaction.transactionId}>
                <div className="card-body">
                  <p><strong>ID: </strong>{transaction.transactionId}</p>
                  <p><strong>Mat. Number: </strong> {transaction.materialNumber}</p>          
                  <p><strong>Mat. Description: </strong>{transaction.materialDescription}</p>          
                  <p><strong>From: </strong>{transaction.transferFrom}</p>          
                  <p><strong>To: </strong>{transaction.transferTo}</p>          
                  <p><strong>Quantity: </strong>{transaction.transferedQuantity}</p>           
                  <p><strong>Date: </strong>{transaction.transferDate}</p>
                </div>
              </div>
            ))}
          </div>
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

export default GetAllTransaction;
