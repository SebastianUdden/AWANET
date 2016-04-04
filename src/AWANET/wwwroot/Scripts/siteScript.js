function editUserInfo(e) {
    //e.preventDefault();
    $("#loader").show();
    $.post("/account/EditDetails", {
        'FirstName': $('#firstName').val(),
        'LastName': $('#lastName').val(),
        'Phone': $('#phone').val(),
        'Street': $('#street').val(),
        'Zip': $('#zip').val(),
        'City': $('#city').val()
    }, function (partial) {
        $("#editUserDiv").html(partial);
        $("#loader").hide();
        //$("#accordionDetails").accordion().activate(3);
    });
}

function editPassword(e) {
    //e.preventDefault();
    $("#loader").show();
    $.post("/account/EditPassword", {
        'OldPassword': $('#oldPassword').val(),
        'NewPassword': $('#newPassword').val(),
        'ConfirmNewPassword': $('#confirmNewPassword').val(),
    }, function (partial) {
        $("#editPasswordDiv").html(partial);
        $.validator.unobtrusive.parse($("#editPasswordDiv"));
        $("#loader").hide();
        //$("#accordionDetails").accordion().activate(3);
    });
}

function getcontactbymail(Email, Id) {
    $("#loader").show();
    $.get("/ContactList/GetContact", { 'Email': Email, 'UserId': Id }, function (data) {
        $("#renderModal").html(data);
        $("#contactModal").modal('show');
        $("#loader").hide();
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
    $("#loader").show();
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
            $("#loader").hide();

        },
        error: function (er) {
            $("#uploadPictureDiv").html(partial);
            $("#loader").hide();
            //alert(er);
        }
    });
}
function ToggleAdmin(eMail, adminAction) {
    $("#loader").show();
    $.get("/Admin/ToggleAdmin", { 'eMail': eMail, 'adminAction': adminAction }, function (data) {
        $("#contactList").html(data);
        $("#loader").hide();
    });
}

function TerminateUser(eMail) {
    var answer = confirm('Vill du verkligen ta bort användaren ' + eMail + '?');
    if (answer) {
        $("#loader").show();
        $.get("/Admin/TerminateUser", { 'eMail': eMail }, function (data) {
            $("#contactList").html(data);
            $("#loader").hide();
        });
    }
}
function showMessageModal() {
    $("#loader").show();
    $.get("/Home/PostMessage", null, function (data) {
        $("#editor").html(data);
        $("#messageModal").modal('show');
        $("#loader").hide();
    });
}