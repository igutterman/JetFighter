"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var roomName = document.getElementById("roomName").innerHTML;


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    connection.invoke("JoinRoom", roomName).catch(function (err) {
        return console.error(err.toString());
    });

});


connection.on("ReceiveGroupMessage", function (user, message, group) {
    console.log("Group message received");

    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;

});


document.getElementById("sendButton").addEventListener("click", function (event) {

    console.log("sending group message");

    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToGroup", user, message, roomName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function setCell(c, row, col) {

    let cellid = "cell" + row.toString() + col.toString();
    let cell = document.getElementById(cellid);
    cell.innerHTML = c;

}


connection.on("ReceiveTurn", function (c, row, col) {

    setCell(c, row, col);

})

function sendTurn(row, col) {
    connection.invoke("ReceiveTurn", roomName, row, col);
}

connection.on("ReceiveWin", function (c) {
    if (c == 'X') {
        alert("Player 1 wins!");
    } else {
        alert("Player 2 wins!")
    }
})