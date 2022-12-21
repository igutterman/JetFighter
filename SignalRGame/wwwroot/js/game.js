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

var crashAudio = new Audio("images/jetCrash.mp3");

//gun audio depends on var playerNum from chat.js
var ownGun = new Audio("images/ownGun.mp3");
var enemyGun = new Audio("images/enemyGun.mp3");



//scaling
//canvas scaling is in receivegamestate function

let windowheight = window.innerHeight;
let windowwidth = window.innerWidth;

var healthLevel1 = 100;
var healthLevel2 = 100;

window.onload = function (e) {

    document.getElementById("GameContainer").style.height = windowheight * 0.8;
    document.getElementById("GameContainer").style.width = windowwidth;

    let containerheight = document.getElementById("GameContainer").style.height;
    let containerwidth = document.getElementById("GameContainer").style.width;

    containerheight = windowheight * 0.8;
    containerwidth = windowwidth;

    let scale;
    if (containerheight >= 1000 && containerwidth >= 1000) {
        scale = 1;
    } else if (containerheight >= 1000) {
        scale = containerwidth / 1000;
    } else if (containerwidth >= 1000) {
        scale = containerheight / 1000;
    } else {
        scale = Math.max(containerheight, containerwidth) / 1000;
    }
    console.log("scaled by");
    console.log(scale);



    canvas.style.height = 1000 * scale;
    canvas.style.width = 1000 * scale;


    document.getElementById("health-level-1").style.width = healthLevel1 + "%";
    document.getElementById("health-level-2").style.width = healthLevel2 + "%";

    //crashAudio.play();
}





var mig = new Image(109, 150);
mig.src = "images/mig.svg";

var f16 = new Image(113, 130);
f16.src = "images/f16.svg";

var jetExp1 = new Image(154, 150);
jetExp1.src = "images/jetExp1.svg";

var jetExp2 = new Image(140, 130);
jetExp2.src = "images/jetExp2.svg";

var jetExp3 = new Image(110, 104);
jetExp3.src = "images/jetExp3.svg";

var jetExp4 = new Image(90, 90);
jetExp4.src = "images/jetExp4.svg";

var jetExp5 = new Image(75, 77);
jetExp5.src = "images/jetExp5.svg";

var jetExp6 = new Image(40, 38);
jetExp6.src = "images/jetExp6.svg";

var bullet1 = new Image(15, 15);
bullet1.src = "images/rBullet1.svg";

var bullet2 = new Image(15, 15);
bullet2.src = "images/rBullet2.svg";


var bulletExp1 = new Image(30, 30);
bulletExp1.src = "images/jetExp5.svg";

var bulletExp2 = new Image(20, 20);
bulletExp2.src = "images/jetExp6.svg";

var animationStarted = false;

var keysMap = {};

function getDummyState() {
    connection.invoke("SendDummyState");
}


function sendKeys() {

    if (keysMap["left"] === true) {
        connection.invoke("TurnLeft", game);
    }
    if (keysMap["right"] === true) {
        connection.invoke("TurnRight", game);
    }
    if (keysMap["shoot"] === true) {
        connection.invoke("Shoot", game);
    }

}

window.setInterval(function () {
    if (animationStarted) {
        sendKeys();
    }

}, 40);


connection.on("ReceiveGameState", function (state) {

    gameState = state;

    if (!animationStarted) {
        console.log("here");

        //ctx.scale(scale, scale);

        draw();
        animationStarted = true;
    }


    if (state.playCrashAudio === true) {
        crashAudio.play();
    }

    if (state.jetOneFired === true) {
        if (playerNum === 1) {
            ownGun.play();
        } else {
            enemyGun.play();
        }
    }

    if (state.jetTwoFired === true) {
        if (playerNum === 2) {
            ownGun.play();
        } else {
            enemyGun.play();
        }
    }
    
});

document.addEventListener("keydown", (event) => {

    if (!animationStarted) {
        return;
    }

    if (event.keyCode === 65 || event.keyCode === 37) {

        keysMap["left"] = true;
    }

    if (event.keyCode === 68 || event.keyCode === 39) {
        keysMap["right"] = true;

    }

    if (event.keyCode === 32) {
        keysMap["shoot"] = true;

    }

    event.preventDefault();

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

document.getElementById("startGameButton").addEventListener("click", function () {
    startGame();
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
    console.log(state);


    for (let i = 0; i < state.jets.length; i++) {


        let jet = state.jets[i];
        //console.log("here222");
        //console.log(jet);

        //update health bars
        if (jet.jetID === 1) {
            healthLevel1 = jet.health;
            document.getElementById("health-level-1").style.width = healthLevel1 + "%";
        } else {
            healthLevel2 = jet.health;
            document.getElementById("health-level-2").style.width = healthLevel2 + "%";
        }


        //change this so it keeps drawing bullets until they are deleted
        if (jet.drawState < 60) {
            //console.log("here1111");
            drawFour(jet);
            drawBullets(jet);
        }

    }


}

function drawRotated(img, x, y, angle) {

    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(angle);
    ctx.drawImage(img, -img.width / 4, -img.height / 4, img.width / 2, img.height / 2)
    ctx.restore()

}

function round(num) {

    return (0.5 + num) << 0;


}


function drawFour(jet) {

    console.log("drawfour called");

    let img;

    if (jet.jetID === 1 && jet.drawState === 0) {
        img = mig;
        console.log("case 1");
    } else if (jet.jetID === 2 && jet.drawState === 0) {
        img = f16;
    } else if (jet.drawState > 0 && jet.drawState < 10) {
        img = jetExp1;

    } else if (jet.drawState > 10 && jet.drawState < 20) {
        img = jetExp2;
    } else if (jet.drawState > 20 && jet.drawState < 30) {
        img = jetExp3;
    } else if (jet.drawState > 30 && jet.drawState < 40) {
        img = jetExp4;
    } else if (jet.drawState > 40 && jet.drawState < 50) {
        img = jetExp5;
    } else if (jet.drawState > 50 && jet.drawState < 60) {
        img = jetExp6;
    }



    let x = jet.x;
    let y = jet.y;
    let angle = jet.angle;

    //round x and y to get rid of decimals
    x = round(x);

    y = round(y);

    console.log(x);
    console.log(y);


    //On server 0 angle points 90 degrees right, so we have to rotate the plane for display
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

    //let img = bullet;



    for (let i = 0; i < jet.bullets.length; i++) {


        let img;
        if (jet.jetID === 1) {
            img = bullet1;
        } else if (jet.jetID === 2) {
            img = bullet2;
        }

        if (jet.bullets[i].drawState > 0 && jet.bullets[i].drawState <= 10) {
            img = bulletExp1;
        } else if (jet.bullets[i].drawState > 10 && jet.bullets[i].drawState < 20) {
            img = bulletExp2;
        }



        let x = jet.bullets[i].x;
        let y = jet.bullets[i].y;
        let angle = jet.bullets[i].angle;

        //round x and y to get rid of decimals
        x = round(x);

        y = round(y);




        drawRotated(img, x, y, angle);
    }


}