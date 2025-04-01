import React from 'react'
import { useState, useEffect } from 'react'

function AddNewMaterial(props) 
{
  const [materialData, setMaterialdata] = useState(
    {
      number: "",
      description: "",
      unit: "",
      priceUnit: ""
    })

useEffect(() =>
  {
    if (props.materialObj)
    {
        setMaterialdata({
        number: props.materialObj.number || '',
        description: props.materialObj.description || '',
        unit: props.materialObj.unit || '',
        priceUnit: props.materialObj.priceUnit || ''
          });
        }
      }, [props.materialObj]);


const handleChange = (event) => {
    const { name, value } = event.target;
    setMaterialdata({ ...materialData, [name]: value });
  }

  const handleSubmit = async (event) => 
  {
    const url = "https://localhost:7055/api/Materials"
    event.preventDefault();

    const request = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(materialData),
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
            <input type="number" id="number" name="number" value={materialData.number} onChange={handleChange} className='form-control' placeholder='Material Number'/>
          <label>Description:</label>
            <input type="text" id="description" name="description" value={materialData.description} onChange={handleChange} className='form-control' placeholder='Material Description'/>
          <label>Unit:</label>
            <input type="text" id="unit" name="unit" value={materialData.unit} onChange={handleChange} className='form-control' placeholder='Material Unit'/>
          <label>Price/Unit:</label>
            <input type="number" id="priceUnit" name="priceUnit" value={materialData.priceUnit} onChange={handleChange} className='form-control' placeholder='Material Priceunit' />
          <button type="submit" className='btn btn-primary'>Submit</button>
          </form>
      </div>
  )
}

export default AddNewMaterial