import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Button } from 'react-bootstrap';


const NavigationBar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    navigate('/dashboard'); 
  };

  const navbarStyle = {
    background: 'linear-gradient(to right,rgb(129, 176, 230),rgb(32, 60, 112))',
    backgroundSize: 'cover',
    backgroundPosition: 'center',
  };

  return (
    <Navbar style={navbarStyle} expand="lg">
      <Navbar.Brand as={Link} to="/dashboard">Warehouse Management</Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="ms-auto">
          <Nav.Link as={Link} to="/dashboard">Dashboard</Nav.Link>
          <Nav.Link as={Link} to="/locations">Location</Nav.Link>
          <Nav.Link as={Link} to="/materials">Material</Nav.Link>
        </Nav>
        <Nav>
          <Nav.Link as={Link} to="/login">Login</Nav.Link>
          <Button variant="outline-light" onClick={handleLogout}>Logout</Button>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavigationBar;

