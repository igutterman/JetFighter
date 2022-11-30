"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//set back to null when leave game?
var gameName = null;

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;


function AddToRoomsList(roomName) {
    var li = document.createElement("li");
    var btn = document.createElement("button");
    btn.textContent = "Join Room";
    btn.className = "joinRoomButton";
    btn.value = roomName;
    btn.onclick = function () {
        console.log("join button clicked");

        //let form = document.getElementById("joinRoomForm");
        //let input = document.getElementById("joinRoomInput");
        //input.value = roomName;


        //form.submit();

        connection.invoke("AddPlayerToGame", roomName);
        gameName = roomName;
        event.preventDefault();
    }
    document.getElementById("roomsList").appendChild(li);

    li.id = `${roomName + "li"}`;

    li.textContent = `${roomName}`;
    var buttonDiv = document.createElement("div");
    buttonDiv.className = "buttonDiv";
    li.appendChild(buttonDiv);
    console.log(buttonDiv);
    buttonDiv.appendChild(btn);
}

function RemoveFromRoomsList(roomName) {
    let room = document.getElementById(roomName + "li");
    room.remove();
}

function AddChatRoom(roomName) {

    let chatRooms = document.getElementById("ChatRooms");

    let chatRoom = document.createElement("div");
    chatRooms.appendChild(chatRoom);
    let textSpan = document.createElement("span");
    chatRoom.appendChild(textSpan);
    textSpan.textContent = "Room: " + roomName;
    chatRoom.className = roomName + "ChatRoom";
    let msgList = document.createElement("ul");
    chatRoom.appendChild(msgList);
    msgList.id = roomName;

    let input = document.createElement("input");
    input.type = "text";
    input.className = "col-4";
    input.id = roomName + "input";

    chatRoom.appendChild(input);

    let button = document.createElement("button");
    button.textContent = "Send Message To Room";
    button.className = "RoomMessageButton";
    button.value = roomName;
    button.onclick = function () {
        let room = button.value;
        let message = document.getElementById(roomName + "input").value;
        connection.invoke("SendMessageToGroup", message, room).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }

    chatRoom.appendChild(button);

}

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveGroupMessage", function (message, group) {
    console.log("Group message received");
    console.log(message);
    console.log(group);
    let list = document.getElementById(group);
    let li = document.createElement("li");
    list.appendChild(li);
    li.textContent = `${message}`;
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    BuildRoomsList();
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
    AddToRoomsList(roomName);
    //AddChatRoom(roomName);
});

connection.on("RemoveRoom", function (roomName) {
    RemoveFromRoomsList(roomName);
})

document.getElementById("addRoomButton").addEventListener("click", function (event) {
    var roomName = document.getElementById("roomNameInput").value;
    connection.invoke("CreateRoom", roomName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


function BuildRoomsList() {

    connection.invoke("PassRoomsList").catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveRoomsList", function (roomsList) {

        for (const [key, value] of Object.entries(roomsList)) {
            console.log(key, value);
            AddToRoomsList(key);
        }

    });


}

function GetJetFighter() {
    connection.invoke("SendJetFighter").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceiveJetFighter", function (jet) {
    console.log(jet);
});