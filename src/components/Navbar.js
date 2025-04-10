import React from 'react';
import { Link } from 'react-router-dom';
import { Navbar, Nav, Button } from 'react-bootstrap';

const NavigationBar = ({ isAuthenticated }) => {

  const handleLogout = () => {
    localStorage.removeItem('jwt');
    localStorage.setItem('logoutSuccess', 'true');
    window.location.href = "/home";
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
    {isAuthenticated && (
      <>
        <Nav.Link as={Link} to="/users" style={{ color: 'white' }}>Users</Nav.Link>
        <Nav.Link as={Link} to="/inventories" style={{ color: 'white' }}>Inventory</Nav.Link>
        <Nav.Link as={Link} to="/transactions" style={{ color: 'white' }}>Transactions</Nav.Link>
        <Nav.Link as={Link} to="/locations" style={{ color: 'white' }}>Locations</Nav.Link>
        <Nav.Link as={Link} to="/materials" style={{ color: 'white' }}>Materials</Nav.Link>
      </>
    )}
      </Nav>
    <Nav>
    {!isAuthenticated ? (
      <>
        <Nav.Link as={Link} variant="outline-light" to="/register" style={{ color: 'white' }}>Register</Nav.Link>
        <Nav.Link as={Link} variant="outline-light" to="/login" style={{ color: 'white' }}>Login</Nav.Link>
      </>
    ) : (
      <Button variant="outline-light" onClick={handleLogout}>Logout</Button>
    )}
    </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavigationBar;
