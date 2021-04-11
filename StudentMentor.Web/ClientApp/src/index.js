import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import configureAxios from "./services/axiosConfiguration";
import App from "./App";
import BrowserHistoryWrapper from "./utils/BrowserHistoryWrapper";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const rootElement = document.getElementById("root");

configureAxios();

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <BrowserHistoryWrapper>
      <App />
    </BrowserHistoryWrapper>
  </BrowserRouter>,
  rootElement
);
