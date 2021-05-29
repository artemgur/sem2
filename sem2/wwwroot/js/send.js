function send() {
    name =  document.forms["first"].elements["name"].value ;

    mess =  document.forms["first"].elements["mess"].value ;

    $.post( "https://dwweb.ru/__a-data/__all_for_scripts/__examples/js/chat/submit.php",  { name,mess,},

        function( data ) { $( ".show_rezult" ).html(data); }

    );
    return false;
}