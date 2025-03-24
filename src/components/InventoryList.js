import React, { useState, useEffect } from 'react';
//import axios from "axios";
import { Table, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

const InventoryList = () => {
  
  const url = "http://localhost:5118/api/Inventories"

  const [InventoryData, setInventoryData] = useState([]);

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
        setInventoryData(response.result);
        console.log("Inventory fetched successfully:", response.message);
      } catch (error) {
        console.error("Failed to fetch inventory:", error);
      }
    })();
  }, );

    const InventoryElements = InventoryData.map((inventory) => 
    {
      return (
        <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{inventory.materialNumber}</TableCell>
              <TableCell>{inventory.materialDescription}</TableCell>
              <TableCell>{inventory.locationName}</TableCell>
              <TableCell>{inventory.inventoryQuantity}</TableCell>
            </TableRow>
          </TableHead>
        </Table>
      </TableContainer>
      )
    })
    return (
      <>
        <div>
          {InventoryElements}
        </div>
      </>
    )
}

export default InventoryList;