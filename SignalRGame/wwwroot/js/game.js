//on connection to room:
//assign player #


//get canvas element


//draw plane in player's starting position
//draw any other elements (health bar, score counter)

//onreceive "two players are connected":
//activate start button

//onpress "start"
//send start signal

//on receive "start"
//start game

//game loop
//on keypress
//send playerid, key

//on receivestate
//render

//on receive end signal
//display scores
//display or activate play again button
//display or activate close button (closes game interface to go back to server list page)


//on other player disconnected
//set draw other player to false?
//display or activate close button (closes game interface to go back to server list page)

var gameState;

var canvas = document.getElementById("gameCanvas");
var ctx = canvas.getContext('2d');


var mig = new Image(109, 150);
mig.src = "images/mig.svg";


function getDummyState() {
    connection.invoke("SendDummyState");
}

connection.on("ReceiveGameState", function (state) {
    console.log(state);
    gameState = state;
});

function startGame() {

    if (gameName === null) {
        alert("Failed to start game: You are not in a game room!");
        return;
    }

    connection.invoke("StartGame", gameName);
}


function drawState(state) {

    for (let i = 0; i < state.jets.length; i++) {
        eval('var jet' + i + ' =  state.jets[i]');

        let jet = state.jets[i];

    }

}

function drawRotated(img, x, y, angle) {

    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(angle);
    ctx.drawImage(img, -img.width / 4, -img.height / 4, img.width / 2, img.height / 2)
    ctx.restore()

}


function drawFour(player, x, y, angle) {

    let img;

    if (player === 1) {
        img = mig;
    }

    //add else for player 2 and NATO svg

    let x1, y1;

    if (x > 500) {
        x1 = -(1000 - x);
    } else {
        x1 = 1000 + x;
    }

    if (y > 500) {
        y1 = -(1000 - y);
    } else {
        y1 = 1000 + y;
    }

    drawRotated(img, x, y, angle);
    drawRotated(img, x1, y1, angle);
    drawRotated(img, x1, y, angle);
    drawRotated(img, x, y1, angle);

}