import React from "react";
import "./Home.css";
import AccountComponent from "./AccountComponent";
import GraphComponent from "./GraphComponent";
import OverviewComponent from "./OverviewComponent";
import FooterComponent from "./FooterComponent";

const Home = () => {
  return (
    <div className="home-container">
      <div className="accounts">
        <AccountComponent
          type="wallet"
          acc="BE18321332323665"
          balance="85,74"
        />
        <AccountComponent
          type="piggy"
          acc="BE18897762864965"
          balance="2541,30"
        />
      </div>

      <div className="graphs">
        <GraphComponent />
        <GraphComponent />
      </div>

      <OverviewComponent />
    </div>
  );
};

export default Home;
