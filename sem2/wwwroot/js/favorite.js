async function addToFavorite(filmId){
    document.getElementById("favorite_svg").setAttribute("fill", "red")
    document.getElementById("favorite_button").setAttribute("onclick", `removeFromFavorite(${filmId})`)
    let response = await fetch(`/AddToFavorite?filmId=${filmId}`, {
        method: 'POST',
        credentials: 'include',
    });
    let result = await response.json();
}

async function removeFromFavorite(filmId){
    document.getElementById("favorite_svg").setAttribute("fill", "black")
    document.getElementById("favorite_button").setAttribute("onclick", `addToFavorite(${filmId})`)
    let response = await fetch(`/RemoveFromFavorite?filmId=${filmId}`, {
        method: 'POST',
        credentials: 'include',
    });
    if(response.ok){
        $('#favorites-button').attr('fill', 'red');
    }
    let result = await response.json();
}