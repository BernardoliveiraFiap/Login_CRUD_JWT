(function (global) {
  function RegisterScreen() {
    const { values, errors, status, loading, handleChange, handleSubmit } =
      global.useRegisterForm();

    return (
      <div className="card shadow-sm">
        <div className="card-body">
          <h5 className="card-title mb-3">Cadastro de Usuário</h5>
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
              label="Nome"
              value={values.name}
              onChange={handleChange("name")}
              placeholder="Nome completo"
              error={errors.name}
            />
            <global.FormInput
              label="Senha"
              type="password"
              value={values.password}
              onChange={handleChange("password")}
              placeholder="******"
              error={errors.password}
            />
            <button type="submit" className="btn btn-success w-100" disabled={loading}>
              {loading ? "Cadastrando..." : "Cadastrar"}
            </button>
          </form>
          <global.Alert type={status.type || "info"} message={status.message} />
        </div>
      </div>
    );
  }

  global.RegisterScreen = RegisterScreen;
})(window);
