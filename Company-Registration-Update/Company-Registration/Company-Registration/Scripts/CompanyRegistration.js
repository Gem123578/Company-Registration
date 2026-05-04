<script>
    $(document).on("click", ".edit-btn", function () {
        var id = $(this).data("id");

    $("#modalBody").load('/CompanyRegistration/GetCompanyById?id=' + id, function () {
        $("#companyModal").modal('show');
        });
    });

    $(document).on("submit", "#updateForm", function (e) {
        e.preventDefault();

    $.post($(this).attr("action"), $(this).serialize(), function (res) {
            if (res.success) {
        alert(res.message);
    location.reload();
            } else {
        alert(res.message);
            }
        });
    });

    $(document).on("click", ".delete-btn", function () {
        if (!confirm("Are you sure to delete?")) return;

    var id = $(this).data("id");

    $.post('/CompanyRegistration/DeleteCompany', {id: id }, function (res) {
            if (res.success) {
        location.reload();
            } else {
        alert(res.message);
            }
        });
    });
</script>
