import axios from 'axios';
import { __BASE_URL, __HEADERS } from '../helpers/constants';

export const getAllPokemons = (async () => {
    return await axios(__BASE_URL + "/pokemons", {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const getPokemonById = (async (id) => {
    return await axios(__BASE_URL + "/pokemon/" + id, {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const updatePokemon = (async (pokemon) => {
    let bodyFormData = new FormData();
    bodyFormData.append("name", pokemon.name);
    bodyFormData.append("picture", pokemon.picture);

    return await axios({
        method: "put",
        url: __BASE_URL + "/pokemon/" + pokemon.id,
        data: bodyFormData,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const deletePokemon = (async (id) => {
    return await axios({
        method: "delete",
        url: __BASE_URL + "/pokemon/" + id,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const addPokemon = (async (pokemon) => {
    let bodyFormData = new FormData();
    bodyFormData.append("name", pokemon.name);
    bodyFormData.append("picture", pokemon.picture);

    return await axios({
        method: "post",
        url: __BASE_URL + "/pokemon",
        data: bodyFormData,
        headers: __HEADERS
    })
});

export const addTypeToPokemon = (async (pokemonId, typeId) => {
    return await axios({
        method: "post",
        url: __BASE_URL + `/pokemon/add-existing-type/${pokemonId}/${typeId}`,
        headers: __HEADERS
    })
});

export const removeTypeFromPokemon = (async (pokemonId, typeId) =>  {
    return await axios({
        method: "delete",
        url: __BASE_URL + `/pokemon/remove-type/${pokemonId}/${typeId}`,
        headers: __HEADERS
    })
});