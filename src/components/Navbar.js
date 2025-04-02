import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Button } from 'react-bootstrap';

const NavigationBar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    navigate('/home');
  };

  const navbarStyle = {
    background: 'linear-gradient(to right, rgb(129, 176, 230), rgb(32, 60, 112))',
    backgroundSize: 'cover',
    backgroundPosition: 'center',
  };

  const logoStyle = {
    display: 'flex',
    alignItems: 'center',
    gap: '10px',
    fontWeight: 'bold',
    color: 'white',
    textTransform: 'uppercase',
  };

  return (
    <Navbar style={navbarStyle} expand="lg">
      <Navbar.Brand as={Link} to="/home" style={logoStyle}>
        <img src="/boxes.png" alt="Logo" style={{ width: 30, height: 30 }} />
        SD Warehouse Management
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="ms-auto">
          <Nav.Link as={Link} to="/inventories">Inventory</Nav.Link>
          <Nav.Link as={Link} to="/transactions">Transactions</Nav.Link>
          <Nav.Link as={Link} to="/locations">Location</Nav.Link>
          <Nav.Link as={Link} to="/materials">Material</Nav.Link>
        </Nav>
        <Nav>
          <Nav.Link as={Link} variant="outline-light" to="/login">Login</Nav.Link>
          <Button variant="outline-light" onClick={handleLogout}>Logout</Button>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavigationBar;

