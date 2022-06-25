$(function () {
 
    $("#btnSave").click(function () {
        MODULO.Save();
    });

    $("#btnBack").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });


    


    $("#btnAddRegraTotal").click(function () {
        var validoMenu = true;
        var validoRegra = true;
        $("#drpMenu").removeClass("form-control-error");
        $("#drpRegra").removeClass("form-control-error");
        if ($("#drpMenu").val() === "") {
            $("#drpMenu").addClass("form-control-error");
            validoMenu = false;
        }

        if ($("#drpRegra").val() === "") {
            $("#drpRegra").addClass("form-control-error");
            validoRegra = false;
        }

        if (validoMenu === true && validoRegra === true) {
            
            if ($("#hdnId").val() !== "") {
                //JSERVICE.Loading(true);
                MODULO.SavePermission($("#drpMenu").val(), $("#drpRegra").val());
                //JSERVICE.Loading(false);
            }
            else {
                swal("Primeiro realize o cadastro do Perfil!");
            }
            
        }
    });

    $("#btnAddRegraModal").click(function () {

        $("#drpMenu").removeClass("form-control-error");
        $("#drpRegra").removeClass("form-control-error");


        if ($("#drpMenu").val() === "") {
            $("#drpMenu").addClass("form-control-error");
        }
        else {
            if ($("#hdnId").val() !== "") {
                
                $("#addRegraModal").modal("show");
            }
            else {                
                swal("Primeiro realize o cadastro do Perfil!");
            }
            
        }
    });


    $("#btnSaveModalRegra").click(function () {
        if ($("#hdnId").val() !== "") {



            var validoRegra = true;
            var validoAction = true;
            $("#drpRegraModal").removeClass("form-control-error");
            $("#txtActionModal").removeClass("form-control-error");

            if ($("#drpRegraModal").val() === "") {
                $("#drpRegraModal").addClass("form-control-error");
                validoRegra = false;
            }

            if ($("#txtActionModal").val() === "") {
                $("#txtActionModal").addClass("form-control-error");
                validoAction = false;
            }

            if (validoAction === true && validoRegra === true) {
                MODULO.SaveRole($("#drpRegraModal").val(), $("#drpRegraModal option:selected").text(), $("#txtActionModal").val());
                $("#addRegraModal").modal("hide");
            }
        }
        else {
            swal("Primeiro realize o cadastro do Perfil!");
        }
    });

    

    $("#drpMenu").change(function () {
        //JSERVICE.Loading(true);        
        if ($(this).val() !== "") {
            MODULO.LoadRegras();
        } else {
            var html = "<option value=''>:: Selecione ::</option>";            
            $("#drpRegra").html(html);
        }
        //JSERVICE.Loading(false);
    });




    window.setTimeout(function () {

        MODULO.LoadMenu();
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
    ControllerNome: "Perfil",
    RemoveRole: function (regraid) {

        swal({
            title: "",
            text: "Deseja remover este registro?",
            type: "error",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Remover",
            cancelButtonText: "Cancelar",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function () {

                MODULO.RemoveRoleConfirm(regraid);
                    //swal({
                    //    title: "Deleted!",
                    //    text: "Your imaginary file has been deleted.",
                    //    type: "success",
                    //    confirmButtonClass: "btn-success"
                    //});
                
                    //swal({
                    //    title: "Cancelled",
                    //    text: "Your imaginary file is safe :)",
                    //    type: "error",
                    //    confirmButtonClass: "btn-danger"
                    //});
                
            });
             
              
           

    },
    RemoveRoleConfirm: function (regraid) {

        var perfilid = $("#hdnId").val();
        var json = $("form").serializeArray();
        json.push({ "name": "regraid", "value": regraid });
        json.push({ "name": "perfilid", "value": perfilid });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/RemoveRole", json, function (retorno) {

            if (retorno.Error) {
            }
            else {

                //swal({
                //    title: "Removido!",
                //    text: "Registro removido com sucesso.",
                //    type: "success",
                //    confirmButtonClass: "btn-success"
                //})

                MODULO.Load();

            }
        }, true);

        swal.close();
    },
    SaveRole: function (chave, nome, action) {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();
        json.push({ "name": "chave", "value": chave });
        json.push({ "name": "nome", "value": nome });
        json.push({ "name": "action", "value": action });
        
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveRole", json, function (retorno) {

            if (retorno.Error) {
            }
            else {
                MODULO.LoadRegras();
                
            }
        }, true);

        //JSERVICE.Loading(false);
    },
    SavePermission: function (menuid, regraid) {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SavePermission", json, function (retorno) {

            if (retorno.Error) {
            }
            else {

                MODULO.Load();
            }
        }, true);

        //JSERVICE.Loading(false);
    },
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
    LoadRegras: function () {
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/ListRegras", json, function (retorno) {

            if (retorno.Error) {
            }
            else {
                var html = "<option value=''>:: Selecione ::</option>";
                $(retorno.Data).each(function () {
                    html = html + "<option value='" + this.Id + "'>" + this.Nome + "</option>";
                });

                $("#drpRegra").html(html);
            }

        }, true);

    },
 
    LoadMenu: function () {
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/ListMenus", json, function (retorno) {

            if (!retorno.Error) {
                //retorno.Data.PerfilId
                var html = "<option value=''>:: Selecione ::</option>";
                $(retorno.Data).each(function () {
                    html = html + "<option value='" + this.MenuId + "'>" + this.Nome + "</option>";
                });

                $("#drpMenu").html(html);
            }

        }, true);
        
    },
    Load: function () {
        //JSERVICE.Loading(true);        
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            var html = "";

            if (retorno.Error) {
            }
            else {
                $("#txtNome").val(retorno.Data.Nome);
            }
            
        }, true);


        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/ListPermissoes", json, function (retorno) {

            var html = "";
            
            if (retorno.Error) {
            }
            else {
                
                var html = "";

                ///alert(JSON.stringify(retorno.Data));
                $(retorno.Data).each(function () {
                    //
                    html = html + "<tr>";
                    html = html + "<td>";
                   // html = html + "<button type='button' class='btn btn-rounded btn-inline btn-primary btn-sm ladda-button' onclick='MODULO.AddRole(" + this.MenuId + ");'>+</button>";
                    html = html + this.Nome;
                    html = html + "<br/><small>(" + this.Module +")</small>";
                    html = html + "</td>";
                    html = html + "<td>";

                    html = html + "<ul>";
                    $(this.SubItems).each(function (index, element) {
                        html = html + "<li>";

                        html = html + "<table cellspacing='0' cellpadding='0'><tr>";
                        html = html + "<td style='border: 0px'>";
                        html = html + "<button  type='button' data-style='expand-right' onclick='MODULO.RemoveRole(" + element.RoleId +");' data-size='s' class='btn btn-inline btn-danger btn-sm ladda-button'><span class='ladda-label'>-</span><span class='ladda-spinner'></span></button>";                        
                        html = html + "</td>";
                        html = html + "<td style='border: 0px'>";
                        html = html + "<label for='check-toggle-1'>" + element.Nome + "</label>";
                        html = html + "</td>";
                        html = html + "</tr></table>";


                        html = html + "</li>";
                    });
                    html = html + "</ul>";
                    html = html + "</td>";
                    html = html + "</tr>";
                });
               
              
                $("#tbGrid tbody").html(html);
            }

        }, true);

        //JSERVICE.Loading(false);
    }
}
