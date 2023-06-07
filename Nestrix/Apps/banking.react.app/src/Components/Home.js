import React, { useEffect, useState } from "react";
import "./Home.css";
import AccountComponent from "./AccountComponent";
import GraphComponent from "./GraphComponent";
import OverviewComponent from "./OverviewComponent";
import { useAuth } from "../Context/AuthProvider";

const Home = () => {
  const [transactions, setTransaction] = useState([]);

  const { user, rekening1, setRekening1, rekening2, setRekening2 } = useAuth();

/*   const fetchData = async () => {
    try {
      const response = await fetch(
        `http://vichogent.be:40076/Rekening/${rekening1}?depth=10`
      );
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      setTransaction(data.transacties);
    } catch (error) {
      console.error("An error occured: ", error);
    }
  }; */

  const fetchAccounts = async () => {
    try {
      const response = await fetch(
        `http://vichogent.be:40076/Rekening/Rekeningen?gebruikerId=${user.id}`
      )
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json()
      setRekening1(data[0])
      setRekening2(data[1])
      setTransaction(data[0].transacties)
    } catch (error) {
      console.error("An error occured: ", error)
    }
  }

  useEffect(() => {
    fetchAccounts()
    console.log(user);
  }, []);


  return (
    <div className="home-container">
      <div className="accounts">
        <AccountComponent
          type={rekening1.rekeningType === 0 ? "wallet" : "piggy"}
          acc={rekening1.iban}
          balance={rekening1.saldo}
        />
        <AccountComponent
          type={rekening2.rekeningType === 0 ? "wallet" : "piggy"}
          acc={rekening2.iban}
          balance={rekening2.saldo}
        />
      </div>

      <div className="graphs">
        <GraphComponent />
        <GraphComponent />
      </div>

      <OverviewComponent transactions={transactions} />
    </div>
  );
};

export default Home;
