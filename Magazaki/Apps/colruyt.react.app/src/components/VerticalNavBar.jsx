import React, { useEffect, useState } from "react";
import Style from "./VerticalNavBar.module.css";
//import Categorieen from "../Utils/Categorieen.json";
import Axios from "axios";
import ProductCard from "./ProductCard"

const VerticalNavBar = ({ categorie }) => {
  const [categorieen, setCategorieen] = useState([]);

  useEffect(() => {
    const fetchCategorieen = async () => {
      try {
        const response = await Axios.get('http://localhost:5298/api/Category')
        setCategorieen(response.data)
      } catch (error) {
        console.log(error)
      }
    }
    fetchCategorieen()

  }, [])

  const categorieExist = () => {
    if (
      categorieen.find((c) => c.name === categorie) ||
      categorie === "Producten"
    ) {
      return (
        <>
          <div className={Style.scrollable}>
            {categorieen.map(c => (
              <a href="/" key={c.id}>
                <div className={Style.navBarItem}>
                  <p>{c.name}</p>
                </div>
              </a>
            ))}
          </div>
        </>
      );
    }
  };
  return (
    <div className={Style.navContainer}>
      <div className={Style.navBarLogo}>
        <svg
          version="1.1"
          viewBox="0 0 47 31"
          xmlns="http://www.w3.org/2000/svg"
        >
          <polygon
            points="14.654 0 47 0 32.346 31 9.1932e-14 31"
            fill="#F5822A"
          ></polygon>
        </svg>
      </div>
      <div className={Style.scrollable}>
        {categorieen.map(c => (
          <a href="/" key={c.id}>
            <div className={Style.navBarItem}>
              <p>{c.name}</p>
            </div>
          </a>
        ))}
      </div>
      {categorieExist()}
    </div>
  );
};

export default VerticalNavBar;
