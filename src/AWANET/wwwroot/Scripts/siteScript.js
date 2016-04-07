$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

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
function editFirstTimeUserInfo(e) {
    //e.preventDefault();
    $("#loader").show();
    $.post("/firsttimeuser/index", {
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
function ChooseReceiver(c) {
    $("#receiver").val(c);
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

function ConfirmMessagePost() {
    tinyMCE.triggerSave();
    $.validator.unobtrusive.parse($("#newMessageForm"));
    if ($('#newMessageForm').valid()) {

        var list = document.getElementById('listReceiver');
        var inputValue = $("#receiver").val();
        var isNew = true;

        $("#listReceiver li").each(function () {
            if ($(this).attr('id') == inputValue) {
                isNew = false;
            }
        });

        if (isNew) {
            var resultCategory = confirm("Vill du skapa ett nytt gruppflöde? Ditt nya gruppflöde är " + inputValue + ".");
            if (resultCategory) {
                $("#newMessageForm").submit();
            }

        }
        else {
            $("#newMessageForm").submit();
        }
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
        $.validator.unobtrusive.parse($("#messageModal"));
        $("#messageModal").modal('show');
        $("#loader").hide();
    });
}

function showGroupModal() {
    $("#loader").show();
    $.get("/Home/JoinGroup", null, function (data) {
        $("#joingroupdiv").html(data);
        $.validator.unobtrusive.parse($("#joinGroupModal"));
        $("#joinGroupModal").modal('show');
        $("#loader").hide();
    });
}

function ConfirmGroupJoin() {
    var list = $('#Groups').val();
    var inputValue = $('#groupInput').val();
    var isNew = true;

    var groupId = 0;

    $('#Groups option').each(function () {
        if ($(this).val() == inputValue) {
            groupId = $(this).attr('id');
            isNew = false;
        }
    });

    //.attr('id') 

    if (isNew) {
        $("#errorMessageGroup").html('Du kan bara välja grupper ur listan.');
    }
    else {
        JoinGroup(groupId);
    }
}
function JoinGroup(gid) {
    $("#loader").show();

    $.post("/Home/JoinGroup", { 'id': gid }, function (data) {
        location.reload();
        $("#loader").hide();
    });
}

function editMessageModal(id) {
    $("#loader").show();
    $.get("/Home/EditMessage", { "id": id }, function (data) {
        $("#editor").html(data);
        $.validator.unobtrusive.parse($("#messageModal"));
        $("#messageModal").modal('show');
        $("#loader").hide();
    });
}

function RemoveMessage(messageId, groupId) {
    $("#loader").show();

    $.post("/Home/RemoveMessage", { 'id': messageId, 'groupId': groupId }, function (data) {
        window.location.href = '/home/index/' + groupId;
        $("#loader").hide();
    });
}
function postComment(messageId) {
    $("#loader").show();
    var commentBody = "#textarea" + messageId;
    var comment = $(commentBody).val();
    var loadArea = '#loadComment' + messageId;
    $.post("/Home/PostComment", { 'id': messageId, 'commentBody': comment }, function (data) {
        $(loadArea).html(data);
        $("#loader").hide();
    });
}
//Om groupInput == GroupVM.GroupName
//Ta ut GroupVM.Id
//var groupId = GroupVM.Id;

//skicka med groupId => ConfirmGroupJoin(groupId);
