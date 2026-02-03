(function (global) {
  const { useState } = React;

  function useLoginForm(onSuccess) {
    const [values, setValues] = useState({ email: "", password: "" });
    const [errors, setErrors] = useState({});
    const [status, setStatus] = useState({ type: "", message: "" });
    const [loading, setLoading] = useState(false);

    const validate = () => {
      const nextErrors = {};

      if (!values.email) {
        nextErrors.email = "Informe o login.";
      } else if (!values.email.includes("@")) {
        nextErrors.email = "Email inválido.";
      }

      if (!values.password) {
        nextErrors.password = "Informe a senha.";
      }

      setErrors(nextErrors);
      return Object.keys(nextErrors).length === 0;
    };

    const handleChange = (field) => (event) => {
      const { value } = event.target;
      setValues((prev) => ({ ...prev, [field]: value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

    const handleSubmit = async (event) => {
      event.preventDefault();
      setStatus({ type: "", message: "" });

      if (!validate()) {
        return;
      }

      setLoading(true);
      try {
        const response = await fetch("/api/auth/login", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ email: values.email, password: values.password })
        });

        const contentType = response.headers.get("content-type") || "";
        const payload = contentType.includes("application/json")
          ? await response.json()
          : null;

        if (!response.ok) {
          const message = payload?.message || "Falha no login.";
          throw new Error(message);
        }

        localStorage.setItem("token", payload.token);
        localStorage.setItem("auth_user", JSON.stringify(payload.user));
        setStatus({ type: "success", message: "Login realizado com sucesso." });
        if (typeof onSuccess === "function") {
          onSuccess({ token: payload.token, user: payload.user });
        }
      } catch (err) {
        setStatus({ type: "danger", message: err.message || "Falha no login." });
      } finally {
        setLoading(false);
      }
    };

    return {
      values,
      errors,
      status,
      loading,
      handleChange,
      handleSubmit
    };
  }

  global.useLoginForm = useLoginForm;
})(window);
