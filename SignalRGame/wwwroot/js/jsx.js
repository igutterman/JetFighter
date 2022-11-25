"use strict";
import { HubConnection } from '~/js/signalr/dist/browser/signalr.js';

function MyApp() {
    return <h1>Hello, world!</h1>;
}

const container = document.getElementById('root');
const root = ReactDOM.createRoot(container);

//const hubConnection = new HubConnection('http://localhost:5134/chatHub');
root.render(<MyApp />);
