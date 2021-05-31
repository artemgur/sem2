async function addToFavorite(filmId){
    let response = await fetch(`/add_to_favorite?filmId=${filmId}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json'
        },
        credentials: 'include',
    });
    let result = await response.json();
}