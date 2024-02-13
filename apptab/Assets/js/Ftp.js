let User;
let Origin;

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = "../";
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetUsers();
});

//let urlOrigin = "https://localhost:44334";
//let urlOrigin = "http://softwell.cloud/OPAVI";
function GetUsers() {
    let formData = new FormData();

    let dbase;

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    $.ajax({
        type: "POST",
        url: Origin + '/FTPSend/DetailsFTP',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var Datas = JSON.parse(result);
            console.log(Datas);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }

            $("#Hote").val(Datas.data.HOTE);
            $("#Identifiant").val(Datas.data.IDENTIFIANT);
            $("#MDP").val(Datas.data.FTPPWD);
            $("#Path").val(Datas.data.PATH);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

$(`[data-action="UpdateUser"]`).click(function () {
    let user = $("#Hote").val();
    let db = $("#MDP").val();
    let inst = $("#Identifiant").val();
    if (!user || !db || !inst) {
        alert("Veuillez renseigner les informations sur la connexion FTP. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    formData.append("ftp.HOTE", $(`#Hote`).val());
    formData.append("ftp.IDENTIFIANT", $(`#Identifiant`).val());
    formData.append("ftp.FTPPWD", $(`#MDP`).val());
    formData.append("ftp.PATH", $(`#Path`).val());

    $.ajax({
        type: "POST",
        url: Origin + '/FTPSend/UpdateFTP',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var Datas = JSON.parse(result);

            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "success") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
            }
        },
    });
});