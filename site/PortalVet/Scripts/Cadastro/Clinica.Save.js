$(function () {
    $("#btnImageRemove").click(function () {
        MODULO.RemoveImage($("#hdnId").val(), "", "", "");
    });

    $("#btnBack").click(function () {
        MODULO.Back();
    });

    $("#btnSave").click(function () {
        MODULO.Save();
    });
 

});

window.setTimeout(function () {

    if ($("#hdnId").val() !== "") {
        MODULO.Load();
    }
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
    PageSize: 30,
    AreaNome: "Cadastro",
    ControllerNome: "Clinica",

    Back: function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    },
    SaveImage: function (id, path, image, thumb) {


        if (id !== "") {
            var json = $("form").serializeArray();
            json.push({ "name": "id", "value": id });
            json.push({ "name": "image", "value": image });

            JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveImage", json, function (retorno) {

                if (!retorno.Error) {

                    $("#btnImageRemove").show();
                    $("#btnImageAdd").hide();

                    $("#uplImage").attr("src", path + "/" + image);
                    //$("#hdnFileFoto").val(image);
                    $("#hdnPicture").val(image);
                    setTimeout(function () {
                        $('#progress .progress-bar').css('width', '0%');
                        $('#files').html('<p class="upl"></p>');
                    }, 4000);
                }
            });
        } else {

            $("#btnImageRemove").show();
            $("#btnImageAdd").hide();

            $("#uplImage").attr("src", path + "/" + image);

            $("#hdnPicture").val(image);

            setTimeout(function () {
                $('#progress .progress-bar').css('width', '0%');
                $('#files').html('<p class="upl"></p>');
            }, 4000);
        }
    },
    RemoveImage: function (id, path, image, thumb) {

        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        json.push({ "name": "image", "value": image });
        json.push({ "name": "oldfile", "value": $("#hdnFileFoto").val() });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveImage", json, function (retorno) {

            if (!retorno.Error) {

                $("#btnImageRemove").hide();
                $("#btnImageAdd").show();


                $("#uplImage").attr("src", JSERVICE.rootApplication + "Content/img/avatar-1-128.png");
                $("#hdnFileFoto").val("");
                $("#hdnPicture").val("");
                setTimeout(function () {
                    $('#progress .progress-bar').css('width', '0%');
                    $('#files').html('<p class="upl"></p>');
                }, 4000);
            }
        });
    },
    Save: function () {

        $(".form-control-error").removeClass("form-control-error");

        //$("input.form-control, select.select2").each(function () {
        //    $(this).removeClass("form-control-error");
        //    $(this).next().removeClass("form-control-error");
        //});

        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                var mensagem = "";
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                    $("#" + this.FieldId).next().addClass("form-control-error");
                    $("#" + this.FieldId).focus();

                    if (this.FieldId === "MENSAGEM") {
                        mensagem = this.Message;
                    }
                });

              
                    if (mensagem === "") {
                        $("#msgErrorText").html("<strong>Aviso!</strong> Favor, preencha os campos corretamente abaixo.");
                    } else {
                        $("#msgErrorText").html("<strong>Aviso!</strong> " + mensagem);
                    }
                    $("#msgError").addClass("show");
               

            }
            else {
                var mensagemRetorno = "Registro criado com sucesso.";
                if ($("#hdnId").val() !== "") {
                    mensagemRetorno = "Registro atualizado com sucesso.";
                }
                swal({
                    title: "Atenção",
                    text: mensagemRetorno,
                    html: ' ',
                    // type: "info",
                    showCancelButton: false,
                    cancelButtonClass: "btn-default",
                    confirmButtonText: "OK",
                    confirmButtonClass: "btn-primary"
                },
                    function (isConfirm) {
                        MODULO.Back();
                    }
                );
            }
        }, true);

        //JSERVICE.Loading(false);
    },
    Load: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            if (!retorno.Error) {

                
                $("#txtNome").val(retorno.Data.Nome);
                $("#txtWhatsapp").val(retorno.Data.Whatsapp);
                $("#txtEmailCompany").val(retorno.Data.Email);
                $("#txtUrl").val(retorno.Data.Url);

                if (retorno.Data.Imagem !== null && retorno.Data.Imagem !== "") {
                    $("#uplImage").attr("src", $("#hdnPathUrlUser").val() + retorno.Data.Imagem);
                    $("#hdnFileFoto").val(retorno.Data.Imagem);
                    $("#hdnPicture").val(retorno.Data.Imagem);
                    $("#txtTexto").html(retorno.Data.Texto);

                  

                    $("#txtChave").val(retorno.Data.Chave);
                    $("#btnImageRemove").show();
                    $("#btnImageAdd").hide();
                }
                else {
                    $("#hdnFileFoto").val("");
                    $("#hdnPicture").val("");
                }
            }

        }, true);

        //JSERVICE.Loading(false);
    },
      
}
