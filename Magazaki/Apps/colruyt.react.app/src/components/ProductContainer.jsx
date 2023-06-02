import React from "react";
import ProductCard from "./ProductCard";
import Style from "./ProductContainer.module.css";
import producten from "../Utils/Producten.json";

const ProductContainer = () => {
  return (
    <>
      <div className={Style.mainContainer}>
        <div className={Style.productContainer}>
          {producten.map((p) => (
            <ProductCard key={p.id} product={p} />
          ))}
        </div>
      </div>
    </>
  );
};

export default ProductContainer;
