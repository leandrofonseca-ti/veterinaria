


$(function () {

    var css = ".editorTexto";

    $(css).summernote({
        height: 350,   //set editable area's height
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
    $("#btnSalvarDuvidaLaudador").click(function () {
        MODULO.SalvarDuvida("LAUDADOR");
    });
    $("#btnSalvarDuvidaClinica").click(function () {
        MODULO.SalvarDuvida("CLINICA");
    });
    $("#drpEspecie").change(function () {
        $(this).val() === "1"
        $("#areaEspecieOutros").hide();

        if ($(this).val() === "1" ||
            $(this).val() === "2") {
            MODULO.CarregarRaca($(this).val());
            $("#areaEspecieOutros").hide();

        } else {
            $("#areaRaca").hide();
            $("#areaEspecieOutros").show();
        }
    });
    $("#drpModelo").change(function () {

        if ($(this).val() !== "") {
            MODULO.CarregarModelo($(this).val());
        }
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

        window.location = JSERVICE.rootApplication + "Dashboard/Index";
        /*
                if ($("#hdnPerfilCodigo").val() === "4" ||
                    $("#hdnPerfilCodigo").val() === "5") {
                    window.location = JSERVICE.rootApplication + "Dashboard/Index";  
                }
                else {
                    window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
                }*/
    });


    $("#btnRealizarAnalise").click(function () {
        MODULO.RealizarLaudo();
    });


    $("#btnLiberarAnalise").click(function () {
        MODULO.LiberarLaudo();
    });




    $("#btnRespostaAnaliseLaudador").click(function () {
        MODULO.Save(2);
    });

    $("#btnRespostaConcluirGerente").click(function () {
        MODULO.Save(5);
    });

    $("#btnCriarExame").click(function () {
        MODULO.Save(1);
    });

    $("#btnCriarExameConcluir").click(function () {
        MODULO.Save(3);
    });

    $("#btnCriarExameLaudador").click(function () {
        MODULO.Save(2);
    });
    $('.timer').mask('00:00');
    $('.data').mask('00/00/0000');
    window.setTimeout(function () {
        if ($("#hdnId").val() !== "") {

            MODULO.Load();
            MODULO.LoadHistorico();
            $("#areaArquivos").show();
        } else {
            /*
       Administrador = 1,
       Gerente = 2,
       Clinica = 3,
       Cliente = 4,
       Laudador = 5
    */
            if ($("#hdnPerfilCodigo").val() === "3" ||
                $("#hdnPerfilCodigo").val() === "4" ||
                $("#hdnPerfilCodigo").val() === "5") {

                $(".clsInput").prop("disabled", true); //Disable
                $(".select2").prop("disabled", true);
                $("#txtHistorico").prop("disabled", true);
                $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();
                $("#areaExameFinal").hide();
                $("#areaHistoricoTab").hide();
                $(".clsImages").hide();
                $("#btnCriarExame").hide();
                $("#collapseOneImg").addClass("show");
            }
        }
    }, 100);

});



var MODULO = {
    PageIndex: 1,
    PageSize: 50,
    AreaNome: "Cadastro",
    ControllerNome: "Exame",
    CarregarModelo: function (codigo) {
        var json = [];
        json.push({ "name": "codigo", "value": codigo });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/CarregarModelo", json, function (retorno) {

            if (retorno.Data !== null) {
                //$("#editorRodape").summernote('code', retorno.Data.Rodape);
                $("#editorTexto").summernote('code', retorno.Data.Descricao);
                $("#drpModelo").val("");
                $("#drpModelo").select2();
            }
        }, true);
    },

    CarregarRaca: function (codigo, status) {


        var json = [];
        json.push({ "name": "codigo", "value": codigo });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/CarregarRaca", json, function (retorno) {
            var htmlItem = "<option value=''>:: Selecione ::</option>";
            if (retorno.Data !== null) {

                $(retorno.Data).each(function (w, e) {
                    if (status === e.Value)
                        htmlItem = htmlItem + "<option value='" + e.Value + "' selected>" + e.Text + "</option>";
                    else
                        htmlItem = htmlItem + "<option value='" + e.Value + "'>" + e.Text + "</option>";
                });

                $("#drpRaca").html(htmlItem);
                $("#drpRaca").select2();
            }
        }, true);

        $("#areaRaca").show();
    },
    SalvarDuvida: function (tipo) {

        var valido = false;
        if ($("#editorQuestao").val() === "") {
            $("#editorQuestao").addClass("error-field");
        } else {
            valido = true;
            $("#editorQuestao").removeClass("error-field");
        }

        if (valido) {

            var json = $("form").serializeArray();
            if (tipo === "CLINICA") {
                json.push({ "name": "mensagem", "value": $("#editorQuestaoClinica").val() });
            }
            if (tipo === "LAUDADOR") {
                json.push({ "name": "mensagem", "value": $("#editorQuestaoLaudador").val() });
            }
            json.push({ "name": "tipo", "value": tipo });

            JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SaveDuvida", json, function (retorno) {

                if (retorno.Criticas.length > 0) {
                    //var message = "";
                    //$(retorno.Criticas).each(function () {
                    //    $("#" + this.FieldId).addClass("error-field");
                    //    $("#" + this.FieldId).next().addClass("error-field");
                    //    if (this.FieldId === "MENSAGEM") {
                    //        message = this.Message;
                    //    }

                    //});

                    //if (message !== "")
                    //    JSERVICE.Mensagem(message, "", "error");
                    //else
                    JSERVICE.Mensagem("Preencha corretamente os campos.", "", "error");

                }
                else {

                    
                    if (tipo === "CLINICA") {
                        MODULO.LoadDuvidasClinica(retorno.Data);
                        $("#editorQuestaoClinica").val("");
                    }

                    if (tipo === "LAUDADOR") {
                        MODULO.LoadDuvidasLaudador(retorno.Data);
                        $("#editorQuestaoLaudador").val("");
                    }

                    window.setTimeout(function () {
                        Lobibox.notify('success', {
                            size: 'mini',
                            msg: 'Mensagem cadastrada com sucesso!'
                        });
                    }, 500);
                    //window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + retorno.Data.Data.Id;
                }
            }, true);
        }

    },

    LoadDuvidasClinica: function (data) {


        var htmlHist = "";
        if (data !== null && data !== undefined) {

            $(data).each(function (j, k) {
                htmlHist += "<div class='widget-activity-item'>";
                htmlHist += "<div class='user-card-row'>";
                htmlHist += "<div class='tbl-row'>";
                htmlHist += "<div class='tbl-cell tbl-cell-photo'>";
                htmlHist += "<a href='#'><img src='https://imob.sohtec.com.br/content/img/avatar-2-64.png' alt=''></a>";
                htmlHist += "</div>";
                htmlHist += "<div class='tbl-cell'>";
                htmlHist += "<p>";
                htmlHist += "<a href='#' class='semibold'>" + k.UsuarioNome + " <small>(" + k.UsuarioEmail + ")</small></a>";
                htmlHist += "&nbsp;&nbsp;<small>" + k.Mensagem + "</small>";
                htmlHist += "</p>";
                htmlHist += "<p>" + k.DataCadastroFmt + "</p>";
                htmlHist += "</div>";
                htmlHist += "</div>";
                htmlHist += "</div>";
                htmlHist += "</div>";
            });
        }
        $("#areaHistoricoDuvidasClinica").html(htmlHist);
    },


    LoadDuvidasLaudador: function (data) {


        var htmlHist = "";
        if (data !== null && data !== undefined) {

            $(data).each(function (j, k) {
                htmlHist += "<div class='widget-activity-item'>";
                htmlHist += "<div class='user-card-row'>";
                htmlHist += "<div class='tbl-row'>";
                htmlHist += "<div class='tbl-cell tbl-cell-photo'>";
                htmlHist += "<a href='#'><img src='https://imob.sohtec.com.br/content/img/avatar-2-64.png' alt=''></a>";
                htmlHist += "</div>";
                htmlHist += "<div class='tbl-cell'>";
                htmlHist += "<p>";
                htmlHist += "<a href='#' class='semibold'>" + k.UsuarioNome + " <small>(" + k.UsuarioEmail + ")</small></a>";
                htmlHist += "&nbsp;&nbsp;<small>" + k.Mensagem + "</small>";
                htmlHist += "</p>";
                //htmlHist += "<p><small>Situação: <strong>" + k.SituacaoNome + "</strong></small></p>";
                htmlHist += "<p>" + k.DataCadastroFmt + "</p>";
                htmlHist += "</div>";
                htmlHist += "</div>";
                htmlHist += "</div>";
                htmlHist += "</div>";
            });
        }
        $("#areaHistoricoDuvidasLaudador").html(htmlHist);
    },
    SalvarLaudador: function () {
        var json = $("form").serializeArray();
        json.push({ "name": "hdnCurrentEmpresaId", "value": $("#hdnCurrentEmpresaId").val() });
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
        json.push({ "name": "hdnEmpresaId", "value": $("#hdnCurrentEmpresaId").val() });

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
        json.push({ "name": "hdnEmpresaId", "value": $("#hdnCurrentEmpresaId").val() });

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
        json.push({ "name": "hdnCurrentEmpresaId", "value": $("#hdnCurrentEmpresaId").val() });
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
    CopiarLink: function () {
        var input = $("#txtUrlCopiar");
        input.focus();
        input.select();
        document.execCommand('Copy');
        JSERVICE.Mensagem("Link copiado com sucesso.", "", "success");
    },
    Load: function () {

        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

            if (!retorno.Error) {


                //$("#hdnPerfilLaudadorId").val()
                //$("#hdnPerfilGerenteId").val()
                //$("#hdnPerfilClinicaId").val()





                $("#areaCodigo").show();

                $("#hdnClinicaId").val(retorno.Data.ClinicaId);
                $("#lblCodigo").html(retorno.Data.Id);

                $("#hdnSituacaoId").val(retorno.Data.SituacaoId);
                $("#lblStatus").html(retorno.Data.SituacaoNome);
                $("#txtDataExame").val(retorno.Data.DataExameFmt);
                $("#txtDataExameTime").val(retorno.Data.DataExameHH + ":" + retorno.Data.DataExameMM);

                $("#txtUrlCopiar").val(retorno.Data.LinkExame);

                $("#txtFormaPgto").val(retorno.Data.FormaPagamento);
                $("#txtValor").val(retorno.Data.Valor);
                $("#txtHistorico").html(retorno.Data.Historico);
                $("#txtVeterinario").val(retorno.Data.Veterinario);
                $("#txtProprietario").val(retorno.Data.Proprietario);
                $("#txtPaciente").val(retorno.Data.Paciente);
                $("#txtEspecie").val(retorno.Data.Especie);
                $("#txtIdade").val(retorno.Data.Idade);
                $("#editorTexto").summernote('code', retorno.Data.Descricao);
                //$("#editorRodape").summernote('code', retorno.Data.Rodape);

                $("#txtProprietarioNome").val(retorno.Data.Proprietario);
                $("#txtProprietarioEmail").val(retorno.Data.ProprietarioEmail);
                $("#txtProprietarioTelefone").val(retorno.Data.ProprietarioTelefone);
                $("#txtEspecieSelecao").val(retorno.Data.EspecieSelecao);
                $("#txtRacaSelecao").val(retorno.Data.RacaSelecao);


                if (retorno.Data.AssinaturaId > 0) {
                    $("#drpRodape").val(retorno.Data.AssinaturaId);
                    $("#drpRodape").select2();
                }
                
                if (retorno.Data.LaudadorId > 0) {
                    $("#drpLaudador").val(retorno.Data.LaudadorId);
                    $("#drpLaudador").select2();
                }
                if (retorno.Data.ClienteId > 0) {
                    $("#drpCliente").val(retorno.Data.ClienteId);
                    $("#drpCliente").select2();
                }

                if (retorno.Data.ClinicaId > 0) {
                    $("#drpClinica").val(retorno.Data.ClinicaId);
                    $("#drpClinica").select2();
                }

                // PERFIL GERENTE
                if ($("#hdnPerfilCodigo").val() !== "2") {
                    $("#drpClinica").prop("disabled", true);
                }

                $("#drpSituacao").val(retorno.Data.SituacaoId);
                $("#drpSituacao").select2();


                MODULO.LoadDuvidasClinica(retorno.Data.DuvidasClinica);

                MODULO.LoadDuvidasLaudador(retorno.Data.DuvidasLaudador);


                /*
                    [Description("Administrador")]
                    Administrador = 1,
                    [Description("Gerente")]
                    Gerente = 2,
                    [Description("Clínica")]
                    Clinica = 3,
                    [Description("Cliente")]
                    Cliente = 4,
                    [Description("Laudador")]
                    Laudador = 5
    
    
                    <option value="2">Em Análise (Clínica)</option>
                    <option value="3">Em Análise (Laudador)</option>
                    <option value="4">Em Análise (Gerente)</option>
                    <option value="5">Concluído (Gerente)</option>
                    <option value="6">Concluído</option>
                 */
                console.log("hdnPerfilCodigo: " + $("#hdnPerfilCodigo").val());
                console.log("SituacaoId: " + retorno.Data.SituacaoId);
                // PERFIL IGUAL CLINICA = 3  &&  SITUACAOID DIFF "Em Análise (Clínica) = 2" ENTAO BLOQUEIA
                if ($("#hdnPerfilCodigo").val() === "3" && retorno.Data.SituacaoId !== 2) {
                    $(".clsInput").prop("disabled", true); //Disable
                    $(".select2").prop("disabled", true);
                    $("#txtHistorico").prop("disabled", true);
                    $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();
                    $("#areaExameFinal").hide();
                    $("#areaHistoricoTab").hide();
                    $(".clsImages").hide();
                    $("#btnCriarExame").hide();
                    $("#collapseOneImg").addClass("show");
                }

                // PERFIL IGUAL LAUDADOR = 5  &&  SITUACAOID DIFF "Em Análise (Laudador) = 3" ENTAO BLOQUEIA
                if ($("#hdnPerfilCodigo").val() === "5" && retorno.Data.SituacaoId !== 3) {
                    $(".clsInput").prop("disabled", true); //Disable
                    $(".select2").prop("disabled", true);
                    $("#txtHistorico").prop("disabled", true);
                    $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();
                    $("#areaExameFinal").hide();
                    $("#areaHistoricoTab").hide();
                    $(".clsImages").hide();
                    $("#btnCriarExame").hide();
                    $("#collapseOneImg").addClass("show");
                }
                if ($("#hdnPerfilCodigo").val() === "5" && retorno.Data.SituacaoId === 3) {
                    $("#areaHistoricoTab").hide();
                    $(".clsImages").hide();
                    $("#collapseOnePdf").addClass("show");
                    $("#collapseOneImg").addClass("show");

                    $(".clsInput").prop("disabled", true); //Disable
                    $("#txtHistorico").prop("disabled", true);
                    //$(".select2").prop("disabled", true);
                    $("#drpLaudador, #drpCliente").prop("disabled", true);
                    $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();

                }


                // PERFIL IGUAL CLIENTE = 4  &&  SITUACAOID DIFF "Concluído = 6" ENTAO BLOQUEIA
                if ($("#hdnPerfilCodigo").val() === "4" && retorno.Data.SituacaoId !== 6) {
                    $(".clsInput").prop("disabled", true); //Disable
                    $(".select2").prop("disabled", true);
                    $("#txtHistorico").prop("disabled", true);
                    $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();
                    $("#areaExameFinal").hide();
                    $("#areaHistoricoTab").hide();
                    $(".clsImages").hide();
                    $("#btnCriarExame").hide();
                    $("#collapseOneImg").addClass("show");
                }


                if ($("#hdnPerfilCodigo").val() === "4") {
                    $(".clsInput").prop("disabled", true); //Disable
                    $(".select2").prop("disabled", true);
                    $("#txtHistorico").prop("disabled", true);
                    $("#btnOpenModalCliente, #btnOpenModalLaudador").hide();
                    $("#areaExameFinal").hide();
                    $("#areaHistoricoTab").hide();
                    $(".clsImages").hide();
                    $("#btnCriarExame").hide();
                    $("#collapseOneImg").addClass("show");
                }


                //$("input").prop("disabled", false); //Enable


                //$("#drpRaca").val(
                //  entidade.SituacaoId = Int32.Parse(dr["SituacaoId"].ToString());
                // entidade.ClienteId = Int32.Parse(dr["ClienteId"].ToString());
                // entidade.LaudadorId = Int32.Parse(dr["LaudadorId"].ToString());
                // entidade.Descricao = dr["Descricao"].ToString();
                //  entidade.RacaId = Int32.Parse(dr["RacaId"].ToString());


                //if (retorno.Data.Ativo) {
                //  $("#chkAtivo").attr('checked', 'checked');
                //}


                if ($("#hdnPerfilId").val() === $("#hdnPerfilClinicaId").val()) {

                    ///TODO:
                    $("#txtDataExame").attr("readonly", "readonly");
                    $("#txtDataExameTime").attr("readonly", "readonly");
                    $("#txtVeterinario").attr("readonly", "readonly");
                    $("#txtIdade").attr("readonly", "readonly");
                    $("#txtPaciente").attr("readonly", "readonly");
                    $("#txtEspecieSelecao").attr("readonly", "readonly");
                    $("#txtRacaSelecao").attr("readonly", "readonly");
                    $("#txtHistorico").attr("readonly", "readonly");

                    $("#areaExameFinal").hide();
                    $("#areaArquivos").hide();
                    $("#areaHistoricoTab").hide();
                    $("#btnCriarExame").hide();

                    
                    
                    $("#areaMensagensClinica").show();
                    $("#areaMensagensLaudador").hide();
                    $("#areaMensagemClickClinica").click();

                    $("#btnCriarExameLaudador").hide();
                    $("#btnCriarExameConcluir").hide();
                }


                if ($("#hdnPerfilId").val() === $("#hdnPerfilLaudadorId").val()) {

                    // SituacaoId = 5 : CONCLUIDO para GERENTE

                    // SituacaoId = 3 : ANALISE para LAUDADOR
                    $("#areaMensagensClinica").hide();
                    $("#areaMensagensLaudador").show();
                    //$("#areaMensagemClickLaudador").click();


                    if (retorno.Data.SituacaoId === 5) {
                        $("#btnRespostaAnaliseLaudador").hide();
                        $("#btnRespostaConcluirGerente").hide();
                        $("#btnLiberarAnalise").hide();
                        $("#btnRealizarAnalise").hide();
                    }
                    else {
                        if (parseInt($("#hdnUsuarioExameId").val()) === retorno.Data.LaudadorId) {
                            $("#btnRealizarAnalise").hide();
                            $("#btnLiberarAnalise").show();

                            $("#btnRespostaAnaliseLaudador").show();
                            $("#btnRespostaConcluirGerente").show();

                            if (retorno.Data.PeriodoTermino !== "") {
                                $("#lblTimer").show();
                                MODULO.StartTimer(retorno.Data.PeriodoTerminoFmt, retorno.Data.PeriodoTermino2Fmt);
                            }

                            ///$("#lblTimer").html(retorno.Data.PeriodoTermino + "&nbsp;&nbsp;");

                            $("#areaMensagensLaudador").show();
                            $("#areaMensagensClinica").hide();
                            $("#areaExameFinal").show();
                            $("#areaArquivos").show();
                            $("#areaHistoricoTab").hide();
                        } else {

                            if (retorno.Data.LaudadorId === 0) {
                                $("#btnRealizarAnalise").show();
                            }
                            else {
                                $("#btnRealizarAnalise").hide();

                                //console.log("PeriodoTermino :" + retorno.Data.PeriodoTermino);
                                if (retorno.Data.PeriodoTermino === "") {
                                    $("#btnRealizarAnalise").show();
                                    $("#lblTimer").show();


                                    if (retorno.Data.PeriodoTerminoFmt === "" &&
                                        retorno.Data.PeriodoTermino2Fmt === "") {
                                        $("#lblTimer").html("Laudo em andamento" + "&nbsp;&nbsp;");
                                    }
                                    else {
                                        if (MODULO.ValidaPrazo(retorno.Data.PeriodoTerminoFmt, retorno.Data.PeriodoTermino2Fmt) === false) {
                                            MODULO.LiberarLaudoConcorrente();

                                            Lobibox.notify('success', {
                                                size: 'mini',
                                                msg: 'Exame está disponível!'
                                            });

                                        }
                                    }
                                }
                                else {
                                    $("#lblTimer").show();
                                    MODULO.StartTimerView(retorno.Data.PeriodoTerminoFmt, retorno.Data.PeriodoTermino2Fmt);
                                }


                            }
                            $("#btnLiberarAnalise").hide();

                            $("#btnRespostaAnaliseLaudador").hide();
                            $("#btnRespostaConcluirGerente").hide();


                            
                            $("#areaMensagensLaudador").hide();
                            $("#areaMensagensClinica").hide();
                            $("#areaExameFinal").hide();
                            $("#areaArquivos").hide();
                            $("#areaHistoricoTab").hide();
                        }

                        $("#btnCriarExame").hide();
                        $("#btnCriarExameLaudador").hide();
                        $("#btnCriarExameConcluir").hide();
                    }

                }

                if ($("#hdnPerfilId").val() === $("#hdnPerfilGerenteId").val()) {
                    $("#btnCriarExameLaudador").show();
                    $("#btnCriarExameConcluir").show();

                    $("#areaMensagemClickClinica").html("Informe suas dúvidas / sugestões (CLÍNICA)");

                    $("#areaMensagemClickLaudador").html("Informe suas dúvidas/sugestões (LAUDADOR)");


                }


                // CONCLUIDO
                if (retorno.Data.SituacaoId === 6) {

                    $("#btnSalvarDuvidaClinica").hide();
                    $("#btnSalvarDuvidaLaudador").hide();
                    $("#btnCriarExame").hide();
                    $("#btnRespostaAnaliseLaudador").hide();
                    $("#btnRespostaConcluirGerente").hide();
                    $("#btnRealizarAnalise").hide();
                    $("#btnLiberarAnalise").hide();
                    $("#btnCriarExameLaudador").hide();
                    $("#btnCriarExameConcluir").hide();
                    $("#areaExameFinal").hide();
                    $("#editorQuestao").hide();
                    $("#lblTimer").hide();

                }
            }
            else {
                //$(".tab-content").hide();
                $(".tab-content").html("<h4>Sem permissão para acessar os dados!</h4>");
            }

        }, true);

    },

    StartTimer: function (dt, dt2) {

        // Set the date we're counting down to
        var countDownDate = new Date(dt2).getTime();//"Jan 5, 2024 15:37:25").getTime();

        // Update the count down every 1 second
        var x = setInterval(function () {

            // Get today's date and time
            //var now = new Date(dt2).getTime();//"Jan 5, 2024 15:37:25").getTime();
            var now = new Date().getTime();
            var finalizou = false;
            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            // Display the result in the element with id="demo"

            if (days > 0) {
                document.getElementById("lblTimer").innerHTML = days + "d " + hours + "h "
                    + minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else if (hours > 0) {
                document.getElementById("lblTimer").innerHTML = hours + "h "
                    + minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else if (minutes > 0) {
                document.getElementById("lblTimer").innerHTML = minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else {
                document.getElementById("lblTimer").innerHTML = seconds + "s&nbsp;&nbsp;";

                if (seconds <= 0) {
                    finalizou = true;
                }
            }

            // console.log("Distance:" + distance + " | finalizou:" + finalizou);
            // If the count down is finished, write some text
            if (distance < 0 && finalizou === true) {
                clearInterval(x);
                document.getElementById("lblTimer").innerHTML = "&nbsp;&nbsp;";

                MODULO.LiberarLaudoExpirou();

                Lobibox.notify('error', {
                    size: 'mini',
                    msg: 'Tempo expirou para finalizar exame!'
                });
            }
        }, 1000);
    },

    ValidaPrazo: function (dt, dt2) {

        var countDownDate = new Date(dt2).getTime();//"Jan 5, 2024 15:37:25").getTime();
        var now = new Date().getTime();
        var distance = countDownDate - now;

        if (distance < 0) {
            return false;
        }
        else {
            return true;
        }
    },
    StartTimerView: function (dt, dt2) {

        // Set the date we're counting down to
        var countDownDate = new Date(dt2).getTime();//"Jan 5, 2024 15:37:25").getTime();

        // Update the count down every 1 second
        var x = setInterval(function () {

            // Get today's date and time
            //var now = new Date(dt2).getTime();//"Jan 5, 2024 15:37:25").getTime();
            var now = new Date().getTime();
            var finalizou = false;
            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            // Display the result in the element with id="demo"

            if (days > 0) {
                document.getElementById("lblTimer").innerHTML = "Em andamento: " + days + "d " + hours + "h "
                    + minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else if (hours > 0) {
                document.getElementById("lblTimer").innerHTML = "Em andamento: " + hours + "h "
                    + minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else if (minutes > 0) {
                document.getElementById("lblTimer").innerHTML = "Em andamento: " + minutes + "m " + seconds + "s&nbsp;&nbsp;";
            }
            else {
                document.getElementById("lblTimer").innerHTML = "Em andamento: " + seconds + "s&nbsp;&nbsp;";

                if (seconds <= 0) {
                    finalizou = true;
                }
            }

            // console.log("Distance:" + distance + " | finalizou:" + finalizou);
            // If the count down is finished, write some text
            if (distance < 0 && finalizou === true) {
                clearInterval(x);
                document.getElementById("lblTimer").innerHTML = "&nbsp;&nbsp;";

                MODULO.LiberarLaudoConcorrente();

                Lobibox.notify('success', {
                    size: 'mini',
                    msg: 'Exame está disponível!'
                });
            }
        }, 1000);
    },
    RealizarLaudo: function () {

        //$("#btnRealizarAnalise").fadeOut();

        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/VincularLaudador", json, function (retorno) {
            //$("#btnRealizarAnalise").fadeIn();
            if (retorno.Data) {


                MODULO.Load();
                window.setTimeout(function () {

                    $("#btnRealizarAnalise").hide();
                    $("#btnLiberarAnalise").show();

                    MODULO.ReloadModelos();

                }, 500);


            }
        }, true);
    },
    ReloadModelos: function () {

        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": $("#hdnId").val() });        
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/CarregarModelos", json, function (retorno) {
            if (retorno.Data !== null) {
                var html = "<option value=''>:: Selecione ::</option>";
                $(retorno.Data).each(function () {
                    html = html + "<option value='" + this.Value + "'>" + this.Text + "</option>";
                });

                $("#drpModelo").html(html);
                $("#drpModelo").select2();
            }
        }, true);
    },
    LiberarLaudo: function () {
        //$("#btnLiberarAnalise").fadeOut();
        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/DesvincularLaudador", json, function (retorno) {
            //$("#btnLiberarAnalise").fadeIn();
            if (retorno.Data) {


                MODULO.Load();

                window.setTimeout(function () {
                    $("#btnLiberarAnalise").hide();
                    $("#btnRealizarAnalise").show();
                    $("#lblTimer").hide();

                }, 500);
            }


        }, true);
    },
    LiberarLaudoConcorrente: function () {
        //$("#btnLiberarAnalise").fadeOut();
        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/DesvincularLaudadorConcorrente", json, function (retorno) {
            //$("#btnLiberarAnalise").fadeIn();
            if (retorno.Data) {


                MODULO.Load();

                window.setTimeout(function () {
                    $("#btnLiberarAnalise").hide();
                    $("#btnRealizarAnalise").show();
                    $("#lblTimer").hide();

                }, 500);
            }


        }, true);
    },
    LiberarLaudoExpirou: function () {
        //$("#btnLiberarAnalise").fadeOut();
        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/DesvincularLaudadorExpirou", json, function (retorno) {
            ///$("#btnLiberarAnalise").fadeIn();
            if (retorno.Data) {


                MODULO.Load();

                window.setTimeout(function () {
                    $("#btnLiberarAnalise").hide();
                    $("#btnRealizarAnalise").show();
                    $("#lblTimer").hide();

                }, 500);
            }


        }, true);
    },
    Save: function (option) {

        //$("#btnCriarExame").fadeOut();
        //$("#btnCriarExameLaudador").fadeOut();
        //$("#btnCriarExameConcluir").fadeOut();
        //alert($("#hdnId").val());

        var json = $("form").serializeArray();
        json.push({ "name": "opt", "value": option });
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
                    }, 1700);
                    //$("#hdnIdhdnId").val(retorno.Data.Id);
                    //$("#lblCodigo").html(retorno.Data.Id);
                    //$("#areaCodigo").show()
                }
                else {
                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: 'Registro atualizado com sucesso!'
                    });

                    window.setTimeout(function () {
                        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Save/" + retorno.Data.Data.Id;
                    }, 1700);
                }

                MODULO.LoadHistorico();

            }
            //$("#btnCriarExame").fadeIn();
            //$("#btnCriarExameLaudador").fadeIn();
            //$("#btnCriarExameConcluir").fadeIn();
        }, true);

    }
};



var FILECONTROL = {
    AreaNome: "Cadastro",
    ControllerNome: "Exame",
    MaxImages: 30,
    OpenModalImage: function (url) {
        JSERVICE.Ajax.GetData("Base/VerificaSessao", {}, function (retorno) {
            if (retorno.Criticas.length > 0) {
                window.location.href = $("#UrlRootSite").val();
            }
            else {
                $("#hdnTotalFotos").val($("#listImages li").length);
                var total = parseInt($("#hdnTotalFotos").val());
                if (total < FILECONTROL.MaxImages) {
                    $("#frmUploads").attr("src", url + "?codigo=" + $("#hdnId").val() + "&t=" + Date.now());
                    $('#myUpload').modal({ backdrop: 'static', keyboard: false });
                }
                else {
                    if (FILECONTROL.MaxImages === 1) {
                        alert("Maximo 1 imagem");
                    } else {
                        alert("Maximo " + FILECONTROL.MaxImages + " imagens");
                    }
                }

            }
        }, true);

    },
    CloseModalImage: function () {
        $("#myUpload").modal("hide");
    },
    ErrorUpdateImage: function (message, fileName) {

        Lobibox.notify("error", {
            size: 'mini',
            rounded: true,
            delayIndicator: false,
            sound: false,
            position: 'center top',
            title: "Aviso",
            icon: false,
            msg: message + " (" + fileName + ")"
        });

    },
    UpdateImage: function (path, fileName, base64) {
        var index = "";
        $.ajax({
            //Tipo do envio das informações GET ou POST
            //async: false,
            type: "POST",
            url: JSERVICE.rootApplication + FILECONTROL.AreaNome + "/" + FILECONTROL.ControllerNome + "/JsonAddFile",//$("#hdnPathControl").val() + "/JsonAddFile",
            data: JSON.stringify({
                "path": path,
                "name": fileName,
                "index": index,
                "base64": base64
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //Tratamento dos dados de retorno.

                //alert(JSON.stringify(data.d[0]));
                var obj = data;
                if (obj.status === "OK") {
                    $("#listImages").html("");
                    $("#hdnTotalFotos").val(obj.listImages.length);
                    var htmlImages = FILECONTROL.CreateList(obj.listImages);
                    $("#listImages").html(htmlImages);
                }

            },
            //Se acontecer algum erro é executada essa função
            error: function (erro) {
                alert("Erro:" + JSON.stringify(erro));
            }
        });
    },
    RefreshImages: function (id) {
        $.ajax({
            //Tipo do envio das informações GET ou POST
            //async: false,
            type: "POST",
            url: JSERVICE.rootApplication + FILECONTROL.AreaNome + "/" + FILECONTROL.ControllerNome + "/JsonRefresh",
            data: JSON.stringify({
                "id": id
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //Tratamento dos dados de retorno.

                //alert(JSON.stringify(data.d[0]));
                var obj = data;
                if (obj.status === "OK") {
                    $("#listImages").html("");
                    $("#hdnTotalFotos").val(obj.listImages.length);
                    var htmlImages = FILECONTROL.CreateList(obj.listImages);
                    $("#listImages").html(htmlImages);
                }

            },
            //Se acontecer algum erro é executada essa função
            error: function (erro) {
                alert("Erro:" + JSON.stringify(erro));
            }
        });
    },
    CreateList: function (arrObj) {
        var htmlLine = "";
        var _empresaId = $("#hdnEmpresaId").val();
        var _exameId = $("#hdnId").val();
        var ctFoto = 0;
        for (var i = 0; i < arrObj.length; i++) {

            ctFoto = i + 1;
            htmlLine = htmlLine + "<div class='list-group-item list-group-item-success mt-1'>";
            htmlLine = htmlLine + "<div class='row'>";
            htmlLine = htmlLine + "<div class='col-10 text-left'>";
            //htmlLine = htmlLine + "<span class=\"mb-1\" style=\"font-size: 11px;\">" + arrObj[i].OriginalFileName + "</span>";

            htmlLine = htmlLine + "<a href=\"javascript:;\" data-id=\"" + ctFoto + "\" onclick=\"FILECONTROL.OpenEditImage(this,'" + JSERVICE.rootApplication + "upload/exames/" + _empresaId + "/" + _exameId + "/" + arrObj[i].OriginalFileName + "'," + _empresaId + "," + _exameId + ",'" + arrObj[i].OriginalFileName + "');\" >";
            htmlLine = htmlLine + "<img  id='picture_" + ctFoto + "' src='" + JSERVICE.rootApplication + "upload/exames/" + _empresaId + "/" + _exameId + "/" + arrObj[i].OriginalFileName + "' style = 'height:100px;' />";
            htmlLine = htmlLine + "<span class='mb-1' style='font-size: 11px;'>&nbsp;" + arrObj[i].OriginalFileName + "</span>";
            htmlLine = htmlLine + "</a>";

            htmlLine = htmlLine + "</div>";
            htmlLine = htmlLine + "<div class='col-2'>";
            // htmlLine = htmlLine + "<button type='button' class='btn btn-sm btn-inline btn-danger' onclick='FILECONTROL.RemoveFile(" + i + ")'>X</button>";
            htmlLine = htmlLine + "<button type='button' class='btn btn-sm btn-inline btn-danger clsImages' title='remover'  onclick='FILECONTROL.RemoveFile(" + i + ")'><i class='font-icon font-icon-del'></i></button>";
            htmlLine = htmlLine + "<br/>";
            htmlLine = htmlLine + "<button type='button' class='btn btn-sm btn-inline btn-primary' title='visualizar'  onclick='FILECONTROL.ViewFile(\"" + JSERVICE.rootApplication + "upload/exames/" + _empresaId + "/" + _exameId + "/" + arrObj[i].OriginalFileName + "\")'><i class='glyphicon glyphicon-search'></i></button>";
            htmlLine = htmlLine + "<br/>";
            htmlLine = htmlLine + "<button type='button' class='btn btn-sm btn-inline btn-warning' title='download'  onclick='FILECONTROL.DownFile(\"" + JSERVICE.rootApplication + "upload/exames/" + _empresaId + "/" + _exameId + "/" + arrObj[i].OriginalFileName + "\")'><i class='glyphicon glyphicon-download-alt'></i></button>";


            htmlLine = htmlLine + "</div>";
            htmlLine = htmlLine + "</div>";




            //htmlLine = htmlLine + "<br />";
            //htmlLine = htmlLine + "<a href='" + JSERVICE.rootApplication + arrObj[i].Path + arrObj[i].Name + "\' target='_blank' class='btn btn-sm btn-inline btn-secondary mr-2'><span class='glyphicon glyphicon-save-file' aria-hidden='true' ></span>Visualizar</a>";
            htmlLine = htmlLine + "</div>";
        }

        return htmlLine;
    },
    ViewFile: function (path) {
        window.open(path, "_blank");
    },
    ZoomFile: function (path) {
        window.open(JSERVICE.rootApplication + "Cadastro/Exame/Zoom?p=" + path);
    },
    DownFile: function (path) {
        /*
        var link = document.createElement('a');
        link.download = path;
        link.href = path;
        link.click();
        */

        var text = "Download file";
        var element = document.createElement('a');
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
        element.setAttribute('download', path);
        element.style.display = 'none';
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    },
    SalvarEditImage: function () {

        var id = $("#hdnId").val();
        var empresaId = $("#hdnEmpresaId").val();
        var filename = FILECONTROL.FileSelected;
        var filenametemp = FILECONTROL.FileTempSelected;
        var valor = FILECONTROL.ValorEditImageContraste;

        var json = Array();
        //json.push({ "name": "valor", "value": valor });
        json.push({ "name": "eid", "value": empresaId });
        json.push({ "name": "id", "value": id });
        json.push({ "name": "filename", "value": filename });
        json.push({ "name": "filenametemp", "value": filenametemp });

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/JsonSaveNewImage", json, function (retorno) {
            if (retorno !== null && retorno.Status === "OK") {
                //var url = $("#hdnUrlPath").val() + "exames/" + empresaId + "/" + id + "/" + retorno.FileName + "?t=" + retorno.Timestamp;
                //$("#picture_" + FILECONTROL.FileIndex).attr("src", url);
                //console.log("url: " + url);
                //console.log("#picture_" + FILECONTROL.FileIndex);
                $("#myEditImage").modal("hide");
                FILECONTROL.RefreshImages(id);
            }
        }, true);
    },
    ValorEditImageContraste: 0,
    ValorEditImageBrilho: 0,
    ValorEditImageZoom: 0,
    ComponenteRange: null,
    ComponenteRangeBrilho: null,
    ComponenteRangeZoom: null,

    FileSelected: "",
    FileIndex: "",
    FileTempSelected: "",
    OpenEditImage: function (obj, pathFile, eid, id, file) {
        // alert(pathFile);
        $("#imgPictureModal").attr("src", pathFile);
        $("#myEditImage").modal("show");
        FILECONTROL.ValorEditImageContraste = 0;
        FILECONTROL.FileSelected = file;
        try {
            //$("#rngSliderContrast").reset();
            if (FILECONTROL.ComponenteRange !== null) {
                FILECONTROL.ComponenteRange.update({
                    from: 0
                });
            }

        } catch (err) { console.log(JSON.stringify(err)); }


        try {
            //$("#rngSliderContrast").reset();
            if (FILECONTROL.ComponenteRangeBrilho !== null) {
                FILECONTROL.ComponenteRangeBrilho.update({
                    from: 0
                });
            }

        } catch (err) { console.log(JSON.stringify(err)); }



        try {
            //$("#rngSliderContrast").reset();
            if (FILECONTROL.ComponenteRangeZoom !== null) {
                FILECONTROL.ComponenteRangeZoom.update({
                    from: 0
                });
            }

        } catch (err) { console.log(JSON.stringify(err)); }

        $("#rngSliderBrilho").ionRangeSlider({
            grid: true,
            min: -100,
            max: 100,
            from: 0,
            step: 1,
            prefix: "",
            onFinish: function (data) {
                // fired on pointer release
                //console.log(data.from_percent);
                //console.log(data.from);
                var json = $("form").serializeArray();
                json.push({ "name": "valor", "value": data.from });
                json.push({ "name": "eid", "value": eid });
                json.push({ "name": "id", "value": id });
                json.push({ "name": "file", "value": FILECONTROL.FileSelected });
                json.push({ "name": "mapPath", "value": $("#hdnMapPath").val() });

                //alert(JSON.stringify(json));

                JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/JsonEditImageBrilho", json, function (retorno) {

                    if (retorno !== null && retorno.Status === "OK") {
                        FILECONTROL.ValorEditImageBrilho = data.from;
                        FILECONTROL.FileTempSelected = retorno.FileNameTemp;
                        FILECONTROL.FileIndex = $(obj).attr("data-id");

                        //alert(FILECONTROL2.FileIndex);
                        var url = $("#hdnUrlPath").val() + "exames/" + eid + "/" + id + "/temp/" + retorno.FileNameTemp + "?t=" + retorno.Timestamp;
                        console.log("url: " + url);
                        $("#imgPictureModal").attr("src", url);
                        //alert($(obj).find("img").attr("src"));
                    }
                }, true);
            },
        });




        //$("#rngSliderZoom").ionRangeSlider({
        //    grid: true,
        //    min: -100,
        //    max: 100,
        //    from: 0,
        //    step: 1,
        //    prefix: "",
        //    onFinish: function (data) {
        //        // fired on pointer release
        //        //console.log(data.from_percent);
        //        //console.log(data.from);
        //        var json = $("form").serializeArray();
        //        json.push({ "name": "valor", "value": data.from });
        //        json.push({ "name": "eid", "value": eid });
        //        json.push({ "name": "id", "value": id });
        //        json.push({ "name": "file", "value": FILECONTROL.FileSelected });
        //        json.push({ "name": "mapPath", "value": $("#hdnMapPath").val() });

        //        //alert(JSON.stringify(json));

        //        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/JsonEditZoom", json, function (retorno) {

        //            if (retorno !== null && retorno.Status === "OK") {
        //                FILECONTROL.ValorEditImageZoom = data.from;
        //                FILECONTROL.FileTempSelected = retorno.FileNameTemp;
        //                FILECONTROL.FileIndex = $(obj).attr("data-id");

        //                //alert(FILECONTROL.FileIndex);
        //                var url = $("#hdnUrlPath").val() + "exames/" + eid + "/" + id + "/temp/" + retorno.FileNameTemp + "?t=" + retorno.Timestamp;
        //                console.log("url: " + url);
        //                $("#imgPictureModal").attr("src", url);
        //                //alert($(obj).find("img").attr("src"));
        //            }
        //        }, true);
        //    },
        //});


        $("#rngSliderContrast").ionRangeSlider({
            grid: true,
            min: -100,
            max: 100,
            from: 0,
            step: 1,
            prefix: "",
            onFinish: function (data) {
                // fired on pointer release
                //console.log(data.from_percent);
                //console.log(data.from);
                var json = $("form").serializeArray();
                json.push({ "name": "valor", "value": data.from });
                json.push({ "name": "eid", "value": eid });
                json.push({ "name": "id", "value": id });
                json.push({ "name": "file", "value": FILECONTROL.FileSelected });
                json.push({ "name": "mapPath", "value": $("#hdnMapPath").val() });

                //alert(JSON.stringify(json));

                JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/JsonEditImage", json, function (retorno) {

                    if (retorno !== null && retorno.Status === "OK") {
                        FILECONTROL.ValorEditImageContraste = data.from;
                        FILECONTROL.FileTempSelected = retorno.FileNameTemp;
                        FILECONTROL.FileIndex = $(obj).attr("data-id");

                        //alert(FILECONTROL.FileIndex);
                        var url = $("#hdnUrlPath").val() + "exames/" + eid + "/" + id + "/temp/" + retorno.FileNameTemp + "?t=" + retorno.Timestamp;
                        console.log("url: " + url);
                        $("#imgPictureModal").attr("src", url);
                        //alert($(obj).find("img").attr("src"));
                    }
                }, true);
            },
        });
        if (FILECONTROL.ComponenteRange === null) {
            FILECONTROL.ComponenteRange = $("#rngSliderContrast").data("ionRangeSlider");
        }
        if (FILECONTROL.ComponenteRangeBrilho === null) {
            FILECONTROL.ComponenteRangeBrilho = $("#rngSliderBrilho").data("ionRangeSlider");
        }
        //if (FILECONTROL.ComponenteRangeZoom === null) {
        //    FILECONTROL.ComponenteRangeZoom = $("#rngSliderZoom").data("ionRangeSlider");
        //}
    },
    RemoveFile: function (index) {


        swal({
            title: "",
            text: "Deseja remover este arquivo?",
            type: "error",
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Remover",
            cancelButtonText: "Cancelar",
            closeOnConfirm: false,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {

                    $.ajax({
                        //Tipo do envio das informações GET ou POST
                        //async: false,
                        type: "POST",
                        //url: '@Url.Action("JsonRemoveFile", "CadastroCredpago")',
                        url: JSERVICE.rootApplication + FILECONTROL.AreaNome + "/" + FILECONTROL.ControllerNome + "/JsonRemoveFile",
                        //data: "index=" + index,
                        data: JSON.stringify({
                            "index": index
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            //Tratamento dos dados de retorno.
                            //alert(JSON.stringify(data));
                            var obj = data;
                            if (obj.status === "OK") {
                                $("#listImages").html("");
                                $("#hdnTotalFotos").val(obj.listImages.length);
                                var htmlImages = FILECONTROL.CreateList(obj.listImages);
                                $("#listImages").html(htmlImages);
                            }

                        },
                        //Se acontecer algum erro é executada essa função
                        error: function (erro) {
                            alert("Erro:" + JSON.stringify(erro));
                        }
                    });
                }
                swal.close();
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

