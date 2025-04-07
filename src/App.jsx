import React from "react";
import { BrowserRouter as Router, Route, Routes, Navigate, useLocation } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import GetInventory from "./components/Inventory/GetInventory";
import GetAllMaterial from "./components/Material/GetAllMaterial";
import GetTransactions from "./components/Transactions/GetTransactions";
import HomePage from "./pages/Home";
import NavigationBar from './components/Navbar';
import GetLocation from "./components/Location/GetLocation";
import VantaBackground from "./components/VantaBackground";
import 'bootstrap/dist/css/bootstrap.min.css';
import RegisterPage from "./pages/RegisterPage";
// import GetAllUser from "./components/Users/GetAllUsers";

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
        <Route path = "*" element={<HomePage/>} />
        <Route path="/home" style={{ color: 'white' }} element={isAuthenticated ? <HomePage /> : <Navigate from="/login" />} />
        <Route path="/login" style={{ color: 'white' }} element={<LoginPage /> } />
        <Route path="/register" style={{ color: 'white' }} element={<RegisterPage /> } />
        <Route path="/inventories" style={{ color: 'white' }} element={<GetInventory />} />
        <Route path="/transactions" style={{ color: 'white' }} element={<GetTransactions />} />
        <Route path="/locations" style={{ color: 'white' }} element={<GetLocation />} />
        <Route path="/materials" style={{ color: 'white' }} element={<GetAllMaterial />} />
      </Routes>
    </div>
  );
};

export default App;
