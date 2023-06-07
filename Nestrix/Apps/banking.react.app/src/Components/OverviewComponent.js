import React from 'react'
import "./OverviewComponent.css"

const OverviewComponent = (props) => {
  return (
    <div className='overview-container'>
        {props.transactions.map((transaction) => {
          let date = new Date(transaction.datum);

          let month = date.getMonth() + 1;
          let year = date.getFullYear();

          let formattedDate = (month < 10 ? '0' + month : month) + '/' + String(year).slice(-2);

          return (
            <div className='transaction' key={transaction.id}>
              <p>{formattedDate}</p>
              <p>{transaction.mededeling}</p>
              <p>{transaction.bedrag} EUR</p>
            </div>
          )
        })}
    </div>
  )
}

export default OverviewComponent
