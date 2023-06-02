import React from 'react'
import Style from "./LogInBody.module.css";

const LogInBody = ({ onGoToRegister }) => {
    const handleGoToRegister = (event) => {
        event.preventDefault();
        onGoToRegister();
    };

    return (
        <>
            <div className={Style.pageBody}>
                <div className={Style.loginContainer}>
                    <img className={Style.logo} src="../Images/xtra.svg" alt="" />
                    <h4 className={Style.txt}>Aanmelden</h4>
                    <form action="" className={Style.form}>
                        <div className={Style.inputContainer}>
                            <input className={Style.input} type="email" />
                            <label htmlFor="">E-mail of Xtra-kaartnummer</label>
                            <span className={Style.tooltip}></span>
                            <p className={Style.tooltipTxt}>Op je Xtra-kaart vind je een 12-cijferige code. Dit is je kaartnummer.</p>
                        </div>
                        <button className={Style.button}>Volgende</button>
                    </form>
                    <div className={Style.containerFooter}>
                        <p>Nieuw bij Xtra? <a href="#" onClick={handleGoToRegister}>Maak je profiel aan</a></p>
                    </div>
                </div>
            </div>
        </>
    )
}

export default LogInBody