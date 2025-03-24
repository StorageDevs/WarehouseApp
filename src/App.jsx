import React, { useState, useEffect, useRef } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import MaterialsPage from './pages/MaterialsPage';
import LocationPage from "./pages/LocationPage";
import InventoryPage from "./pages/InventoryPage";
import HomePage from "./pages/Home";
import Navbar from './components/Navbar';
import * as THREE from "three";
import CLOUDS from "vanta/src/vanta.clouds";
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
  const [vantaEffect, setVantaEffect] = useState(null);
  const vantaRef = useRef(null);

 // const isAuthenticated = localStorage.getItem('access_token'); 
  const isAuthenticated = true;

  useEffect(() => {
    if (!vantaEffect) {
      setVantaEffect(
        CLOUDS({
          el: vantaRef.current,
          THREE,
          color: 0x1a1a2e,
          backgroundColor: 0x0f3460,
          speed: 1.4,
        })
      );
    }
    return () => {
      if (vantaEffect) vantaEffect.destroy();
    };
  }, [vantaEffect]);

  return (
    <div ref={vantaRef} style={{ minHeight: "100vh", width: "100vw", position: "fixed", top: 0, left: 0 }}>
    <Router>
    {isAuthenticated && <Navbar />} {}
      <Routes>
        <Route path = "*" element={<LoginPage/>} />
        <Route path="/home" element={isAuthenticated ? <HomePage /> : <Navigate from="/login" />} />
        <Route path="/login" element={<LoginPage /> } />
        <Route path="/inventories" element={<InventoryPage />} />
        <Route path="/locations" element={<LocationPage />} />
        <Route path="/materials" element={<MaterialsPage />} />
      
      </Routes>
    </Router>
    </div>
  );
};

export default App;
