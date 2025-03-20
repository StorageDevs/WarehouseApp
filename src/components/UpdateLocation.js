import React from "react";

function UpdateLocation(props) 
{
    const handleLocationData = async() =>
    {
        const url = `http://localhost:5118/api/Locations?id=${props.locationId}`
        const request = await fetch(url, {
            method: "PUT",
            body: JSON.stringify(props.locationData),
            headers: {
                "Content-Type": "application/json",
              },
        })
        if(!request.ok)
        {
            console.log("Error")
            return
        }
        var response = await request.json()
        console.log(response.message)
        props.handleCount()
    }    
    return (
        <button onClick={handleLocationData} type='button' className='btn btn-warning'>Modify</button>        
    )
}

export default UpdateLocation