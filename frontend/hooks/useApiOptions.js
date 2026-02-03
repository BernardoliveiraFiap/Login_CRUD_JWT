(function (global) {
  const { useEffect, useState } = React;

  function useApiOptions(auth) {
    const [info, setInfo] = useState(null);
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [status, setStatus] = useState({ type: "", message: "" });

    const fetchInfo = async () => {
      const response = await fetch("/api/info");
      if (!response.ok) {
        throw new Error("Falha ao carregar info da API.");
      }
      return response.json();
    };

    const fetchUsers = async (token) => {
      const response = await fetch("/api/users", {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      const contentType = response.headers.get("content-type") || "";
      const payload = contentType.includes("application/json")
        ? await response.json()
        : null;

      if (!response.ok) {
        const message = payload?.message || "Falha ao carregar usuários.";
        throw new Error(message);
      }

      return payload || [];
    };

    const loadData = async () => {
      if (!auth?.token) return;
      setLoading(true);
      setStatus({ type: "", message: "" });

      try {
        const [infoData, usersData] = await Promise.all([
          fetchInfo(),
          fetchUsers(auth.token)
        ]);

        setInfo(infoData);
        setUsers(usersData);
      } catch (err) {
        setStatus({ type: "danger", message: err.message || "Erro ao carregar dados." });
      } finally {
        setLoading(false);
      }
    };

    useEffect(() => {
      loadData();
    }, [auth?.token]);

    const deleteUser = async (id) => {
      if (!auth?.token) return;
      if (auth?.user?.id === id) {
        setStatus({ type: "warning", message: "Você não pode excluir o próprio usuário." });
        return;
      }

      setLoading(true);
      setStatus({ type: "", message: "" });
      try {
        const response = await fetch(`/api/users/${id}`, {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${auth.token}`
          }
        });

        if (!response.ok) {
          const contentType = response.headers.get("content-type") || "";
          const payload = contentType.includes("application/json")
            ? await response.json()
            : null;
          const message = payload?.message || "Falha ao excluir usuário.";
          throw new Error(message);
        }

        setUsers((prev) => prev.filter((user) => user.id !== id));
        setStatus({ type: "success", message: "Usuário excluído com sucesso." });
      } catch (err) {
        setStatus({ type: "danger", message: err.message || "Erro ao excluir usuário." });
      } finally {
        setLoading(false);
      }
    };

    return {
      info,
      users,
      loading,
      status,
      reload: loadData,
      deleteUser
    };
  }

  global.useApiOptions = useApiOptions;
})(window);
