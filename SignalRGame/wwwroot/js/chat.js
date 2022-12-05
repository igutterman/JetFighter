"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//set back to null when leave game?
var game = null;

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveAddToGameResponse", function (responseMessage, success, gameName) {
    alert(responseMessage);

    //change response type to bool, check if true
    if (success === true) {
        game = gameName;
    }
    
    event.preventDefault();
});

function AddToGamesList(gameName) {
    var li = document.createElement("li");
    var btn = document.createElement("button");
    btn.textContent = "Join Game";
    btn.className = "joinGameButton";
    btn.value = gameName;
    btn.onclick = function () {
        console.log("join button clicked");

        //let form = document.getElementById("joinRoomForm");
        //let input = document.getElementById("joinRoomInput");
        //input.value = roomName;


        //form.submit();

        connection.invoke("AddPlayerToGame", gameName);
        
        event.preventDefault();
    }
    document.getElementById("roomsList").appendChild(li);

    li.id = `${gameName + "li"}`;

    li.textContent = `${gameName}`;
    var buttonDiv = document.createElement("div");
    buttonDiv.className = "buttonDiv";
    li.appendChild(buttonDiv);
    console.log(buttonDiv);
    buttonDiv.appendChild(btn);
}



function RemoveFromGamesList(gameName) {
    //console.log("removing game: " + gameName)
    let game = document.getElementById(gameName + "li");
    game.remove();
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

    BuildGamesList();
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

connection.on("AddGame", function (gameName) {
    AddToGamesList(gameName);
    //AddChatRoom(roomName);
});

connection.on("RemoveGame", function (gameName) {
    RemoveFromGamesList(gameName);
})

document.getElementById("addRoomButton").addEventListener("click", function (event) {
    var gameName = document.getElementById("roomNameInput").value;
    connection.invoke("CreateGame", gameName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


function BuildGamesList() {

    connection.invoke("PassGamesList").catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveGamesList", function (gamesList) {

        console.log("receivegameslist received");
        console.log(gamesList);

        for (const [key, value] of Object.entries(gamesList)) {
            console.log(key, value);
            AddToGamesList(key);
        }

    });


}
