import React from 'react'
import './GraphComponent.css'
import Graph from '../images/graph.svg'

const GraphComponent = () => {
  return (
    <img className='graph-img' src={Graph} alt="Grafiek"/>
  )
}

export default GraphComponent