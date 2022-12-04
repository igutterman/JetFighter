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

var f16 = new Image(113, 150);
f16.src = "images/f16.svg";

var bullet = new Image(15, 15);
bullet.src = "images/bullet.svg";

var animationStarted = false;

var keysMap = {};

function getDummyState() {
    connection.invoke("SendDummyState");
}

connection.on("ReceiveGameState", function (state) {
    //console.log(state);
    gameState = state;
    //console.log(gameState.jets.length);
    //drawState(gameState);
    if (!animationStarted) {
        console.log("here");
        draw();
        animationStarted = true;
    }
    
});

document.addEventListener("keydown", (event) => {

    if (!animationStarted) {
        return;
    }

    if (event.keyCode === 65 || event.keyCode === 37) {
        //connection.invoke("TurnLeft", game);
        keysMap["left"] = true;
    }

    if (event.keyCode === 68 || event.keyCode === 39) {
        keysMap["right"] = true;
        //connection.invoke("TurnRight", game);
    }

    if (event.keyCode === 32) {
        keysMap["shoot"] = true;
        //connection.invoke("Shoot", game);
    }

    if (keysMap["left"] === true) {
        connection.invoke("TurnLeft", game);
    }
    if (keysMap["right"] === true) {
        connection.invoke("TurnRight", game);
    }
    if (keysMap["shoot"] === true) {
        connection.invoke("Shoot", game);
    }

})

document.addEventListener("keyup", (event) => {

    if (!animationStarted) {
        return;
    }

    if (event.keyCode === 65 || event.keyCode === 37) {
        keysMap["left"] = false;
    }

    if (event.keyCode === 68 || event.keyCode === 39) {
        keysMap["right"] = false;
    }

    if (event.keyCode === 32) {
        keysMap["shoot"] = false;
    }
    

})



function startGame() {

    if (game === null) {
        alert("Failed to start game: You are not in a game room!");
        return;
    }

    connection.invoke("StartGame", game);
}

connection.on("NotifyPlayerLeft", function (playerID) {
    alert("player left");
})

var fps = 100;
function draw() {
    setTimeout(function () {
        requestAnimationFrame(draw);
        drawState(gameState);
    }, 1000 / fps); 
}

function drawState(state) {
    ctx.clearRect(0, 0, 1000, 1000);
    ctx.fillStyle = "rgba(0, 191, 255, 0.95)";
    ctx.fillRect(0, 0, 1000, 1000);
    //console.log(state);



    for (let i = 0; i < state.jets.length; i++) {
        //eval('var jet' + i + ' =  state.jets[i]');


        let jet = state.jets[i];
        //console.log(jet);
        drawFour(jet);
        drawBullets(jet);

    }
    //requestAnimationFrame(drawState);

}

function drawRotated(img, x, y, angle) {

    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(angle);
    ctx.drawImage(img, -img.width / 4, -img.height / 4, img.width / 2, img.height / 2)
    ctx.restore()

}


function drawFour(jet) {

    //console.log("drawfour called");

    let img;

    if (jet.jetID === 1) {
        img = mig;
    } else if (jet.jetID === 2) {
        img = f16;
    }



    let x = jet.x;
    let y = jet.y;
    let angle = jet.angle;

    angle += Math.PI / 2;
    if (angle > Math.PI) {
        angle -= 2 * Math.PI;
    }



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

function drawBullets(jet) {

    let img = bullet;

    for (let i = 0; i < jet.bullets.length; i++) {
        drawRotated(img, jet.bullets[i].x, jet.bullets[i].y, jet.bullets[i].angle);
    }


}