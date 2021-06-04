async function addToFavorite(filmId){
    let response = await fetch(`/AddToFavorite?filmId=${filmId}`, {
        method: 'POST',
        credentials: 'include',
    });
    let result = await response.json();
}

async function removeFromFavorite(filmId){
    let response = await fetch(`/RemoveFromFavorite?filmId=${filmId}`, {
        method: 'POST',
        credentials: 'include',
    });
    if(response.ok){
        $('#favorites-button').attr('fill', 'red');
    }
    
    let result = await response.json();
    
}