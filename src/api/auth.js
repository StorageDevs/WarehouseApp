import axios from 'axios';

const API_URL = 'http://localhost:5118/api/auth/'; // Backend URL

export const login = async (email, password) => {
  try {
    const response = await axios.post(`${API_URL}login`, { email, password });
    localStorage.setItem('access_token', response.data.accessToken);
    localStorage.setItem('refresh_token', response.data.refreshToken);
    return response.data;
  } catch (error) {
    console.error('Login failed', error);
  }
};

export const refreshToken = async () => {
  const refreshToken = localStorage.getItem('refresh_token');
  try {
    const response = await axios.post(`${API_URL}refresh-token`, { refreshToken });
    localStorage.setItem('access_token', response.data.accessToken);
    return response.data;
  } catch (error) {
    console.error('Failed to refresh token', error);
  }
};