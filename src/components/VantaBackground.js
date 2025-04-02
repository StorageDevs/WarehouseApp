import React, { useState, useEffect, useRef } from "react";
import * as THREE from "three";
import CLOUDS from "vanta/src/vanta.clouds";

const VantaBackground = () => {
  const [vantaEffect, setVantaEffect] = useState(null);
  const vantaRef = useRef(null);

  useEffect(() => {
    if (!vantaEffect) {
      setVantaEffect(
        CLOUDS({
          el: vantaRef.current,
          THREE,
          color: 0x1a1a2e,
          backgroundColor: 0x0f3460,
          speed: 1.4,
          vertexColors: false,
        })
      );
    }
    return () => {
      if (vantaEffect) vantaEffect.destroy();
    };
  }, [vantaEffect]);

  return (
    <div ref={vantaRef} style={{ minHeight: "100vh", width: "100vw", position: "fixed", top: 0, left: 0, zIndex: -1 }} />
  );
};

export default VantaBackground;
