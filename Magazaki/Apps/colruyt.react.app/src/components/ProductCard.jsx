import React from "react";
import { AddCartButton } from "./AddCartButton";
import Style from "./ProductCard.module.css";

const ProductCard = ({ product }) => {
  return (
    <div className={Style.productCard}>
      <div>
        <div className={Style.imgContainer}>
          <img
            className={Style.productImg}
            src={`../Images/producten/${product.id}.jpg`}
            alt={product.naam}
          />
          <img
            className={Style.nutriImg}
            src={`../Images/nutriscore/${product.nutriScore}.svg`}
            alt={product.nutriScore}
          />
        </div>
        <div className={Style.textContainer}>
          <p className={Style.merk}>{product.merk}</p>
          <p className={Style.naam}>{product.naam}</p>
        </div>
      </div>

      <div className={Style.prijsContainer}>
        <div className={Style.prijs}>
          <p className={Style.bedrag}>{`€${product.prijs}`}</p>
          <p className={Style.type}>{product.prijsType}</p>
        </div>
        <AddCartButton id={product.naam} />
      </div>
    </div>
  );
};

export default ProductCard;
