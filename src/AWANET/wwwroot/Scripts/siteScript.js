function editUserInfo(e)
{
    //e.preventDefault();
    $.post("/account/EditDetails", $("userDetails").serialize(), function (partial)
    {
        $("#editUserDiv").html(partial);
    });
}