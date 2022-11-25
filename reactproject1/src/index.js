import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

import { HubConnection, HubConnectionBuilder, HttpTransportType } from "@microsoft/signalr";


const hubConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7134/chatHub")
    .withAutomaticReconnect()
    .build();

hubConnection.on("ReceiveMessage", function (user, message) {
    console.log(`${user} says ${message}`);
});

hubConnection.start().catch (function (err) {
    return console.error(err.toString());
});

hubConnection.invoke("SendMessage", "hello").catch(function (err) {
    return console.error(err.toString());
});



const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
