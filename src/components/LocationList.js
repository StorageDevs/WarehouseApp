import React, { useState, useEffect } from 'react';
//import axios from "axios";
import { Table, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

const LocationList = () => {
  
  const url = "http://localhost:7055/api/Locations"

  const [LocationData, setLocationData] = useState([]);

  useEffect(() => {
    (async () => {
      try {
        const request = await fetch(url, {
          headers: {
            "Content-Type": "application/json",
          },
        });
  
        if (!request.ok) {
          throw new Error("Network response was not ok");
        }
  
        const response = await request.json();
        setLocationData(response.result);
        console.log("Locations fetched successfully:", response.message);
      } catch (error) {
        console.error("Failed to fetch materials:", error);
      }
    })();
  }, );

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