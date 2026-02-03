(function (global) {
  function ApiOptionsScreen({ auth, onLogout }) {
    const { info, users, loading, status, reload, deleteUser } =
      global.useApiOptions(auth);

    return (
      <div className="card shadow-sm">
        <div className="card-body">
          <div className="d-flex align-items-start justify-content-between flex-wrap gap-2 mb-3">
            <div>
              <h5 className="card-title mb-1">Opções da API</h5>
              <p className="small-muted mb-0">
                Conectado como <strong>{auth?.user?.name}</strong>
              </p>
            </div>
            <div className="d-flex gap-2">
              <button className="btn btn-outline-dark" onClick={reload} disabled={loading}>
                Atualizar
              </button>
              <button className="btn btn-dark" onClick={onLogout}>
                Sair
              </button>
            </div>
          </div>

          {info && (
            <div className="border rounded-3 p-3 mb-3">
              <div className="row g-3">
                <div className="col-md-4">
                  <div className="small-muted">Aplicação</div>
                  <div className="fw-semibold">{info.name}</div>
                </div>
                <div className="col-md-4">
                  <div className="small-muted">Versão</div>
                  <div className="fw-semibold">{info.version}</div>
                </div>
                <div className="col-md-4">
                  <div className="small-muted">Ambiente</div>
                  <div className="fw-semibold">{info.environment}</div>
                </div>
              </div>
            </div>
          )}

          <div className="mb-3">
            <h6 className="mb-2">Usuários cadastrados</h6>
            <div className="table-responsive">
              <table className="table align-middle">
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Email</th>
                    <th>Status</th>
                    <th>Criado em</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  {users.length === 0 && !loading && (
                    <tr>
                      <td colSpan="5" className="text-center text-muted">
                        Nenhum usuário encontrado.
                      </td>
                    </tr>
                  )}
                  {users.map((user) => {
                    const isSelf = auth?.user?.id === user.id;
                    return (
                      <tr key={user.id}>
                        <td>{user.name}</td>
                        <td>{user.email}</td>
                        <td>{user.isActive ? "Ativo" : "Inativo"}</td>
                        <td>{new Date(user.createdAt).toLocaleDateString()}</td>
                        <td className="text-end">
                          <button
                            className="btn btn-outline-dark btn-sm"
                            onClick={() => deleteUser(user.id)}
                            disabled={loading || isSelf}
                            title={isSelf ? "Você não pode excluir seu próprio usuário" : "Excluir"}
                          >
                            Excluir
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>

          <global.Alert type={status.type || "info"} message={status.message} />
        </div>
      </div>
    );
  }

  global.ApiOptionsScreen = ApiOptionsScreen;
})(window);
