import React from "react";
import Piggy from "../images/piggy.svg";
import Wallet from "../images/wallet.svg";
import "./AccountComponent.css";

const AccountComponent = (props) => {
  const imgSrc = props.type === "wallet" ? Wallet : Piggy;

  return (
    <div className="account-container">
      <div className="upper-part">
        <img className="img" src={imgSrc} alt="typerek"></img>
      </div>
      <div className="lower-part">
        <p id="reknr">{props.acc}</p>
        <p id="saldo">{props.balance} EUR</p>
      </div>
    </div>
  );
};

export default AccountComponent;
