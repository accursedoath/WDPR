"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Van li naar Berichten class
//Een for loop dat oude berichten ophaalt en weergeeft

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    // Hier establish je de link tussen cshtml en java, eigenlijk moet hier json gestuurt worden naar een controller
    // Hier moet dan gebruik worden gemaakt van fetch
    var li = document.createElement("li");
    var lis = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    document.getElementById("messagesList").appendChild(lis);
    // fetch('https://localhost:5001/Chat')  //dit is essentially een get request
    // .then(response => response.json())
    // .then(data => console.log(data));
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}`;
    lis.textContent = `${message}`;
});

connection.start().then(function () {
    fillchat();
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// document.getElementById("sendButton").addEventListener("click", function (event) {
//     var user = document.getElementById("userInput").value;
//     var message = document.getElementById("messageInput").value;
//     var userId = document.getElementById("userId").value;
//     connection.invoke("SendMessage", user, message, userId).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });

document.getElementById("sendButton").addEventListener("click", function (event) {
     var message = document.getElementById("messageInput").value;
    let chatid = document.getElementById("chattid").value;
    var verstuurder = document.getElementById("verstuurder").value;
    connection.invoke("pm", verstuurder, message, chatid).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function fillchat(){
    var chatid = document.getElementById("chattid").value;
    fetch('https://localhost:5001/api/BerichtApi/all/' + chatid)  //dit is essentially een get request
    .then(response => response.json())
    .then(data => {
        for(let x = 0; x < data.length; x++){
            console.log(data[x])
            
            var verzenderli = document.createElement("li");
            var textli = document.createElement("li");
            var tijdli = document.createElement("li");

            document.getElementById("messagesList").appendChild(verzenderli);
            document.getElementById("messagesList").appendChild(textli);
            document.getElementById("messagesList").appendChild(tijdli);

            let verzender =  data[x].verzender.voornaam;
            var bericht = data[x].text
            var tijd = data[x].datum

            verzenderli.textContent = `${verzender}`;
            textli.textContent = `${bericht}`;
            tijdli.textContent = `${tijd}`; 

        }
    });
}

