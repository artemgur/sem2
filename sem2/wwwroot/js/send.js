var connection = null

async function init() {
    connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    connection.on('Receive', function (message, userName) {
        add_message_to_page(userName, message)
    });
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        //setTimeout(start, 5000);
    }
}

function send() {
    //name =  document.forms["first"].elements["name"].value;

    message =  $("#message_input").val();

    connection.invoke("Send", message).catch(function (err) {
        return console.log(err.toString());
    });
    //add_message_to_page(name, message)
    
    return false;
}

function add_message_to_page(name, message){
    document.getElementById("message_container").innerHTML += `<div class="message"><div class="content">${message}</div></div>`
}

// function sendAdmin(userId) {
//     name =  document.forms["first"].elements["name"].value ;
//
//     message =  document.forms["first"].elements["mess"].value ;
//
//     connection.invoke("SendAdmin", message, userId).catch(function (err) {
//         return console.error(err.toString());
//     });
//     add_message_to_page(name, message)
//
//     return false;
// }

function adminInit(userId){
    connection.invoke("Listen", userId).catch(function (err) {
        return console.log(err.toString());
    });
}

window.addEventListener ?
    window.addEventListener("load",init,false)
    :
    window.attachEvent && window.attachEvent("onload",init);