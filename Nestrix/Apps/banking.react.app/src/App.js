import React from "react";
import Home from "./Components/Home";
import Header from "./Components/Header";
import FooterComponent from "./Components/FooterComponent";
import "./App.css";
import { useAuth } from "./Context/AuthProvider";
import LoginScreen from "./Screens/LoginScreen";

const App = () => {
  const { isLoggedIn, setIsLoggedIn } = useAuth();
  return (
    <div className="container">
      {isLoggedIn ? (
        <>
          <Header/>
          <Home />
          <FooterComponent />
        </>
      ) : (
        <>
          <Header /> <LoginScreen />
        </>
      )}
    </div>
  );
};

export default App;
