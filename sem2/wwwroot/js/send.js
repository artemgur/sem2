var connection = null

document.onload = function (){
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    connection.on('Receive', function (message, userName) {
        document.getElementById("message_container").innerHTML += `<div class="message"><div class="content">${message}</div></div>`
    });
}

function send() {
    name =  document.forms["first"].elements["name"].value ;

    message =  document.forms["first"].elements["mess"].value ;

    connection.invoke("Send", name, message).catch(function (err) {
        return console.error(err.toString());
    });

    $.post( "https://dwweb.ru/__a-data/__all_for_scripts/__examples/js/chat/submit.php",  { name,mess,},

        function( data ) { $( ".show_rezult" ).html(data); }

    );
    return false;
}