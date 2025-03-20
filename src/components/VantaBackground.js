import React, { useEffect, useRef, useState } from "react";
import * as THREE from "three";
import CLOUDS from "vanta/dist/vanta.clouds.min";

const VantaBackground = () => {
  const [vantaEffect, setVantaEffect] = useState(null);
  const vantaRef = useRef(null);

  useEffect(() => {
    if (!vantaEffect) {
      setVantaEffect(
        CLOUDS({
          el: vantaRef.current,
          THREE: THREE,
          color: 0x2196f3, 
          backgroundColor: 0x0f0f0f,
        })
      );
    }
    return () => {
      if (vantaEffect) vantaEffect.destroy();
    };
  }, [vantaEffect]);

  return <div ref={vantaRef} style={{ width: "100vw", height: "100vh", position: "fixed", zIndex: -1 }} />;
};

export default VantaBackground;
