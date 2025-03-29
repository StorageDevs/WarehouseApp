import React from 'react'
import { useState, useEffect } from 'react'
import UpdateMaterial from './UpdateMaterial';

function AddNewMaterial(props) 
{
  const [materialData, setMaterialdata] = useState(
    {
      id: "",
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
        id: props.materialObj.id || '',
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
          <label>ID:</label>
            <input type="number" id="id" name="id" value={materialData.id} onChange={handleChange} className='form-control' placeholder='Material ID'/>
          <label>Number:</label>
            <input type="number" id="number" name="number" value={materialData.number} onChange={handleChange} className='form-control' placeholder='Material Number'/>
          <label>Description:</label>
            <input type="text" id="description" name="description" value={materialData.description} onChange={handleChange} className='form-control' placeholder='Material Description'/>
          <label>Unit:</label>
            <input type="text" id="unit" name="unit" value={materialData.unit} onChange={handleChange} className='form-control' placeholder='Material Unit'/>
          <label>PriceUnit:</label>
            <input type="number" id="priceunit" name="priceunit" value={materialData.priceUnit} onChange={handleChange} className='form-control' placeholder='Material Priceunit' />
            <br></br>
          <button type="submit" className='btn btn-primary'>Submit</button>
          <UpdateMaterial materialData={materialData} materialId={props.materialObj.id} handleCount={props.handleCount}/>
          </form>
      </div>
  )
}

export default AddNewMaterial