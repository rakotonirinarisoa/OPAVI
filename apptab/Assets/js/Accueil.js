let User;
let Origin;

let ListCodeJournal;
let ListCompteG;

var content;
let validate;

let ListResult;
let ListResultAnomalie;
let contentAnomalies;
var rdv
let contentpaie;
let ListResultpaie;
let reglementresult;

let listEtat;
let etaCode;
$(document).ready(() => {
    
    User = JSON.parse(sessionStorage.getItem("user"));
    if (User == null || User === "undefined") window.location = "../";
    Origin = User.origin;

    $(`[data-id="username"]`).text(User.LOGIN);

    $(`[tab="autre"]`).hide();

    console.log($(`[tab="autre"]`).hide());


    //GetUR();
    GetListCodeJournal("1");
    

    
    //GetListCompG();
});

function GetListCompG(id) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", id );

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCompteG',
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

            let code = `<option value = "Tous" > Tous</option> `;
            let codeAuxi = ``;
            ListCompteG = ``;
            ListCompteG = Datas.data;
            console.log(ListCompteG);
            $.each(ListCompteG, function (k, v) {
                code += `
                    <option value="${v.COGE}">${v.COGE}</option>
                `;
            });
            $(`[compG-list]`).html('');
            $(`[compG-list]`).append(code);
            
            FillAUXI();
            FillCompteName();
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}

function FillAUXI() {
    var list = ListCompteG.filter(x => x.COGE == $(`[compG-list]`).val()).pop();
    console.log(list);
    let code = `<option value="Tous"> Tous</option> `;
    $.each(list.AUXI, function (k, v) {
        code += `
                    <option value="${v}">${v}</option>
                `;
    });

    $(`[auxi-list]`).html(code);
}
function GetEtat(id) {
    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetEtat',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var Datas = JSON.parse(result);
            listEtat = Datas.data
            if (Datas.type == "error") {
                return;
            }
            if (Datas.type == "login") {
                return;
            }
            etaCode = `<option value = "Tous" > Tous</option> `;
            $.each(listEtat, function (k, v) {
                etaCode += `
                    <option value="${v}">${v}</option>
                `;
            });
            $(`[ETAT-list]`).html('');
            $(`[ETAT-list]`).append(etaCode);

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
function FillCompteName() {
    var nom = $(`[compG-list]`).val() + " " + $(`[auxi-list]`).val();
    $(`[compte-name`).val(nom);
}

$(document).on("change", "[compG-list]", () => {
    FillAUXI();
    FillCompteName();
});

$(document).on("change", "[auxi-list]", () => {
    FillCompteName();
});

function GetListCodeJournal(id) {
    let formData = new FormData();

    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", id);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetCODEJournal',
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

            let code = ``;
            ListCodeJournal = Datas.data;

            $.each(ListCodeJournal, function (k, v) {
                code += `
                    <option value="${v.CODE}">${v.CODE}</option>
                `;
            });

            $(`[codej-list]`).append(code);
            $(`[codej-libelle]`).val(ListCodeJournal[0].LIBELLE);
            GetEtat(id);
        },
        error: function () {
            alert("Problème de connexion. ");
        }
    }).done(function (res) {
        GetListCompG(id);
    });
}
$(document).on("change", "[codej-list]", () => {
    var code = ListCodeJournal.filter(function (e) { return e.CODE == $(`[codej-list]`).val(); })[0];
    $(`[codej-libelle]`).val(code.LIBELLE);
});

$(document).on("click", "[data-target]", function () {
    let me = $(this).closest("[data-target]");
    if ($(me).attr("data-type") == "switch_tab") {
        let target = $(`#${$(me).attr("data-target")}`);


        $(`[data-type="switch_tab"]`).each(function (i) {
            if ($(this).hasClass('active')) {

                console.log($(this));
                $(this).removeClass('active');
                $(`#${$(this).attr("data-target")}`).hide();
            }
        });
        $(me).addClass("active");   
        $(target).show();

        
    }
});
$(`[data-action="CreateTxt"]`).click(function () {
    getelementTXT(0);
    //let formData = new FormData();

    //formData.append("suser.LOGIN", User.LOGIN);
    //formData.append("suser.PWD", User.PWD);
    //formData.append("suser.ROLE", User.ROLE);
    //formData.append("suser.IDSOCIETE", User.IDSOCIETE);

    //$.ajax({
    //    type: "POST",
    //    url: Origin + '/Home/CreateZipFile',
    //    data: formData,
    //    cache: false,
    //    contentType: false,
    //    processData: false,
    //    success: function (result) {
    //        var Datas = JSON.parse(result);
    //        console.log(Datas);

    //        if (Datas.type == "error") {
    //            alert(Datas.msg);
    //            return;
    //        }
    //        if (Datas.type == "login") {
    //            alert(Datas.msg);
    //            window.location = window.location.origin;
    //            return;
    //        }

    //        /*$("#MDPA").val(Datas.data.CRYPTPWD);*/
    //    },
    //    error: function () {
    //        alert("Problème de connexion. ");
    //    }
    //});
})
$(`[data-action="CreateTxtCrypter"]`).click(function () {
    getelementTXT(1);
})
$(`[data-action="CreateTxtSend"]`).click(function () {
    getelementTXT(2);
})
$(`[data-action="CreateTxtFTPCrypter"]`).click(function () {
    getelementTXT(3);
})


$('[data-action="ChargerJs"]').click(function () {
    let formData = new FormData();
    //alert(baseName);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("ChoixBase", baseName);
    if (baseName == 1) {
        //Paie
        formData.append("mois", $('#Pmois').val());
        formData.append("journal", $('#commercial').val());
        formData.append("annee", $('#Pannee').val());
        formData.append("matr1", $('#Pmat1').val());
        formData.append("matr2", $('#Pmat2').val());
        formData.append("datePaie", $('#dpaie').val());
        $.ajax({
            type: "POST",
            url: Origin + '/Home/getelementjsPaie',
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
                if (Datas.type == "success") {
                    //window.location = window.location.origin;
                    ListResult = Datas.data
                    contentpaie = ``;
                    $.each(ListResult, function (k, v) {
                        contentpaie += `
                    <tr compteG-id="${v.No}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td><td>${v.No}</td>
                        <td>${v.Matricule}</td>
                        <td>${v.Nom}</td>
                        <td>${v.Mois}</td>
                        <td>${v.Annee}</td>
                        <td>${v.Libelle}</td>
                        <td>${v.Montant}</td>
                        <td>${v.Cin}</td>
                        <td>${v.Banque}</td>
                        <td>${v.CodeBanque}</td>
                        <td>${v.Guichet}</td>
                        <td>${v.CompteBanque}</td>
                        <td>${v.CleRIB}</td>
                    </tr>`

                    });
                    $('.afb160Paie').empty();
                    $('.afb160Paie').html(contentpaie);
                }


            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });

    } else if (baseName == 2) {
        //compta
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("journal", $('#commercial').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());

        //alert(formData);

        $.ajax({
            type: "POST",
            url: Origin + '/Home/getelementjs',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                var Datas = JSON.parse(result);
                console.log("anomalie" + Datas);

                if (Datas.type == "error") {
                    alert(Datas.msg);
                    return;
                }
                if (Datas.type == "success") {
                    //window.location = window.location.origin;
                    ListResult = Datas.data
                    $.each(ListResult, function (k, v) {
                        content += `
                    <tr compteG-id="${v.No}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td><td>${v.No}</td>
                        <td>${v.DateOrdre}</td>
                        <td>${v.NoPiece}</td>
                        <td>${v.Compte}</td>
                        <td>${v.Libelle}</td>
                        <td>${v.Debit}</td>
                        <td>${v.Credit}</td>
                        <td>${v.MontantDevise}</td>
                        <td>${v.Mon}</td>
                        <td>${v.Rang}</td>
                        <td>${v.FinancementCategorie}</td>
                        <td>${v.Commune}</td>
                        <td>${v.Plan6}</td>
                        <td>${v.Journal}</td>
                        <td>${v.Marche}</td>
                    </tr>`

                    });
                    $('.afb160').empty();
                    $('.afb160').html(content);
                }


            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });

    } else {
        //BR
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("journal", $('#commercial').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());
        formData.append("etat", $('#etat').val());
        formData.append("devise", false);


        $.ajax({
            type: "POST",
            url: Origin + '/Home/getelementjsBR',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                var Datas = JSON.parse(result);
                console.log( Datas);

                if (Datas.type == "error") {
                    alert(Datas.msg);
                    return;
                }
                if (Datas.type == "success") {
                    //window.location = window.location.origin;
                    ListResult = Datas.data
                    content = ``;
                    $.each(ListResult, function (k, v) {
                        content += `
                    <tr compteG-id="${v.No}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td><td>${v.No}</td>
                        <td>${v.Date}</td>
                        <td>${v.NoPiece}</td>
                        <td>${v.Compte}</td>
                        <td>${v.Libelle}</td>
                        <td>${v.Montant}</td>
                        <td>${v.MontantDevise}</td>
                        <td>${v.Mon}</td>
                        <td>${v.Rang}</td>
                        <td>${v.Poste}</td>
                        <td>${v.FinancementCategorie}</td>
                        <td>${v.Commune}</td>
                        <td>${v.Plan6}</td>
                        <td>${v.Journal}</td>
                        <td>${v.Marche}</td>
                        <td>${v.Status}</td>
                    </tr>`

                    });
                    $('.afb160').empty();
                    $('.afb160').html(content);

                }


            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });
        
        $('.afb160').empty()
    }
    
});

$('[data-action="GetElementChecked"]').click(function () {
    let CheckList = $(`[compteg-ischecked]:checked`).closest("tr");
    let list = [];
    $.each(CheckList, (k, v) => {
        list.push($(v).attr("compteG-id"));
    });

    let formData = new FormData();
    console.log(list);
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("listCompte", list);
    formData.append("baseName", baseName);
    formData.append("journal", $('#commercial').val());
    formData.append("etat", $('#etat').val());
    formData.append("devise", false);

    let listid = list.splice(',');
    alert(listid);
    if (baseName == "1") {
        formData.append("mois", $('#Pmois').val());
        formData.append("journal", $('#commercial').val());
        formData.append("annee", $('#Pannee').val());
        formData.append("matriculeD", $('#Pmat1').val());
        formData.append("matriculeF", $('#Pmat2').val());
        formData.append("dateP", $('#dpaie').val());
        formData.append("baseName", baseName);
        $.ajax({
            type: "POST",
            url: Origin + '/Home/GetCheckedComptePaie',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                var Datas = JSON.parse(result);
                reglementresult = Datas.data;
                console.log(reglementresult);
                //alert(baseName);
                validate = "",
                content = ``,
                $.each(listid, function (k, x) {
                    $.each(reglementresult, function (k, v) {
                        if (v.No == x) {
                            validate += `
                            <tr compteG-id="${v.No}">
                                <td>${v.No}</td>
                                <td>${v.Matricule}</td>
                                <td>${v.Nom}</td>
                                <td>${v.Mois}</td>
                                <td>${v.Annee}</td>
                                <td>${v.Libelle}</td>
                                <td>${v.Montant}</td>
                                <td>${v.Cin}</td>
                                <td>${v.Banque}</td>
                                <td>${v.CodeBanque}</td>
                                <td>${v.Guichet}</td>
                                <td>${v.CompteBanque}</td>
                                <td>${v.CleRIB}</td>
                            </tr>`
                        } else {
                            content += `
                            <tr compteG-id="${v.No}"><td>
                                    <input type="checkbox" name = "checkprod" compteg-ischecked/>
                               </td>
                                <td>${v.No}</td>
                                <td>${v.Matricule}</td>
                                <td>${v.Nom}</td>
                                <td>${v.Mois}</td>
                                <td>${v.Annee}</td>
                                <td>${v.Libelle}</td>
                                <td>${v.Montant}</td>
                                <td>${v.Cin}</td>
                                <td>${v.Banque}</td>
                                <td>${v.CodeBanque}</td>
                                <td>${v.Guichet}</td>
                                <td>${v.CompteBanque}</td>
                                <td>${v.CleRIB}</td>
                            </tr>`
                        }
                    });
                    $('.afb160Paie').empty();
                    $('.afb160Paie').html(content);
                    $('.afbpaie').html(validate);
                });
            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });
    } else {
        formData.append("datein", $('#Pdu').val());
        formData.append("dateout", $('#Pau').val());
        formData.append("comptaG", $('#comptaG').val());
        formData.append("auxi", $('#auxi').val());
        formData.append("auxi1", $('#auxi').val());
        formData.append("dateP", $('#Pay').val());
        formData.append("etat", $('#etat').val());
        

        $.ajax({
            type: "POST",
            url: Origin + '/Home/GetCheckedCompte',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                var Datas = JSON.parse(result);
                reglementresult = ``;
                reglementresult = Datas.data;
                console.log(reglementresult);
                $.each(listid, function (k, x) {
                    $.each(reglementresult, function (k, v) {
                        if (v != null) {
                            if (v.No == x) {
                                validate += `
                                    <tr compteG-id="${v.No}">
                                    </td ><td>${v.No}</td>
                                    <td>${v.Date}</td>
                                    <td>${v.NoPiece}</td>
                                    <td>${v.Compte}</td>
                                    <td>${v.Libelle}</td>
                                    <td>${v.Montant}</td>
                                    <td>${v.MontantDevise}</td>
                                    <td>${v.Mon}</td>
                                    <td>${v.Rang}</td>
                                    <td>${v.Poste}</td>
                                    <td>${v.FinancementCategorie}</td>
                                    <td>${v.Commune}</td>
                                    <td>${v.Plan6}</td>
                                    <td>${v.Journal}</td>
                                    <td>${v.Marche}</td>
                                    <td>${v.Status}</td>
                                </tr>`;

                            } else {
                                content += `
                            <tr compteG-id="${v.No}"><td>
                                    <input type="checkbox" name = "checkprod" compteg-ischecked/>
                               </td><td>${v.No}</td>
                                <td>${v.Date}</td>
                                <td>${v.NoPiece}</td>
                                <td>${v.Compte}</td>
                                <td>${v.Libelle}</td>
                                <td>${v.Montant}</td>
                                <td>${v.MontantDevise}</td>
                                <td>${v.Mon}</td>
                                <td>${v.Rang}</td>
                                <td>${v.Poste}</td>
                                <td>${v.FinancementCategorie}</td>
                                <td>${v.Commune}</td>
                                <td>${v.Plan6}</td>
                                <td>${v.Journal}</td>
                                <td>${v.Marche}</td>
                                <td>${v.Status}</td>
                            </tr>`
                            }
                        }
                    });
                    $('.afb160').empty();
                    $('.afb160').html(content);
                    $('#afb').html(validate);
                });
            },
            error: function () {
                alert("Problème de connexion. ");
            }
        });
        }
    //$.ajax({
    //    type: "POST",
    //    url: Origin + '/Home/GetCheckedCompte',
    //    data: formData,
    //    cache: false,
    //    contentType: false,
    //    processData: false,
    //    success: function (result) {
    //        var Datas = JSON.parse(result);
    //        reglementresult = Datas.data;
    //        console.log(reglementresult);
    //        //ListResult
    //        if (baseName == "1") {
    //            //alert(baseName);
    //            $.each(listid, function (k, x) {
    //                $.each(reglementresult, function (k, v) {
    //                    if (v.No == x) {
    //                        validate += `
    //                <tr compteG-id="${v.No}">
    //                    <td>${v.No}</td>
    //                    <td>${v.Matricule}</td>
    //                    <td>${v.Nom}</td>
    //                    <td>${v.Mois}</td>
    //                    <td>${v.Annee}</td>
    //                    <td>${v.Libelle}</td>
    //                    <td>${v.Montant}</td>
    //                    <td>${v.Cin}</td>
    //                    <td>${v.Banque}</td>
    //                    <td>${v.CodeBanque}</td>
    //                    <td>${v.Guichet}</td>
    //                    <td>${v.CompteBanque}</td>
    //                    <td>${v.CleRIB}</td>
    //                </tr>`
    //                    } else {
    //                        content += `
    //                <tr compteG-id="${v.No}">
    //                    <td>${v.No}</td>
    //                    <td>${v.Matricule}</td>
    //                    <td>${v.Nom}</td>
    //                    <td>${v.Mois}</td>
    //                    <td>${v.Annee}</td>
    //                    <td>${v.Libelle}</td>
    //                    <td>${v.Montant}</td>
    //                    <td>${v.Cin}</td>
    //                    <td>${v.Banque}</td>
    //                    <td>${v.CodeBanque}</td>
    //                    <td>${v.Guichet}</td>
    //                    <td>${v.CompteBanque}</td>
    //                    <td>${v.CleRIB}</td>
    //                </tr>`
    //                    }
    //                });
    //                $('.afb160Paie').empty();
    //                $('.afb160Paie').html(content);
    //                $('#afbpaie').html(validate);
    //            });
    //        } else if (baseName == "3") {
    //            $.each(listid, function (k, x) {
    //                $.each(reglementresult, function (k, v) {
    //                    if (v != null) {
    //                        if (v.No == x) {
    //                            validate += `
    //                                <tr compteG-id="${v.No}">
    //                                </td ><td>${v.No}</td>
    //                                <td>${v.Date}</td>
    //                                <td>${v.NoPiece}</td>
    //                                <td>${v.Compte}</td>
    //                                <td>${v.Libelle}</td>
    //                                <td>${v.Montant}</td>
    //                                <td>${v.MontantDevise}</td>
    //                                <td>${v.Mon}</td>
    //                                <td>${v.Rang}</td>
    //                                <td>${v.Poste}</td>
    //                                <td>${v.FinancementCategorie}</td>
    //                                <td>${v.Commune}</td>
    //                                <td>${v.Plan6}</td>
    //                                <td>${v.Journal}</td>
    //                                <td>${v.Marche}</td>
    //                                <td>${v.Status}</td>
    //                            </tr>`;
                        
    //                            } else {
    //                                content += `
    //                        <tr compteG-id="${v.No}"><td>
    //                                <input type="checkbox" name = "checkprod" compteg-ischecked/>
    //                           </td><td>${v.No}</td>
    //                            <td>${v.Date}</td>
    //                            <td>${v.NoPiece}</td>
    //                            <td>${v.Compte}</td>
    //                            <td>${v.Libelle}</td>
    //                            <td>${v.Montant}</td>
    //                            <td>${v.MontantDevise}</td>
    //                            <td>${v.Mon}</td>
    //                            <td>${v.Rang}</td>
    //                            <td>${v.Poste}</td>
    //                            <td>${v.FinancementCategorie}</td>
    //                            <td>${v.Commune}</td>
    //                            <td>${v.Plan6}</td>
    //                            <td>${v.Journal}</td>
    //                            <td>${v.Marche}</td>
    //                            <td>${v.Status}</td>
    //                        </tr>`
    //                        }
    //                    }
    //                });
    //                $('.afb160').empty();
    //                $('.afb160').html(content);
    //                $('#afb').html(validate);
    //            });
    //        } else {
    //            //Compta
    //            $.each(listid, function (k, x) {
    //                $.each(ListResult, function (k, v) {
    //                    if (v.No == x) {
    //                        validate += `
    //                <tr compteG-id="${v.No}">
    //                    <td>
    //                        <input type="checkbox" name = "ProduitCheck" compteg-select/>
    //                    </td><td>${v.No}</td>
    //                    <td>${v.DateOrdre}</td>
    //                    <td>${v.NoPiece}</td>
    //                    <td>${v.Compte}</td>
    //                    <td>${v.Libelle}</td>
    //                    <td>${v.Debit}</td>
    //                    <td>${v.Credit}</td>
    //                    <td>${v.MontantDevise}</td>
    //                    <td>${v.Mon}</td>
    //                    <td>${v.Rang}</td>
    //                    <td>${v.FinancementCategorie}</td>
    //                    <td>${v.Commune}</td>
    //                    <td>${v.Plan6}</td>
    //                    <td>${v.Journal}</td>
    //                    <td>${v.Marche}</td>
    //                </tr>`
    //                    } else {
    //                        content += `
    //                <tr compteG-id="${v.No}">
    //                    <td>
    //                        <input type="checkbox" name = "checkprod" compteg-ischecked/>
    //                    </td><td>${v.No}</td>
    //                    <td>${v.DateOrdre}</td>
    //                    <td>${v.NoPiece}</td>
    //                    <td>${v.Compte}</td>
    //                    <td>${v.Libelle}</td>
    //                    <td>${v.Debit}</td>
    //                    <td>${v.Credit}</td>
    //                    <td>${v.MontantDevise}</td>
    //                    <td>${v.Mon}</td>
    //                    <td>${v.Rang}</td>
    //                    <td>${v.FinancementCategorie}</td>
    //                    <td>${v.Commune}</td>
    //                    <td>${v.Plan6}</td>
    //                    <td>${v.Journal}</td>
    //                    <td>${v.Marche}</td>
    //                </tr>`
    //                    }
    //                });
    //                $('.afb160').empty();
    //                $('.afb160').html(content);
    //                $('#afb').html(validate);
    //            });
    //        }
    //    },
    //    error: function () {
    //        alert("Problème de connexion. ");
    //    }
    //});
});

$('[data-action="GetAnomalieListes"]').click(function () {
    
    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", baseName);

    $.ajax({
        type: "POST",
        url: Origin + '/Home/GetAnomalieBack',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var Datas = JSON.parse(result);
            console.log(Datas);

            ListResultAnomalie = "";
            contentAnomalies = ``;
            if (Datas.type == "error") {
                alert(Datas.msg);
                return;
            }
            if (Datas.type == "success") {
                //window.location = window.location.origin;
                ListResultAnomalie = Datas.data;
                $.each(ListResultAnomalie, function (k, v) {
                    contentAnomalies += `<tr compteG-id="${v.No}">
                        <td>
                            <input type="checkbox" name = "checkprod" compteg-ischecked/>
                        </td><td>${v.No}</td>
                        <td>${v.DateOrdre}</td>
                        <td>${v.NoPiece}</td>
                        <td>${v.Compte}</td>
                        <td>${v.Libelle}</td>
                        <td>${v.Debit}</td>
                        <td>${v.Credit}</td>
                        <td>${v.MontantDevise}</td>
                        <td>${v.Mon}</td>
                        <td>${v.Rang}</td>
                        <td>${v.FinancementCategorie}</td>
                        <td>${v.Commune}</td>
                        <td>${v.Plan6}</td>
                        <td>${v.Journal}</td>
                        <td>${v.Marche}</td>
                    </tr>`

                });
                $('.anomalieslist').html(contentAnomalies);
            }

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
})

function getelementTXT(a) {
    //var anal = new Array();

    //UXD
    //var list = "";
    //$.each($("#afb tr[compteg-id]"), (k, v) => {
    //    list += $(v).attr("compteg-id") + ",";
    //});

    //let formData = new FormData();
    //formData.append("suser.LOGIN", User.LOGIN);
    //formData.append("suser.PWD", User.PWD);
    //formData.append("suser.ROLE", User.ROLE);
    //formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    //formData.append("list", list);

    //$.ajax({
    //    type: "POST",
    //    url: Origin + '/Home/CreateZipFile',
    //    cache: false,
    //    contentType: 'application/json; charset=utf-8',
    //    processData: false,
    //    data: formData,
    //    success: function (result) {
    //        var Datas = JSON.parse(result);
    //        console.log(Datas);
    //    },
    //    error: function () {
    //        alert("Problème de connexion. ");
    //    }
    //});
    //UXD


    //tsy sitonina le ambiny
    //$("#afb tr").each(function () {
    //    var row = $(this);
    //    var ttab = {};
    //    ttab.Numero = row.find("TD").eq(1).html();

    //    ttab.Datedordre = row.find("TD").eq(2).html();

    //    var txtR = row.find("TD").eq(3).html();
    //    var repl = /<br>/gi;
    //    if (txtR) {
    //        txtR = txtR.replace(repl, '\\n');
    //        txtR = jQuery('<p>' + txtR + '</p>').text();
    //    }
    //    ttab.NumPiece = jQuery('<p>' + txtR + '</p>').text();

    //    ttab.Compte = row.find("TD").eq(4).html();
    //    ttab.Libelle = row.find("TD").eq(5).html();
    //    ttab.debit = row.find("TD").eq(6).html();
    //    ttab.credit = row.find("TD").eq(7).html();
    //    ttab.Montadevise = row.find("TD").eq(8).html();
    //    ttab.Mon = row.find("TD").eq(9).html();
    //    ttab.Rang = row.find("TD").eq(10).html();
    //    ttab.FinCat = row.find("TD").eq(11).html();
    //    ttab.Comm = row.find("TD").eq(12).html();
    //    ttab.Plan6 = row.find("TD").eq(13).html();
    //    ttab.Journal = row.find("TD").eq(14).html();
    //    ttab.Marche = row.find("TD").eq(15).html();
    //    anal.push(ttab);
    //});

    //anal.shift();
    //var analY = anal;

    let formData = new FormData();
    formData.append("suser.LOGIN", User.LOGIN);
    formData.append("suser.ID", User.ID);
    formData.append("suser.PWD", User.PWD);
    formData.append("suser.ROLE", User.ROLE);
    formData.append("suser.IDSOCIETE", User.IDSOCIETE);
    formData.append("baseName", baseName);
    formData.append("codeJ", $('#commercial').val());
    formData.append("devise", false);
    formData.append("intbasetype", a);
    $.ajax({
        type: "POST",
        url: Origin + '/Home/CreateZipFile',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            var Datas = JSON.parse(result);
            alert(Datas.data)
            if (Datas.type == "error") {
                return;
            }

            window.location = '/Home/GetFile?file=' + Datas.data;

        },
        error: function () {
            alert("Problème de connexion. ");
        }
    });
}
$(`[tab="autre"]`).hide();
var baseName = "1";
$(`[name="options"]`).on("change", (k, v) => {
    
    var baseId = $(k.target).attr("data-id");
    alert(baseId);
    baseName = baseId;
    if (baseId == "1") {
        $(`[tab="paie"]`).show();
        $(`[tab="autre"]`).hide();
        GetListCodeJournal(baseName);
    } else {
        $(`[tab="autre"]`).show();
        $(`[tab="paie"]`).hide();
        $('.afb160').empty();
        $('#afb').empty();

        ListCompteG = ``;
        GetListCodeJournal(baseName);
    }

});
$('.Checkall').change(function () {
    alert("checkall");
    if ($('.Checkall').prop("checked") == true) {
        $('[compteg-ischecked]').prop("checked", false);
    } else {
        $('[compteg-ischecked]').prop("checked", true);
    }

});
function exportTableToExcel(tableID, filename = 'RAS') {
    var downloadLink;
    var dataType = 'application/vnd.ms-excel';
    var tableSelect = document.getElementById(tableID);
    var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

    // Specify file name
    filename = filename ? filename + '.xls' : 'excel_data.xls';

    // Create download link element
    downloadLink = document.createElement("a");

    document.body.appendChild(downloadLink);
    if (confirm("Télécharger")== true) {
        if (navigator.msSaveOrOpenBlob) {
            var blob = new Blob(['\ufeff', tableHTML], {
                type: dataType
            });
            navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // Create a link to the file
            downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

            // Setting the file name
            downloadLink.download = filename;

            //triggering the function
            downloadLink.click();
        }
    }
   
}

//let urlOrigin = "https://localhost:44334";
//let urlOrigin = "http://softwell.cloud/OPAVI";
