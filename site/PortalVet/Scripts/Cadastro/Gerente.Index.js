$(function () {


    $("#btnClear").click(function () {

        $("#txtFilterNome").val("");
        $("#txtFilterEmail").val("");
        $("#txtFilterCPFCNPJ").val("");
        $("#txtFilterTelefone").val("");
        MODULO.PageIndex = 1;
        //$("#drpFilterStatus").val("");
        //$("#drpFilterStatus").select2();
        MODULO.Buscar();
    });


    $("#btnFilter").click(function () {
        MODULO.PageIndex = 1;
        MODULO.Buscar();
    });


    $("#btnNovoRegistro").click(function () {
        MODULO.Create();
    });


    $('.telefone').mask('00 000000000000');
    $("#txtFilterCPFCNPJ").keydown(function () {
        try {
            $("#txtFilterCPFCNPJ").unmask();
        } catch (e) { }

        var tamanho = $("#txtFilterCPFCNPJ").val().length;

        if (tamanho < 11) {
            $("#txtFilterCPFCNPJ").mask("999.999.999-99");
        } else {
            $("#txtFilterCPFCNPJ").mask("99.999.999/9999-99");
        }

        // ajustando foco
        var elem = this;
        setTimeout(function () {
            // mudo a posição do seletor
            elem.selectionStart = elem.selectionEnd = 10000;
        }, 0);
        // reaplico o valor para mudar o foco
        var currentValue = $(this).val();
        $(this).val('');
        $(this).val(currentValue);
    });

    window.setTimeout(function () {

        //$("#drpFilterPerfil option[value='10']").remove();
        //$("#drpFilterPerfil").select2();


        MODULO.Buscar();
        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), $("#hdnControllerName").val(), "Consulta");
    }, 100);

});

var MODULO = {
    PageIndex: 1,
    PageSize: 50,
    AreaNome: "Cadastro",
    ControllerNome: "Gerente",

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
                window.location = JSERVICE.rootApplication + "Home/ForceLogin?pid=" + $("#hdnUsuarioId").val() + "&id=" + retorno.Data;
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
    Buscar: function (page) {

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

                if (this.PROFILE_NAME === "Corretor")
                    html += "<td></td>";
                else
                    html += "<td><div style='cursor:pointer;' onclick='MODULO.Edit(" + this.Id + ")' class='font-icon font-icon-pencil'></div></td>";

                if (parseInt($("#hdnUsuarioId").val()) === this.ID)
                    html += "<td></td>";
                else
                    html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.Id + ")' class='font-icon font-icon-del'></div></td>";

                // html += "<td><small>" + this.EMPRESANAME + "</small></td>";
                html += "<td>" + this.Nome + "</td>";
                html += "<td>" + this.Sobrenome + "</td>";
                html += "<td>" + this.CPFCNPJFmt + "</td>";
                // html += "<td>" + this.PROFILE_NAME + "</td>";
                html += "<td>" + this.Email + "</td>";
                html += "<td>" + this.Telefone + "</td>";
                
 
                /*
                 * 
                 * 
              

                html += "<td>";

                if (this.ACTIVE && $("#hdnUsuarioId").val() !== this.ID && this.PROFILE_NAME !== "Administrador")
                    html += "<div style='cursor:pointer;' onclick='MODULO.Login(" + this.ID + ")' class='font-icon font-icon-user'></div></td>";

                html += "</td>";
                */
                html += "</tr>";

            });


            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);

            JSERVICE.GenerateSort("tbGrid");

        }, true);

        //JSERVICE.Loading(false);
    }
}
