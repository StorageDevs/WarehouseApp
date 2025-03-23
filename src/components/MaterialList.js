import React, { useState, useEffect } from 'react';
//import axios from "axios";
import { Table, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

const MaterialList = () => {
  
  const url = "http://localhost:7055/api/Materials"

  const [MaterialData, setMaterialData] = useState([]);

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
        setMaterialData(response.result);
        console.log("Materials fetched successfully:", response.message);
      } catch (error) {
        console.error("Failed to fetch materials:", error);
      }
    })();
  }, );

    const MaterialElements = MaterialData.map((material) => 
    {
      return (
        <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{material.materialId}</TableCell>
              <TableCell>{material.materialName}</TableCell>
              <TableCell>{material.materialDescription}</TableCell>
              <TableCell>{material.materialUnit}</TableCell>
              <TableCell>{material.materialPriceUnit}</TableCell>
            </TableRow>
          </TableHead>
        </Table>
      </TableContainer>
      )
    })
    return (
      <>
        <div>
          {MaterialElements}
        </div>
      </>
    )
}

export default MaterialList;