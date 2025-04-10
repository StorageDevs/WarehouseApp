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
import GetAllUser from "./components/Users/GetAllUsers";

const App = () => {
 const isAuthenticated = localStorage.getItem('jwt') != null; 
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
      <NavigationBar isAuthenticated={isAuthenticated} />

      <Routes>
        <Route path="/home" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        {isAuthenticated && (
          <>
            <Route path="*" element={<HomePage />} />
            <Route path="/home" element={<HomePage />} />
            <Route path="/users" element={<GetAllUser />} />
            <Route path="/inventories" element={<GetInventory />} />
            <Route path="/transactions" element={<GetTransactions />} />
            <Route path="/locations" element={<GetLocation />} />
            <Route path="/materials" element={<GetAllMaterial />} />
          </>
        )}
        {!isAuthenticated && (
          <>
            <Route path="*" element={<Navigate to="/home" />} />
            <Route path="/home" element={<Navigate from="/login" />} />
          </>
        )}
      </Routes>
    </div>
  );
};

export default App;