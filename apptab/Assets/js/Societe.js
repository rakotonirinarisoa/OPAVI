let User;
let Origin;

$(document).ready(() => {
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = "../";
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);
    GetListSociete();
});


let urlOrigin = "https://localhost:44334";
//let urlOrigin = "http://softwell.cloud/OPAVI";
function GetListSociete() {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    $.ajax({
        type: "POST",
        url: urlOrigin + '/SuperAdmin/FillTable',
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

            $(`[data-id="ubody"]`).text("");

            var code = ``;
            $.each(Datas.data, function (k, v) {
                code += `
                    <tr data-societeId="${v.ID}" class="text-nowrap last-hover">
                        <td>${v.SOCIETE}</td>
                    </tr>
                `;
            });

            $(`[data-id="ubody"]`).append(code);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

$(`[data-action="AddnewSociete"]`).click(function () {
    let formData = new FormData();
    let newpwd = $(`#MDP`).val();
    let newpwdConf = $(`#MDPC`).val();
    if (newpwd != newpwdConf) {
        alert("Les mots de passe ne correspondent pas. ");
        return;
    }

    let soc = $("#IDSociete").val();
    let firtAdm = $("#Login").val();
    if (!soc){
        alert("Veuillez renseigner la société. ");
        return;
    }
    if (!firtAdm) {
        alert("Veuillez renseigner le login du premier ADMINISTRATEUR. ");
        return;
    }

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    formData.append("societe.SOCIETE", $(`#IDSociete`).val());

    formData.append("user.LOGIN", $(`#Login`).val());
    formData.append("user.PWD", $(`#MDP`).val());

    $.ajax({
        type: "POST",
        url: urlOrigin + '/SuperAdmin/AddSociete',
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
                window.location = urlOrigin + "/SuperAdmin/SocieteList";
            }
            if (Datas.type == "login") {
                alert(Datas.msg);
                window.location = window.location.origin;
                return;
            }
        },
    });
});
