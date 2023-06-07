import React from 'react'
import Transactions from "../transactions.json"
import "./OverviewComponent.css"

const OverviewComponent = () => {
  return (
    <div className='overview-container'>
        {Transactions.map((transaction) => 
        <div className='transaction'>
            <p>{transaction.date}</p>
            <p>{transaction.store}</p>
            <p>{transaction.amount} EUR</p>
        </div>
        )}
    </div>
  )
}

export default OverviewComponent