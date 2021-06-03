$(async ()=>{
    $('input[type="file"]').change(function (){
        let $this = $(this);
        $this.parent().find('.custom-file-label').text($this[0].files[0].name);
    });
    
    window.optionEntries = {};
    window.permissions = await getPermissions();
    
    let $permissionContainer = $('#permissions-container');
    let optionField = getOptionFieldElement(0, $permissionContainer)
    optionField.find('.minus').remove();
    $permissionContainer.append(optionField);
    revalidateForm();
})

async function getPermissions(){
    let response = await fetch(window.location.origin + '/adminpanel/permissions',
        {
                headers: {
                    'Accept': 'application/json'
                },
                credentials: "include"
            });
    
    let result = [];
    if(response.ok)
    {
        result = await response.json();
    }   
    
    return result;
}

function getOptionFieldElement(index, $additionalInfo){
    let html = `
            <div class="option-field">
                <div class="autocomplete w-100">
                    <input type="text" class="form-control" placeholder="Текст опции" name="Permissions" maxlength=64 data-val="true" data-val-required="Текст опции не может быть пустым"/>
                </div>
                <div class="icon-tab">
                    <div class="plus">
                        <div class="m-0">
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-plus" viewBox="0 0 16 16">
                              <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
                            </svg>
                        </div>
                    </div>
                    <div class="minus">
                        <div class="m-0">
                            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-dash" viewBox="0 0 16 16">
                                <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8z"/>
                            </svg>
                        </div>
                    </div>
                </div>
            </div>
        `

    let result = $(htmlToElement(html));
    autocomplete(result.find('input')[0], permissions);
    
    result.find('.plus').click(function (){
        optionEntries[index]++;
        $additionalInfo.append(getOptionFieldElement(index, $additionalInfo))
        revalidateForm();
    });

    result.find('.minus').click(function (){
        result.remove();
        revalidateForm();
    });

    return result;
}

function revalidateForm(){
    let form = $('#form')
        .removeData("validator") /* added by the raw jquery.validate plugin */
        .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin*/

    $.validator.unobtrusive.parse(form);
}