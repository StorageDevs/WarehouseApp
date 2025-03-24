import React from "react";

function UpdateMaterial(props) 
{
    const handleMaterialData = async() =>
    {
        const url = `http://localhost:5118/api/Materials?id=${props.materialId}`
        const request = await fetch(url, {
            method: "PUT",
            body: JSON.stringify(props.materialData),
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
        <button onClick={handleMaterialData} type='button' className='btn btn-warning'>Modify</button>        
    )
}

export default UpdateMaterial