import React from 'react'
import { useState, useEffect } from 'react'
import UpdateLocation from './UpdateLocation';

function AddNewLocation(props) 
{
  const [locationData, setLocationdata] = useState(
    {
      id: "",
      name: "",
      description: "",
      capacity: "",
    })

useEffect(() =>
  {
    if (props.locationObj)
    {
        setLocationdata({
        id: props.locationObj.id || '',
        number: props.locationObj.number || '',
        description: props.locationObj.description || '',
        capacity: props.locationObj.capacity || ''
        
          ? new Date(new Date(props.locationObj.myear).getTime() - new Date().getTimezoneOffset() * 60000)
              .toISOString()
              .split('T')[0]
            : ''
          });
        }
      }, [props.locationObj]);


const handleChange = (event) => {
    const { name, value } = event.target;
    setLocationdata({ ...locationData, [name]: value });
  }

  const handleSubmit = async (event) => 
  {
    const url = "http://localhost:5118/api/Locations"
    event.preventDefault();

    const request = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(locationData),
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
            <input type="number" id="id" name="id" value={locationData.id} onChange={handleChange} className='form-control' placeholder='Location ID'/>
          <label>Name:</label>
            <input type="text" id="name" name="name" value={locationData.name} onChange={handleChange} className='form-control' placeholder='Location Name'/>
          <label>Description:</label>
            <input type="text" id="description" name="description" value={materialData.description} onChange={handleChange} className='form-control' placeholder='Location Description'/>
          <label>Capacity:</label>
            <input type="number" id="capacity" name="capacity" value={locationData.capacity} onChange={handleChange} className='form-control' placeholder='Location Capacity'/>
            <br></br>
          <button type="submit" className='btn btn-primary'>Submit</button>
          <UpdateLocation locationData={locationData} locationId={props.locationObj.id} handleCount={props.handleCount}/>
          </form>
      </div>
  )
}

export default AddNewLocation