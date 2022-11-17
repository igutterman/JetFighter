"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("AddRoom", function (roomName) {
    var li = document.createElement("li");
    var btn = document.createElement("button");
    btn.innerHtml = "Join Room";
    btn.class 
    btn.value = roomName;
    btn.onclick = function () {
        var room = btn.value;
        connection.invoke("JoinRoom", room).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
    document.getElementById("roomsList").appendChild(li);
    li.textContent = `${roomName}`;
    li.appendChild(btn);
})

document.getElementById("addRoomButton").addEventListener("click", function (event) {
    var roomName = document.getElementById("roomNameInput").value;
    connection.invoke("CreateRoom", roomName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})