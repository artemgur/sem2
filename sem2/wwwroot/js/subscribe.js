$(() => {
    $('*[subscribe]').on('click', async function (){
        let $this = $(this);
        let planId = Number($this.attr('subscribe'));
        await showSubscribeForm(planId);
    })
    
    $('.modal').on('show.bs.modal', function (e){
        $('html').addClass('modal-open');
    });

    $('.modal').on('hide.bs.modal', function (e){
        $('html').removeClass('modal-open');
    });
});

async function showSubscribeForm(planId){
    let form = $('#cardInfo-form');
    let alert = $('#error-container')

    let $subName = $('#subPay-name');
    let $subPrice = $('#subPay-price');
    let $subDuration = $('#subPay-duration');
    
    let resp = await fetch(window.location.origin + `/subscriptionInfo/${planId}`,
        {
                headers: {
                    'Accept': 'application/json'
                },
                credentials: "include"
            });
    
    if(resp.ok) {
        let data = await resp.json();
        $subName.text(data.name);
        $subPrice.text(data.price);
        $subDuration.text(data.duration);


        alert.attr('hidden', 'hidden');
        form.off('submit');
        form.on('submit', async function (e) {
            e.preventDefault();
            alert.attr('hidden', 'hidden');
            let formData = new FormData(form[0]);
            let response = await fetch(window.location.origin + `/subscribe/${planId}`, {
                method: 'POST',
                body: formData
            });
            if (response.ok) {
                window.location.replace(window.location.href);
            } else {
                let errorMessage = await response.text();
                alert.removeAttr('hidden');
                alert.text(errorMessage);
            }
            return false;
        });
        
        $('#pay-modal').modal('toggle');
    }
}