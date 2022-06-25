var SOHAPP = angular.module("SOH", []);

$(document).ready(function () {


    $(document).ajaxStart(function () {
        if (JSERVICE.stopAutoLoading === false) {
            JSERVICE.Loading(true);
        }
    }).ajaxStop(function () {
        $.unblockUI();
    });

    $("BODY").fadeIn(500);

    $('.horaMask').mask('00:00', { placeholder: "__:__" });
    $('.dataHora').mask('00/00/0000 - 00:00');


});

var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i) || navigator.userAgent.match(/WPDesktop/i);
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};

var JSERVICE = {
    GetProfileId: function (code) {
        var pid = $("#hdnProfileId" + code).val();
        return pid;
    },
    Criticas: function (retorno) {
        $(".form-control-error").removeAttr("title");
        $(".form-control-error").removeClass("form-control-error");

        if (retorno.Criticas.length > 0) {
            $(retorno.Criticas).each(function () {
                $("#" + this.FieldId).addClass("form-control-error");
                $("#" + this.FieldId).attr("title", this.Message);
            });

            JSERVICE.Mensagem2(retorno);
            return true;
        }

        return false;
    },
    SetProfile: function (pid) {
        var json = $("form").serializeArray();
        json.push({ "name": "pid", "value": pid });
        JSERVICE.Ajax.GetData("/Dashboard/SetProfile", json, function (retorno) {
            if (retorno.Data) {
                window.location.href = JSERVICE.rootApplication + "/Dashboard";
            }
        }, true);
    },
    RefreshDtLembrete: function () {

        //$("#txtLembreteChange").datepicker({
        //    dateFormat: 'dd/mm/yy',
        //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        //    nextText: 'Proximo',
        //    prevText: 'Anterior'
        //});

        $('#txtLembreteChange').datetimepicker({
            language: 'pt-BR',
            format: "dd/mm/yyyy - hh:ii",
            autoclose: true,
            todayBtn: true,

            minuteStep: 10
        });
    },
    ZoomStart: function (areaId) {
        var htmlMinus = $(areaId + " li:first").html();
        //$(areaId + " li:first").html("<span style='background-color:#FFF;padding:10px 10px 20px 10px;' id='topAcoesConsultor'></span>&nbsp;&nbsp;&nbsp;<span style='background-color:#EFEFEF;padding:10px 10px 20px 10px'><a style='padding:5px 0px 0px 0px;font-weight:bold;font-size:15px' href='javascript:JSERVICE.Word(1);'>A+</a>&nbsp;&nbsp;&nbsp;<a style='padding:5px 0px 0px 0px;font-weight:bold;font-size:15px' href='javascript:JSERVICE.Word(-1);'>A-</a></span>" + htmlMinus);

        $(areaId + " li:first").html("<span style='background-color:#EFEFEF;padding:10px 10px 20px 10px'><a style='padding:5px 0px 0px 0px;font-weight:bold;font-size:15px' href='javascript:JSERVICE.Word(1);'>A+</a>&nbsp;&nbsp;&nbsp;<a style='padding:5px 0px 0px 0px;font-weight:bold;font-size:15px' href='javascript:JSERVICE.Word(-1);'>A-</a></span>" + htmlMinus);
    },
    Word: function (param) {
        switch (param) {
            case 1:
                JSERVICE.WordIncrease();
                break;
            case 0:
                JSERVICE.WordReset();
                break;
            case -1:
                JSERVICE.WordDecrease();
                break;
        }
    },
    WordSize: 1,
    WordRemoveClass: function () {
        $("table#boxAtividesRecentes").removeClass("font-size1");
        $("table#boxAtividesRecentes").removeClass("font-size2");
        $("table#boxAtividesRecentes").removeClass("font-size3");
        $("table#boxAtividesRecentes").removeClass("font-size4");
    },
    WordReset: function () {
        JSERVICE.WordRemoveClass();
        JSERVICE.WordSize = 1;
        var index = JSERVICE.WordSize;
        $("table#boxAtividesRecentes").addClass('font-size' + index);
    },
    WordIncrease: function () {

        JSERVICE.WordRemoveClass();
        if (JSERVICE.WordSize < 4) {
            JSERVICE.WordSize = JSERVICE.WordSize + 1;
        }
        var index = JSERVICE.WordSize;
        $("table#boxAtividesRecentes").addClass('font-size' + index);
    },
    WordDecrease: function () {
        JSERVICE.WordRemoveClass();
        if (JSERVICE.WordSize > 1) {
            JSERVICE.WordSize = JSERVICE.WordSize - 1;
        }
        var index = JSERVICE.WordSize;
        $("table#boxAtividesRecentes").addClass('font-size' + index);
    },

    stopAutoLoading: false,
    rootApplicationImob: $("#UrlBaseImob").val(),
    rootApplication: $("#UrlBase").val(),
    rootSohtec: $("#UrlDominio").val(),///  "https://www.sohtec.com.br",
    OrderNumber: 0,
    OrderSort: 'ASC',

    Animate: function (element, animationName, callback) {

        const node = document.querySelector(element)
        node.classList.add('animated', animationName)

        function handleAnimationEnd() {
            node.classList.remove('animated', animationName)
            node.removeEventListener('animationend', handleAnimationEnd)

            if (typeof callback === 'function') callback()
        }

        node.addEventListener('animationend', handleAnimationEnd)

    },

    ShowNewDeploy: function (codigo, dataHora, arrayMessage) {

        var htmlItems = "";
        $(arrayMessage).each(function () {
            //           htmlItems += "<p style='float:left'>&bull; " + this + "</p>";
            htmlItems += "<li style='padding-left:8px;list-style:none;margin-bottom:6px;text-align:left'>&bull; " + this + "</li>";
        });

        //   $('.swal-btn-info').click(function (e) {
        swal({
            title: "<strong  style='font-size:16px'><u>Atualização </u></strong><div style='font-size:13px'>" + dataHora + "</div>",
            text: "<ul style='font-size:14px;padding:0;margin:0;'>" + htmlItems + "</ul>",
            html: ' ',
            // type: "info",
            showCancelButton: false,
            cancelButtonClass: "btn-default",
            confirmButtonText: "OK",
            confirmButtonClass: "btn-primary"
        },
            function (isConfirm) {

                var json = $("form").serializeArray();
                json.push({ "name": "codigo", "value": codigo });

                JSERVICE.Ajax.GetData("/GenericControl/UpdateUserAtualizacao", json, function (retorno) {

                }, true);

            }

        );
        // });
    },
    GetCountMenuLembrete: function () {
        var empresaid = $("#hdnCurrentEmpresaId").val();
        var usuarioid = $("#hdnCurrentUsuarioId").val();

        var json = $("form").serializeArray();
        json.push({ "name": "empresaid", "value": empresaid });
        json.push({ "name": "usuarioid", "value": usuarioid });

        JSERVICE.Ajax.GetData("/GenericControl/GetCountLembrete", json, function (retorno) {
            if (retorno.Data > 0) {
                $("#ctMenuLembrete").show();
                $("#ctMenuLembrete").html(retorno.Data);
            } else {
                $("#ctMenuLembrete").hide();
            }
        }, true);
    },
    EditPrioridadeObj: function (obj) {

        var opt = $(obj).attr("data-pid");
        var code = $(obj).attr("data-ceid");
        var acao = $(obj).attr("data-acao");
        var mid = $(obj).attr("data-mid");
        $("#txtMotivoPrioridade").removeClass("form-control-error");
        $("#txtMotivoPrioridade").val("");
        $("#txtLembreteChange").val("");
        $("#chkLembrete").removeAttr("checked");
        $("#chkLembrete").prop("selected", false);
        $("#areaDtLembrete").hide();
        $("#lnkTab02").removeClass("active");
        $("#lnkTab02").removeClass("show");
        $("#lnkTab01").addClass("active");
        $("#lnkTab01").addClass("show");

        $("#tabs-1-tab-2").removeClass("active");
        $("#tabs-1-tab-2").removeClass("show");
        // $("#tabs-1-tab-1").removeClass("in");
        $("#tabs-1-tab-1").addClass("active");
        $("#tabs-1-tab-1").addClass("show");
        // $("#tabs-1-tab-2").addClass("in");

        var empresaid = $("#hdnCurrentEmpresaId").val();

        var json = $("form").serializeArray();
        json.push({ "name": "code", "value": code });
        json.push({ "name": "acao", "value": acao });
        json.push({ "name": "empresaid", "value": empresaid });
        JSERVICE.Ajax.GetData("/GenericControl/ListViewHistorico", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                //$(retorno.Criticas).each(function () {
                //    $("#" + this.FieldId).addClass("form-control-error");
                //});

                //$("#msgError").addClass("show");
            }
            else {

                var html = "";

                $(retorno.Data).each(function (k, e) {

                    var htmlOpt = "<span class='label label-custom label-pill icon-circle-prior-" + e.Prioridade + "'><small>" + e.PrioridadeTexto + "</small></span>";

                    html += "<tr>";
                    html += "<td>";
                    html += htmlOpt;
                    html += "</td>";

                    html += "<td>";
                    html += "<span class='color-blue-grey-lighter'><small>" + e.ClienteNome + "</small></span>";
                    html += "<br/><small>" + e.ClienteEmail + "</small>";
                    html += "</td>";

                    html += "<td><small>";
                    html += e.Motivo;
                    html += "</small></td>";

                    html += "<td>";

                    if (e.AdminUserNome !== null)
                        html += "<span class='color-blue-grey-lighter'><small>" + e.AdminUserNome + "</small></span><br/>";

                    if (e.AdminUserEmail !== null)
                        html += "<small>" + e.AdminUserEmail + "</small><br/>";

                    html += "<small>" + e.DataHoraFmt + "</small>";
                    html += "</td>";

                    html += "</tr>";


                });

                $("#modalBodyDisparoPrioridade").html(html);

            }
        }, true);


        $("#btnConfirmPrioridade").attr("onclick", "JSERVICE.SavePrioridade(" + opt + "," + code + ",'" + acao + "', " + mid + ")");
        $("#modalPrioridadeHistorico").modal("show");


        $("#drpPrioridadeChange").val(opt).trigger('change');


    },
    EditPrioridadeVisita: function (opt, code, mid) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Visita', mid);
    },
    EditPrioridadeModalidade: function (opt, code, mid) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Modalidade', mid);
    },
    EditPrioridadeLead: function (opt, code) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Lead', 0);
    },
    EditPrioridadeProposta: function (opt, code, mid) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Proposta', mid);
    },
    EditPrioridadeId: function (opt, code, acao, mid, id, dt) {
        $("#hdnModalLembreteId").val(id);
        $("#hdnModalLembreteData").val(dt);
        JSERVICE.EditPrioridade(opt, code, acao, mid);
    },
    EditPrioridade: function (opt, code, acao, mid) {
        $("#txtMotivoPrioridade").removeClass("form-control-error");
        $("#txtMotivoPrioridade").val("");


        $("#lnkTab02").removeClass("active");
        $("#lnkTab02").removeClass("show");
        $("#lnkTab01").addClass("active");
        $("#lnkTab01").addClass("show");

        $("#tabs-1-tab-2").removeClass("active");
        $("#tabs-1-tab-2").removeClass("show");
        // $("#tabs-1-tab-1").removeClass("in");
        $("#tabs-1-tab-1").addClass("active");
        $("#tabs-1-tab-1").addClass("show");
        // $("#tabs-1-tab-2").addClass("in");

        var empresaid = $("#hdnCurrentEmpresaId").val();

        var json = $("form").serializeArray();
        json.push({ "name": "code", "value": code });
        json.push({ "name": "empresaid", "value": empresaid });
        JSERVICE.Ajax.GetData("/GenericControl/ListViewHistorico", json, function (retorno) {

            if (retorno.Criticas.length > 0) {
                //$(retorno.Criticas).each(function () {
                //    $("#" + this.FieldId).addClass("form-control-error");
                //});

                //$("#msgError").addClass("show");
            }
            else {

                var html = "<table style='width:100%' cellpadding='10' class='tbl-typical' cellspacing='10'>";
                html += "<colgroup>";
                html += "<col style='width:10%'/>";
                html += "<col style='width:20%'/>";
                html += "<col style='width:%'/>";
                html += "<col style='width:20%'/>";
                html += "</colgroup>";

                html += "<tr>";
                html += "<td>";
                html += "<small><strong>Prioridade</strong></small>";
                html += "</td>";
                html += "<td>";
                html += "<center><small><strong>Usuário</strong></small></center>";
                html += "</td>";
                html += "<td>";
                html += "<center><small><strong>Motivo</strong></small></center>";
                html += "</td>";
                html += "<td>";
                html += "<center><small><strong>Criado/Alterado Por</strong></small></center>";
                html += "</td>";
                html += "</tr>";

                $(retorno.Data).each(function (k, e) {

                    var htmlOpt = "<span class='label label-custom label-pill icon-circle-prior-" + e.Prioridade + "'><small>" + e.PrioridadeTexto + "</small></span>";


                    html += "<tr>";
                    html += "<td>";
                    html += htmlOpt;
                    html += "</td>";

                    html += "<td>";
                    html += "<span class='color-blue-grey-lighter'><center><small>" + e.ClienteNome + "</small></span>";
                    html += "<br/><small>" + e.ClienteEmail + "</small></center>";
                    html += "</td>";

                    html += "<td><center><small>";
                    html += e.Motivo;
                    html += "</small></center></td>";

                    html += "<td>";

                    if (e.AdminUserNome !== null)
                        html += "<span class='color-blue-grey-lighter'><center><small>" + e.AdminUserNome + "</small></span><br/>";

                    if (e.AdminUserEmail !== null)
                        html += "<small>" + e.AdminUserEmail + "</small><br/>";

                    html += "<small>" + e.DataHoraFmt + "</small></center>";
                    html += "</td>";

                    html += "</tr>";


                });
                html += "</table>";
                $("#modalBodyDisparoPrioridade").html(html);

            }
        }, true);


        $("#btnConfirmPrioridade").attr("onclick", "JSERVICE.SavePrioridade(" + opt + "," + code + ",'" + acao + "'," + mid + ")");
        $("#modalPrioridadeHistorico").modal("show");
        $("#drpPrioridadeChange").val(opt).trigger('change');


        if ($("#hdnModalLembreteId").val() !== "") {
            $("#chkLembrete").attr('checked', 'checked');
            JSERVICE.RefreshDtLembrete();
            $("#txtLembreteChange").val($("#hdnModalLembreteData").val());
            $("#areaDtLembrete").show();
            //       $("#areaDtLembrete").hide();
        }
    },
    SavePrioridade: function (optold, code, acao, mid) {

        //alert("ANTES:" + opt + " : " + code);
        //alert("DEPOIS:" + $("#drpPrioridadeChange").val() + " : " + code);

        var opt = $("#drpPrioridadeChange").val();
        if ($("#txtMotivoPrioridade").val() === "") {
            $("#txtMotivoPrioridade").addClass("form-control-error");
        }
        else {

            var lembreteid = $("#hdnModalLembreteId").val();

            $("#txtMotivoPrioridade").removeClass("form-control-error");
            var lembrete = "0";
            if ($("#chkLembrete").is(":checked")) {
                lembrete = "1";
            }
            var lembretedata = $("#txtLembreteChange").val();

            if (lembrete === "1" && $("#txtLembreteChange").val() === "") {
                $("#txtLembreteChange").addClass("form-control-error");
            }
            else {
                $("#txtLembreteChange").removeClass("form-control-error");

                var empresaid = $("#hdnCurrentEmpresaId").val();
                var usuarioid = $("#hdnCurrentUsuarioId").val();

                //  alert(empresaid + " - " + usuarioid);
                var json = $("form").serializeArray();
                json.push({ "name": "motivo", "value": $("#txtMotivoPrioridade").val() });
                json.push({ "name": "codigo", "value": code });
                json.push({ "name": "opcao", "value": opt });
                json.push({ "name": "acao", "value": acao });
                json.push({ "name": "modalidadeid", "value": mid });
                json.push({ "name": "lembrete", "value": lembrete });
                json.push({ "name": "lembreteid", "value": lembreteid });
                json.push({ "name": "lembretedata", "value": lembretedata });
                json.push({ "name": "empresaid", "value": empresaid });
                json.push({ "name": "usuarioid", "value": usuarioid });
                JSERVICE.Ajax.GetData("/GenericControl/AlterarPrioridadeMotivo", json, function (retorno) {

                    if (retorno.Criticas.length > 0) {
                        $(retorno.Criticas).each(function () {
                            $("#" + this.FieldId).addClass("form-control-error");
                            $("#" + this.FieldId).next().addClass("form-control-error");
                        });
                        JSERVICE.ShowMessage("Atenção", true, "Informe corretamente os campos abaixo!");
                    }
                    else {
                        if (retorno.Data) {
                            MODULO.Buscar();
                            $("#modalPrioridadeHistorico").modal("hide");
                            JSERVICE.ShowMessage("Atenção", false, "Registro cadastrado com sucesso!");
                        }
                        else {
                            JSERVICE.ShowMessage("Atenção", true, "Erro ao atualizar. Tente mais tarde!");
                        }
                    }
                }, true);
            }
        }
    },
    OpenViewer: function (url) {

        window.open(JSERVICE.rootApplication + '/DocViewer?p=' + url, "_blank");

    },
    CompressZIP: function (obj) {

        var areaId = $(obj).parent().parent().parent().attr("id");
        //alert(areaId);

        //if (areaId === "fine-uploader-gallery-inquilino") {
        window.open(JSERVICE.rootApplication + '/Analise/' + MODULO.ControllerNome + '/CompressDocs?tp=locatario&mail=' + $("#hdnClienteEmail").val() + '&eid=' + $("#hdnEmpresaId").val());
        //}

    },
    ViewFile: function (obj) {


        //var fileName = $(obj).parent().parent().find(".qq-file-info .qq-file-name .qq-upload-file").html();
        //window.open(JSERVICE.rootSohtec + "/upload/forms/" + MODULO.ControllerNome + "/" + $("#hdnClienteEmail").val() + "/" + $("#hdnEmpresaId").val() + "/" + fileName, "_blank");

        var fileName = $(obj).parent().parent().find(".qq-file-info .qq-file-name .qq-upload-file").html();
        //window.open(JSERVICE.rootSohtec + "/upload/forms/" + MODULO.ControllerNome + "/" + $("#hdnClienteEmail").val() + "/" + $("#hdnEmpresaId").val() + "/" + fileName, "_blank");
        var url = JSERVICE.rootSohtec + "/upload/forms/" + MODULO.ControllerNome + "/" + $("#hdnClienteEmail").val() + "/" + $("#hdnEmpresaId").val() + "/" + fileName;
        JSERVICE.OpenViewer(url);

    },

    AssinantesLoad: function (id, moduloid, status, empresaid) {

        JSERVICE.AssinanteModuloId = moduloid;
        JSERVICE.AssinanteId = id;

        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "moduloId", "value": moduloid });
        json.push({ "name": "status", "value": status });
        json.push({ "name": "empresaId", "value": empresaid });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesLoad", json, function (retorno) {

            if (retorno.Error === false) {
                $("#hdnAssinantesTotalMin").val(retorno.Data.totalMin);
                $("#hdnAssinantesTotalMax").val(retorno.Data.totalMax);
                JSERVICE.AssinantesBuildGrid(retorno.Data.list, id, moduloid);

                if (retorno.Data.list.length > 0) {
                    $("#areaAssinantes").show();
                } else {
                    $("#areaAssinantes").hide();
                }
            }

        }, true);
    },


    HistoricoAtividades: function (id, mid, eid) {

        var json = $("form").serializeArray();
        json.push({ "name": "id", "value": id });
        json.push({ "name": "mid", "value": mid });
        json.push({ "name": "eid", "value": eid });

        JSERVICE.Ajax.GetData("/GenericControl/HistoricoAtividades", json, function (retorno) {

            if (retorno.Error === false) {
                var html = "";

                if (retorno.Data !== null && retorno.Data.length > 0) {
                    $(retorno.Data).each(function (e, k) {

                        html += "<div class='widget-activity-item'>";
                        html += "<div class='user-card-row'>";
                        html += "<div class='tbl-row'>";
                        html += "<div class='tbl-cell tbl-cell-photo'>";
                        html += "<a href='#'>";
                        html += "<img src='" + $("#UrlBase").val() + "content/img/avatar-2-64.png' alt=''>";
                        html += "</a>";
                        html += "</div>";
                        html += "<div class='tbl-cell'>";
                        html += "<p>";
                        html += "<a href='#' class='semibold'>" + k.UserName + " <small>(" + k.UserEmail + ")</small></a> ";

                        if (k.TipoMessage === "Proposta") {
                            html += "<small><strong>" + k.Message + "</small>";
                        }
                        else {
                            html += "<small><strong>" + k.PerfilNome + "</strong> alterou status para <strong>" + k.StatusDescricao + "</strong></small>";//Message;
                        }
                        // html += "<a href='#'>Free UI Kit</a>";
                        html += "</p>";
                        html += "<p>" + k.DateInsertFmt + "</p>";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                    });
                }

                $("#accordionBody").html(html);
                $("#accordionHistorico").show();
            }

        }, true);
    },
    EventUpdateGrid: function (id, moduloid) {

        var jsonData = new Array();
        var jsonDataIDs = new Array();
        var ct = 1;
        $("#gridAssinantesGlobal tbody tr").each(function () {

            if ($(this).find(".codeID").val() !== undefined) {


                if (jQuery.inArray($(this).find(".codeID").val(), jsonDataIDs) !== -1) {
                    // console.log("JA EXISTE:" + $(this).find(".codeID").val());
                } else {
                    // console.log("NOVO:" + $(this).find(".codeID").val());
                    jsonDataIDs.push($(this).find(".codeID").val());
                    jsonData.push({
                        "Id": $(this).find(".codeID").val(),
                        "Campo": $(this).find(".codeAlias").val(),
                        "Nome": $(this).find(".codeNome").val(),
                        "Email": $(this).find(".codeEmail").val(),
                        "Selfie": $(this).find(".codeCheck").is(":checked"),
                        "Ordem": ct
                    });
                    ct++;
                }
            }
        });

        // alert(JSON.stringify(jsonData));

        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "moduloId", "value": moduloid });
        json.push({ "name": "json", "value": JSON.stringify(jsonData) });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesUpdateAll", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGrid(retorno.Data, id, moduloid);
            }

        }, true);


    },
    AssinanteModuloId: 0,
    AssinanteId: 0,
    AssinantesBuildGrid: function (data, id, moduloid) {


        var htmlBody = "";
        var minAssinantes = parseInt($("#hdnAssinantesTotalMin").val());
        var totalAssinantes = parseInt($("#hdnAssinantesTotalMax").val());
        var totalItems = data.length;
        var ctNum = 1;
        $(data).each(function (k, item) {

            if (ctNum === 1) {
                htmlBody += "<tr style='background-color:#eee'>";
            }
            else {
                htmlBody += "<tr>";
            }
            // htmlBody += "<td><label>" + item.Ordem + "</label></td>";
            htmlBody += "<td style='cursor:pointer'><i class='fa fa-bars'></i><small>&nbsp;" + ctNum + "º</small></td>";

            htmlBody += "<td>";
            htmlBody += "<input type='hidden' id='hdnAssinanteID_" + item.Ordem + "' name='hdnAssinanteID_" + item.Ordem + "' value='" + item.Id + "' class='codeID'  />";

            htmlBody += "<div class='input-group'>";
            htmlBody += "<input type='text' id='txtAlias_" + item.Ordem + "' name='txtAlias_" + item.Ordem + "' value='" + item.Campo + "' class='form-control codeAlias' onChange='return JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");'  maxlength='100'/>";
            htmlBody += "</div>";
            htmlBody += "</td>";
            htmlBody += "<td>";
            htmlBody += "<div class='input-group'>";
            htmlBody += "<input type='text' id='txtValor_" + item.Ordem + "' name='txtValor_" + item.Ordem + "' value='" + item.Nome + "' class='form-control codeNome' onChange='return JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");' maxlength='100' />";
            htmlBody += "</div>";
            htmlBody += "</td>";
            htmlBody += "<td>";
            htmlBody += "<div class='input-group'>";
            htmlBody += "<input type='text' id='txtEmail_" + item.Ordem + "' name='txtEmail_" + item.Ordem + "' value='" + item.Email + "' class='form-control codeEmail' onChange='return JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");'  maxlength='100' />";
            htmlBody += "</div>";
            htmlBody += "</td>";
            htmlBody += "<td>";
            htmlBody += "<div class='input-group'>";
            if (item.Selfie === true) {
                htmlBody += "<input type='checkbox' id='chkSelfie_" + item.Ordem + "' name='chkSelfie_" + item.Ordem + "' value='' checked='checked' class='form-control codeCheck' onclick='JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");' />";
            }
            else {
                htmlBody += "<input type='checkbox' id='chkSelfie_" + item.Ordem + "' name='chkSelfie_" + item.Ordem + "' value='' class='form-control codeCheck'  onclick='JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");'/>";
            }

            htmlBody += "</div>";
            htmlBody += "</td>";
            htmlBody += "<td>";
            if (item.Ordem > minAssinantes) {
                htmlBody += "<span style='cursor:pointer;' onclick='JSERVICE.AssinantesRemoveItem(" + item.Ordem + "," + id + "," + moduloid + ");'>";
                htmlBody += "<span class='glyphicon glyphicon-minus' aria-hidden='true'></span>";
                htmlBody += "</span>";
            }
            htmlBody += "</td>";
            htmlBody += "<td>";
            if (item.Ordem === totalItems && item.Ordem < totalAssinantes) {
                htmlBody += "<span style='cursor:pointer;' onclick='JSERVICE.AssinantesAddItem(" + id + "," + moduloid + ");'>";
                htmlBody += "<span class='glyphicon glyphicon-plus' aria-hidden='true'></span>";
                htmlBody += "</span>";
            }


            htmlBody += "</td>";
            htmlBody += "</tr>";

            ctNum++;
        });


        $("#gridAssinantesGlobal tbody").html(htmlBody);

    },

    GetGridGeralAssinantes: function () {
        var json = new Array();

        var ct = 1;
        $("#gridAssinantesGlobal tbody tr").each(function () {

            json.push({
                "Ordem": ct,
                "Alias": $(this).find("#txtAlias_" + ct).val(),
                "Valor": $(this).find("#txtValor_" + ct).val(),
                "Email": $(this).find("#txtEmail_" + ct).val(),
                "Selfie": $(this).find("#chkSelfie_" + ct).is(":checked")
            });
            ct++;
        });

        return json;



    },


    AssinantesRemoveItem: function (ordem, id, moduloid) {
        var json = $("form").serializeArray();
        json.push({ "name": "ordem", "value": ordem });
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "moduloId", "value": moduloid });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesRemove", json, function (retorno) {
            //      JSERVICE.Ajax.GetView("/GenericControl/AssinantesRemove", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGrid(retorno.Data, id, moduloid);
            }

        }, true);
    },
    AssinantesAddItem: function (id, moduloid) {
        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "moduloId", "value": moduloid });

        //  var gridDataJson = JSERVICE.GetGridDataJson();
        //if (gridDataJson.length < JSERVICE.MaxGridItems) {
        // $("#msgErrorMessage").hide();
        //json.push({ "name": "json", "value": JSON.stringify(gridDataJson) });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesAdd", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGrid(retorno.Data, id, moduloid);
            }

        }, true);
        //} else {
        //  $("#dvMessageError").html("Máximo " + JSERVICE.MaxGridItems + " assinante(s)!");
        //  $("#msgErrorMessage").show();
        //}


    },

    MaxGridItems: parseInt($("#hdnAssinantesTotalMax").val()),
    //GetGridDataJson: function () {
    //    var json = new Array();


    //    var ct = 1;
    //    $("#gridAssinantesGlobal tbody tr").each(function () {

    //        json.push({
    //            "Ordem": ct,
    //            "Alias": $(this).find("#txtAlias_" + ct).val(),
    //            "Valor": $(this).find("#txtValor_" + ct).val(),
    //            "Email": $(this).find("#txtEmail_" + ct).val(),
    //            "Selfie": $(this).find("#chkSelfie_" + ct).is(":checked")
    //        });
    //        ct++;
    //    });
    //    return json;



    //},
    DeleteFile: function (obj) {

        var fileName = $(obj).parent().parent().find(".qq-file-info .qq-file-name .qq-upload-file").html();
        var areaId = $(obj).parent().parent().parent().parent().parent().parent().attr("id");


        swal({
            title: "Atenção",
            text: "Deseja remover o arquivo? " + fileName,
            showCancelButton: true,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    var json = $("form").serializeArray();
                    json.push({ "name": "filename", "value": fileName });
                    json.push({ "name": "areaid", "value": areaId });
                    json.push({ "name": "clienteemail", "value": $("#hdnClienteEmail").val() });
                    JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/SubmitFilesRemove", json, function (retorno) {
                        if (retorno.Error === true) {
                            swal({
                                title: "Atenção",
                                text: "Erro ao tentar remover arquivo :(",
                                type: "error",
                                confirmButtonClass: "btn-danger"
                            });
                        }
                        else {
                            MODULO.MontarGridArquivos(retorno.Data);
                            swal({
                                title: "Atenção",
                                text: "Arquivo removido com sucesso.",
                                type: "success",
                                confirmButtonClass: "btn-success"
                            });
                        }

                    }, true);
                } else {
                    swal.close();
                }
            });

    },


    MontarGridArquivosMessage: function (objArray) {

        JSERVICE.GetComponentUploadMessage().clearStoredFiles();

        $(objArray).each(function (k, e) {

            JSERVICE.GetComponentUploadMessage().addInitialFiles([
                {
                    "name": e.NOME,
                    "uuid": e.NOME,
                    "size": e.SIZE,
                    "thumbnailUrl": e.CAMINHO_RELATIVO
                }]);
        });


        //alert("1");
        $("div#fine-uploader-gallery-message div#btnZip.btn.btn-inline.btn-warning-outline.btn-sm.ladda-button").hide();
        //////////if (objArray.length === 0) {
        // $("#btnZip").hide();
        //   JSERVICE.GetComponentUploadMessage();
        // } else {
        // $("#btnZip").show();
        //  }
    },
    ComponentUploadInstanceMessage: null,
    GetComponentUploadMessage: function () {
        var tid = $("#hdnTipoModuloId").val();
        var mid = $("#hdnId").val();
        var eid = $("#hdnEmpresaId").val();
        var oid = 0;
        if (JSERVICE.ComponentUploadInstanceMessage === null) {
            JSERVICE.ComponentUploadInstanceMessage = new qq.FineUploader({
                element: document.getElementById("fine-uploader-gallery-message"),
                template: 'qq-template-gallery',
                request: {
                    endpoint: JSERVICE.rootApplication + '/BaseAdmin/SubmitFiles',
                    params: { tipoupload: '1', areaid: 'fine-uploader-gallery-message', tipoid: tid, moduloid: mid, obsid: oid, empresaid: eid }
                },
                thumbnails: {
                    placeholders: {
                        waitingPath: JSERVICE.rootApplication + '/content/fine-uploader/placeholders/waiting-generic.png',
                        notAvailablePath: JSERVICE.rootApplication + '/content/fine-uploader/placeholders/not_available-generic.png'
                    }
                },
                validation: {
                    allowedExtensions: ['*']
                },
                callbacks: {
                    onComplete: function (id, oldStatus, data) {
                        $("div#" + data.areaid + " div.qq-uploader-selector.qq-uploader.qq-gallery ul.qq-upload-list-selector.qq-upload-list li.qq-file-id-" + id + ".qq-upload-success div.qq-file-info div.qq-file-name span.qq-upload-file-selector.qq-upload-file").html(data.filename);
                        $("div#" + data.areaid + " div.qq-uploader-selector.qq-uploader.qq-gallery ul.qq-upload-list-selector.qq-upload-list li.qq-file-id-" + id + ".qq-upload-success div.qq-file-info div.qq-file-name span.qq-upload-file-selector.qq-upload-file").attr("title", data.filename);
                        $("div#" + data.areaid + " div.qq-uploader-selector.qq-uploader.qq-gallery ul.qq-upload-list-selector.qq-upload-list li.qq-file-id-" + id + ".qq-upload-success div.qq-file-info div.qq-upload-size-selector.qq-upload-size").html(data.size);

                        var files = $("#hdnFilesMessage").val();

                        if (files === "") {
                            $("#hdnFilesMessage").val(data.filename);
                        } else {
                            files = files + "|" + data.filename;
                            $("#hdnFilesMessage").val(files);
                        }

                        //alert("_JSERVICE.js: " + $("#hdnFilesMessage").val());
                        //$("div#" + data.areaid + " div.qq-uploader-selector.qq-uploader.qq-gallery div#btnZip.btn.btn-inline.btn-warning-outline.btn-sm.ladda-button").show();
                    }
                }
            });
        }
        return JSERVICE.ComponentUploadInstanceMessage;

    },

    BuscarByCol: function (orderNumber, orderSort) {
        if (orderNumber !== undefined && orderSort !== undefined) {
            JSERVICE.OrderNumber = orderNumber;
            JSERVICE.OrderSort = orderSort;
        }

        MODULO.Buscar();
    },
    GenerateSort: function (tableid) {
        var ct = 0;

        var colorSelected = "#000000";
        var colorNoSelected = "#cecece";

        $("#" + tableid + " thead .col_sort").each(function () {
            ct++;
            if ($(this).find("span").length > 0) {
                $(this).find("span").remove();
            }

            var order = $(this).attr("col_order");
            var columnName = $(this).html();
            var htmlColumnOrder = "";
            if (JSERVICE.OrderNumber !== 0 && JSERVICE.OrderNumber === ct) {
                if (JSERVICE.OrderSort === 'DESC') {
                    //  htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorSelected + ";' class='fa fa-arrow-up'></i></span>";
                    htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorSelected + ";'></i></span>";

                } else {
                    htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorSelected + ";'></i></span>";
                    //  htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorNoSelected + ";' class='fa fa-arrow-up'></i></span>";
                }
            }
            else {
                htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i></span>";
                // htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorNoSelected + ";' class='fa fa-arrow-up'></i></span>";
            }
            $(this).html(htmlColumnOrder);

        });

    },
    Mensagem: function (message, titulo, tipo) {

        //'error'
        //'success'
        //'info'
        //'warning'
        //'confirm'
        //'progress'
        //'prompt'
        //'default'
        //'window'

        Lobibox.notify(tipo, {
            size: 'mini',
            rounded: true,
            delayIndicator: false,
            sound: false,
            position: 'center top',
            title: titulo,
            icon: false,
            msg: message
        });
    },

    Mensagem2: function (retorno) {

        //'error'
        //'success'
        //'info'
        //'warning'
        //'confirm'
        //'progress'
        //'prompt'
        //'default'
        //'window'

        Lobibox.notify(retorno.MessageTipo, {
            size: 'mini',
            rounded: true,
            delayIndicator: false,
            sound: false,
            position: 'center top',
            title: "",
            icon: false,
            msg: retorno.Message
        });
    },

    Mensagem3: function (retorno, fncCallback) {
        swal({
            title: "Mensagem do Sistema",
            text: retorno.Message,
            type: retorno.MessageTipo,
            confirmButtonClass: "btn-danger",
            confirmButtonText: "OK"
        },
            function (isConfirm) {
                if (jQuery.isFunction(fncCallback)) {
                    fncCallback();
                }
            }
        );
    },

    AplicaMascaras: function () {
        $('.cnpjMask').mask('00.000.000/0000-00', { placeholder: "__.___.___/____-__" });
        $('.telefoneMask').mask('(00) 0000-0000', { placeholder: "(__) ____-____" });
        $('.cepMask').mask('00000-000', { placeholder: "_____-___" });
        $('.horaMask').mask('00:00', { placeholder: "__:__" });

        $('.tagEditor').tagEditor();
    },

    HexaColor: function () {
        $('.hexaColor').each(function () {
            $(this).minicolors({
                control: $(this).attr('data-control') || 'hue',
                defaultValue: $(this).attr('data-defaultValue') || '',
                format: $(this).attr('data-format') || 'hex',
                keywords: $(this).attr('data-keywords') || '',
                inline: $(this).attr('data-inline') === 'true',
                letterCase: $(this).attr('data-letterCase') || 'lowercase',
                opacity: $(this).attr('data-opacity'),
                position: $(this).attr('data-position') || 'bottom left',
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                theme: 'bootstrap'
            });
        });
    },

    AplicaCalendario: function () {
        $('.calendario').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            "locale": {
                "format": "DD/MM/YYYY",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    "Do",
                    "Se",
                    "Te",
                    "Qu",
                    "Qu",
                    "Se",
                    "Sa"
                ],
                "monthNames": [
                    "Jan",
                    "Fev",
                    "Mar",
                    "Abr",
                    "Mai",
                    "Jun",
                    "Jul",
                    "Ago",
                    "Set",
                    "Out",
                    "Nov",
                    "Dez"
                ],
                "firstDay": 1
            }
        });
    },

    AddToolTip: function (obj, message, blockToolTip) {
        if (blockToolTip === true) {
            $(obj).tooltip({
                delay: 500,
                showURL: false
            });
        }
        else {
            $(obj).tooltip({
                delay: 500,
                bodyHandler: function () {
                    return message;
                },
                showURL: false
            });
        }
    },

    LoadingMessage: function (show, message) {
        var regionBlock = "bodyLayout";
        if (show) {
            $.blockUI({
                message: '<div class="blockui-default-message"><i class="fa fa-circle-o-notch fa-spin"></i><h6>' + message + '</h6></div>',
                overlayCSS: {
                    background: 'rgba(142, 159, 167, 0.8)',
                    opacity: 1,
                    cursor: 'wait'
                },
                css: {
                    width: '250px',
                    left: ($(window).width() - 250) / 2 + 'px'
                },
                blockMsgClass: 'block-msg-default'
            });
        }
        else {
            setTimeout(function () {
                $('#' + regionBlock).unblock();
            }, 150);
        }
    },

    Loading: function (show) {
        var regionBlock = "bodyLayout";
        if (show) {
            $.blockUI({
                message: '<div class="blockui-default-message"><i class="fa fa-circle-o-notch fa-spin"></i><h6>Processando. <br> Por favor aguarde.</h6></div>',
                overlayCSS: {
                    background: 'rgba(142, 159, 167, 0.8)',
                    opacity: 1,
                    cursor: 'wait'
                },
                css: {
                    width: '150px',
                    left: ($(window).width() - 150) / 2 + 'px'
                },
                blockMsgClass: 'block-msg-default'
            });
        }
        else {
            setTimeout(function () {
                $('#' + regionBlock).unblock();
            }, 150);
        }
    },

    LoadingArea: function (show, area) {
        var regionBlock = area;
        //var regionBlock = "bodyLayout";
        //var regionBlock = "allHtml";
        if (show) {
            //$('#blockui-block-element-default').on('click', function () {
            $('#' + regionBlock).block({
                message: '<div class="blockui-default-message"><i class="fa fa-circle-o-notch fa-spin"></i><h6>Processando. <br> Por favor aguarde.</h6></div>',
                overlayCSS: {
                    background: 'rgba(142, 159, 167, 0.8)',
                    opacity: 1,
                    cursor: 'wait'
                },
                css: {
                    width: '50%'
                },
                blockMsgClass: 'block-msg-default'
            });
        }
        else {
            setTimeout(function () {
                $('#' + regionBlock).unblock();
            }, 2000);
        }
        //});

    },

    ShowMessage: function (title, error, message, callbackSucesso) {

        if (error) {
            swal({
                title: title,
                text: message,
                type: "error",
                confirmButtonClass: "btn-danger",
                confirmButtonText: "OK"
            },
                function (isConfirm) {
                    if (jQuery.isFunction(callbackSucesso)) {
                        callbackSucesso();
                    }
                }
            );
        } else {
            //e.preventDefault();
            swal({
                title: title,
                text: message,
                type: "success",
                confirmButtonClass: "btn-success",
                confirmButtonText: "OK"
            },
                function (isConfirm) {
                    if (jQuery.isFunction(callbackSucesso)) {
                        callbackSucesso();
                    }
                }
            );

        }

    },

    ChangePage: function (obj) {

        if ($(obj).val() !== "") {
            MODULO.Pagination($(obj).val());
        }
    },

    ChangePage1: function (obj) {

        if ($(obj).val() !== "") {
            MODULO.Pagination1($(obj).val());
        }
    },

    ChangePage2: function (obj) {

        if ($(obj).val() !== "") {
            MODULO.Pagination2($(obj).val());
        }
    },
    BuildPagination: function (retorno) {
        var footer = "";

        if (retorno.PageIndex > 1) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination(1);' aria-label='Previous'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'>«</span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex > 1) {
            var prev = retorno.PageIndex - 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination(" + prev + ");' aria-label='Previous'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;'  aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'><small>anterior</small></span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        var pgs = retorno.PageTotal / retorno.PageSize;
        if ((retorno.PageTotal % retorno.PageSize) > 0) {
            pgs++;
        }

        var options = "";

        for (var i = 1; i <= parseInt(pgs); i++) {
            if (retorno.PageIndex === i)
                options += "<option value='" + i + "' selected>" + i + "</option>";
            else
                options += "<option value='" + i + "'>" + i + "</option>";
        }

        footer += "<li class='page-item'><select id='drpPaged' onchange='JSERVICE.ChangePage(this)' class='page-link select2' style='width:90px !important; height:36px !important'>" + options + "</select></li>";
        var start = (retorno.PageIndex - 1) * retorno.PageSize;
        var finish = start + retorno.PageSize < retorno.PageTotal ? start + retorno.PageSize : retorno.PageTotal;


        if (retorno.PageIndex < parseInt(pgs)) {
            var next = retorno.PageIndex + 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination(" + next + ");' aria-label='Next'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }
        footer += "<span aria-hidden='true'><small>próxima</small></span>";
        footer += "<span class='sr-only'>Next</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex < parseInt(pgs)) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination(" + parseInt(pgs) + ");' aria-label='Next'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }

        footer += "<span aria-hidden='true'>»</span>";
        footer += "<span class='sr-only'>Last</span>";
        footer += "</a>";
        footer += "</li>";

        $(".pagination").html(footer);
        $(".pagination").show();
        if (retorno.PageTotal === 0) {
            $("#pageMessage").html("Nenhum registro encontrado.");
            $(".pagination").hide();
        }
        else {
            var ctStart = start + 1;
            $("#pageMessage").html("Mostrando " + ctStart + " a " + finish + " de " + retorno.PageTotal + " registros");
        }
    },
    BuildPaginationSuffix: function (retorno, suffix) {
        var footer = "";
        if (retorno.PageIndex > 1) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination" + suffix + "(1);' aria-label='Previous'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'>«</span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex > 1) {
            var prev = retorno.PageIndex - 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination" + suffix + "(" + prev + ");' aria-label='Previous'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;'  aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'><small>anterior</small></span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        var pgs = retorno.PageTotal / retorno.PageSize;
        if ((retorno.PageTotal % retorno.PageSize) > 0) {
            pgs++;
        }

        var options = "";

        for (var i = 1; i <= parseInt(pgs); i++) {
            if (retorno.PageIndex === i)
                options += "<option value='" + i + "' selected>" + i + "</option>";
            else
                options += "<option value='" + i + "'>" + i + "</option>";
        }

        footer += "<li class='page-item'><select id='drpPaged' onchange='JSERVICE.ChangePage" + suffix + "(this)' class='page-link select2' style='width:90px !important; height:36px !important'>" + options + "</select></li>";
        var start = (retorno.PageIndex - 1) * retorno.PageSize;
        var finish = start + retorno.PageSize < retorno.PageTotal ? start + retorno.PageSize : retorno.PageTotal;


        if (retorno.PageIndex < parseInt(pgs)) {
            var next = retorno.PageIndex + 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination" + suffix + "(" + next + ");' aria-label='Next'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }
        footer += "<span aria-hidden='true'><small>próxima</small></span>";
        footer += "<span class='sr-only'>Next</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex < parseInt(pgs)) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination" + suffix + "(" + parseInt(pgs) + ");' aria-label='Next'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }

        footer += "<span aria-hidden='true'>»</span>";
        footer += "<span class='sr-only'>Last</span>";
        footer += "</a>";
        footer += "</li>";

        $(".pagination" + suffix).html(footer);
        $(".pagination" + suffix).show();
        if (retorno.PageTotal === 0) {
            $("#pageMessage" + suffix).html("Nenhum registro encontrado.");
            $(".pagination" + suffix).hide();
        }
        else {
            var ctStart = start + 1;
            $("#pageMessage" + suffix).html("Mostrando " + ctStart + " a " + finish + " de " + retorno.PageTotal + " registros");
        }
    },
    BuildPagination2: function (retorno) {
        var footer = "";
        var suffix = "2";
        if (retorno.PageIndex2 > 1) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination2(1);' aria-label='Previous'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'>«</span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex2 > 1) {
            var prev = retorno.PageIndex2 - 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination2(" + prev + ");' aria-label='Previous'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;'  aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'><small>anterior</small></span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        var pgs = retorno.PageTotal2 / retorno.PageSize2;
        if ((retorno.PageTotal2 % retorno.PageSize2) > 0) {
            pgs++;
        }

        var options = "";

        for (var i = 1; i <= parseInt(pgs); i++) {
            if (retorno.PageIndex2 === i)
                options += "<option value='" + i + "' selected>" + i + "</option>";
            else
                options += "<option value='" + i + "'>" + i + "</option>";
        }

        footer += "<li class='page-item'><select id='drpPaged' onchange='JSERVICE.ChangePage2(this)' class='page-link select2' style='width:90px !important; height:36px !important'>" + options + "</select></li>";
        var start = (retorno.PageIndex2 - 1) * retorno.PageSize2;
        var finish = start + retorno.PageSize2 < retorno.PageTotal2 ? start + retorno.PageSize2 : retorno.PageTotal2;


        if (retorno.PageIndex2 < parseInt(pgs)) {
            var next = retorno.PageIndex2 + 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination2(" + next + ");' aria-label='Next'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }
        footer += "<span aria-hidden='true'><small>próxima</small></span>";
        footer += "<span class='sr-only'>Next</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex2 < parseInt(pgs)) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination2(" + parseInt(pgs) + ");' aria-label='Next'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }

        footer += "<span aria-hidden='true'>»</span>";
        footer += "<span class='sr-only'>Last</span>";
        footer += "</a>";
        footer += "</li>";

        $(".pagination" + suffix).html(footer);
        $(".pagination" + suffix).show();
        if (retorno.PageTotal2 === 0) {
            $("#pageMessage" + suffix).html("Nenhum registro encontrado.");
            $(".pagination" + suffix).hide();
        }
        else {
            var ctStart = start + 1;
            $("#pageMessage" + suffix).html("Mostrando " + ctStart + " a " + finish + " de " + retorno.PageTotal2 + " registros");
        }
    },
    BuildPaginationCustom: function (sufixo, messageId, paginationId, retorno) {
        var footer = "";

        if (retorno.PageIndex > 1) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination" + sufixo + "(1);' aria-label='Previous'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'>«</span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex > 1) {
            var prev = retorno.PageIndex - 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.Pagination" + sufixo + "(" + prev + ");' aria-label='Previous'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;'  aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'><small>anterior</small></span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        var pgs = retorno.PageTotal / retorno.PageSize;
        if ((retorno.PageTotal % retorno.PageSize) > 0) {
            pgs++;
        }

        var options = "";

        for (var i = 1; i <= parseInt(pgs); i++) {
            if (retorno.PageIndex === i)
                options += "<option value='" + i + "' selected>" + i + "</option>";
            else
                options += "<option value='" + i + "'>" + i + "</option>";
        }

        footer += "<li class='page-item'><select id='drpPaged' onchange='MODULO.ChangePage" + sufixo + "(this)' class='page-link select2' style='width:90px !important; height:36px !important'>" + options + "</select></li>";
        var start = (retorno.PageIndex - 1) * retorno.PageSize;
        var finish = start + retorno.PageSize < retorno.PageTotal ? start + retorno.PageSize : retorno.PageTotal;


        if (retorno.PageIndex < parseInt(pgs)) {
            var next = retorno.PageIndex + 1;
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination" + sufixo + "(" + next + ");' aria-label='Next'>";
        } else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }
        footer += "<span aria-hidden='true'><small>próxima</small></span>";
        footer += "<span class='sr-only'>Next</span>";
        footer += "</a>";
        footer += "</li>";

        if (retorno.PageIndex < parseInt(pgs)) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.Pagination" + sufixo + "(" + parseInt(pgs) + ");' aria-label='Next'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }

        footer += "<span aria-hidden='true'>»</span>";
        footer += "<span class='sr-only'>Last</span>";
        footer += "</a>";
        footer += "</li>";

        $("#" + paginationId).html(footer);
        $("#" + paginationId).show();
        if (retorno.PageTotal === 0) {
            $("#" + messageId).html("Nenhum registro encontrado.");
            $("#" + paginationId).hide();
        }
        else {
            var ctStart = start + 1;
            $("#" + messageId).html("Mostrando " + ctStart + " a " + finish + " de " + retorno.PageTotal + " registros");
        }
    },

    BuildPaginationHistory: function (retorno) {
        var footer = "";

        if (retorno.PageIndex > 1) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick='MODULO.PaginationHistory(1);' aria-label='Previous'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Previous'>";
        }

        footer += "<span aria-hidden='true'>«</span>";
        footer += "<span class='sr-only'>Previous</span>";
        footer += "</a>";
        footer += "</li>";

        var pgs = retorno.PageTotal / retorno.PageSize;
        if ((retorno.PageTotal % retorno.PageSize) > 0) {
            pgs++;
        }
        for (var i = 1; i <= parseInt(pgs); i++) {
            if (retorno.PageIndex === i)
                footer += "<li class='page-item active'><a class='page-link' href='javascript:;'>" + i + " <span class='sr-only'>(current)</span></a></li>";
            else
                footer += "<li class='page-item'><a class='page-link' href='javascript:;' onclick='MODULO.PaginationHistory(" + i + ");'>" + i + "</a></li>";
        }

        var start = (retorno.PageIndex - 1) * retorno.PageSize;
        var finish = start + retorno.PageSize < retorno.PageTotal ? start + retorno.PageSize : retorno.PageTotal;

        if (retorno.PageIndex < parseInt(pgs)) {
            footer += "<li class='page-item'>";
            footer += "<a class='page-link' href='javascript:;' onclick= 'MODULO.PaginationHistory(" + parseInt(pgs) + ");' aria-label='Next'>";
        }
        else {
            footer += "<li class='page-item disabled'>";
            footer += "<a class='page-link' href='javascript:;' aria-label='Next'>";
        }

        footer += "<span aria-hidden='true'>»</span>";
        footer += "<span class='sr-only'>Next</span>";
        footer += "</a>";
        footer += "</li>";

        $(".pagination").html(footer);
        $(".pagination").show();
        if (retorno.PageTotal === 0) {
            $("#pageMessage").html("Nenhum registro encontrado.");
            $(".pagination").hide();
        }
        else {
            var ctStart = start + 1;
            $("#pageMessage").html("Mostrando " + ctStart + " a " + finish + " de " + retorno.PageTotal + " registros");
        }
    },
    ShowBreadcrumb: function (area, controller, title) {
        $("#ltlTitle").html(controller);
        $("#actionName").html(title);
        if (controller === "") {
            $("#areaNameLi").hide();
        }
        $("#controllerName").html(controller);
        $(".breadcrumb").fadeIn();
    },
    Ajax: function () {
        return {
            Name: '',
            GetDataHome: function (url, jsonData, callbackSucesso, async) {

                var asyncTemp = true;
                if (async !== null && async !== undefined) {
                    asyncTemp = async;
                }

                $.ajax({
                    type: "POST",
                    async: asyncTemp,
                    data: jsonData,
                    url: JSERVICE.rootApplication + url,
                    success: function (resposta) {
                        if (resposta.ErrorMessage !== "") {
                            //alert("No 1");
                            window.location.href = JSERVICE.rootApplication;
                            return;
                        }
                        else {
                            if (jQuery.isFunction(callbackSucesso)) {
                                callbackSucesso(resposta);
                            }
                        }
                    },
                    error: function (resposta) {
                        console.log("ERRO:" + JSON.stringify(resposta));
                        // INTRANET.MessageBox(resposta.responseText);
                    }
                });
            },


            GetDataExt: function (url, jsonData, callbackSucesso, async) {

                var asyncTemp = true;
                if (async !== null && async !== undefined) {
                    asyncTemp = async;
                }

                $.ajax({
                    type: "GET",
                    async: asyncTemp,
                    data: jsonData,
                    url: url,
                    success: function (resposta) {
                        callbackSucesso(resposta);
                    },
                    error: function (resposta) {
                        console.log(resposta);
                    }
                });
            },


            GetData: function (url, jsonData, callbackSucesso, async) {

                var asyncTemp = true;
                if (async !== null && async !== undefined) {
                    asyncTemp = async;
                }
                // console.log("Url Request: " + JSERVICE.rootApplication + url);
                $.ajax({
                    type: "POST",
                    async: asyncTemp,
                    data: jsonData,
                    url: JSERVICE.rootApplication + url,
                    // withCredentials: true,
                    success: function (resposta) {

                        if (resposta.Authenticated === false) {
                            console.log("ERRO 2:" + JSON.stringify(resposta));
                            //alert("No Authenticated");
                            //window.location.href = JSERVICE.rootApplication;
                            return;
                        }
                        else if (resposta.ErrorMessage !== "") {
                            console.log("ERRO 3:" + resposta.ErrorMessage);
                            // alert("No");
                            //window.location.href = JSERVICE.rootApplication;
                            return;
                        }
                        else {
                            if (jQuery.isFunction(callbackSucesso)) {
                                callbackSucesso(resposta);
                            }
                        }
                    },
                    error: function (resposta) {
                        console.log("ERRO:" + JSON.stringify(resposta) + "Url Request: " + JSERVICE.rootApplication + url);
                        swal({
                            title: "Atenção",
                            text: "Não foi possível realizar sua requisição :( Tente mais tarde!!",
                            type: "error",
                            confirmButtonClass: "btn-danger"
                        });
                    }
                });
            },


            GetView: function (url, jsonData, callbackSucesso) {
                $.ajax({
                    type: "POST",
                    data: jsonData,
                    async: false,
                    url: JSERVICE.rootApplication + url,
                    success: function (resposta) {
                        if (jQuery.isFunction(callbackSucesso)) {
                            callbackSucesso(resposta)
                        }
                    },
                    error: function (resposta) {
                        console.log(JSON.stringify(resposta));
                        // INTRANET.MessageBox(resposta.responseText);
                    }
                });
            }
        }
    }()

};