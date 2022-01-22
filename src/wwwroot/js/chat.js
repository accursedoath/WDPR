"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Van li naar Berichten class
//Een for loop dat oude berichten ophaalt en weergeeft

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, berichtid) {
    // Hier establish je de link tussen cshtml en java, eigenlijk moet hier json gestuurt worden naar een controller
    // Hier moet dan gebruik worden gemaakt van fetch

    var li = document.createElement("li");
    var lis = document.createElement("li");

    document.getElementById("messagesList").appendChild(li);
    document.getElementById("messagesList").appendChild(lis);

    li.textContent = `${user}`;
    lis.textContent = `${message}`;

    var uniqueid = "i"+ berichtid;
    if(berichtid != "0"){
        lis.id = uniqueid
        lis.innerHTML = message + "   " + "<button class = 'btn btn-dark btn-sm' onclick='PostIt(this)' id='"+ uniqueid +"'> Rapporteer</button>"
    }

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

    if(document.getElementById("magchatten").value == "ja"){
        document.getElementById("sendButton").disabled = false;
      }
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
    else {                                //Groeps chat route
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
                var uniqueid = "b" + data[x].id; 
                tijdli.innerHTML = tijd + "   " + "<button class = 'btn btn-dark btn-sm' onclick='PostIt(this)' id='"+ uniqueid +"'> Rapporteer</button>"
            
                verzenderli.textContent = `${verzender}`;
                textli.textContent = `${bericht}`;
                // tijdli.textContent = `${tijd}`;
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


async function PostIt($this){    //bericht id nodig voor bericht ophalen, daarna post naar melding api
    $this.hidden = true;
    var berichtid = $this.id.substring(1)
    var groepnaam = document.getElementById("groepnaam");
    var meldingreden = "anonieme misbruikmelding in groep " + groepnaam;
            postData('https://localhost:5001/api/MisbruikApi/', {melding : meldingreden, berichtid : berichtid });
    }


   // Post method
   async function postData(url = '', data = {}) {
       console.log(data);
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

