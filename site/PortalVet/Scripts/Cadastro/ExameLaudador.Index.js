$(function () {


    $("#btnClear").click(function () {
        $("#drpFilterCliente").val("");
        $("#drpFilterCliente").select2();

        $("#drpFilterSituacao").val("");
        $("#drpFilterSituacao").select2();

        $("#drpFilterLaudador").val("");
        $("#drpFilterLaudador").select2();

        $("#txtFilterCodigo").val("");
        $("#txtFilterEmail").val("");
        $("#txtFilterDtExame").val("");


        MODULO.PageIndex = 1;
        MODULO.Buscar();
    });


    $("#txtFilterDtExame").datepicker({

        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Proximo',
        prevText: 'Anterior'
    });


    $('.data').mask('00/00/0000');



    $("#btnFilter").click(function () {
        MODULO.PageIndex = 1;
        MODULO.Buscar();
    });


    $("#btnNovoRegistro").click(function () {
        MODULO.Create();
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
    ControllerNome: "ExameLaudador",


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
                html += "<td><div style='cursor:pointer;' onclick='MODULO.Edit(" + this.Id + ")' class='font-icon font-icon-pencil'></div></td>";
               // html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.Id + ")' class='font-icon font-icon-del'></div></td>";
                html += "<td><strong>" + this.Id + "</strong></td>";
                html += "<td>" + this.DataExameFmt + " " + this.DataExameHH + ":" + this.DataExameMM + "</td>";
                html += "<td>" + this.NomeCliente + "</td>";
                html += "<td>" + this.NomeLaudador + "</td>";
                html += "<td>" + this.SituacaoNome + "</td>";

                html += "</tr>";

            });


            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);

            JSERVICE.GenerateSort("tbGrid");

        }, true);

    }
}
