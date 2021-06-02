$(() => {
    let form = $('#cardInfo-form');
    let alert = $('#error-container')
    form.submit(async function (e){
        e.preventDefault();
        let formData = new FormData(form[0]);
        let response = await fetch(window.location.href, {
            method: 'POST',
            body: formData
        });
        if(response.ok){
            let redirectUrl = await response.text();
            window.location.replace(redirectUrl);
        }
        else{
            let errorMessage = await response.text();
            alert.removeAttr('hidden');
            alert.text(errorMessage);
        }
        return false;
    })
});