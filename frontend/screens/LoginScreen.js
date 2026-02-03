(function (global) {
  function LoginScreen({ onLogin }) {
    const { values, errors, status, loading, handleChange, handleSubmit } =
      global.useLoginForm(onLogin);

    return (
      <div className="card shadow-sm">
        <div className="card-body">
          <h5 className="card-title mb-3">Login</h5>
          <form onSubmit={handleSubmit} noValidate>
            <global.FormInput
              label="Login (Email)"
              type="email"
              value={values.email}
              onChange={handleChange("email")}
              placeholder="usuario@ipool.com"
              error={errors.email}
            />
            <global.FormInput
              label="Senha"
              type="password"
              value={values.password}
              onChange={handleChange("password")}
              placeholder="******"
              error={errors.password}
            />
            <button type="submit" className="btn btn-primary w-100" disabled={loading}>
              {loading ? "Entrando..." : "Entrar"}
            </button>
          </form>
          <global.Alert type={status.type || "info"} message={status.message} />
        </div>
      </div>
    );
  }

  global.LoginScreen = LoginScreen;
})(window);
