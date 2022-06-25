$(function () {


    $("#btnClear").click(function () {
        $("#drpFilterEmpresa").val("");
        $("#drpFilterEmpresa").select2();

        $("#drpFilterPerfil").val("");
        $("#drpFilterPerfil").select2();

        $("#txtFilterEmail").val("");
        MODULO.PageIndex = 1;
        MODULO.Buscar();
    });


    $("#btnFilter").click(function () {
        MODULO.PageIndex = 1;
        MODULO.Buscar();
    });


    $("#btnNovoRegistro").click(function () {
        MODULO.Create();
    });

    window.setTimeout(function () {

        if ($("#hdnFilterEmpresaId").val() !== "") {
            $("#drpFilterEmpresa").val($("#hdnFilterEmpresaId").val());
            $("#drpFilterEmpresa").select2();
        }

        MODULO.PageIndex = 1;
        MODULO.Buscar();
  

        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), "Usuário", "Consulta");
    }, 100);
});

var MODULO = {
    PageIndex: 1,
    PageSize: 50,
    AreaNome: "Administracao",
    ControllerNome: "User",

    Login: function (id) {
        $("#modalLogin").modal("show");
        $("#btnConfirmLogin").attr("onclick", "MODULO.LoginConfirm(" + id + ");");
    },
    LoginConfirm: function (id) {
        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/LoginUser", json, function (retorno) {
            if (retorno.Data === 0) {
                // e.preventDefault();
                swal("Não foi possível realizar login!");
            }
            else {
                window.location.href = JSERVICE.rootApplication + "Admin/ForceLogin?pid=" + $("#hdnUsuarioId").val() + "&id=" + retorno.Data;
            }
            $("#modalLogin").modal("hide");
        }, true);

    },
    Create: function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save";
    },
    Edit: function (id) {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + id;
    },
    Remove: function (id) {
        $("#modalRemove").modal("show");
        $("#btnConfirmRemove").attr("onclick", "MODULO.RemoveConfirm(" + id + ");");
    },
    RemoveConfirm: function (id) {

        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Remove", json, function (retorno) {
            MODULO.PageIndex = 1;
            MODULO.Buscar();
        }, true);
        $("#modalRemove").modal("hide");
    },
    Pagination: function (index) {
        MODULO.PageIndex = index;
        MODULO.Buscar();
    },
    EqueciSenha: function (email, userid) {

        swal({
            title: "Atenção",
            text: "Deseja recuperar senha para o '" + email + "'?",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    var json = $("form").serializeArray();
                    json.push({ "name": "userid", "value": userid });
                    json.push({ "name": "email", "value": email });
                    JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/EnviarCredenciais", json, function (retorno) {
                        if (retorno.Data === true) {
                            JSERVICE.Mensagem("E-mail para recuperar senha enviado com sucesso", "Aviso", "success");
                            swal.close();
                        }
                        else {
                            JSERVICE.Mensagem("Não foi possível enviar, tente mais tarde!", "Aviso", "error");
                            swal.close();
                        }

                    }, true);
                } else {
                    swal.close();
                }
            });


    },
    Buscar: function () {

        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();
        json.push({ "name": "pageIndex", "value": MODULO.PageIndex });
        json.push({ "name": "pageSize", "value": MODULO.PageSize });
        json.push({ "name": "pageOrderCol", "value": JSERVICE.OrderNumber });
        json.push({ "name": "pageOrderSort", "value": JSERVICE.OrderSort });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/List", json, function (retorno) {

            var html = "";

            $(retorno.Data).each(function () {

                html += "<tr>";

                //if (this.PROFILE_NAME === "Corretor")
                //    html += "<td></td>";
                //else
                    html += "<td><div style='cursor:pointer;' onclick='MODULO.Edit(" + this.Id + ")' class='font-icon font-icon-pencil'></div></td>";


                html += "<td>";
                html += "<div style='cursor:pointer;' title='Recuperar senha' onclick='MODULO.EqueciSenha(\"" + this.Email + "\"," + this.Id + ")' class='font-icon font-icon-mail'></div></td>";
                html += "</td>";


                if (parseInt($("#hdnUsuarioId").val()) === this.Id)
                    html += "<td></td>";
                else
                    html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.Id + ")' class='font-icon font-icon-del'></div></td>";

                var empresaNome = "";
                $(this.Empresas).each(function (t, k) {

                    //if (k.PerfilId === 1) {
                    //    IsAdmin = true;
                    //}
                    if (empresaNome === "") {
                        empresaNome = k.Nome;
                    }
                    else {
                        empresaNome += ", " + k.Nome;
                    }
                });


                html += "<td><small>" + empresaNome + "</small></td>";
                html += "<td>" + this.Nome + "</td>";
                html += "<td>" + this.Email + "</td>";
                //html += "<td>" + this.PROFILE_NAME + "</td>";

                var pushIds = new Array();
                var perfisNome = "";
                var IsAdmin = false;
                $(this.Perfis).each(function (t, k) {

                    if (k.PerfilId === 1) {
                        IsAdmin = true;
                    }
                    if (perfisNome === "") {
                        perfisNome = k.Nome;
                    }
                    else {
                        perfisNome += ", " + k.Nome;
                    }

                    pushIds.push(k.PerfilId);
                });
                html += "<td><small>" + perfisNome + "</small></td>";
                // html += "<td>" + this.EMAIL + "</td>";
                if (this.Active)
                    html += "<td>Sim</td>";
                else
                    html += "<td>Não</td>";

                html += "<td>";

                var temPerfilAcesso = false;
                //if (jQuery.inArray(parseInt($("#hdnProfileIdImobiliaria").val()), pushIds) !== -1 ||
                //    jQuery.inArray(parseInt($("#hdnProfileIdCorretor").val()), pushIds) !== -1 ||
                //    jQuery.inArray(parseInt($("#hdnProfileIdCorretora").val()), pushIds) !== -1 ||
                //    jQuery.inArray(parseInt($("#hdnProfileIdConsultor").val()), pushIds) !== -1 ||
                //    jQuery.inArray(parseInt($("#hdnProfileIdConsultorTotal").val()), pushIds) !== -1
                //) {

                   temPerfilAcesso = true;
                //}

                //this.ACTIVE && 
                if (parseInt($("#hdnCurrentUsuarioId").val()) !== this.Id && this.Active && temPerfilAcesso === true)
                    html += "<div style='cursor:pointer;' onclick='MODULO.Login(" + this.Id + ")' class='font-icon font-icon-user'></div></td>";

                html += "</td>";




                html += "</tr>";

            });


            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);

            JSERVICE.GenerateSort("tbGrid");

        }, true);

        //JSERVICE.Loading(false);
    }
}
