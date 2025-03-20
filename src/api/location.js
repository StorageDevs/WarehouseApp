import axios from 'axios';

const API_URL = 'http://localhost:5118/api/Locations/';

export const getMaterials = async () => {
  try {
    const response = await axios.get(API_URL, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('access_token')}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch materials', error);
  }
};

export const addLocation = async (location) => {
  try {
    const response = await axios.post(API_URL, location, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('access_token')}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to add location', error);
  }
};