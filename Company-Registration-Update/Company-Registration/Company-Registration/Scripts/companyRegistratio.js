function approveCompany(id) {
    $.post('/CompanyRegistration/Approve', { id: id }, function (res) {
        alert(res.message);
        location.reload();
    });
}

function rejectCompany(id) {
    $.post('/CompanyRegistration/Reject', { id: id }, function (res) {
        alert(res.message);
        location.reload();
    });
}
