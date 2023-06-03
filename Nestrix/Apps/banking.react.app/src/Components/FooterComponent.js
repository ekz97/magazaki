import React from 'react'
import './FooterComponent.css'
import House from '../images/house.svg'
import Trans from '../images/trans.svg'
import QR from '../images/qr.svg'

const FooterComponent = () => {
  return (
    <div className='footer-container'>
        <img src={QR} alt='qr'/>
        <img src={House} alt='house'/>
        <img src={Trans} alt='trans'/>
    </div>
  )
}

export default FooterComponent