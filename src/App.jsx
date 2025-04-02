import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate, useLocation } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import GetInventory from "./components/Inventory/GetInventory";
import GetAllMaterial from "./components/Material/GetAllMaterial";
import GetTransactions from "./components/Transactions/GetTransactions";
import HomePage from "./pages/Home";
import NavigationBar from './components/Navbar';
import GetLocation from "./components/Location/GetLocation";
import VantaBackground from "./components/VantaBackground"; // Importáljuk az új komponenst
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
 // const isAuthenticated = localStorage.getItem('access_token'); 
  const isAuthenticated = true;

  return (
    <Router>
      <MainContent isAuthenticated={isAuthenticated} />
    </Router>
  );
};

const MainContent = ({ isAuthenticated }) => {
  const location = useLocation();
  const isHomePage = location.pathname === "/home";

  return (
    <div style={{ minHeight: "100vh", width: "100vw", position: "relative" }}>
    {!isHomePage && <VantaBackground />}
    {isAuthenticated && <NavigationBar />} {}
      <Routes>
        <Route path = "*" element={<LoginPage/>} />
        <Route path="/home" element={isAuthenticated ? <HomePage /> : <Navigate to="/login" />} />
        <Route path="/login" element={<LoginPage /> } />
        <Route path="/inventories" element={<GetInventory />} />
        <Route path="/transactions" element={<GetTransactions />} />
        <Route path="/locations" element={<GetLocation />} />
        <Route path="/materials" element={<GetAllMaterial />} />
      
      </Routes>
    </div>
  );
};

export default App;
