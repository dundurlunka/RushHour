$(document).ready(function () {
    $('.alert').delay(3000).fadeOut();

    $('.alert').on('click', function () {
        $(this).fadeOut();
    })

    PNotify.prototype.options.styling = "bootstrap3";

    function addSuccessNotification(message) {
        new PNotify({
            title: 'Success!',
            text: message,
            type: 'success'
        });
    }

    function addErrorNotification(message) {
        new PNotify({
            title: 'Oh No!',
            text: message,
            type: 'error'
        });
    }

    function addWarningNotification(message) {
        new PNotify({
            title: 'Warning',
            text: message
        });
    }
})