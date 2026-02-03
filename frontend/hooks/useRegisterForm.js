(function (global) {
  const { useState } = React;

  function useRegisterForm() {
    const [values, setValues] = useState({
      email: "",
      name: "",
      password: ""
    });
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

      if (!values.name) {
        nextErrors.name = "Informe o nome.";
      }

      if (!values.password) {
        nextErrors.password = "Informe a senha.";
      } else if (values.password.length < 6) {
        nextErrors.password = "Senha deve ter ao menos 6 caracteres.";
      }

      setErrors(nextErrors);
      return Object.keys(nextErrors).length === 0;
    };

    const handleChange = (field) => (event) => {
      const { value } = event.target;
      setValues((prev) => ({
        ...prev,
        [field]: value
      }));
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
        const response = await fetch("/api/auth/register", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            name: values.name,
            email: values.email,
            password: values.password
          })
        });

        const contentType = response.headers.get("content-type") || "";
        const payload = contentType.includes("application/json")
          ? await response.json()
          : null;

        if (!response.ok) {
          const message = payload?.message || "Falha no cadastro.";
          throw new Error(message);
        }

        setStatus({ type: "success", message: "Usuário cadastrado com sucesso." });
      } catch (err) {
        setStatus({ type: "danger", message: err.message || "Falha no cadastro." });
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

  global.useRegisterForm = useRegisterForm;
})(window);
