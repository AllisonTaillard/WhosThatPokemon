import React from 'react';
import "./HomeView.css";

const HomeView = ({message}) => {
    return (
        <div>
            {/* Message d'info après redirection dans certains cas */}
            {
                message ? <div class="alert alert-success mb-5 mt-5" role="alert">{message}</div> : null
            }
            <h1>Home</h1>
        </div>
    );
}

export default HomeView;
