var connection = null

document.onload = function (){
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    connection.on('Receive', function (message, userName) {
        add_message_to_page(userName, message)
    });
}

function send() {
    name =  document.forms["first"].elements["name"].value ;

    message =  document.forms["first"].elements["mess"].value ;

    connection.invoke("Send", message).catch(function (err) {
        return console.error(err.toString());
    });
    add_message_to_page(name, message)
    
    return false;
}

function add_message_to_page(name, message){
    document.getElementById("message_container").innerHTML += `<div class="message"><div class="content">${message}</div></div>`
}

function sendAdmin(userId) {
    name =  document.forms["first"].elements["name"].value ;

    message =  document.forms["first"].elements["mess"].value ;

    connection.invoke("SendAdmin", message, userId).catch(function (err) {
        return console.error(err.toString());
    });
    add_message_to_page(name, message)

    return false;
}