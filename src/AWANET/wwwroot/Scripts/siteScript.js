function editUserInfo(e) {
    //e.preventDefault();
    $.post("/account/EditDetails", {
        'FirstName': $('#firstName').val(),
        'LastName': $('#lastName').val(),
        'Phone': $('#phone').val(),
        'Street': $('#street').val(),
        'Zip': $('#zip').val(),
        'City': $('#city').val()
    }, function (partial) {
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

function getcontactbymail(Email, Id) {
    $.get("/ContactList/GetContact", { 'Email': Email, 'UserId': Id }, function (data) {
        $("#renderModal").html(data);
        $("#contactModal").modal('show');
    });
}

function ChooseCategory(c) {
    $("#Category").val(c);
}

function ConfirmUserCreation() {
    var list = document.getElementById('listCategory');
    var inputValue = $("#Category").val();
    var isNew = true;

    $("#listCategory li").each(function () {
        if ($(this).attr('id') == inputValue) {
            isNew = false;
        }
    });

    if (isNew) {
        var resultCategory = confirm("Vill du skapa en ny kategori? Ditt nya kategorinamn är " + inputValue + ".");
        if (resultCategory) {
            $("#createUserForm").submit();
        }

    }
    else {
        $("#createUserForm").submit();
    }
}

function submitPicture() {
    $("#loader").toggle();
    var data = new FormData();
    var files = $("#uploadPicture").get(0).files;
    if (files.length > 0) {
        data.append('file', files[0]);
    }
        $.ajax({
            url: '/account/UploadProfilePicture',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (partial) {
                $("#uploadPictureDiv").html(partial);
                $("#loader").toggle();

            },
            error: function (er) {
                $("#uploadPictureDiv").html(partial);
                $("#loader").toggle();
                //alert(er);
            }

        });

    
}