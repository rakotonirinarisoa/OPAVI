let User;
let Origin;

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = "../";
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetListRole();
    GetUsers();
});

let urlOrigin = "https://localhost:44334";
//let urlOrigin = "http://softwell.cloud/OPAVI";
function GetListRole() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    $.ajax({
        type: "POST",
        url: urlOrigin + '/User/GetAllRole',
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

            $(`[data-id="User-role-list"]`).text("");

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <option value="${k}" id="${k}">${v}</option>
                `;
            });

            $(`[data-id="User-role-list"]`).append(code);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function GetUsers() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    formData.append("UserId", getUrlParameter("UserId"));

    $.ajax({
        type: "POST",
        url: urlOrigin + '/User/DetailsUser',
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

            $("#Login").val(Datas.data.LOGIN);
            $("#MDP").val(Datas.data.PWD);
            $("#Role").val(`${Datas.data.ROLE}`);
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

//function UpdateUser(id) {
//    let newpwd = $(`#MDP`).val();
//    let newpwdConf = $(`#MDPC`).val();
//    if (newpwd != newpwdConf) {
//        alert("Les mots de passe ne correspondent pas");
//        return;
//    }

//    let formData = new FormData();

//    formData.append("suser.LOGIN", User.LOGIN);
//    formData.append("suser.PWD", User.PWD);
//    formData.append("suser.ROLE", User.ROLE);
//    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

//    formData.append("UserId", id);

//    formData.append("user.LOGIN", $(`#Login`).val());
//    formData.append("user.ROLE", $(`#Role`).val());
//    formData.append("user.PWD", newpwd);

//    $.ajax({
//        type: "POST",
//        url: urlOrigin + '/User/UpdateUser',
//        data: formData,
//        cache: false,
//        contentType: false,
//        processData: false,
//        success: function (result) {
//            var Datas = JSON.parse(result);
//            console.log(Datas);

//            if (Datas.type == "error") {
//                alert(Datas.msg);
//                return;
//            }
//            if (Datas.type == "success") {
//                alert(Datas.msg);
//                window.location = urlOrigin + "/User/List";
//                /*window.history.back();*/
//                /*location.replace(document.referrer);*/
//            }
//            if (Datas.type == "login") {
//                alert(Datas.msg);
//                window.location = window.location.origin;
//            }
//        },
//        error: function () {
//            alert("Connexion Problems");
//        }
//    });
//}

$(`[data-action="UpdateUser"]`).click(function () {
    let newpwd = $(`#MDP`).val();
    let newpwdConf = $(`#MDPC`).val();
    if(newpwd != newpwdConf){
        alert("Les mots de passe ne correspondent pas. ");
        return;
    }

    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    formData.append("user.LOGIN", $(`#Login`).val());
    formData.append("user.ROLE", $(`#Role`).val());
    formData.append("user.PWD", newpwd);

    formData.append("UserId", getUrlParameter("UserId"));
    
    $.ajax({
        type: "POST",
        url: urlOrigin + '/User/UpdateUser',
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
                window.location = urlOrigin + "/User/List";
                /*window.history.back();*/
                /*location.replace(document.referrer);*/
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
            }
        },
    });
});