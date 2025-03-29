import axios from 'axios';

const API_URL = 'https://localhost:7055/api/Inventories/';

export const getInventory = async () => {
  try {
    const response = await axios.get(API_URL, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('access_token')}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch inventory', error);
  }
};