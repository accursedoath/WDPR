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
    li.textContent = `${user}`;
    lis.textContent = `${message}`;
});

connection.start().then(function () {   //start connectie van chat

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

async function sendGroupMessage(){      //het versturen van een groepsbericht
    var groepsnaam = document.getElementById("groepnaam").value;
    var message = document.getElementById("messageInput").value;
    var groepid = document.getElementById("groepid").value;
    console.log("Fat neek");
    console.log(message + "    " + groepsnaam + "     " + groepid);

    applyname();
    await connection.invoke("gm", groepsnaam, groepid, message).catch(function (err) {
        return console.error(err.toString());
    });
    applyhr();

}

async function sendPm(){                //het versturen van een prive bericht
        var message = document.getElementById("messageInput").value;
       let chatid = document.getElementById("chattid").value;
       var verstuurder = document.getElementById("verstuurder").value;

       applyname();
       await connection.invoke("pm", verstuurder, message, chatid).catch(function (err) {
           return console.error(err.toString());
       });
       applyhr();
}

async function fillchat(chattype){      //het laden van eerdere chatberichten S

    if(chattype == "privechat"){        //Prive chat route
        var chatid = document.getElementById("chattid").value;
       await fetch('https://localhost:5001/api/BerichtApi/all/' + chatid)  //dit is essentially een get request
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
    else {                                //Hulpverlener chat route
        var groepid = document.getElementById("groepid").value;
       await fetch('https://localhost:5001/api/BerichtApi/allGroup/' + groepid)  //dit is essentially een get request
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
                // var ece = "";
            
                verzenderli.textContent = `${verzender}`;
                textli.textContent = `${bericht}`;
                tijdli.textContent = `${tijd}`;
                // empt.textContent = `${ece}`;
            }
        });
    }
    applyhr();
}

function applyname(){                           //naam van verstuurde toevoegen met melding
    var voornaam = document.createElement("li");
    var melding = document.createElement("span");
    melding.innerHTML = "https://localhost:5001/GroepsChat/Chat/1/Melding"
    voornaam.appendChild(melding);
    document.getElementById("messagesList").appendChild(voornaam);
    var naam = document.getElementById("naam").value;
    voornaam.textContent = `${naam}`;
}

function applyhr(){
    var hr = document.createElement("hr");
    document.getElementById("messagesList").appendChild(hr);
}

