import logo from './logo.svg';
import './App.css';
import NavbarComponent from './layout/NavbarComponent/NavbarComponent';
import FooterComponent from './layout/FooterComponent/FooterComponent';
import HeaderComponent from './layout/HeaderComponent/HeaderComponent';
import { setAuthToken } from './services/AuthenticationService';

function App() {
  // Checker s'il y a un token dans le localStorage
  const token = localStorage.getItem("token");
  if (token)
    setAuthToken(token); // si oui, on met le token dans le header axios

  return (
    <div className="container App">
      <HeaderComponent/>
      <NavbarComponent/>
      <FooterComponent/>
    </div>
  );
}

export default App;