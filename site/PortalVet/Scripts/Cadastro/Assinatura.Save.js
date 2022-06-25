const variaveis = [];

$(function () {

    $("#btnSave").click(function () {
        MODULO.Save();
    });

    $("#btnBack").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });

    window.setTimeout(function () {
      
        MODULO.Load();

    }, 100);
   
});

var MODULO = {
    PageIndex: 1,
    PageSize: 30,
    AreaNome: "Cadastro",
    ControllerNome: "Assinatura",

    Save: function () {

        $("input.form-control, select.select2").each(function () {
            $(this).removeClass("form-control-error");
            $(this).next().removeClass("form-control-error");
        });

        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();
        // json.push({ "name": "hdnPicture", "value": $("#hdnPicture").val() }); 
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                    $("#" + this.FieldId).next().addClass("form-control-error");
                });

                //if (retorno.Message === "") {
                //    $("#msgErrorText").html("<strong>Aviso!</strong> Favor, preencha os campos corretamente abaixo.");
                //} else {
                //    $("#msgErrorText").html("<strong>Aviso!</strong> " + retorno.Message);
                //}
                //$("#msgError").addClass("show");

                $("#" + retorno.Criticas[0].FieldId).focus();

                JSERVICE.Mensagem("Favor, preencha os campos corretamente abaixo.", "Aviso!", "error");
               
                
            }
            else {
                JSERVICE.Mensagem("Dados salvos com sucesso.", "Aviso!", "success");

                window.setTimeout(function () {

                    window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";

                }, 3000);
               
            }
        }, true);
    },
    
 
    Load: function () {

        var id = $("#hdnId").val();

        if (id !== "") {
            var json = $("form").serializeArray();
            json.push({ "id": id });

            JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

                if (!retorno.Error) {

                    $("#hdnArquivo1").val("");
                    if (retorno.Data.AssinaturaImagem !== null && retorno.Data.AssinaturaImagem !== "") {
                        $("#hdnArquivo1").val(retorno.Data.AssinaturaImagem);
                        var itemArquivo1 = { "Path": "/", "Name": retorno.Data.AssinaturaImagem };
                        $("#listImages1").html("");
                        $("#hdnTotalFotos1").val(1);
                        var htmlImages1 = FILECONTROL1.CreateItem(itemArquivo1);
                        $("#listImages1").html(htmlImages1);
                    }


                    
                    $("#txtNomeAssinatura").val(retorno.Data.AssinaturaNome);
                    $("#txtCRMAssinatura").val(retorno.Data.AssinaturaCRM);
                    $("#txtProfissaoAssinatura").val(retorno.Data.AssinaturaProfissao);
                    $("#txtWhatsappAssinatura").val(retorno.Data.AssinaturaWhatsapp);

                    $("#txtNomeModelo").val(retorno.Data.Nome);
                }


            }, true);
        }
    },

}
    
 

var FILECONTROL1 = {
    ID: "1",
    AreaNome: "Cadastro",
    ControllerNome: "Assinatura",
    MaxImages: 1,
    IsMultiFile: false,
    BuildSingleFile: function (anexo, folder) {
        var CURRENT_CONTROL = this;
        $("#hdnArquivo" + CURRENT_CONTROL.ID).val(anexo);
        var itemArquivo1 = { "Path": JSERVICE.rootSohtec + "/upload/" + folder + "/", "Name": anexo };
        $("#listImages" + CURRENT_CONTROL.ID).html("");
        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(1);
        var htmlImages = CURRENT_CONTROL.CreateItem(itemArquivo1);
        $("#listImages" + CURRENT_CONTROL.ID).html(htmlImages);
    },
    BuildMultiFile: function (anexos, folder) {
        ///MODULO.BuildMultiFile(retorno.Data.ArquivosAnexos);
        var CURRENT_CONTROL = this;
        var result = "";
        var arrayItems = new Array();
        $(anexos).each(function (k, r) {

            if (result === "") {
                result = r.Arquivo;
            }
            else {
                result = result + "|" + r.Arquivo;
            }

            arrayItems.push({ "Path": JSERVICE.rootSohtec + "/upload/" + folder + "/", "Name": r.Arquivo });
        });

        $("#hdnArquivo" + CURRENT_CONTROL.ID).val(result);

        $("#listImages" + CURRENT_CONTROL.ID).html("");
        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(CURRENT_CONTROL.MaxImages);
        var htmlImagesMulti = CURRENT_CONTROL.CreateList(arrayItems);
        $("#listImages" + CURRENT_CONTROL.ID).html(htmlImagesMulti);
    },
    OpenModalImage: function (url) {
        var email = $("#hdnClienteEmail").val();
        var empresaid = $("#hdnCurrentEmpresaId").val();
        var CURRENT_CONTROL = this;
        var ismultiple = CURRENT_CONTROL.IsMultiFile ? "1" : "0";
        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val($("#listImages" + CURRENT_CONTROL.ID + " li").length);
        var total = parseInt($("#hdnTotalFotos" + CURRENT_CONTROL.ID).val());
        if (total < CURRENT_CONTROL.MaxImages) {
            $("#frmUploads").attr("src", url + "?tid=" + CURRENT_CONTROL.ID + "&email=" + email + "&empresaid=" + empresaid + "&ismultiple=" + ismultiple + "&t=" + Date.now());
            $("#myUpload").modal('show');
        }
        else {
            if (CURRENT_CONTROL.MaxImages === 1) {
                alert("Máximo 1 arquivo");
            } else {
                alert("Máximo " + CURRENT_CONTROL.MaxImages + " arquivos");
            }
        }
    },
    CloseModalImage: function () {
        $("#myUpload").modal("hide");
    },

    UpdateImage: function (path, fileName, base64, pathServer) {
        var CURRENT_CONTROL = this;
        var ismultiple = CURRENT_CONTROL.IsMultiFile ? "1" : "0";
        var index = $("#listImages" + CURRENT_CONTROL.ID + " li").length;

        $.ajax({
            //Tipo do envio das informações GET ou POST
            //async: false,
            type: "POST",
            url: JSERVICE.rootApplication + this.AreaNome + "/" + this.ControllerNome + "/JsonAddFile",//$("#hdnPathControl").val() + "/JsonAddFile",
            data: JSON.stringify({
                "path": path,
                "name": fileName,
                "index": index,
                "base64": base64,
                "key": this.ID,
                "ismultiple": ismultiple,
                "pathServer": pathServer
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //Tratamento dos dados de retorno.

                //alert(JSON.stringify(data.d[0]));
                var obj = data;
                if (obj.status === "OK") {
                    $("#listImages" + CURRENT_CONTROL.ID).html("");
                    if (obj.listImages !== null)
                        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(obj.listImages.length);
                    else
                        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(0);



                    //if (CURRENT_CONTROL.IsMultiFile) {
                    var result = "";
                    //var arrayItems = new Array();
                    $(obj.listImages).each(function (k, r) {

                        if (result === "") {
                            result = r.Name;
                        }
                        else {
                            result = result + "|" + r.Name;
                        }

                    });
                    $("#hdnArquivo" + CURRENT_CONTROL.ID).val(result);
                    var htmlImageMulti = CURRENT_CONTROL.CreateList(obj.listImages);
                    $("#listImages" + CURRENT_CONTROL.ID).html(htmlImageMulti);

                    //}
                    //else {
                    //    $("#hdnArquivo" + CURRENT_CONTROL.ID).val(fileName);
                    //    var htmlImageSigle = "";
                    //    if (obj.listImages.length > 0)
                    //        htmlImageSigle = CURRENT_CONTROL.CreateItem(obj.listImages[0]);
                    //    $("#listImages" + CURRENT_CONTROL.ID).html(htmlImageSigle);
                    //}

                }

            },
            //Se acontecer algum erro é executada essa função
            error: function (erro) {
                alert("Erro:" + JSON.stringify(erro));
            }
        });
    },

    CreateItem: function (arrObj) {
        var CURRENT_CONTROL = this;
        var htmlLine = "";
        var ctFoto = 1;
        htmlLine = htmlLine + "<li class='list-group-item list-group-item-warning' style=''>";
        htmlLine = htmlLine + "<div class='controls'>&nbsp;";
        htmlLine = htmlLine + "<a onclick='FILECONTROL" + CURRENT_CONTROL.ID + ".OpenViewerGo(\"" + arrObj.Name + "\")' href='javascript:;'  class='btn btn-inline btn-secondary'><span class='glyphicon glyphicon-save-file' aria-hidden='true' ></span>Visualizar</a>";
        htmlLine = htmlLine + "&nbsp;<button type='button' class='btn btn-inline btn-danger btnFileRemove' onclick='FILECONTROL" + CURRENT_CONTROL.ID + ".RemoveFile(" + 0 + ")'><span class='glyphicon glyphicon-floppy-remove' aria-hidden='true'></span> Excluir</button>";
        htmlLine = htmlLine + "</div>";
        htmlLine = htmlLine + "</li>";
        return htmlLine;
    },


    CreateList: function (arrObj) {
        var CURRENT_CONTROL = this;
        var htmlLine = "";
        var ctFoto = 0;
        if (arrObj !== null) {
            for (var i = 0; i < arrObj.length; i++) {
                ctFoto = i + 1;
                htmlLine = htmlLine + "<li class='list-group-item list-group-item-warning' style=''>";
                htmlLine = htmlLine + "<div class='controls'>&nbsp;";
                //htmlLine = htmlLine + "<button type='button' class='btn btn-icon btn-info' onclick='this.DownloadFile(\"" + arrObj[i].Path + arrObj[i].Name + "\")'><small>Visualizar</small></button>";
                //htmlLine = htmlLine + "<button type='button' onclick='this.DownloadFile(\"" + arrObj[i].Path + arrObj[i].Name + "\")'  class='btn btn-inline btn-secondary-outline'><span class='glyphicon glyphicon-save-file' aria-hidden='true' ></span>Visualizar</button>";
                htmlLine = htmlLine + "<a onclick='FILECONTROL" + CURRENT_CONTROL.ID + ".OpenViewerGo(\"" + arrObj[i].Name + "\")' href='javascript:;'  class='btn btn-inline btn-secondary'><span class='glyphicon glyphicon-save-file' aria-hidden='true' ></span>Visualizar</a>";
                //htmlLine = htmlLine + "&nbsp;<button type='button' class='btn btn-icon btn-danger' onclick='this.RemoveFile(" + i + ")'><small>Remover</small></button>";
                htmlLine = htmlLine + "&nbsp;<button type='button' class='btn btn-inline btn-danger btnFileRemove' onclick='FILECONTROL" + this.ID + ".RemoveFile(" + i + ")'><span class='glyphicon glyphicon-floppy-remove' aria-hidden='true'></span> Excluir</button>";

                htmlLine = htmlLine + "</div>";
                htmlLine = htmlLine + "</li>";
            }
        }
        return htmlLine;
    },

    OpenViewerGo: function (filename) {
        var url = $("#UrlDominio").val() +"upload/assinaturas/"+ filename;
        window.open(url, "_blank");
    },

    RemoveFile: function (index) {

        var CURRENT_CONTROL = this;
        var ismultiple = CURRENT_CONTROL.IsMultiFile ? "1" : "0";
        $.ajax({
            //Tipo do envio das informações GET ou POST
            //async: false,
            type: "POST",
            //url: '@Url.Action("JsonRemoveFile", "CadastroCredpago")',
            url: JSERVICE.rootApplication + CURRENT_CONTROL.AreaNome + "/" + CURRENT_CONTROL.ControllerNome + "/JsonRemoveFile",
            //data: "index=" + index,
            data: JSON.stringify({
                "index": index,
                "key": CURRENT_CONTROL.ID,
                "ismultiple": ismultiple
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //Tratamento dos dados de retorno.
                //alert(JSON.stringify(data));
                var obj = data;
                if (obj.status === "OK") {
                    $("#hdnArquivo" + CURRENT_CONTROL.ID).val("");
                    $("#listImages" + CURRENT_CONTROL.ID).html("");

                    if (obj.listImages !== null)
                        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(obj.listImages.length);
                    else
                        $("#hdnTotalFotos" + CURRENT_CONTROL.ID).val(0);

                    //if (CURRENT_CONTROL.IsMultiFile) {
                    var result = "";
                    //var arrayItems = new Array();
                    $(obj.listImages).each(function (k, r) {

                        if (result === "") {
                            result = r.Name;
                        }
                        else {
                            result = result + "|" + r.Name;
                        }

                    });
                    $("#hdnArquivo" + CURRENT_CONTROL.ID).val(result);
                    var htmlImageMulti = CURRENT_CONTROL.CreateList(obj.listImages);
                    $("#listImages" + CURRENT_CONTROL.ID).html(htmlImageMulti);

                    //}
                    //else {
                    //    $("#hdnArquivo" + CURRENT_CONTROL.ID).val(fileName);
                    //    var htmlImageSigle = "";
                    //    if (obj.listImages.length > 0)
                    //        htmlImageSigle = CURRENT_CONTROL.CreateItem(obj.listImages[0]);
                    //    $("#listImages" + CURRENT_CONTROL.ID).html(htmlImageSigle);
                    //}



                }

            },
            //Se acontecer algum erro é executada essa função
            error: function (erro) {
                alert("Erro:" + JSON.stringify(erro));
            }
        });
    },

    DownloadFile: function (filename) {
        var text = "Download file";
        var element = document.createElement('a');
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
        element.setAttribute('download', filename);

        element.style.display = 'none';
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);


    }
};