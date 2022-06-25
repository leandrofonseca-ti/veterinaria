$(function () {

    $("#btnSave").click(function () {
        MODULO.Save();
    });

    $("#btnBack").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });


    $("#chkAtivo").click(function () {
        if ($(this).is(":checked")) {
            $("#areaEmpresas").hide();
        } else {
            $("#areaEmpresas").show();
        }
    });


    window.setTimeout(function () {

        if ($("#hdnId").val() !== "") {
            MODULO.Load();
        }

        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), $("#hdnControllerName").val(), "Cadastro"); 
    }, 100);

});

var MODULO = {
    PageIndex: 1,
    PageSize: 10,
    AreaNome: "Administracao",
    ControllerNome: "Menu",


    Save: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                });

                $("#msgError").addClass("show");
            }
            else {
                window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
            }
        }, true);

        //JSERVICE.Loading(false);
    },

    Load: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            var html = "";

            if (retorno.Error === false) {

                $("#txtNome").val(retorno.Data.Nome);
                $("#txtCaminho").val(retorno.Data.Path);
                $("#txtModulo").val(retorno.Data.Modulo);
                $("#txtIcone").val(retorno.Data.IconeCss);
                $("#txtArea").val(retorno.Data.AreaNome);
                $("#txtController").val(retorno.Data.ControllerNome);
                $("#txtAction").val(retorno.Data.ActionNome);
                $("#txtOrdem").val(retorno.Data.Ordem);
                $("#drpMenuPai").val(retorno.Data.ParentId);
                $("#drpMenuPai").select2();
                if (retorno.Data.Ativo) {
                    $("#chkAtivo").attr('checked', 'checked');
                    $("#areaEmpresas").hide();
                }
                else {
                    $("#areaEmpresas").show();


                    var codigosEmpresa = [];
                    $(retorno.Data.ListEmpresasTeste).each(function (k, b) {
                        codigosEmpresa.push("" + b.EmpresaId + "");
                    });

                    $("#drpEmpresa").val(codigosEmpresa).trigger('change');


                }
            }

        }, true);

        //JSERVICE.Loading(false);
    }
};
