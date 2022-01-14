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
    
    fetch('https://localhost:5001/Chat')  //dit is essentially een get request
    .then(response => response.json())
    .then(data => console.log(data));
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}`;
    lis.textContent = `${message}`;
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

async function PostIt(){
    var chatmessage = document.getElementById("messageInput").value;
    var user = document.getElementById("userInput").value;

    var today = new Date();
    var date = today.getFullYear()+'-'+(today.getMonth()+1)+'-'+today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    var dateTime = date+' '+time;

    await postData('https://localhost:5001/api/Bericht/', {Verzender : user, text : chatmessage, Datum = dateTime });
    await getTable();
}

async function postData(url = '', data = {}) {
    const response = await fetch(url, {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
  }
