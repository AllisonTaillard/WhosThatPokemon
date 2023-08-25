import React, { useState, useEffect } from 'react';
import './NavbarComponent.css';
import { BrowserRouter, Routes, Route, Outlet, Link } from "react-router-dom";
import { getUserById } from '../../services/UserApiService';
import { hasJWT } from '../../services/AuthenticationService';

const NavbarComponent = () => {
    // State pour le user connecté
    const [activeUser, setActiveUser] = useState({});

    // Quand le NavbarComponent est chargé, on requête l'API pour récupérer les infos nécessaires
    useEffect(() => {

        // Récupérer les données du user et les stocker dans le state
        const fetchActiveUser = async () => {
            let userId = localStorage.getItem('id');
            let tmpUser = await getUserById(userId);
            setActiveUser(tmpUser.data);
        }

        if (hasJWT())
            fetchActiveUser();

        console.log(activeUser);
    }, []);

    return (
        <div>

        </div>
    );
}

export default NavbarComponent;