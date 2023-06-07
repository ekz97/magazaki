import React, { useState } from "react";
import "./FooterComponent.css";
import House from "../images/house.svg";
import Trans from "../images/trans.svg";
import QR from "../images/qr.svg";
import { useAuth } from "../Context/AuthProvider";

const FooterComponent = () => {
  const [showPopup, setShowPopup] = useState(false);
  const [from, setFrom] = useState("");
  const [to, setTo] = useState("");
  const [amount, setAmount] = useState("");
  const [currency, setCurrency] = useState("USD");
  const [description, setDescription] = useState("");

  const { rekening1, rekening2 } = useAuth();

  const handleTransaction = async () => {
    try {
      console.log(from);
      console.log(to);
      console.log(currency);
      const response = await fetch(
        "http://vichogent.be:40076/Rekening/transactie",
        {
          method: "POST",
          headers: {
            Accept: "*/*",
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            from,
            to,
            amount,
            currency,
            description,
          }),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      console.log("Transaction successful: ", data);
      setShowPopup(false)
    } catch (error) {
      console.error("An error occurred:", error);
      alert("Foutieve gegevens voor overschrijving")
    }
  };
  return (
    <>
      <div className="footer-container">
        <img src={QR} alt="qr" />
        <img src={House} alt="house" />
        <img src={Trans} alt="trans" onClick={() => setShowPopup(true)} />
      </div>
      {showPopup && (
        <div className="popup-backdrop" onClick={() => setShowPopup(false)}>
          <div className="popup" onClick={(e) => e.stopPropagation()}>
            <div className="dropdowns">
              <div className="dropdown">
                <label>Verzender:</label>
                <select onChange={(e) => setFrom(e.target.value)}>
                  <option value={rekening1.rekeningnummer}>{rekening1.iban}</option>
                  <option value={rekening2.rekeningnummer}>{rekening2.iban}</option>
                </select>
              </div>
              <div className="dropdown">
                <label>Begunstigde:</label>
                <input
                  type="text"
                  placeholder="Rekening"
                  onChange={(e) => setTo(e.target.value)}
                />
                <select onChange={(e) => setCurrency(e.target.value)}>
                  <option value="USD">US Dollar (USD)</option>
                  <option value="EUR">Euro (EUR)</option>
                  <option value="JPY">Japanese Yen (JPY)</option>
                  <option value="GBP">British Pound (GBP)</option>
                  <option value="AUD">Australian Dollar (AUD)</option>
                  <option value="CAD">Canadian Dollar (CAD)</option>
                  <option value="CHF">Swiss Franc (CHF)</option>
                  <option value="CNY">Chinese Yuan (CNY)</option>
                  <option value="SEK">Swedish Krona (SEK)</option>
                  <option value="NZD">New Zealand Dollar (NZD)</option>
                </select>
              </div>
            </div>
            <input
              type="number"
              placeholder="Bedrag"
              onChange={(e) => setAmount(e.target.value)}
            />
            <input
              type="text"
              placeholder="Omschrijving"
              onChange={(e) => setDescription(e.target.value)}
            />
            <button onClick={handleTransaction}>Verstuur</button>
          </div>
        </div>
      )}
    </>
  );
};

export default FooterComponent;
