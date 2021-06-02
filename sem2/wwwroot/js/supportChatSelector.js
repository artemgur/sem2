async function run(){
    connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
    }
    connection.invoke("GetNonAnsweredUsers").then(function (users) {
        let select = document.getElementById("chat_selector")
        $.each(users, function () {
            let userId = this;
            console.log(userId)
            select.innerHTML += `<option value="${userId}">${userId}</option>`
        });
    });
    // $.connection.hub.stop().then(function() { //TODO doesn't work, fix
    //     console.log('stopped');
    // });
}