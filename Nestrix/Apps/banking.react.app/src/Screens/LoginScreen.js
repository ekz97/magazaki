import React, { useState } from "react";
import { useAuth } from "../Context/AuthProvider";

const LoginScreen = () => {
  const { setIsLoggedIn, setUser, user } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const handleLogin = async (event) => {
    event.preventDefault();
    try {
      const response = await fetch(
        "http://vichogent.be:40076/Gebruiker/loginEmail",
        {
          method: "POST",
          headers: {
            Accept: "*/*",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ email, password }),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const userData = await response.json();

      if (userData.gebruiker === null) {
        const data = registerUser(email, password)
        setIsLoggedIn(true)
        setUser(data)
      } else {
        setIsLoggedIn(true);
        setUser(userData.gebruiker);
      }
    } catch (error) {
      console.error("An error occurred:", error);
      alert("Email of Wachtwoord foutief!");
    }
  };

  const registerUser = async (email, password) => {
    try {
      const response = await fetch(
        "http://vichogent.be:40076/Gebruiker/Register?bankId=6a32d332-9a6e-4654-a809-3dfdbac32212",
        {
          method: "POST",
          headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            email,
            password,
          }),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.text();
      console.log("User registered successfully:", data);
      return data
    } catch (error) {
      console.error("An error occurred:", error);
    }
  };

  return (
    <div className="login-screen">
      <form onSubmit={handleLogin}>
        <div className="login-form">
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            name="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="login-form">
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default LoginScreen;
