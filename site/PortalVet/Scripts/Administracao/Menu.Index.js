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
    AreaNome: "Administracao",
    ControllerNome: "Menu",
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
        

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/List", json, function (retorno) {

            var html = "";

            $(retorno.Data).each(function () {

                html += "<tr>";
                //html += "<td class='bs-checkbox'>";
                //html += "<div class='checkbox checkbox-only' style='text-wrap:none;overflow-wrap:inherit'>";
                //html += "<input id='datatable-checkbox-0' data-index='0' name='btSelectItem' type='checkbox' value='0' />";
                //html += "<label for='datatable-checkbox-0'></label>";
                //html += "</div>";
                //html += "</td>";
                html += "<td><div style='cursor:pointer;' onclick='MODULO.Edit(" + this.MenuId + ")' class='font-icon font-icon-pencil'></div></td>";
                html += "<td><div style='cursor:pointer;' onclick='MODULO.Remove(" + this.MenuId + ")' class='font-icon font-icon-del'></div></td>";
                if (this.ParentNome === null)
                    html += "<td>-</td>";
                else
                    html += "<td>" + this.ParentNome + "</td>";

                html += "<td>" + this.Nome + "</td>";
                html += "<td>" + this.Modulo + "</td>";

                if (this.Ativo)
                    html += "<td>Sim</td>";
                else
                    html += "<td>Não</td>";
                html += "</tr>";

            });

            $("#tbGrid tbody").html(html);

            JSERVICE.BuildPagination(retorno);
 
            
        }, true);

        //JSERVICE.Loading(false);
    }
}
