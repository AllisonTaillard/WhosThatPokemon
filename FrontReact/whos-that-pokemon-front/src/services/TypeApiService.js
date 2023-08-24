import axios from 'axios';
import { __BASE_URL, __HEADERS } from '../helpers/constants';

export const getAllTypes = (async () => {
    return await axios(__BASE_URL + "/types", {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const getTypeById = (async (id) => {
    return await axios(__BASE_URL + "/type/" + id, {
        method: "get",
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const updateType = (async (type) => {
    let bodyFormData = new FormData();
    bodyFormData.append("name", type.name);

    return await axios({
        method: "put",
        url: __BASE_URL + "/type/" + type.id,
        data: bodyFormData,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const deleteType = (async (id) => {
    return await axios({
        method: "delete",
        url: __BASE_URL + "/type/" + id,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

export const addType = (async (type) => {
    let bodyFormData = new FormData();
    bodyFormData.append("name", type.name);

    return await axios({
        method: "post",
        url: __BASE_URL + "/type",
        data: bodyFormData,
        headers: __HEADERS
    })
});