


$(function () {

    var css = ".editorTexto";

    $(css).summernote({
        height: 150,   //set editable area's height
        codemirror: { // codemirror options
            theme: 'monokai'
        },
        toolbar: [
            // [groupName, [list of button]]
            ['style', ['bold', 'italic', 'underline']],//, 'clear']],
            //['font', ['strikethrough', 'superscript', 'subscript']],
            //['fontsize', ['fontsize']],
            //['color', ['color']],
            ['para', ['ul', 'ol']]//, 'paragraph']],
            //['height', ['height']]
        ]
    });

    $("#btnOpenModalLaudador").click(function () {


        $("#modalCadastroLaudador").modal("show");
    });

    $("#btnOpenModalCliente").click(function () {
        $("#modalCadastroCliente").modal("show");
    });



    $("#txtModalCliCPFCNPJ").keydown(function () {
        try {
            $("#txtModalCliCPFCNPJ").unmask();
        } catch (e) { }

        var tamanho = $("#txtModalCliCPFCNPJ").val().length;

        if (tamanho < 11) {
            $("#txtModalCliCPFCNPJ").mask("999.999.999-99");
        } else {
            $("#txtModalCliCPFCNPJ").mask("99.999.999/9999-99");
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

    $("#btnSalvarCliente").click(function () {

        MODULO.SalvarCliente();

    });

    $("#btnSalvarLaudador").click(function () {

        MODULO.SalvarLaudador();
    });

    $("#txtDataExame").datepicker({

        dateFormat: 'dd/mm/yy',
        timeFormat: 'hh:mm',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Proximo',
        prevText: 'Anterior'
    });


    $("#btnBackCriarExame").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });
    $("#btnCriarExame").click(function () {
        MODULO.Save();
    });
    $('.timer').mask('00:00');
    $('.data').mask('00/00/0000');
    window.setTimeout(function () {
        if ($("#hdnId").val() !== "") {
            MODULO.Load();
            MODULO.LoadHistorico();
        }
    }, 100);

});



var MODULO = {
    PageIndex: 1,
    PageSize: 10,
    AreaNome: "Cadastro",
    ControllerNome: "ExameClinica",
    SalvarLaudador: function () {
        var json = $("form").serializeArray();
        //json.push({ "name": "empresaid", "value": $("#hdnEmpresaId").val() });
        //json.push({ "name": "txtModalNome", "value": $("#txtModalNome").val() });
        //json.push({ "name": "txtModalEmail", "value": $("#txtModalEmail").val() });
        //json.push({ "name": "txtModalSenha", "value": $("#txtModalSenha").val() });
        //json.push({ "name": "txtModalSenhaCC", "value": $("#txtModalSenhaCC").val() });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveLaudador", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                var message = "";
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("error-field");
                    $("#" + this.FieldId).next().addClass("error-field");
                    if (this.FieldId === "MENSAGEM") {
                        message = this.Message;
                    }

                });

                if (message !== "")
                    JSERVICE.Mensagem(message, "", "error");
                else
                    JSERVICE.Mensagem("Preencha corretamente os campos abaixo.", "", "error");

            }
            else {




                $("#modalCadastroLaudador").modal("hide");

                MODULO.ReloadComboLaudador();


                window.setTimeout(function () {
                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: 'Laudador cadastrado com sucesso!'
                    });
                }, 500);
                //window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + retorno.Data.Data.Id;
            }
        }, true);
    },
    ReloadComboLaudador: function () {
        var json = $("form").serializeArray();
        json.push({ "name": "empresaid", "value": $("#hdnEmpresaId").val() });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/LoadLaudadores", json, function (retorno) {

            var html = "<option value=''>:: Selecione ::</option>";
            $(retorno.Data).each(function () {
                html = html + "<option value='" + this.Value + "'>" + this.Text + "</option>";
            });

            $("#drpLaudador").html(html);
            $("#drpLaudador").select2();

        });
    },
    ReloadComboCliente: function () {

        var json = $("form").serializeArray();
        json.push({ "name": "empresaid", "value": $("#hdnEmpresaId").val() });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/LoadClientes", json, function (retorno) {

            var html = "<option value=''>:: Selecione ::</option>";
            $(retorno.Data).each(function () {
                html = html + "<option value='" + this.Value + "'>" + this.Text + "</option>";
            });

            $("#drpCliente").html(html);
            $("#drpCliente").select2();

        });
    },
    SalvarCliente: function () {
        var json = $("form").serializeArray();
        //json.push({ "name": "empresaid", "value": $("#hdnEmpresaId").val() });
        //json.push({ "name": "txtModalCliNome", "value": $("#txtModalCliNome").val() });
        //json.push({ "name": "txtModalCliSobreNome", "value": $("#txtModalCliSobreNome").val() });
        //json.push({ "name": "txtModalCliCPFCNPJ", "value": $("#txtModalCliCPFCNPJ").val() });
        //json.push({ "name": "txtModalCliEmail", "value": $("#txtModalCliEmail").val() });
        //json.push({ "name": "txtModalCliSenha", "value": $("#txtModalCliSenha").val() });
        //json.push({ "name": "txtModalCliSenhaCC", "value": $("#txtModalCliSenhaCC").val() });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveCliente", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                var message = "";
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("error-field");
                    $("#" + this.FieldId).next().addClass("error-field");
                    if (this.FieldId === "MESSAGE") {
                        message = this.Message;
                    }
                });

                if (message !== "")
                    JSERVICE.Mensagem(message, "", "error");
                else
                    JSERVICE.Mensagem("Preencha corretamente os campos abaixo.", "", "error");
            }
            else {

                $("#modalCadastroCliente").modal("hide");

                MODULO.ReloadComboCliente();

                window.setTimeout(function () {
                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: 'Cliente cadastrado com sucesso!'
                    });
                }, 500);
                //window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + retorno.Data.Data.Id;
            }
        }, true);
    },

    LoadHistorico: function () {

        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/LoadHistorico", json, function (retorno) {
            var htmlHist = "";
            if (retorno.Data !== null && retorno.Data !== undefined) {

                $(retorno.Data).each(function (j, k) {
                    htmlHist += "<div class='widget-activity-item'>";
                    htmlHist += "<div class='user-card-row'>";
                    htmlHist += "<div class='tbl-row'>";
                    htmlHist += "<div class='tbl-cell tbl-cell-photo'>";
                    htmlHist += "<a href='#'><img src='https://imob.sohtec.com.br/content/img/avatar-2-64.png' alt=''></a>";
                    htmlHist += "</div>";
                    htmlHist += "<div class='tbl-cell'>";
                    htmlHist += "<p>";
                    htmlHist += "<a href='#' class='semibold'>" + k.UsuarioNome + " <small>(" + k.UsuarioEmail + ")</small></a>";
                    htmlHist += "&nbsp;&nbsp;<small>" + k.Descricao + "</small>";
                    htmlHist += "</p>";
                    htmlHist += "<p><small>Situação: <strong>" + k.SituacaoNome + "</strong></small></p>";
                    htmlHist += "<p>" + k.DataCadastroFmt + "</p>";
                    htmlHist += "</div>";
                    htmlHist += "</div>";
                    htmlHist += "</div>";
                    htmlHist += "</div>";
                });

            }
            $("#areaHistorico").html(htmlHist);
        }, true);

    },
    Load: function () {

        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            if (!retorno.Error) {

                if (retorno.Data.SituacaoId !== 2) {
                    $(".clsAreaForm").addClass("disabledArea");
                    $("#btnCriarExame").fadeOut();
                    $("#collapseOne").addClass("show");
                }
                $("#areaCodigo").show();

                $("#lblCodigo").html(retorno.Data.Id);
                $("#lblSituacao").html(retorno.Data.SituacaoNome);
                $("#txtDataExame").val(retorno.Data.DataExameFmt);
                $("#txtDataExameTime").val(retorno.Data.DataExameHH + ":" + retorno.Data.DataExameMM);



                $("#txtVeterinario").val(retorno.Data.Veterinario);
                $("#txtProprietario").val(retorno.Data.Proprietario);
                $("#txtPaciente").val(retorno.Data.Paciente);
                $("#txtEspecie").val(retorno.Data.Especie);
                $("#txtIdade").val(retorno.Data.Idade);
                $("#editorTexto").summernote('code', retorno.Data.Descricao);
                if (retorno.Data.RacaId > 0) {
                    $("#drpRaca").val(retorno.Data.RacaId);
                    $("#drpRaca").select2();
                }
                if (retorno.Data.LaudadorId > 0) {
                    $("#drpLaudador").val(retorno.Data.LaudadorId);
                    $("#drpLaudador").select2();
                }
                if (retorno.Data.ClienteId > 0) {
                    $("#drpCliente").val(retorno.Data.ClienteId);
                    $("#drpCliente").select2();
                }

                //$("#drpSituacao").val(retorno.Data.SituacaoId);
                //$("#drpSituacao").select2();
                //$("#drpRaca").val(
                //  entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());
                // entidade.ClienteId = Int32.Parse(dr["ClienteId"].ToString());
                // entidade.LaudadorId = Int32.Parse(dr["LaudadorId"].ToString());
                // entidade.Descricao = dr["Descricao"].ToString();
                //  entidade.RacaId = Int32.Parse(dr["RacaId"].ToString());


                //if (retorno.Data.Ativo) {
                //  $("#chkAtivo").attr('checked', 'checked');
                //}
            }

        }, true);

    },
    Save: function () {

        $("#btnCriarExame").fadeOut();
        //alert($("#hdnId").val());

        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                var message = "";
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("error-field");
                    $("#" + this.FieldId).next().addClass("error-field");
                    if (this.FieldId === "MESSAGE") {
                        message = this.Message;
                    }
                });

                if (message !== "")
                    JSERVICE.Mensagem(message, "", "error");
                else
                    JSERVICE.Mensagem("Preencha corretamente os campos abaixo.", "", "error");

            }
            else {
                //JSERVICE.Mensagem("Cadastro realizado com sucesso.", "", "success");
                //alert(JSON.stringify(retorno.Data));
                if ($("#hdnId").val() === "") {

                    JSERVICE.Loading(true);
                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: 'Registro cadastrado com sucesso!'
                    });

                    $(".lobibox-notify-wrapper").css("z-index", "999999");

                    window.setTimeout(function () {
                        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + retorno.Data.Data.Id;
                    }, 2000);
                    //$("#hdnId").val(retorno.Data.Id);
                    //$("#lblCodigo").html(retorno.Data.Id);
                    //$("#areaCodigo").show()
                }
                else {
                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: 'Registro atualizado com sucesso!'
                    });

                    window.setTimeout(function () {
                        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
                    }, 1000);
                }

                MODULO.LoadHistorico();

            }
            $("#btnCriarExame").fadeIn();

        }, true);

    }
};