$(()=>{
    let filmId = Number($('#filmId').val())
    $('button.watch').on('click', async function (e){
        let $this = $(this);
        
        let response = await fetch(window.location.origin + `/api/hasWatchPermission`);
        if(response.ok){
            let result = await response.text();
            if(result === "true")
                window.location.replace(window.location.origin + `/film/${filmId}/watch`)
            else {
                $('#sub-error-modal').modal('toggle');
            }
        }
    })
})