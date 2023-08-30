import React, { useState, useEffect } from 'react';
import "./AuthenticationView.css";
import { login, setAuthToken } from '../../services/AuthenticationService';
import { addUser } from '../../services/UserApiService';
import { hasEnoughUpperLowerChars, hasEnoughChars, areSame, hasEnoughSpecialChars, hasEnoughDigits } from '../../helpers/regexValidators';

const AuthenticationView = ({ isNew, errorMessage }) => {
    // States pour les messages d'erreur à afficher lors de l'authentification
    const [emptyFieldsError, setEmptyFieldsError] = useState(false);
    const [passwordConfirmationError, setPasswordConfirmationError] = useState(false);
    const [upperLowercaseMissingError, setUpperLowercaseMissingError] = useState(false);
    const [digitMissingError, setDigitMissingError] = useState(false);
    const [specialcharMissingError, setSpecialcharMissingError] = useState(false);
    const [passwordLengthError, setPasswordLengthError] = useState(false);

    useEffect(() => {
        const clearErrorMessages = () => {
            setEmptyFieldsError(false);
            setPasswordConfirmationError(false);
            setUpperLowercaseMissingError(false);
            setDigitMissingError(false);
            setSpecialcharMissingError(false);
            setPasswordLengthError(false);
        }

        clearErrorMessages();
    }, [isNew]);

    function passwordChanged() {
        // Récupérer les champs 
        const password = document.getElementById("password").value;
        const passwordConfirmation = document.getElementById("confirmation-password").value;

        // Vérifications de sécurité du password si le user se trouve sur la page d'inscription
        if (!areSame(password, passwordConfirmation)) setPasswordConfirmationError(true); else setPasswordConfirmationError(false);
        if (!hasEnoughChars(password)) setPasswordLengthError(true); else setPasswordLengthError(false);
        if (!hasEnoughUpperLowerChars(password)) setUpperLowercaseMissingError(true); else setUpperLowercaseMissingError(false);
        if (!hasEnoughSpecialChars(password)) setSpecialcharMissingError(true); else setSpecialcharMissingError(false);
        if (!hasEnoughDigits(password)) setDigitMissingError(true); else setDigitMissingError(false);
    }

    function passwordConfirmationChanged(e) {
        // Récupérer les champs 
        const password = document.getElementById("password").value;
        const passwordConfirmation = document.getElementById("confirmation-password").value;

        // Vérifier la correspondance entre les 2 passwords rentrés
        if (!areSame(password, passwordConfirmation)) setPasswordConfirmationError(true); else setPasswordConfirmationError(false);
    }

    async function register() {
        const password = document.getElementById("password").value;
        const passwordConfirmation = document.getElementById("confirmation-password").value;
        const pseudo = document.getElementById("pseudo").value;
        try {
            if (pseudo == "" || password == "" || passwordConfirmation == "") {
                setEmptyFieldsError(true);
                alert("Merci de remplir tous les champs");
            }
            else {
                // Si les champs sont remplis
                setEmptyFieldsError(false);

                // Autres vérifs
                if (hasEnoughChars(password) && areSame(password, passwordConfirmation) && hasEnoughSpecialChars(password) && hasEnoughDigits(password)) {
                    // Si les autres vérifs sont passées
                    // Récupération des entrées utilisateur
                    const user = { pseudo, password };

                    // inscription
                    const registerResponse = await addUser(user);

                    // login automatique
                    if (registerResponse.status === 200 || registerResponse.status === 201)
                        await tryLogin();
                }
                else
                    alert("Le mot de passe rentré ne répond pas aux normes de sécurité");
            }
        } catch (err) {
            console.log(err); // mettre alert bootstrap de 5 secondes
        }
    }

    async function tryLogin() {
        try {
            // Récupération des entrées utilisateur
            const password = document.getElementById("password").value;
            const pseudo = document.getElementById("pseudo").value;
            const user = { pseudo, password };

            // login et mettre le token dans le header axios
            const response = await login(user);
            setAuthToken(response.data.token);

            // refresh local storage et state du user actif
            localStorage.setItem("token", response.data.token);
            localStorage.setItem("id", response.data.user.id);

            // redirection + setActiveUser après redirection
            window.location.reload();
        } catch (err) {
            console.log(err); // mettre alert bootstrap de 5 secondes
        }
    }

    return (
        <div>
            {/* Message d'erreur après redirection dans certains cas */}
            {
                errorMessage ? <div class="alert alert-danger mb-5 mt-5" role="alert">{errorMessage}</div> : null
            }

            <div className="mb-3">
                <label htmlFor="pseudo" className="form-label">Pseudo*</label>
                <input type="text" className="form-control" id="pseudo" placeholder={isNew ? "Choisissez un pseudo" : "Entrez votre pseudo"} />
            </div>
            <div className="mb-3">
                <label htmlFor="password" className="form-label">Mot de passe*</label>
                <input type="password" className="form-control" id="password" onChange={() => isNew ? passwordChanged() : null} placeholder={isNew ? "Choisissez un mot de passe sécurisé" : "Entrez votre mot de passe"} />
                {upperLowercaseMissingError ? <li><small className="form-text text-danger">Le mot de passe doit avoir au moins une majuscule et deux minuscules !</small></li> : null}
                {digitMissingError ? <li><small class="form-text text-danger">Le mot de passe doit avoir au moins deux chiffres ! </small></li> : null}
                {specialcharMissingError ? <li><small className="form-text text-danger">Le mot de passe doit avoir au moins un caractère spécial ! </small></li> : null}
                {passwordLengthError ? <li><small className="form-text text-danger">Le mot de passe doit comporter entre 8 et 15 caractères !</small></li> : null}
            </div>
            {
                isNew ?
                    <div className="mb-3">
                        <label htmlFor="confirmation-password" className="form-label">Confirmation du mot de passe*</label>
                        <input type="password" className="form-control" id="confirmation-password" onChange={() => passwordConfirmationChanged()} placeholder="Retapez le même mot de passe" />
                        {passwordConfirmationError ? <li><small id="emailHelp" className="form-text text-danger">Les mots de passe ne correspondent pas !</small></li> : null}
                    </div>
                    :
                    null
            }
            <div className="mb-3">
                <button onClick={() => isNew ? register() : tryLogin()} className="btn btn-primary form-control">{isNew ? "S'inscrire" : "Se connecter"}</button>
                {emptyFieldsError ? <small id="emailHelp" class="form-text text-danger">Veuillez remplir tous les champs !</small> : null}
            </div>
        </div>
    );
}

export default AuthenticationView;