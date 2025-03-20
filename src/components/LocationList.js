import React, { useState, useEffect } from 'react';
//import axios from "axios";
import { Table, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

const LocationList = () => {
  
  const url = "http://localhost:5118/api/Locations"

  const [LocationData, setLocationData] = useState([]);

  useEffect(() => 
  {
    (async () => 
      {
      const request = await fetch(url, {
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
      setLocationData(response.result);
      console.log(response.message);
      })()
    },);

    const LocationElements = LocationData.map((location) => 
    {
      return (
        <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{location.locationId}</TableCell>
              <TableCell>{location.locationName}</TableCell>
              <TableCell>{location.locationDescription}</TableCell>
              <TableCell>{location.locationCapacity}</TableCell>
            </TableRow>
          </TableHead>
        </Table>
      </TableContainer>
      )
    })
    return (
      <>
        <div>
          {LocationElements}
        </div>
      </>
    )
}

export default LocationList;