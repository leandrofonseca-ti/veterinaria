$(function () {

    $("#btnNovoRegistro").click(function () {
        MODULO.Create();
    });

    window.setTimeout(function () {
        MODULO.Buscar();
        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), $("#hdnControllerName").val(), "Consulta");
    }, 100);


});

var MODULO = {
    PageIndex: 1,
    PageSize: 50,
    AreaNome: "Cadastro",
    ControllerNome: "Assinatura",
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
    View: function (id) {
        var location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/ViewContratoModeloPDF/" + id;
        window.open(location, "_blank");

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


        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/List", json, function (retorno) {

            var html = "";

            $(retorno.Data).each(function () {

                html += "<tr>";
                html += "<td><div style='cursor:pointer;' onclick='MODULO.Edit(" + this.Id + ")' class='font-icon font-icon-pencil'></div></td>";
                html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.Id + ")' class='font-icon font-icon-del'></div></td>";

                html += "<td>" + this.Nome + "</td>";

                //if (this.Perfil == "PF")
                //    html += "<td>Pessoa Fisíca</td>";
                //else
                //    html += "<td>Pessoa Jurídica</td>";

              //  html += "<td><div style='cursor:pointer;' onclick='MODULO.View(" + this.Id + ")' class='fa fa-search'></div></td>";


                html += "</tr>";

            });

            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);


        }, true);

        //JSERVICE.Loading(false);
    }
}
