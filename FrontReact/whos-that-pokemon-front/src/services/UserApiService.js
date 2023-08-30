import axios from 'axios';
import { __BASE_URL, __HEADERS } from '../helpers/constants';

export const getAllUsers = (async () => {
    return await axios(__BASE_URL + "/users", {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const getUserById = (async (id) => {
    return await axios(__BASE_URL + "/user/" + id, {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const updateUser = (async (user) => {
    let bodyFormData = new FormData();
    bodyFormData.append("pseudo", user.pseudo);
    bodyFormData.append("password", user.password);
    bodyFormData.append("level", user.level);
    bodyFormData.append("xp", user.xp);
    bodyFormData.append("isAdmin", user.isAdmin);

    return await axios({
        method: "put",
        url: __BASE_URL + "/user/" + user.id,
        data: bodyFormData,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const deleteUser = (async (id) => {
    return await axios({
        method: "delete",
        url: __BASE_URL + "/user/" + id,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const addUser = (async (user) => {
    let bodyFormData = new FormData();
    bodyFormData.append("pseudo", user.pseudo);
    bodyFormData.append("password", user.password);

    return await axios({
        method: "post",
        url: __BASE_URL + "/user",
        data: bodyFormData,
        headers: __HEADERS
    })
});

export const addPokemonToUser = (async (userId, pokemonId) => {
    return await axios({
        method: "post",
        url: __BASE_URL + `/user/add-existing-type/${userId}/${pokemonId}`,
        headers: __HEADERS
    })
});

export const removePokemonFromUser = (async (userId, pokemonId) =>  {
    return await axios({
        method: "delete",
        url: __BASE_URL + `/user/remove-type/${userId}/${pokemonId}`,
        headers: __HEADERS
    })
});

// la fonction pour le login se trouve dans AuthenticationService.js