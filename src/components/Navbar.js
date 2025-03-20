import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AppBar, Toolbar, Button, Typography } from '@mui/material';

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    navigate('/login'); 
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h5" style={{ flexGrow: 1 }}>
        <Button color="inherit" component={Link} to="/dashboard">
          Warehouse Management
        </Button>
        </Typography>
        <Button color="inherit" component={Link} to="/materials">
          Location
        </Button>
        <Button color="inherit" component={Link} to="/materials">
          Material
        </Button>
        <Button color="inherit" component={Link} to="/login">
          Login
        </Button>
        <Button color="inherit" onClick={handleLogout}>
          Logout
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;

