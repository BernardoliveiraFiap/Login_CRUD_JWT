(function (global) {
  function FormInput({
    label,
    type = "text",
    value,
    onChange,
    placeholder,
    error,
    disabled,
    readOnly
  }) {
    const inputClass = error ? "form-control is-invalid" : "form-control";

    return (
      <div className="mb-3">
        <label className="form-label">{label}</label>
        <input
          type={type}
          className={inputClass}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          disabled={disabled}
          readOnly={readOnly}
        />
        {error && <div className="invalid-feedback">{error}</div>}
      </div>
    );
  }

  global.FormInput = FormInput;
})(window);
