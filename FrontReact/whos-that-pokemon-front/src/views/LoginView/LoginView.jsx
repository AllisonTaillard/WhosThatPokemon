import React from 'react';
import "./LoginView.css";
import { login, setAuthToken } from '../../services/AuthenticationService';

const LoginView = ({ activeUser, setActiveUser }) => {

    async function tryLogin() {
        try {
            // Récupération des entrées utilisateur
            const pseudo = document.getElementById("pseudo").value;
            const password = document.getElementById("password").value;
            const user = { pseudo, password };

            // login et mettre le token dans le header axios
            const response = await login(user);
            setAuthToken(response.data.token);

            // refresh local storage et state du user actif
            localStorage.setItem("token", response.data.token);
            localStorage.setItem("id", response.data.user.id);
            setActiveUser(response.data.user); 
        } catch (err) {
            alert("Mauvais identifiants"); // mettre alert bootstrap de 5 secondes
        }
    }

    return (
        <div>
            <div className="mb-3">
                <label htmlFor="pseudo" className="form-label">Pseudo</label>
                <input type="text" className="form-control" id="pseudo" placeholder="Votre pseudo" />
            </div>
            <div className="mb-3">
                <label htmlFor="password" className="form-label">Mot de passe</label>
                <input type="password" className="form-control" id="password" placeholder="Votre mot de passe" />
            </div>
            <div className="mb-3">
                <button onClick={() => tryLogin()} className="btn btn-primary form-control">Se connecter</button>
            </div>
        </div>
    );
}

export default LoginView;