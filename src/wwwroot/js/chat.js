"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build(); //connectie vastleggen met signalr voor de chat

document.getElementById("sendButton").disabled = true;                           //button uit zetten

connection.on("ReceiveMessage", function (user, message, berichtid) {             //als de "ReceiveMessage" method wordt uitgevoerd dan kom je hierin terecht
    var li = document.createElement("li");
    var lis = document.createElement("li");

    document.getElementById("messagesList").appendChild(li);                      //maken van de berichten die verschijnen op de ui
    document.getElementById("messagesList").appendChild(lis);

    li.textContent = `${user}`;
    lis.textContent = `${message}`;

    var uniqueid = "i"+ berichtid;
    if(berichtid != "0"){
        lis.id = uniqueid;
        buildLi(lis, uniqueid, message)
    }

});

connection.start().then(function () {                                            //start connectie van chat

    var chattype = document.getElementById("chattype").value;
    if(chattype == "privechat"){
        fillchat(chattype);                                                       //chat vullen van de prive chat
    }
    else {                                                                        //Groepschat route
        fillchat("groupchat");                                                    //chat vullen van een groepschat
        var groepsnaam = document.getElementById("groepnaam").value;
        connection.invoke("AddToGroup", groepsnaam).catch(function (err) {        //bij elke refresh toevoegen aan de connectie groep van signalr
            return console.error(err.toString());                                 //Zodat je nog in de groep zit nadat je ooit hebt geklikt om erin te komen
        });
    }

    if(document.getElementById("magchatten").value == "ja"){                      //checken of je mag chatten
        document.getElementById("sendButton").disabled = false;
      }
}).catch(function (err) {
    return console.error(err.toString());
});

function buildLi(li, uniqueid, content){                                                              //template voor het bouwen van een li (bericht) dat vaker terugkomt
    li.innerHTML = content + "   " + "<button class = 'btn btn-dark btn-sm' onclick='PostIt(this)' id='"+ uniqueid +"'> Rapporteer</button>"
}                                                                                  //deze li zorgt ervoor dat je een bericht annoniem kan melden

async function sendGroupMessage(){                                                 //het versturen van een groepsbericht
    var groepsnaam = document.getElementById("groepnaam").value;
    var message = document.getElementById("messageInput").value;
    var groepid = document.getElementById("groepid").value;

    applyname();
    await connection.invoke("gm", groepsnaam, groepid, message).catch(function (err) {  //js voert hier een method uit genaamd gm "group message"
        return console.error(err.toString());                                       
    });
    applyhr();
}

async function sendPm(){                                                            //het versturen van een prive bericht
    var message = document.getElementById("messageInput").value;
       let chatid = document.getElementById("chattid").value;
       var verstuurder = document.getElementById("verstuurder").value;

       applyname();
       await connection.invoke("pm", verstuurder, message, chatid).catch(function (err) {
           return console.error(err.toString());
       });
       applyhr();
}

async function fillchat(chattype){                                                  //het laden van eerdere chatberichten S

    if(chattype == "privechat"){                                                    //Prive chat route
        var chatid = document.getElementById("chattid").value;
       await fetch('https://localhost:5001/api/BerichtApi/all/' + chatid)           //dit is een get request voor de chatberichten naar de berichtenApi
        .then(response => response.json())                                          //voor prive chat berichten
        .then(data => {
            for(let x = 0; x < data.length; x++){
                buildMessage(data, x, "private");                                         //template voor het bouwen van een bericht
            }
        });
    }
    else {                                                                          //Groeps chat route
        var groepid = document.getElementById("groepid").value;
       await fetch('https://localhost:5001/api/BerichtApi/allGroup/' + groepid)     //dit is een get request voor de chatberichten naar de berichtenApi
        .then(response => response.json())                                          //voor groepschat berichten
        .then(data => {
            for(let x = 0; x < data.length; x++){
                buildMessage(data, x, "group");                                           //template voor het bouwen van een bericht
            }
        });
    }
    applyhr();
}

function buildMessage(data , x , type){
        var exactTime = data[x].datum
        var strinag = exactTime.substring(11, 16) + "    " + exactTime.substring(0, 9)
    
        var verzenderli = document.createElement("li");                             //berichten maken met behulp van een li 
        var textli = document.createElement("li");
        var tijdli = document.createElement("li");
        var empt = document.createElement("hr");
    
        document.getElementById("messagesList").appendChild(empt);                  //berichten aan de berichtenlijst toevoegen (ul)
        document.getElementById("messagesList").appendChild(verzenderli);           //die staat gedefineerd in de frontend van de chat
        document.getElementById("messagesList").appendChild(textli);                
        document.getElementById("messagesList").appendChild(tijdli);
    
        let verzender =  data[x].verzender.voornaam;                                //hiermee voorkomen we dat user input tot gevaar kan lijden
        var bericht = data[x].text
        var tijd = strinag
    
        verzenderli.textContent = `${verzender}`;                                   //de li's vullen we hier met data
        textli.textContent = `${bericht}`;

        if(type != "private"){                                                      //kijken of het een prive bericht is, zo niet dan beschouwen
            var uniqueid = "b" + data[x].id;                                        //we het als een groepsbericht die je kan rapporteren
            buildLi(tijdli, uniqueid, tijd);                                              //bouwen van een li (hiermee voorkomen we code smells)
        }
        else {
            tijdli.textContent = `${tijd}`; 
        }
}

function applyname(){                                                               //naam van bericht verstuurder toevoegen 
    var voornaam = document.createElement("li");
    document.getElementById("messagesList").appendChild(voornaam);
    var naam = document.getElementById("naam").value;
    voornaam.textContent = `${naam}`;
}

function applyhr(){                                                                 //horizontal line toevoegen na elke bericht
    var hr = document.createElement("hr");
    document.getElementById("messagesList").appendChild(hr);
}

async function PostIt($this){                                                        //bericht id nodig voor bericht ophalen, daarna post naar melding api
    $this.hidden = true;
    var berichtid = $this.id.substring(1)
    var groepnaam = document.getElementById("groepnaam");
    var meldingreden = "anonieme misbruikmelding in groep " + groepnaam;
            postData('https://localhost:5001/api/MisbruikApi/', {melding : meldingreden, berichtid : berichtid });
    }
                                                                                    // Post method template
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

