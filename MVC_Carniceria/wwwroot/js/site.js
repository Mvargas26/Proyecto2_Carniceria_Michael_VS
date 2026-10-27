
//efecto de desaparecer los alert suavemente
document.addEventListener("DOMContentLoaded", function () {
    const alerts = document.querySelectorAll('.alert-dismissible');

    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        }, 5000); // 5000 ms = 5 segundos
    });
});