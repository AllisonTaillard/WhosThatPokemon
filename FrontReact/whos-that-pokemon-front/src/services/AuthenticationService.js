import axios from 'axios';
import { __BASE_URL, __HEADERS } from '../helpers/constants';

export const login = (async (loginRequest) => {
    let bodyFormData = new FormData();
    bodyFormData.append('pseudo', loginRequest.pseudo);
    bodyFormData.append('password', loginRequest.password);

    return await axios({
        method: "post",
        url: __BASE_URL + "/login",
        data: bodyFormData,
        headers: __HEADERS
    }).catch(err => {
        alert(err);
        console.log(err);
    })
});

// Mettre le token dans le header axios pour l'authentification
export const setAuthToken = token => {
    if (token) {
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    }
    else
        delete axios.defaults.headers.common["Authorization"];
 }

  // Checker si le user a un JWT dans le localStorage
  export const hasJWT = () => {
    return localStorage.getItem("token") ? true : false;
}

export const logout = () => {
    localStorage.removeItem('id');
    localStorage.removeItem('token');
    window.location.reload();
}