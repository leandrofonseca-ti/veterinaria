$(function () {

    //var css = ".editorTexto";

    //$(css).summernote({
    //    height: 150,   //set editable area's height
    //    codemirror: { // codemirror options
    //        theme: 'monokai'
    //    },
    //    toolbar: [
    //        // [groupName, [list of button]]
    //        ['style', ['bold', 'italic', 'underline']],//, 'clear']],
    //        //['font', ['strikethrough', 'superscript', 'subscript']],
    //        //['fontsize', ['fontsize']],
    //        //['color', ['color']],
    //        ['para', ['ul', 'ol']]//, 'paragraph']],
    //        //['height', ['height']]
    //    ]
    //});

    $("#chkHabilita").click(function () {
        if ($(this).is(":checked")) {
            $("#controlCamposSenha").show();
            $("#txtSenha").val("");
            $("#txtSenhaConfirma").val("");
        } else {
            $("#controlCamposSenha").hide();
        }
    });


    $("#btnSave").click(function () {

        MODULO.Save();
    });
    $("#btnBack").click(function () {

        MODULO.Back();
    });

    //$("#btnVincularUser").click(function () {

    //    MODULO.VincularUsuario();
    //});


    //$("#btnVincularUserClose").click(function () {

    //    MODULO.FecharVincularUsuario();
    //});


    $("#txtCPFCNPJ").keydown(function () {
        try {
            $("#txtCPFCNPJ").unmask();
        } catch (e) { }

        var tamanho = $("#txtCPFCNPJ").val().length;

        if (tamanho < 11) {
            $("#txtCPFCNPJ").mask("999.999.999-99");
        } else {
            $("#txtCPFCNPJ").mask("99.999.999/9999-99");
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


    if ($("#hdnId").val() === "") {
        $("#controlEdicaoSenha").hide();
        $("#controlCamposSenha").show();
    }
    else {
        $("#controlEdicaoSenha").show();
        $("#controlCamposSenha").hide();
    }
    $('.telefone').mask('00 000000000000');
    //$('.data').mask('00/00/0000');

    window.setTimeout(function () {
        if ($("#hdnId").val() !== "") {
            MODULO.Load();
        }
        //    MODULO.LoadConfig();
    }, 100);
});


MODULO = {
    PageIndex: 1,
    PageSize: 30,
    AreaNome: "Cadastro",
    ControllerNome: "Laudador",
    Back: function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    },
    Load: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {


            if (!retorno.Error) {
                $("#txtNome").val(retorno.Data.Nome);
                $("#txtEmail").val(retorno.Data.Email);
                $("#txtSobrenome").val(retorno.Data.Sobrenome);
                $("#txtCPFCNPJ").val(retorno.Data.CPFCNPJFmt);
                $("#txtTelefone").val(retorno.Data.Telefone);
                $("#txtTelefone2").val(retorno.Data.Telefone2);
            }

        }, true);

        //JSERVICE.Loading(false);
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
                var habilitaVinculo = false;
                var tipoVinculo = false;
                var codigoExistente = false;
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                    $("#" + this.FieldId).next().addClass("form-control-error");
                    $("#" + this.FieldId).focus();


                    if (this.FieldId === "MENSAGEM") {
                        mensagem = this.Message;
                    }
                    if (this.FieldId === "CARREGAR_USUARIO") {
                        habilitaVinculo = true;
                        tipoVinculo = this.Message;
                        codigoExistente = this.Auxiliar;
                    }
                });

                if (habilitaVinculo) {
                    MODULO.AbrirVincularUsuario(tipoVinculo, codigoExistente);
                }
                else {
                    if (mensagem === "") {
                        $("#msgErrorText").html("<strong>Aviso!</strong> Favor, preencha os campos corretamente abaixo.");
                    } else {
                        $("#msgErrorText").html("<strong>Aviso!</strong> " + mensagem);
                    }
                    $("#msgError").addClass("show");
                }
                //$(document).scrollTop($("#actionName").offset().top);

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

    VincularUsuario: function (cod) {

       // alert($("#hdnCodigoExistente").val());


        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": cod });
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/VincularExistente", json, function (retorno) {

            if (retorno.Data) {
                $("#alertExistente").hide();

                var mensagemRetorno = "Usuário vinculado com sucesso.";
                swal({
                    title: "Atenção",
                    text: mensagemRetorno,
                    html: ' ',
                    showCancelButton: false,
                    cancelButtonClass: "btn-default",
                    confirmButtonText: "OK",
                    confirmButtonClass: "btn-primary"
                },
                    function (isConfirm) {
                        MODULO.Back();
                    }
                );

            } else {
                swal({
                    title: "Atenção",
                    text: "Erro ao vincular usuário com sua clínica :(",
                    type: "error",
                    confirmButtonClass: "btn-danger"
                });
            }
        }, true);
    },
    //FecharVincularUsuario: function () {
    //   // $("#alertExistente").removeClass("show");
    //},
    AbrirVincularUsuario: function (tipoVinculo, codigo) {
        console.log("AbrirVincularUsuario: " + codigo);
        $("#hdnCodigoExistente").val(codigo);
       // $("#alertExistente").addClass("show");
        var campo = "";
        if (tipoVinculo === "CPFCNPJ_CADASTRADO") {
            campo = "CPF/CNPJ \"" + $("#txtCPFCNPJ").val() + "\"";
        }
        if (tipoVinculo === "EMAIL_CADASTRADO") {
            campo = "E-mail \"" + $("#txtEmail").val() + "\"";
        }

        swal({
            title: "Atenção",
            text: "Usuário com o " + campo + " já existente. Deseja vinculá-lo?",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    MODULO.VincularUsuario(codigo);
                } else {
                    swal.close();
                }
            });
    }
};