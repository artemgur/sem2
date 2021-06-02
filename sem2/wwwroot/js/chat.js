var connection = null

async function init() {
    //TODO кнопка должна становиться активной
    connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    connection.on('Receive', function (message) {
        add_message_to_page(message)
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
    
    add_message_to_page(message)
}

async function disconnect(userId){
    //TODO кнопка должна становиться неактивной
    connection.invoke("Disconnect", userId).catch(function (err) {
        return console.log(err.toString());
    });
}

function add_message_to_page(message){
    document.getElementById("message_container").innerHTML += `<div class="message"><div class="content">${message}</div></div>`
}
