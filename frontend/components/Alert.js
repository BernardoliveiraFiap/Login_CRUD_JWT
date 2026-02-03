(function (global) {
  function Alert({ type, message }) {
    if (!message) return null;

    return (
      <div className={`alert alert-${type} mt-3 mb-0`} role="alert">
        {message}
      </div>
    );
  }

  global.Alert = Alert;
})(window);
