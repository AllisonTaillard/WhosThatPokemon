import React, { useState, useEffect } from 'react';
import './NavbarComponent.css';
import { BrowserRouter, Routes, Route, Outlet, Link } from "react-router-dom";
import { getUserById } from '../../services/UserApiService';
import { hasJWT } from '../../services/AuthenticationService';
import 'bootstrap/dist/css/bootstrap.min.css';
import { logout } from '../../services/AuthenticationService';
import HomeView from '../../views/HomeView/HomeView';
import PlayView from '../../views/PlayView/PlayView';
import AuthenticationView from '../../views/AuthenticationView/AuthenticationView';
import RankingView from '../../views/RankingView/RankingView';
import UserProfileView from '../../views/UserProfileView/UserProfileView';
import AdminView from '../../views/AdminView/AdminView';

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
    }, []);

    return (
        <div>
            <BrowserRouter>
                {/* navbar */}
                <nav className="navbar navbar-expand-lg bg-body-tertiary">
                    <div className="container-fluid">
                        <Link className="navbar-brand" to={"/"}>Who's That Pokémon</Link>
                        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="collapse navbar-collapse" id="navbarNavDropdown">
                            <ul className="navbar-nav">
                                <li className="nav-item">
                                    <Link className='nav-link' to={"/"}>Accueil</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className='nav-link' to={"/play"}>Jouer</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className='nav-link' to={"/ranking"}>Classement</Link>
                                </li>
                                {
                                    hasJWT() ?
                                        <li className="nav-item">
                                            <Link className='nav-link' to={"/me"}>{activeUser.pseudo}</Link>
                                        </li>
                                        :
                                        <li className="nav-item">
                                            <Link className='nav-link' to={"/login"}>Connexion</Link>
                                        </li>
                                }
                                {
                                    hasJWT() ?
                                        <li className="nav-item">
                                            <a className='nav-link' onClick={() => logout()}>Déconnexion</a>
                                        </li>
                                        :
                                        <li className="nav-item">
                                            <Link className='nav-link' to={"/registration"}>Inscription</Link>
                                        </li>
                                }
                                {
                                    hasJWT() && activeUser.isAdmin ?
                                        <li className="nav-item">
                                            <Link className='nav-link' to={"/admin"}>Admin</Link>
                                        </li>
                                        :
                                        null
                                }
                            </ul>
                        </div>
                    </div>
                </nav>

                {/* définition des routes */}
                <Routes>
                    <Route path='/' element={<HomeView />} />
                    <Route path='/*' element={<HomeView />} />

                    {/* La page pour jouer est accessible uniquement si on est connecté             */}
                    <Route
                        path="/play"
                        element=
                        {
                            hasJWT() ?
                                <PlayView />
                                : 
                                <AuthenticationView isNew={false} errorMessage={"Vous devez être connecté pour pouvoir jouer"}/>
                        }
                    />

                    <Route path='/ranking' element={<RankingView />} />

                    <Route
                        path='/me'
                        element=
                        {
                            hasJWT() ?
                                <UserProfileView />
                                :
                                <AuthenticationView isNew={false}/>
                        }
                    />
                    <Route
                        path='/login'
                        element=
                        {
                            !hasJWT() ?
                                <AuthenticationView isNew={false}/>
                                :
                                <HomeView message={"Re-bonjour, " + activeUser.pseudo + " !"}/>
                        }
                    />
                    <Route
                        path='/registration'
                        element=
                        {
                            !hasJWT() ?
                                <AuthenticationView isNew={true}/>
                                :
                                <HomeView message={"Bienvenue " + activeUser.pseudo + " !"}/>
                        }
                    />
                    <Route
                    path='/admin'
                    element=
                    {
                        hasJWT() && activeUser.isAdmin ?
                        <AdminView/>
                        :
                        <HomeView/>
                    }
                    />
                </Routes>
            </BrowserRouter>
            <Outlet />  {/* le rendu de la view active */}
        </div>
    );
}

export default NavbarComponent;