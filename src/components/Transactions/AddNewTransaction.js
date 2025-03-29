import React from 'react'
import { useState, useEffect } from 'react'

function AddNewTransaction(props) 
{
  const [transactionData, setTransactiondata] = useState(
    {
      matNumber: "",
      fromLocationName: "",
      toLocationName: "",
      quantity: "",
      userName: ""
    })

useEffect(() =>
  {
    if (props.transactionObj)
    {
        setTransactiondata({
        matNumber: props.transactionObj.matNumber || '',
        fromLocationName: props.transactionObj.fromLocationName || '',
        toLocationName: props.transactionObj.toLocationName || '',
        quantity: props.transactionObj.quantity || '',
        userName: props.transactionObj.userName || ''
          });
        }
      }, [props.transactionObj]);


const handleChange = (event) => {
    const { name, value } = event.target;
    setTransactiondata({ ...transactionData, [name]: value });
  }

  const handleSubmit = async (event) => 
  {
    const url = "https://localhost:7055/api/Transactions"
    event.preventDefault();

    const request = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(transactionData),
    });
    if(!request.ok)
    {
       console.log("Error")
       return
    }
    const response = await request.json();
    props.handleCount();
    console.log(response.message);
  }

    return (
      <div>
        <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "300px" }}>
          <label>Number:</label>
            <input type="number" id="number" name="number" value={transactionData.number} onChange={handleChange} className='form-control' placeholder='Material Number'/>
          <label>From:</label>
            <input type="text" id="from" name="from" value={transactionData.fromLocationName} onChange={handleChange} className='form-control' placeholder='Transaction From'/>
          <label>To:</label>
            <input type="text" id="to" name="to" value={transactionData.toLocationName} onChange={handleChange} className='form-control' placeholder='Transaction To'/>
          <label>Quantity:</label>
            <input type="number" id="quantity" name="quantity" value={transactionData.quantity} onChange={handleChange} className='form-control' placeholder='Transacted Quantity'/>
          <label>Username:</label>
            <input type="text" id="username" name="username" value={transactionData.userName} onChange={handleChange} className='form-control' placeholder='Username'/>
            <br></br>
          <button type="submit" className='btn btn-primary'>Submit</button>
          </form>
      </div>
  )
}

export default AddNewTransaction