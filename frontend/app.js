const { useState } = React;
const { LoginScreen, RegisterScreen, ApiOptionsScreen } = window;

function App() {
  const [screen, setScreen] = useState("login");
  const [auth, setAuth] = useState(() => {
    const token = localStorage.getItem("token");
    const userRaw = localStorage.getItem("auth_user");
    if (!token || !userRaw) return null;
    try {
      return { token, user: JSON.parse(userRaw) };
    } catch (err) {
      return null;
    }
  });

  const handleLogin = (nextAuth) => {
    setAuth(nextAuth);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("auth_user");
    setAuth(null);
    setScreen("login");
  };

  return (
    <div className="app-shell">
      <div className="text-center mb-4">
        <img
          src="https://ipoolcare.com/wp-content/uploads/2024/05/logo.png"
          alt="iPool"
          className="brand-logo mb-3"
        />
        <h2 className="fw-bold brand-title">iPool</h2>
        <p className="brand-subtitle mb-0">Login e Cadastro de Usuários</p>
      </div>

      {!auth && (
        <>
          <ul className="nav nav-tabs mb-4 justify-content-center">
            <li className="nav-item">
              <button
                className={`nav-link ${screen === "login" ? "active" : ""}`}
                onClick={() => setScreen("login")}
              >
                Login
              </button>
            </li>
            <li className="nav-item">
              <button
                className={`nav-link ${screen === "register" ? "active" : ""}`}
                onClick={() => setScreen("register")}
              >
                Cadastro
              </button>
            </li>
          </ul>

          <div className="row justify-content-center">
            <div className="col-lg-7 col-md-9">
              {screen === "login" ? (
                <LoginScreen onLogin={handleLogin} />
              ) : (
                <RegisterScreen />
              )}
            </div>
          </div>
        </>
      )}

      {auth && (
        <div className="row justify-content-center">
          <div className="col-lg-9">
            <ApiOptionsScreen auth={auth} onLogout={handleLogout} />
          </div>
        </div>
      )}

      <footer className="footer text-center">
        <div>iPool Care · Plataforma de acesso</div>
        <div className="small">
          Desenvolvido com foco em simplicidade e segurança <span className="accent">●</span>
        </div>
      </footer>
    </div>
  );
}

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<App />);
