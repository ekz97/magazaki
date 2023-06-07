import React from "react";
import Header from "./Header";
import "./Login.css";
import { Field, Form, Formik } from "formik";

const Login = () => {
  return (
    <div>
      <Header />
      <div className="login-container">
        <div className="form">
          <form noValidate>
            <span>Login</span>

            <input
              type="email"
              name="email"
              placeholder="E-mailadres"
              className="form-control inp_text"
              id="email"
            />

            <input
              type="password"
              name="password"
              placeholder="Wachtwoord"
              className="form-control"
            />

            <button type="submit">Login</button>
          </form>
        </div>
      </div>

      <Formik>
        <Form>
          <Field type="email" name="email"/>
          <Field type="password" name="password"/>
        </Form>
      </Formik>
    </div>
  );
};

export default Login;
