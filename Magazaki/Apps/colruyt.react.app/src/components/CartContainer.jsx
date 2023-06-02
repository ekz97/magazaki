import React, { useState } from 'react';
//import ReactDOM from 'react-dom';
import './CartContainer.css'


const CartContainer = () => {
    const productData = [
        {
            "id": 1,
            "image": "http://dummyimage.com/440x620.png/cc0000/ffffff",
            "name": "Coke - Diet, 355 Ml",
            "price": 120,
            "qty": 1,
        },
        {
            "id": 2,
            "image": "http://dummyimage.com/440x620.png/dddddd/000000",
            "name": "Pork - Hock And Feet Attached",
            "price": 150,
            "qty": 1,
        },
        {
            "id": 3,
            "image": "http://dummyimage.com/440x620.png/cc0000/ffffff",
            "name": "Veal - Jambu",
            "price": 135,
            "qty": 1,
        },
        {
            "id": 4,
            "image": "http://dummyimage.com/440x620.png/dddddd/000000",
            "name": "Almonds Ground Blanched",
            "price": 110,
            "qty": 1,
        },
        {
            "id": 5,
            "image": "http://dummyimage.com/440x620.png/5fa2dd/ffffff",
            "name": "Passion Fruit",
            "price": 80,
            "qty": 1,
        }
    ]

    const [products, SetProducts] = useState(productData);

    // -----Increment Event------
    const increaseQuantity = (i) => {
        SetProducts((preValue) =>
            preValue.map((data, o) => {
                if (i === o) {
                    return {
                        ...data,
                        qty: data.qty + 1
                    };
                }
                return data;
            })
        );
    };

    // -----Decrement Event------
    const decreaseQuantity = (i) => {
        SetProducts((preValue) =>
            preValue.map((data, o) => {
                if (i === o) {
                    if (data.qty > 1) {
                        return { ...data, qty: data.qty - 1 };
                    } else {
                        return data;
                    }
                }
                return data;
            })
        );
    };



    // -----Remove Event------
    const removeFromCart = (i) => {
        if (window.confirm("Are you sure you want to remove into your cart?")) {
            SetProducts(prevCart =>
                prevCart.filter((item, o) => {
                    return i !== o;
                })
            );

        } else {
            // alert('No');
        }
    };


    // -empty-cart--------
    const emptycart = () => {
        if (window.confirm("Remove all items into your cart?")) {
            SetProducts([]);
        } else {
            // alert('No');
        }
    }

    // ------Total Product Incart and Total Price of cart
    const cartTotalQty = products.reduce((acc, data) => acc + data.qty, 0);
    const cartTotalAmount = products.reduce((acc, data) => acc + data.price * data.qty, 0);



    return (

        <div className="row justify-content-center m-0">
            <div className="col-md-8 mt-5 mb-5">
                <div className="card">
                    <div className="card-header p-3">
                        <div className="card-header-flex">
                            <h5 className="text-white m-0">Cart Calculation {products.length > 0 ? `(${products.length})` : ''}</h5>
                            {
                                products.length > 0 ? <button className="btn btn-danger mt-0 btn-sm" onClick={() => emptycart()}><i className="fa fa-trash-alt mr-2"></i><span>Empty Cart</span></button> : ''}
                        </div>
                    </div>
                    <div className="card-body p-0">
                        {
                            products.length === 0 ? <table className="table cart-table mb-0">
                                <tbody>
                                    <tr>
                                        <td colSpan="6">
                                            <div className="cart-empty">
                                                <i className="fa fa-shopping-cart"></i>
                                                <p>Your Cart Is empty</p>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table> :
                                <table className="table cart-table mb-0">
                                    <thead>
                                        <tr>
                                            <th>Action</th>
                                            <th>Product</th>
                                            <th>Name</th>
                                            <th>Price</th>
                                            <th>Qty</th>
                                            <th className="text-right"><span id="amount" className="amount">Total Amount</span></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            products.map((data, index) => {
                                                const { id, image, name, price, qty } = data;
                                                return (
                                                    <tr key={index}>
                                                        <td><button className="prdct-delete" onClick={() => removeFromCart(index)}><i className="fa fa-trash-alt"></i></button></td>
                                                        <td><div className="product-img"><img src={image} alt="" /></div></td>
                                                        <td><div className="product-name"><p>{name}</p></div></td>
                                                        <td>${price}</td>
                                                        <td>
                                                            <div className="prdct-qty-container">
                                                                <button className="prdct-qty-btn" type="button" onClick={() => decreaseQuantity(index)}>
                                                                    <i className="fa fa-minus"></i>
                                                                </button>
                                                                <input type="text" name="qty" className="qty-input-box" value={qty} disabled />
                                                                <button className="prdct-qty-btn" type="button" onClick={() => increaseQuantity(index)}>
                                                                    <i className="fa fa-plus"></i>
                                                                </button>
                                                            </div>
                                                        </td>
                                                        <td className="text-right">${(qty * price).toFixed(0)}</td>
                                                    </tr>
                                                )
                                            })
                                        }
                                    </tbody>
                                    <tfoot className="footerTable">
                                        <tr>
                                            <th colSpan="2">Items in Cart<span className="ml-2 mr-2">:</span><span className="text-danger">{cartTotalQty}</span></th>
                                            <th colSpan="2" className="text-right">Total Price<span className="ml-2 mr-2">:</span><span className="text-danger">$ {cartTotalAmount.toFixed(0)}</span></th>
                                            <th colSpan="2" className="payBtnRow"><button className="payButton ml-2 mr-2">Pay</button></th>
                                        </tr>
                                    </tfoot>
                                </table>
                        }
                    </div>
                </div>
            </div>
        </div>

    );
}


export default CartContainer;