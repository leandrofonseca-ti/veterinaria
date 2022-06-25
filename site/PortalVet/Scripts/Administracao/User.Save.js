$(function () {

    $("#btnSave").click(function () {
        MODULO.Save();
    });

    $("#btnBack").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });

    $("#btnImageRemove").click(function () {
        MODULO.RemoveImage($("#hdnId").val(), "", "", "");
    });

    $("#chkAlterarSenha").change(function () {
        if ($("#chkAlterarSenha").is(":checked")) {
            $("#txtSenha").val("");
            $("#fieldSenha").show();
        } else {
            $("#fieldSenha").hide();
        }
        //MODULO.RemoveImage($("#hdnId").val(), "", "", "");
    });

    window.setTimeout(function () {

        if ($("#hdnId").val() !== "") {
            MODULO.Load();
        }

        $("#drpPerfil option[value='10']").remove();
        $("#drpPerfil").select2();


        $("#drpEmpresas").select2();

        $("#drpEmpresa").select2();

        $('#txtTelefone').mask('(99) 9999-999999');
        JSERVICE.ShowBreadcrumb($("#hdnAreaName").val(), "Usuário", "Cadastro"); 
    }, 100);

});


/*jslint unparam: true */
/*global window, $ */
$(function () {
    'use strict';
    // Change this to the location of your server-side upload handler:
    //var url = window.location.hostname === 'blueimp.github.io' ? '//jquery-file-upload.appspot.com/' : 'server/php/';
    var url = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/FileSave";
    $('#fileupload').fileupload({

        url: url,
        //dataType: 'json',
        autoUpload: true,
        /*add: function (e, data) {
            data.submit();
        },*/
        progressall: function (e, data) {

            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css(
                'width',
                progress + '%'
            );

            $('#files').html('<p class="upl">Enviando arquivos. ' + progress + '%</p>');

            if (progress >= 100) {
                $('#files').html("Arquivo(s) enviados com sucesso!");
                // updateImages();
            }

        },
        done: function (e, data) {
            var result = JSON.parse(data.result);

            if (result.Status === 'OK') {
                MODULO.SaveImage($("#hdnId").val(), result.Path, result.FileName, result.FileNameThumb);


            }
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});



var MODULO = {
    PageIndex: 1,
    PageSize: 10,
    AreaNome: "Administracao",
    ControllerNome: "User",


    Save: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();


        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                });

                $("#msgError").addClass("show");
                if (retorno.Error && retorno.Message !== "") {                    
                    var msg = "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>";
                    msg = msg  +"<span aria-hidden='true'>×</span>";
                    msg = msg +"</button>";
                    msg = msg + "<strong>Atenção!</strong>&nbsp;" + retorno.Message;
                    $("#msgError").html(msg);
                }

            }
            else {
                window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
            }
        }, true);

        //JSERVICE.Loading(false);
    },
    SaveImage: function (id, path, image, thumb) {

        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        json.push({ "name": "image", "value": image });
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveImage", json, function (retorno) {

            if (retorno.Error) {

            } else {
                $("#uplImage").attr("src", path + image);

                setTimeout(function () {
                    $('#progress .progress-bar').css('width', '0%');
                    $('#files').html('<p class="upl"></p>');
                }, 4000);
            }
        });
    },

    RemoveImage: function (id, path, image, thumb) {

        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        json.push({ "name": "image", "value": image });
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveImage", json, function (retorno) {

            if (retorno.Error) {

            } else {
                $("#uplImage").attr("src", JSERVICE.rootApplication + "Content/img/avatar-1-128.png");

                setTimeout(function () {
                    $('#progress .progress-bar').css('width', '0%');
                    $('#files').html('<p class="upl"></p>');
                }, 4000);
            }
        });
    },
    Load: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            var html = "";

            if (!retorno.Error) {

                //retorno.Data.PerfilId
                $("#txtNome").val(retorno.Data.Nome);
                $("#txtSobreNome").val(retorno.Data.Sobrenome);
                $("#txtEmail").val(retorno.Data.Email);
                $("#txtTelefone").val(retorno.Data.TelefoneFmt);   
                

                $(retorno.Data.Perfis).each(function (z, obj) {
                    $("#drpPerfil option[value='" + obj.Id + "']").prop("selected", true);
                });
                $("#drpPerfil").select2();

                
                $(retorno.Data.Empresas).each(function (z, obj) {
                    $("#drpEmpresas option[value='" + obj.Id + "']").prop("selected", true);
                });
                $("#drpEmpresas").select2();

           
                //$("#txtUsuario").val(retorno.Data.USERNAME);
                if (retorno.Data.Imagem !== null && retorno.Data.Imagem !== "") {
                    $("#uplImage").attr("src", $("#hdnPathUrlUser").val() + retorno.Data.Imagem);
                }
                $("#areaFoto").show();
                if (retorno.Data.Active) {
                    $("#chkAtivo").attr('checked', 'checked');
                }
            }

        }, true);

        //JSERVICE.Loading(false);
    }
}
