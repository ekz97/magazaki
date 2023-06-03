import React from "react";
import { Link } from "react-router-dom";
import "./Header.css";
import exit from "../images/exit.svg";
import logo from "../images/logo.png";

const Header = () => {
  return (
    <div className="header-container">
      <img src={exit} alt="exit icon" className="exit-icon" />

      <img src={logo} alt="logo" className="logo" />
    </div>
  );
};

export default Header;
