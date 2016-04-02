function editUserInfo(e)
{
    //e.preventDefault();
    $.post("/account/EditDetails", {
        'FirstName': $('#firstName').val(),
        'LastName': $('#lastName').val(),
        'Phone': $('#phone').val(),
        'Street': $('#street').val(),
        'Zip': $('#zip').val(),
        'City': $('#city').val()
    }, function (partial)
    {
        $("#editUserDiv").html(partial);
        //$("#accordionDetails").accordion().activate(3);
    });
}

function editPassword(e) {
    //e.preventDefault();
    $.post("/account/EditPassword", {
        'OldPassword': $('#oldPassword').val(),
        'NewPassword': $('#newPassword').val(),
        'ConfirmNewPassword': $('#confirmNewPassword').val(),
    }, function (partial) {
        $("#editPasswordDiv").html(partial);
        $.validator.unobtrusive.parse($("#editPasswordDiv"));
        //$("#accordionDetails").accordion().activate(3);
    });
}

function getcontactbymail(Email,Id) {

    $.get("/ContactList/GetContact", {'Email':Email,'UserId':Id} , function (data) {
        $("#renderModal").html(data);
        $("#contactModal").modal('show');
    });
}