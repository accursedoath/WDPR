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

    var chattype = document.getElementById("chattype").value;
    if(chattype == "privechat"){
        fillchat(chattype);
    }
    else {  //Groepschat route
        fillchat("groupchat");
        var groepsnaam = document.getElementById("groepnaam").value;
        connection.invoke("AddToGroup", groepsnaam).catch(function (err) {
            return console.error(err.toString());
        });
    }
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});



// document.getElementById(uniquebutton).addEventListener("click", function (event) {
//     var groepsnaam = document.getElementById("groepsnaam").value; //world trigger 
//     connection.invoke("AddToGroup", groepsnaam).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });

function sendGroupMessage(){
    var groepsnaam = document.getElementById("groepnaam").value;
    var message = document.getElementById("messageInput").value;
    var groepid = document.getElementById("groepid").value;
    console.log("Fat neek");
    console.log(message + "    " + groepsnaam + "     " + groepid);
    connection.invoke("gm", groepsnaam, groepid, message).catch(function (err) {
        return console.error(err.toString());
    });
}

function sendPm(){
        var message = document.getElementById("messageInput").value;
       let chatid = document.getElementById("chattid").value;
       var verstuurder = document.getElementById("verstuurder").value;
       connection.invoke("pm", verstuurder, message, chatid).catch(function (err) {
           return console.error(err.toString());
       });
}

// function JoinGroep(groepsnaam){
//     console.log(groepsnaam);
//     connection.invoke("AddToGroup", groepsnaam).catch(function (err) {
//         return console.error(err.toString());
//     });
// }

// function JoinGroep($this){
//     var groepsnaam = $this.id;
//     console.log(groepsnaam);
//     connection.invoke("AddToGroup", groepsnaam).catch(function (err) {
//         return console.error(err.toString());
//     });
// }

// document.getElementById("sendButton").addEventListener("click", function (event) {
//      var message = document.getElementById("messageInput").value;
//     let chatid = document.getElementById("chattid").value;
//     var verstuurder = document.getElementById("verstuurder").value;
//     connection.invoke("pm", verstuurder, message, chatid).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });

function fillchat(chattype){

    if(chattype == "privechat"){
        var chatid = document.getElementById("chattid").value;
        fetch('https://localhost:5001/api/BerichtApi/all/' + chatid)  //dit is essentially een get request
        .then(response => response.json())
        .then(data => {
            for(let x = 0; x < data.length; x++){
                var exactTime = data[x].datum
                var strinag = exactTime.substring(11, 16) + "    " + exactTime.substring(0, 9)
            
                var verzenderli = document.createElement("li");
                var textli = document.createElement("li");
                var tijdli = document.createElement("li");
                var empt = document.createElement("hr");
            
                document.getElementById("messagesList").appendChild(empt);
                document.getElementById("messagesList").appendChild(verzenderli);
                document.getElementById("messagesList").appendChild(textli);
                document.getElementById("messagesList").appendChild(tijdli);

                let verzender =  data[x].verzender.voornaam;
                var bericht = data[x].text
                var tijd = strinag
                //var ece = "";
            
                verzenderli.textContent = `${verzender}`;
                textli.textContent = `${bericht}`;
                tijdli.textContent = `${tijd}`; 
                //empt.textContent = `${ece}`;
            }
        });
    }
    else {
        var groepid = document.getElementById("groepid").value;
        fetch('https://localhost:5001/api/BerichtApi/allGroup/' + groepid)  //dit is essentially een get request
        .then(response => response.json())
        .then(data => {
            for(let x = 0; x < data.length; x++){
                var exactTime = data[x].datum
                var strinag = exactTime.substring(11, 16) + "    " + exactTime.substring(0, 9)
            
                var verzenderli = document.createElement("li");
                var textli = document.createElement("li");
                var tijdli = document.createElement("li");
                var empt = document.createElement("li");
            
                document.getElementById("messagesList").appendChild(empt);
                document.getElementById("messagesList").appendChild(verzenderli);
                document.getElementById("messagesList").appendChild(textli);
                document.getElementById("messagesList").appendChild(tijdli);
            
                let verzender =  data[x].verzender.voornaam;
                var bericht = data[x].text
                var tijd = strinag
                var ece = "";
            
                verzenderli.textContent = `${verzender}`;
                textli.textContent = `${bericht}`;
                tijdli.textContent = `${tijd}`;
                empt.textContent = `${ece}`;
            }
        });
    }

}

function sendChat(x){

}

