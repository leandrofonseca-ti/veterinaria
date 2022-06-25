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


        /*
               Administrador = 1,
               Gerente = 2,
               Clinica = 3,
               Cliente = 4,
               Laudador = 5
            */
        if ($("#hdnPerfilCodigo").val() === "4" ||
            $("#hdnPerfilCodigo").val() === "5") {
            $(".campoFiltro").hide();
        }

        if ($("#hdnPerfilCodigo").val() === "3" ||
            $("#hdnPerfilCodigo").val() === "4" ||
            $("#hdnPerfilCodigo").val() === "5") {
            $("#btnNovoRegistro").hide();
        }

        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), $("#hdnControllerName").val(), "Consulta");
    }, 100);

});

var MODULO = {
    PageIndex: 1,
    PageSize: 10,
    AreaNome: "Cadastro",
    ControllerNome: "Exame",


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
    ViewPDF: function (id) {
        var location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/ViewContratoModeloPDF/" + id;
        window.open(location, "_blank");

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


                /*
                    Administrador = 1,
                    Gerente = 2,
                    Clinica = 3,
                    Cliente = 4,
                    Laudador = 5
                 */
                if ($("#hdnPerfilCodigo").val() === "1" ||
                    $("#hdnPerfilCodigo").val() === "2") {
                    html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.Id + ")' class='font-icon font-icon-del'></div></td>";
                }
                else {
                    html += "<td></td>";
                }
                //  html += "<td><div style='cursor:pointer;' onclick='MODULO.ViewPDF(" + this.Id + ")' class='font-icon font-icon-pdf-fill'></div></td>";



                html += "<td><strong>" + this.Id + "</strong></td>";
                html += "<td>" + this.DataExameFmt + " " + this.DataExameHH + ":" + this.DataExameMM + "</td>";
                html += "<td>" + this.NomeCliente + "</td>";
                html += "<td>" + this.NomeLaudador + "</td>";
                html += "<td>" + this.SituacaoNome + "</td>";

                html += "<td>";
                html += "<div class='dropdown'>";
                html += "<button class='btn btn-inline dropdown-toggle' type='button' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>";
                html += "<span class='font-icon font-icon-burger'>&nbsp;<small></small></span>";
                html += "</button>";
                html += "<div class='dropdown-menu' aria-labelledby='dd-header-add'>";
                //html += "<a class='dropdown-item label-encaminhar' style='color:#fff' href='@Url.Action(' Save','Exame',new { @area = 'Cadastro' })/{{ item.Id }}'><span class='label label-encaminhar  cursorPointer'>Edição</span></a>";
                //html += "<a class='dropdown-item label-warning' style='color:#fff' href='javascript:alert(2);' ><span class='label label-warning  cursorPointer'>Links</span></a>";
                html += "<a class='dropdown-item label-info' style='color:#fff' href='javascript:;' onclick='MODULO.ViewPDF(" + this.Id + ")'  ><span class='label label-info cursorPointer'>Visualizar PDF</span></a>";
                //html += "<a class='dropdown-item label-primary' style='color:#fff' href='javascript:alert(4);' ><span class='label label-primary cursorPointer'>Enviar PDF</span></a>";
                html += "</div>";
                html += "</div>";
                html += "</td>";

                html += "</tr>";

            });


            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);

            JSERVICE.GenerateSort("tbGrid");

        }, true);

    }
}
