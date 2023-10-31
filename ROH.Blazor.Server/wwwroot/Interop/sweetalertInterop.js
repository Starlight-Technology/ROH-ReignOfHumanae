window.sweetalertInterop = {
    showSweetAlert: function (title, message, icon) {
        return Swal.fire({
            title: title,
            text: message,
            icon: icon,
        });
    }
};