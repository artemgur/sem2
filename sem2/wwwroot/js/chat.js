var connection = null

async function init() { //TODO button should appear to disconnect
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

async function send(userId) {
    if (connection === null)
        await init()
    
    let message =  $("#message_input").val();

    connection.invoke("Send", message, userId).catch(function (err) {
        return console.log(err.toString());
    });
    
    return false;
}

async function disconnect(userId){
    
}

function add_message_to_page(name, message){
    document.getElementById("message_container").innerHTML += `<div class="message"><div class="content">${message}</div></div>`
}
