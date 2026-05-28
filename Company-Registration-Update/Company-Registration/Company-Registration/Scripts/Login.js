function resendConfirmation() {
    var email = $("#EmailAddress").val();

    if (!email) {
        alert("Enter email first");
        return;
    }

    $.post('/CompanyApplicant/ResendConfirmation',
        { email: email },
        function (res) {
            alert(res.Message);
        });
}
function closeModal(id) {
    document.getElementById(id).style.display = "none";
}

function resendEmail() {

    $.ajax({
        url: '@Url.Action("ResendConfirmation", "CompanyApplicant")',
        type: 'POST',
        data: {
            email: '@TempData["ExpiredEmail"]'
        },
        success: function (response) {

            alert(response.Message);

            closeModal('expireModal');
        }
    });
}
document.addEventListener("DOMContentLoaded", function () {

    var modalElement = document.getElementById('successModal');

    if (modalElement) {

        var myModal = new bootstrap.Modal(modalElement);
        myModal.show();
    }
});