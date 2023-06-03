import React from "react";
import Home from "./Components/Home";
import Login from "./Components/Login";
import Header from "./Components/Header"
import FooterComponent from "./Components/FooterComponent";
import "./App.css";

const App = () => {
  return (
    <div className="container">
      {/* <Login/> */}
      <Header/>
      <Home/>
      <FooterComponent/>
    </div>
  );
};

export default App;
