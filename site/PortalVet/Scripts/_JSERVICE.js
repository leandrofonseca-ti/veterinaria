var PORTALVETAPP = angular.module("PORTALVET", []);

window.odometerOptions = {
    auto: false, // Don't automatically initialize everything with class 'odometer'
    selector: '', // Change the selector used to automatically find things to be animated
    format: '', // Change how digit groups are formatted, and how many digits are shown after the decimal point
    duration: 1500, // Change how long the javascript expects the CSS animation to take
    theme: 'default', // Specify the theme (if you have more than one theme css file on the page)
    animation: 'count' // Count is a simpler animation method which just increments the value,
    // use it when you're looking for something more subtle.
};



PORTALVETAPP.directive('modalTemplateMensagem', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: JSERVICE.rootApplication + 'scripts/_controls/modalTemplateMensagem.html?d=' + Date.now().toString(),
        link: function (scope, $element, $attrs) {

            scope.templateMesagem = new Object();

            scope.modelos = new Object();
            scope.btnCancelar = false;
            scope.btnSalvar = false;
            scope.btnAdicionaNovo = true;
            scope.btnEnviar = false;
            scope.mostraTexto = false;
            scope.TextoModelo = "";
            scope.TextoEditModelo = "";
            scope.TituloEditModelo = "";
            scope.IdEditModelo = 0;
            scope.editModelo = false;
            scope.ClienteMensagemModelo = new Object();
            scope.EnvioPorEMail = false;
            scope.EnvioPorWhats = false;
            scope.txtarea = null;
            scope.Start = 0;
            scope.End = 0;
            scope.DisableWhats = true;

            scope.BlurText = function (oblId) {

                scope.txtarea = document.getElementById(oblId);
                console.log(oblId);
                console.log(scope.txtarea);

                scope.Start = scope.txtarea.selectionStart;
                scope.End = scope.txtarea.selectionEnd;

                console.log(scope.Start);
                console.log(scope.End);

            }

            scope.CopyParte = function (item, oblId, hdnObjId) {

                scope.txtarea = document.getElementById(oblId);
                $(`#${hdnObjId}`).val(item);
                $(`#${hdnObjId}`).select();
                document.execCommand('copy');
                $(`#${hdnObjId}`).val("");

                scope.txtarea.selectionEnd = scope.Start;
                scope.txtarea.selectionStart = scope.End;
                scope.txtarea.focus();

                navigator.clipboard.readText().then(function (text) {
                    document.execCommand("insertText", false, text || "");
                    console.log("FOI");
                });

            }

            scope.templateMesagem.openModal = function (entity) {

                scope.Variaveis = [];
                scope.Variaveis.push({ "key": "", "value": "Selecione..." });


                if (entity.Menu === "CLIENTES") {
                    scope.Variaveis.push({ "key": "{URL_EXAME}", "value": "Url do Exame" });
                    scope.Variaveis.push({ "key": "{CLIENTE}", "value": "Nome do Cliente" });
                    scope.Variaveis.push({ "key": "{CLINICA}", "value": "Nome da Clínica" });
                    scope.Variaveis.push({ "key": "{USUARIO_LOGADO}", "value": "Nome Usuário Logado" });
                }
                scope.ClienteMensagemModelo.ExameId = entity.ExameId;
                scope.ClienteMensagemModelo.ClienteNome = entity.ClienteNome;
                scope.ClienteMensagemModelo.ClienteEmail = entity.ClienteEmail;
                scope.ClienteMensagemModelo.ClienteTelefoneLinkWhats = entity.ClienteTelefoneLinkWhats;
                scope.ClienteMensagemModelo.ClienteTelefone = entity.ClienteTelefoneLinkWhats;
                scope.ClienteMensagemModelo.Entity = entity;

                console.log(entity);

                scope.NomeClienteModelo = scope.ClienteMensagemModelo.ClienteNome;
                scope.editModelo = false;
                scope.btnSalvar = false;
                scope.btnAdicionaNovo = true;
                scope.btnEnviar = false;
                scope.EnvioPorEMail = false;
                scope.EnvioPorWhats = false;
                scope.modeloSelecioanado = null;
                scope.AlteraModelo(null);
                scope.TextoEditModelo = "";
                scope.TituloEditModelo = "";

                $("#hdnCopyPaste").val("");
                $("#modalTemplateSMSWhatsMail").modal("show");
            }

            scope.AlteraModelo = function (modelo) {
                scope.mostraTexto = false;
                scope.btnEnviar = false;
                scope.TextoModelo = "";
                scope.DisableWhats = true;

                if (modelo !== null && modelo !== "") {
                    scope.TextoModelo = modelo.Mensagem;
                    scope.mostraTexto = true;
                    scope.btnEnviar = true;
                    scope.DisableWhats = false;
                }

                if (scope.ClienteMensagemModelo.Entity.ClienteTelefone == null || scope.ClienteMensagemModelo.Entity.ClienteTelefone == undefined || scope.ClienteMensagemModelo.Entity.ClienteTelefone == "") {
                    scope.DisableWhats = true;
                    //console.log("scope.DisableWhats", scope.DisableWhats);
                    //console.log("scope.ClienteMensagemModelo.Entity.ClienteTelefone", scope.ClienteMensagemModelo.Entity.ClienteTelefone);
                }

            }

            scope.InserirNovo = function (modelo) {
                scope.modeloSelecioanado = null;
                scope.AlteraModelo(null);
                scope.editModelo = true;
                scope.btnSalvar = true;
                //$scope.btnEnviar = false;
                scope.btnAdicionaNovo = false;
                scope.TextoEditModelo = "Olá {CLIENTE}";
                scope.TituloEditModelo = "";
                scope.IdEditModelo = 0;
                if (modelo !== null && modelo !== undefined) {
                    scope.IdEditModelo = modelo.Id;
                    scope.TextoEditModelo = modelo.Mensagem;
                    scope.TituloEditModelo = modelo.Titulo;
                }
            }

            scope.VoltaModelo = function () {
                scope.editModelo = false;
                scope.btnSalvar = false;
                //$scope.btnEnviar = true;
                scope.btnAdicionaNovo = true;
            }

            scope.SalvarModelo = function (modelo) {
                var objMensagem = new Object();
                console.log(modelo);
                objMensagem.Id = scope.IdEditModelo;
                objMensagem.Titulo = scope.TituloEditModelo;
                objMensagem.Mensagem = scope.TextoEditModelo;

                JSERVICE.Ajax.GetData("Dashboard/SalvarModeloMensagem", objMensagem, function (retorno) {
                    if (!JSERVICE.Criticas(retorno)) {
                        scope.modeloSelecioanado = null;
                        scope.AlteraModelo(null);
                        scope.editModelo = false;
                        scope.btnSalvar = false;
                        scope.btnEnviar = true;
                        scope.btnAdicionaNovo = true;
                        scope.TextoEditModelo = "";
                        scope.TituloEditModelo = "";
                        scope.CarregarModelosMensagem();
                        JSERVICE.Mensagem2(retorno);
                    }
                }, true);
            }

            scope.EnviarMensagemModelo = function () {
                var objMensagem = new Object();
                objMensagem.Mensagem = scope.TextoModelo;
                objMensagem.ExameId = scope.ClienteMensagemModelo.ExameId;
                objMensagem.ClienteNome = scope.ClienteMensagemModelo.ClienteNome;
                objMensagem.ClienteEmail = scope.ClienteMensagemModelo.ClienteEmail;
                objMensagem.ClienteTelefone = scope.ClienteMensagemModelo.ClienteTelefoneLinkWhats;
                objMensagem.EnvioPorEMail = scope.EnvioPorEMail;
                objMensagem.EnvioPorWhats = scope.EnvioPorWhats;

                objMensagem.Codigo = scope.ClienteMensagemModelo.Entity.CEId;
                objMensagem.PrioridadeId = scope.ClienteMensagemModelo.Entity.PrioridadeId;
                objMensagem.ModalidadeId = scope.ClienteMensagemModelo.Entity.MId;
                objMensagem.CodigoImovel = scope.ClienteMensagemModelo.Entity.CodigoImovel;

                JSERVICE.Ajax.GetData("Dashboard/EnviarMensagemModelo", objMensagem, function (retorno) {
                    if (!JSERVICE.Criticas(retorno)) {
                        if (objMensagem.EnvioPorWhats) {
                            window.open(retorno.Data, "_blank");
                        }
                        $("#modalTemplateSMSWhatsMail").modal("hide");
                        JSERVICE.Mensagem3(retorno);
                    }
                }, true);

            }

            scope.CarregarModelosMensagem = function () {
                JSERVICE.Ajax.GetData("Dashboard/CarregarModelosMensagem", {}, function (retorno) {
                    scope.modelos = retorno.Data.Modelos;
                    scope.EmailTo = retorno.Data.EmailTo;
                    scope.$apply();
                }, true);
            }

            scope.CarregarModelosMensagem();

        }
    };
});


PORTALVETAPP.directive('sortLabel', function () {
    return {
        restrict: 'E',
        replace: false,
        transclude: true,
        template: '{{text}}<span ng-click="order()" style="float:right; display: {{display}};"><i class="fa fa-arrows-v col-order" style="cursor: pointer; color: #ccc;"></i></span>',
        scope: {
            text: '@',
            showOrderColumns: '=',
            fieldNumber: '@'
        },
        link: function (scope, elem, attrs) {
            scope.linkColor = "#ccc";
            scope.orderTipo = "DESC";
            scope.order = function () {
                $(".col-order").css('color', "#ccc");
                $(elem).find(".col-order").css('color', "#000");
                if (scope.orderTipo === "DESC") {
                    scope.orderTipo = "ASC";
                }
                else {
                    scope.orderTipo = "DESC";
                }
                scope.$parent.filtro.orderfieldNumber = scope.fieldNumber;
                scope.$parent.filtro.order = scope.orderTipo;
                scope.$parent.clickOrderColumns();
                console.log("fieldNumber:", scope.fieldNumber);
                console.log("order:", scope.orderTipo);
            }
            scope.$watch('showOrderColumns', function () {
                scope.display = "none";
                if (scope.showOrderColumns) {
                    scope.display = "block";
                }
            });
        }
    };
});

PORTALVETAPP.directive('comboPretencao', function () {
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        template:
            '<select class="manual select2-no-search-default">' +
            //'<option ng-repeat="item in ngDataSource" value="{{item.Value}}">{{item.Text}}</option>' +
            '<option ng-repeat="item in itens" value="{{item.Value}}">{{item.Text}}</option>' +
            '</select>',
        scope: {
            ngModel: '=',
            //ngDataSource: '=',
            id: '@'
        },
        link: function (scope, elem, attrs) {
            scope.itens = [];
            scope.itens.push({ "Value": "", "Text": "Carregando..." });
            JSERVICE.Ajax.GetData("BaseAdmin" + "/CarregaComboPretencao", {}, function (retorno) {
                if (scope.ngModel === undefined) {
                    scope.ngModel = retorno.Data[0].Value;
                }
                scope.itens = [];
                scope.itens = retorno.Data;
                scope.$apply();
            }, true);
        }
    };
});

PORTALVETAPP.directive('sohPaginacao', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: JSERVICE.rootApplication + 'scripts/_controls/paginacao.html',
        link: function (scope, elem, attrs) {

            scope.pager = new Object();
            scope.pager.callback = null;
            scope.pager.pageIndex = 1;
            scope.pager.pageSize = 0;

            scope.pager.build = function (retorno) {
                scope.pageTotal = retorno.PageTotal;
                if (scope.pageTotal > 0) {
                    scope.paginaCorrente = retorno.PageIndex;
                    scope.pager.pageIndex = retorno.PageIndex;
                    scope.pgs = retorno.PageTotal / retorno.PageSize;
                    scope.exibindoDe = ((retorno.PageIndex - 1) * retorno.PageSize);
                    scope.exibindoAte = scope.exibindoDe + retorno.PageSize < retorno.PageTotal ? scope.exibindoDe + retorno.PageSize : retorno.PageTotal;
                    scope.exibindoDe += 1;
                    if ((retorno.PageTotal % retorno.PageSize) > 0) {
                        scope.pgs++;
                    }
                    scope.pgs = parseInt(scope.pgs);
                    for (var i = 1; i <= scope.pgs; i++) {
                        scope.paginas.push(i);
                    }
                }
            };

            scope.pager.AlteraPagina = function (pagina) {
                scope.paginaCorrente = parseInt(pagina);
                scope.pager.pageIndex = scope.paginaCorrente;
                if (angular.isFunction(scope.pager.callback)) {
                    scope.pager.callback();
                }
            };
        }
    };
});

PORTALVETAPP.controller('MasterController', function ($scope) {

    $scope.IsAdminDashboard = $("#hdnAdminDashboard").val() === "true";
    $scope.ControllerName = $("#hdnControllerName").val();
    $scope.AreaName = $("#hdnAreaName").val();

    $scope.lembreteTotal = 0;
    $scope.lembreteVisibleStyle = "display:none;";
    $scope.ListProfilesSelected = 0;
    $scope.ListProfiles = [];
    $scope.ProfileName = "";
    // console.log("ProfileName: 01");
    if ($.connection !== undefined && $.connection.hubUsuarios !== undefined) {

        var hub = $.connection.hubUsuarios;

        var logModuloAcoes = new Array();

        hub.client.StatusCargaZapVivareal = function (status) {
            $("#divStatusCarga").html(status);
            //$scope.listaUsuarios = usuarios;
            //$scope.$apply();
        };

        hub.client.usersOnline = function (usuarios) {
            //console.log("usersOnline:");
            //console.log(usuarios);
            $scope.listaUsuarios = usuarios;
            $scope.$apply();
        };
        // if ($scope.AreaName === "Imobiliaria" && $scope.ControllerName === "EnvioDireto") {
        //hub.client.notificacoes = function (entity) {
        //    if (parseInt($("#hdnCurrentEmpresaId").val()) === parseInt(entity.empresaid)) {
        //        var htmlItem = "";
        //        var count = 0;//entity.total;
        //        //console.log("[ " + $("#hdnCurrentEmpresaId").val() + " x " + entity.empresaid + " ] .....>" + JSON.stringify(entity));
        //        $(entity.lista).each(function (r, k) {
        //            var habilitado = true;
        //            var jaLeu = k.Leitores.indexOf($("#hdnCurrentUsuarioId").val()) > -1;
        //            //console.log("jaLeu :" + jaLeu);
        //            if ($("#hdnPerfilCodigo").val() === $("#hdnProfileIdConsultor").val() ||
        //                $("#hdnPerfilCodigo").val() === $("#hdnProfileIdConsultorTotal").val()) {
        //                if (parseInt(k.ConsultorId) !== parseInt($("#hdnCurrentUsuarioId").val())) {
        //                    habilitado = false;
        //                }
        //            }

        //            if (habilitado && !jaLeu) {
        //                count++;
        //                var url = "";
        //                if (k.Url !== "") {
        //                    url = k.Url;
        //                }
        //                htmlItem += "<a href='javascript:JSERVICE.CheckNotificacaoLeitura(\"" + url + "\"," + k.Id + "," + $("#hdnCurrentUsuarioId").val() + "," + entity.empresaid + ");' class='mess-item' title='" + k.DataCadastroStr + "'>";
        //                htmlItem += "<span class='avatar-preview avatar-preview-32'><span class='glyphicon " + k.Icone + "' style='font-size: 28px;color: silver;'></span></span>";
        //                htmlItem += "<span class='mess-item-name'>" + k.Titulo + "</span>";
        //                htmlItem += "<span class='mess-item-txt'>" + k.Texto + "</span>";
        //                htmlItem += "</a>";
        //            }
        //        });

        //        if (htmlItem !== "") {
        //            $("#ntfDropdown").show();
        //            //$("#ntfCount").hide();
        //            $("#ntfCount").show();
        //            $("#ntfCount").html(count);
        //            $("#ntfMsg").show();
        //            $("#ntfMsg").html(htmlItem);
        //        }
        //        else {
        //            $("#ntfDropdown").hide();
        //        }
        //    }
        //};
        // }
        //if ($scope.AreaName === "Proprietario" && $scope.ControllerName === "Importacao") {
        //    hub.client.statusImportacao = function (entity) {
        //        if (parseInt($("#hdnCurrentEmpresaId").val()) === parseInt(entity.empresaid)) {
        //            var msg = "Processando. <br/>" + entity.line + " / " + entity.max + "<br/> Por favor aguarde.";
        //            $("#msgLoading").html(msg);
        //        }
        //    };
        //}

        if ($scope.IsAdminDashboard) {
            hub.client.moduloName = function (modulo) {
                logModuloAcoes.push(modulo);
                logModuloAcoes.reverse();
                $scope.listaLogAcoes = logModuloAcoes;
                $scope.$apply();
            };

            hub.client.totalEmailsDia = function (total) {

                if (total !== null) {
                    $scope.dashboard.emailsDia = total;
                    $scope.$apply();
                }

            };

            hub.client.totalLeads = function (total, totalSite, totalRobo) {
                $scope.dashboard.totalLeads = total;
                $scope.dashboard.totalLeads01 = totalSite;
                $scope.dashboard.totalLeads02 = totalRobo;
                $scope.$apply();
            };

        }

        hub.client.reconectar = function () {
            $.connection.hub.start(function () {
                hub.server.connect($("#hdnUsuarioEmail").val(), $("#hdnProfileName").val(), $("#hdnEmpresaId").val());
            });
        };

        //hub.client.alertaLembrete = function (lembrete) {

        //    if (parseInt($("#hdnCurrentEmpresaId").val()) === lembrete.EmpresaId &&
        //        parseInt($("#hdnCurrentUsuarioId").val()) === lembrete.UsuarioId) {


        //        var atualizarLembrete = false;
        //        if ($scope.lembreteTotal !== lembrete.Total) {
        //            atualizarLembrete = true;
        //        }

        //        $scope.lembreteVisibleStyle = "display:none;";
        //        if (lembrete.Total > 0) {
        //            $scope.lembreteVisibleStyle = "";
        //        }

        //        $scope.lembreteTotal = lembrete.Total;



        //        if ($("#hdnControllerName").val() === "Dashboard" && atualizarLembrete) {
        //            switch ($("#hdnPerfilCodigo").val()) {
        //                case JSERVICE.GetProfileId("Imobiliaria"):
        //                case JSERVICE.GetProfileId("Consultor"):
        //                case JSERVICE.GetProfileId("ConsultorTotal"):
        //                    $scope.CarregaGridAcoes();
        //                    break;
        //            }
        //        }

        //        $scope.$apply();
        //    }
        //};

        //hub.client.alertaAvisoRodizio = function (aviso) {
        //    if (parseInt($("#hdnCurrentEmpresaId").val()) === aviso.ID
        //        && $("#hdnPerfilCodigo").val() === JSERVICE.GetProfileId("Imobiliaria") &&
        //        $("#hdnVisualizarProximoConsultor").val() === "1") {
        //        console.log("ACESSOU");
        //        var consultorLocacao = "";
        //        var consultorVendas = "";

        //        if (aviso.Data.ConsultorIdVendas > 0) {
        //            consultorVendas = "<div style='color:midnightblue'><small style='font-size:95%'><strong>" + aviso.Data.ConsultorNomeVendas + "</strong> - <small>VENDAS</small><br/>(" + aviso.Data.ConsultorEmailVendas + ")</small></div>";
        //        }
        //        if (aviso.Data.ConsultorIdLocacao > 0) {
        //            consultorLocacao = "<div style='color:midnightblue'><small style='font-size:95%'><strong>" + aviso.Data.ConsultorNomeLocacao + "</strong> - <small>LOCAÇÃO</small><br/>(" + aviso.Data.ConsultorEmailLocacao + ")</small></div>";
        //        }

        //        if (consultorLocacao !== "") {
        //            $("#topAcoesConsultor").show();
        //        }

        //        if (consultorVendas !== "") {
        //            $("#topAcoesConsultor").show();
        //        }

        //        var htmlArea = "<header class='card-header'>Próximo corretor/consultor a receber Lead de Rodízio</header><div style='margin:-10px 0px -10px 0px'><p class='card-text'><center><table border='0' ><tr><td></td><td style='font-size: 14px;'></td></tr><tr><td></td><td><table><tr><td  style='font-size: 14px;padding-right: 30px;'>" + consultorLocacao + "</td><td  style='font-size: 14px;'>" + consultorVendas + "</td></tr></table></td></tr></table></center></p></div>";


        //        $("#topAcoesConsultor").html(htmlArea);
        //    }
        //};


        hub.client.alertaSMS = function (sms) {
            console.log("....[alertaSMS]:" + JSON.stringify(sms));
            if (parseInt($("#hdnCurrentEmpresaId").val()) === sms.EmpresaId) {

                //console.log("SMS: " + $scope.smsTotal + "  ? " + sms.Total);

                $scope.smsVisibleStyle = "display:none;";
                if (sms.Total > 0) {
                    $scope.smsVisibleStyle = "";
                }

                $scope.smsTotal = sms.Total;
                $scope.smsPercentual = sms.Percentual;
                $scope.$apply();
                //  console.log("S");
            }
            //else {
            //  console.log("N");
            //}
        };


        $.connection.hub.start(function () {
            //console.log("Conexão aberta.")
            //console.log("connectionId = " + this.connection.id);
            hub.server.connect($("#hdnUsuarioEmail").val(), $("#hdnProfileName").val(), $("#hdnEmpresaId").val());
        });

    }

    if ($("#hdnControllerName").val() === "Dashboard") {

        $(document).ajaxStart(function () {
            JSERVICE.Loading(true);
        }).ajaxStop(function () {
            $.unblockUI();
        });


        //  MODULO.scope = $scope;
        /*
        $scope.CarregaReports = function () {
            JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/ReportAdmin", {}, function (retorno) {
                $scope.dashboard = retorno.Data;
                $scope.$apply();
                //JSERVICE.SetOdometer(document.getElementById('divTotalClientes'), $scope.dashboard.totalClientes);
                //JSERVICE.SetOdometer(document.getElementById('divTotalEnvelopesEnviados'), $scope.dashboard.envelopesEnviados);
                //JSERVICE.SetOdometer(document.getElementById('divTotalLeads'), $scope.dashboard.totalLeads);
                //JSERVICE.SetOdometer(document.getElementById('divTotalEmailsGeral'), $scope.dashboard.emailTotalGeral);
                //JSERVICE.SetOdometer(document.getElementById('divTotalEmailDia'), $scope.dashboard.emailsDia);
                //JSERVICE.SetOdometer(document.getElementById('divFCAnalise'), $scope.dashboard.FCAnaliseNoMes);
                //JSERVICE.SetOdometer(document.getElementById('divEmpresasAtividadePeriodo'), $scope.dashboard.empresasAtividadePeriodo);
            }, true);
        };

        $scope.CarregaReportSMS = function () {
            JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/ReportSMS", {}, function (retorno) {
                $scope.listaSMS = retorno.Data;
                $scope.$apply();
            }, true);
        };


        if ($("#hdnIsAdmin").val() === "1") {
            $scope.CarregaReports();
            $scope.CarregaReportSMS();
        }

        $scope.RedirectItem = function (obj) {
            var url = "";

            switch (obj.Acao) {
                case "Proposta":
                    if (obj.Modulo === "LOCACAO") {
                        url = "Locacoes/PropostaLocacao?mail=" + obj.ClienteEmail + "&mod=" + obj.Modulo;
                    }
                    if (obj.Modulo === "VENDAS") {
                        url = "Vendas/PropostaVenda?mail=" + obj.ClienteEmail + "&mod=" + obj.Modulo;
                    }
                    break;

                case "Lead":
                    url = "Cliente?mail=" + obj.ClienteEmail + "&mod=" + obj.Modulo;
                    break;

                case "Visita":
                    url = "Visita?mail=" + obj.ClienteEmail + "&mod=" + obj.Modulo;
                    break;
                case "Modalidade":
                    switch (obj.ModalidadeTipoId) {
                        case 2: //FIANÇA = 2,
                            url = "Analise/Fianca/Save?id=" + obj.MId;
                            break;
                        case 3://FIADOR = 3,
                            url = "Analise/Fiador/Save?id=" + obj.MId;
                            break;
                        case 4: //TITULO = 4,
                            url = "Analise/TituloCapitalizacao/Save?id=" + obj.MId;
                            break;
                        case 5: //CARTAFIANCAEMPRESARIAL = 5,
                            url = "Analise/CartaFiancaEmpresarial/Save?id=" + obj.MId;
                            break;
                        case 6: //CAUCAO = 6,
                            url = "Analise/Caucao/Save?id=" + obj.MId;
                            break;
                        case 7: //CREDPAGO = 7,
                            url = "Analise/CredPago/Save?id=" + obj.MId;
                            break;
                        case 8: //GARANTIAHIPOTECARIA = 8,
                            url = "Analise/GarantiaHipotecaria/Save?id=" + obj.MId;
                            break;
                        case 9: //SEMGARANTIA = 9,
                            url = "Analise/SemGarantir/Save?id=" + obj.MId;
                            break;
                    }
                    break;
            }

            if (url !== "") {
                window.location.href = JSERVICE.rootApplication + "/" + url;
            }
        };
        $scope.AtribuicaoDash = function (obj) {

            JSERVICE.AlterarConsultorDash(obj.ClienteEmail, obj.Modulo);
        };

        $scope.CarregaGridAcoes = function () {

            var jsonData = new Array();
            jsonData.push({ "name": "drpFilterTipo", "value": $("#drpFilterTipo").val() });
            jsonData.push({ "name": "txtFilterDtInicio", "value": $("#txtFilterDtInicio").val() });
            jsonData.push({ "name": "txtFilterDtFim", "value": $("#txtFilterDtFim").val() });
            jsonData.push({ "name": "txtFilterNome", "value": $("#txtFilterNome").val() });
            jsonData.push({ "name": "txtFilterEmail", "value": $("#txtFilterEmail").val() });

            if ($("#hdnPerfilCodigo").val() === $("#hdnProfileIdImobiliaria").val()) {
                jsonData.push({ "name": "drpFilterConsultor", "value": $("#drpFilterConsultor").val() });
            }
            else {
                jsonData.push({ "name": "drpFilterConsultor", "value": "" });
            }

            MODULO.PageIndex = 1;
            jsonData.push({ "name": "txtFilterTelefone", "value": $("#txtFilterTelefone").val() });
            jsonData.push({ "name": "txtFilterCodigo", "value": $("#txtFilterCodigo").val() });
            jsonData.push({ "name": "drpFilterPrioridade", "value": $("#drpFilterPrioridade").val() });
            jsonData.push({ "name": "drpFilterRespAdm", "value": $("#drpFilterRespAdm").val() });
            jsonData.push({ "name": "pageIndex", "value": MODULO.PageIndex });
            jsonData.push({ "name": "pageSize", "value": MODULO.PageSize });
            $scope.ListAcoes(jsonData);

        };

        $scope.PushEmpresa = function (eid) {

            if ($("#hdnVisualizarProximoConsultor").val() === "1") {
                JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/PushEmpresa", { "empresaid": eid }, function (retorno) {

                }, true);
            }
        };
        $scope.ListAcoes = function (json) {

            JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/List", json, function (retorno) {
                // HOTFIX: CLIENTE "BROKER UP = 243" NAO QUER HABILITAR PARA CONSULTORES (by MIGUEL)
                if ($("#hdnEmpresaId").val() === "243") {
                    if ($("#hdnPerfilCodigo").val() === JSERVICE.GetProfileId("Consultor") ||
                        $("#hdnPerfilCodigo").val() === JSERVICE.GetProfileId("ConsultorTotal")) {

                        $(retorno.Data).each(function (t, y) {
                            y.FluxoConsultor = false;
                        });
                    }
                }

                $scope.acoes = retorno.Data;
                $scope.$apply();

                JSERVICE.BuildPagination(retorno);

                $("#areaGrid").show();


                $scope.PushEmpresa($("#hdnEmpresaId").val());

            }, true);
        };

 */
    }



    $scope.CarregaListProfiles = function () {

        var json = new Array();
        var usuarioid = $("#hdnCurrentUsuarioId").val();
        var pid = parseInt($("#hdnPerfilCodigo").val());

        json.push({ "name": "uid", "value": usuarioid });
        
        JSERVICE.Ajax.GetData("/Dashboard/ListProfiles", json, function (retorno) {
            $scope.ListProfilesSelected = pid;
            $scope.ListProfiles = retorno.Data;
            $(retorno.Data).each(function (h, k) {
                if (k.Id === pid) {
                    $scope.ProfileName = k.Nome;
                    //console.log("ProfileName: 02");
                }
            });
            $scope.$apply();
            //console.log($scope.ProfileName);
        }, true);
    };

    $scope.CarregaListProfiles();



    JSERVICE.scope = $scope;
});


PORTALVETAPP.controller('clienteController', function ($scope, $timeout) {

    //$(document).ajaxStart(function () {
    //    JSERVICE.Loading(true);
    //}).ajaxStop(function () {
    //    $.unblockUI();
    //});

    //MODULO.scope = $scope;

    //$scope.filtro = new Object();
    //$scope.clientes = [];
    //$scope.paginas = [];
    //$scope.paginaCorrente = 1;
    //$scope.showOrderColumns = false;

    $scope.AbreGerenciamentoMensagens = function (obj) {
       // alert("AbreGerenciamentoMensagens: " + JSON.stringify(obj));
        // alert(obj.TelefoneCliente);
        var abrirModal = true;
        var text = "";
        if (obj.TelefoneCliente === "" || obj.EmailCliente === "") {
     
            if (obj.TelefoneCliente === "" && obj.EmailCliente === "") {
                text = "Telefone e E-mail não encontrados!";
                $("#chkEnviarWhats").hide();
                $("#chkEnviarEmail").hide();
                abrirModal = false;
            }
            else if (obj.TelefoneCliente === "") {
                text = "Telefone não encontrado!";
                $("#chkEnviarWhats").hide();
                $("#chkEnviarEmail").show();
            }
            else if (obj.EmailCliente === "") {
                text = "E-mail não encontrado!";
                $("#chkEnviarEmail").hide();
                $("#chkEnviarWhats").show();
            }

            if (abrirModal) {
                Lobibox.notify('warning', {
                    size: 'mini',
                    msg: text
                });
            }

        }
        else {
            $("#chkEnviarWhats").show();
            $("#chkEnviarEmail").show();
        }
        if (abrirModal) {
            var entity = new Object();
            entity.ClienteNome = obj.NomeCliente;
            entity.ClienteEmail = obj.EmailCliente;
            entity.ClienteTelefone = obj.TelefoneCliente;
            entity.ClienteTelefoneLinkWhats = obj.TelefoneCliente;
            entity.DataExameHH = obj.DataExameHH;
            entity.DataExameMM = obj.DataExameMM;
            entity.DataExameFmt = obj.DataExameFmt;
            entity.ExameId = obj.Id;
            entity.Menu = "CLIENTES";
            $scope.templateMesagem.openModal(entity);
        }
        else {
            Lobibox.notify('error', {
                size: 'mini',
                msg: text
            });
        }
    };
    $scope.AbreGerenciamentoMensagensLdr = function (obj) {
        //alert("AbreGerenciamentoMensagensLdr: " + JSON.stringify(obj));
        // alert(obj.TelefoneLaudador);
        var abrirModal = true;
        var text = "";
        if (obj.TelefoneLaudador === "" || obj.EmailLaudador === "") {
         
            if (obj.TelefoneLaudador === "" && obj.EmailLaudador === "") {
                text = "Telefone e E-mail não encontrados!";
                $("#chkEnviarWhats").hide();
                $("#chkEnviarEmail").hide();
                abrirModal = false;
            }
            else if (obj.TelefoneLaudador === "") {
                text = "Telefone não encontrado!";
                $("#chkEnviarWhats").hide();
                $("#chkEnviarEmail").show();
            }
            else if (obj.EmailLaudador === "") {
                text = "E-mail não encontrado!";
                $("#chkEnviarEmail").hide();
                $("#chkEnviarWhats").show();
            }
            if (abrirModal) {
                Lobibox.notify('warning', {
                    size: 'mini',
                    msg: text
                });
            }
        }
        else {
            $("#chkEnviarWhats").show();
            $("#chkEnviarEmail").show();
        }

        if (abrirModal) {
            var entity = new Object();
            entity.ClienteNome = obj.NomeLaudador;
            entity.ClienteEmail = obj.EmailLaudador;
            entity.ClienteTelefone = obj.TelefoneLaudador;
            entity.ClienteTelefoneLinkWhats = obj.TelefoneLaudador;
            entity.DataExameHH = obj.DataExameHH;
            entity.DataExameMM = obj.DataExameMM;
            entity.DataExameFmt = obj.DataExameFmt;
            entity.ExameId = obj.Id;
            entity.Menu = "CLIENTES";
            $scope.templateMesagem.openModal(entity);
        }
        else {
            Lobibox.notify('error', {
                size: 'mini',
                msg: text
            });
        }
    };



});
$(document).ready(function () {

    //JSERVICE.AplicaMascaras();

    if ($("#hdnPerfilCodigo").val() === $("#hdnProfileIdImobiliaria").val() ||
        $("#hdnPerfilCodigo").val() === $("#hdnProfileIdConsultor").val() ||
        $("#hdnPerfilCodigo").val() === $("#hdnProfileIdConsultorTotal").val()) {
        if (parseInt($("#hdnListEmpresaCount").val()) === 0) {
            $(".side-menu, .mobile-menu-left-overlay, .container-fluid, .md-chat-widget-wrapper").hide();
            $("#modalReleaseNotes").modal("hide");
            swal({
                title: "Atenção",
                text: "Sem acesso, entre em contato com sua imobiliária :(",
                type: "error",
                confirmButtonClass: "btn-danger"
            });
        }
    }
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

    $("#chkLembrete").change(function () {
        if ($("#chkLembrete").is(":checked")) {
            JSERVICE.RefreshDtLembrete();
            $("#areaDtLembrete").show();
        } else {
            $("#areaDtLembrete").hide();
        }
    });

    if ($("#hdnPerfilCodigo").val() !== undefined) {
        switch ($("#hdnPerfilCodigo").val()) {
            case JSERVICE.GetProfileId("Imobiliaria"):
            case JSERVICE.GetProfileId("Consultor"):
            case JSERVICE.GetProfileId("ConsultorTotal"):
                JSERVICE.GetCountMenuLembrete();
                break;
        }
    }

    if ($("#gridAssinantesGlobal").length > 0) {
        $("#gridAssinantesGlobal tbody").sortable({
            cursor: "move",
            placeholder: "sortable-placeholder",
            stop: function (event, ui) {
                // console.log(JSERVICE.AssinanteModuloId);
                // console.log(JSERVICE.AssinanteId);
                JSERVICE.EventUpdateGrid(JSERVICE.AssinanteId, JSERVICE.AssinanteModuloId);
            },
            helper: function (e, tr) {
                var $originals = tr.children();
                var $helper = tr.clone();
                $helper.children().each(function (index) {
                    // Set helper cell sizes to match the original sizes
                    $(this).width($originals.eq(index).width());
                });
                return $helper;
            }
        }).disableSelection();
    }

    $("#cmbCorrentorResponsavel").change(function () {
        if ($(this).val() !== "") {
            JSERVICE.CarregarFotoCorretor($(this).val());
        }
    });


    $("#chkEnvioAutomaticoProp").click(function () {
        var checked = "0";
        if ($(this).is(":checked")) {
            checked = "1";
        }
        var empresaid = $("#hdnCurrentEmpresaId").val();
        JSERVICE.Ajax.GetData("GenericControl/AtualizarPropAutomatico", { "empresaid": empresaid, "checked": checked }, function (retorno) {
            if (retorno.Data) {
                JSERVICE.Mensagem("Envio automático atualizado!", "", "success");
            }
        }, true);


    });

    $("#chkEnvioAutomaticoProp").change(function () {
        if ($(this).val() !== "") {
            JSERVICE.CarregarFotoCorretor($(this).val());
        }
    });



    $("#drpModeloShare").change(function () {
        if ($("#drpModeloShare").val() !== "") {
            JSERVICE.CarregarModeloSetup($("#drpModeloShare").val());
        } else {
            $("#areaTemplateSetupShare").hide();
        }
    });


    $("#btnConfirmSelecionarShare").click(function () {

        if ($("#drpModeloShare").val() !== "") {
            $("#drpModeloShare").next().removeClass("form-control-error");
            JSERVICE.ConfirmarModelo($("#drpModeloShare").val());
        } else {
            $("#drpModeloShare").next().addClass("form-control-error");
            JSERVICE.Mensagem("Selecione um modelo!", "Atenção", "error");
        }


    });


    window.setTimeout(function () {

        if ($("#cmbCorrentorResponsavel").length) {
            if ($("#cmbCorrentorResponsavel").val() !== "") {
                JSERVICE.CarregarFotoCorretor($("#cmbCorrentorResponsavel").val());
            }
        }

        //JSERVICE.VerificarNotificacao();
        //JSERVICE.ListProfiles();
    }, 100);

    window.setTimeout(function () {
        //JSERVICE.VerificarNotificacao();
    }, 2000);
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
    scope: null,
    VerificarNotificacao: function () {
        var eid = $("#hdnCurrentEmpresaId").val();
        if (parseInt(eid) > 0) {
            var invokeUrl = JSERVICE.rootApplication + "/api/SignaIR/Notificacoes/" + eid;
            JSERVICE.stopAutoLoading = true;
            JSERVICE.Ajax.GetDataExt(invokeUrl, {}, function (retorno) {
                console.log(retorno);
                JSERVICE.stopAutoLoading = false;
            }, true);
        }
    },
    SetOdometer: function (el, value) {
        od = new Odometer({
            el: el,
            value: 0,
            // Any option (other than auto and selector) can be passed in here
            format: '',
            theme: 'default'
        });

        od.update(value);
    },
    AlterarCodigoImovel: function (id, tid) {
        //alert();

        $("#hdnCodRefModuloId").val(id);
        $("#hdnCodRefModuloTipoId").val(tid);
        $("#lblCodigoRefImovel").html($("#txtCodigo_Imovel").val());
        $("#txtCodigoRefImovel").val("");
        $("#modalAlterarCodigoImovel").modal("show");
    },

    AlterarCodigoAlternativo: function (id, tid) {
        //alert();

        $("#hdnCodRefAltModuloId").val(id);
        $("#hdnCodRefAltModuloTipoId").val(tid);
        $("#lblCodigoRefImovelAlt").html($("#txtCodigoAlt_Imovel").val());
        $("#txtCodigoRefImovelAlt").val("");

        $("#modalAlterarCodigoAlternativo").modal("show");
    },


    CheckNotificacaoLeitura: function (url, id, uid, eid) {

        JSERVICE.Ajax.GetData("GenericControl/NotificacaoLeitura",
            {
                id: id,
                uid: uid,
                eid: eid
            }, function (retorno) {
                if (url !== "") {
                    window.location.href = JSERVICE.rootApplication + "/" + url;
                }
            }, true);

    },
    AtualizarCodigoAltImovelSave: function () {

        //$("#hdnCodRefModuloId").val()
        //$("#hdnCodRefModuloTipoId").val()
        var erro = false;
        //  alert($("#lblCodigoRefImovel").html() + " : " + $("#txtCodigoRefImovel").val());
        if ($("#txtCodigoRefImovelAlt").val() === "") {
            erro = true;
            $("#txtCodigoRefImovelAlt").addClass("form-control-error");
            JSERVICE.Mensagem("Informe um código alternativo", "Atenção", "error");
        }
        else if ($("#txtCodigoRefImovelAlt").val() === $("#lblCodigoRefImovel").html()) {
            erro = true;
            $("#txtCodigoRefImovelAlt").addClass("form-control-error");
            JSERVICE.Mensagem("Informe um código alternativo diferente do atual", "Atenção", "error");
        }

        if (erro === false) {

            var json = {
                moduloid: $("#hdnCodRefAltModuloId").val(),
                tipomoduloid: $("#hdnCodRefAltModuloTipoId").val(),
                empresaid: $("#hdnEmpresaId").val(),
                codigo: $("#txtCodigoRefImovelAlt").val(),
                codigoantigo: $("#lblCodigoRefImovelAlt").html(),
            };
            JSERVICE.Ajax.GetData("GenericControl/AtualizarCodigoAltImovelSave",
                json,
                function (retorno) {
                    if (retorno.Data !== null && retorno.Data === true) {
                        $("#txtCodigoAlt_Imovel").val($("#txtCodigoRefImovelAlt").val());
                        JSERVICE.Mensagem("Código atualizado com sucesso!", "Aviso", "success");
                        $("#modalAlterarCodigoAlternativo").modal("hide");

                        JSERVICE.HistoricoAtividades($("#hdnCodRefAltModuloId").val(),
                            $("#hdnCodRefAltModuloTipoId").val(),
                            $("#hdnEmpresaId").val());
                    }
                    else {
                        JSERVICE.Mensagem("Problemas para atualizar registro!", "Atenção", "error");

                    }

                }, true);



        }


    },
    AtualizarCodigoImovelSave: function () {

        //$("#hdnCodRefModuloId").val()
        //$("#hdnCodRefModuloTipoId").val()
        var erro = false;
        //  alert($("#lblCodigoRefImovel").html() + " : " + $("#txtCodigoRefImovel").val());
        if ($("#txtCodigoRefImovel").val() === "") {
            erro = true;
            $("#txtCodigoRefImovel").addClass("form-control-error");
            JSERVICE.Mensagem("Informe um código de imóvel", "Atenção", "error");
        }
        else if ($("#txtCodigoRefImovel").val() === $("#lblCodigoRefImovel").html()) {
            erro = true;
            $("#txtCodigoRefImovel").addClass("form-control-error");
            JSERVICE.Mensagem("Informe um código de imóvel diferente do atual", "Atenção", "error");
        }

        if (erro === false) {

            var json = {
                moduloid: $("#hdnCodRefModuloId").val(),
                tipomoduloid: $("#hdnCodRefModuloTipoId").val(),
                empresaid: $("#hdnEmpresaId").val(),
                codigo: $("#txtCodigoRefImovel").val(),
                codigoantigo: $("#lblCodigoRefImovel").html(),
            };
            JSERVICE.Ajax.GetData("GenericControl/AtualizarCodigoImovelSave",
                json,
                function (retorno) {
                    if (retorno.Data !== null && retorno.Data === true) {
                        $("#txtCodigo_Imovel").val($("#txtCodigoRefImovel").val());
                        JSERVICE.Mensagem("Imóvel atualizado com sucesso!", "Aviso", "success");
                        $("#modalAlterarCodigoImovel").modal("hide");

                        JSERVICE.HistoricoAtividades($("#hdnCodRefModuloId").val(),
                            $("#hdnCodRefModuloTipoId").val(),
                            $("#hdnEmpresaId").val());
                    }
                    else {
                        JSERVICE.Mensagem("Problemas para atualizar registro!", "Atenção", "error");

                    }

                }, true);



        }


    },
    ConfirmarModelo: function (code) {

        var text = $("#editorCorpoShare").val();
        var tid = $("#hdnTipoModalidadeShare").val();
        var email = $("#hdnClienteEmailShare").val();
        var controlid = $("#hdnFileControlIdShare").val();

        var json = {
            moduloid: $("#hdnId").val(),
            empresaid: $("#hdnEmpresaId").val(),
            tipomoduloid: tid,
            email: email,
            code: code,
            texto: text
        };

        JSERVICE.Ajax.GetData("GenericControl/ConfirmarModeloShare",
            json,
            function (retorno) {
                if (retorno.Data !== null) {

                    var htmlImageMulti = JSERVICE.CreateListShare(controlid, retorno.Data.listImages);
                    $("ul.ulListContratoFinal").html(htmlImageMulti);
                    JSERVICE.Mensagem("Modelo selecionado com sucesso!", "", "success");
                    $("#modalModeloShare").modal("hide");

                }
                else {
                    JSERVICE.Mensagem("Problemas para gerar contrato!", "Atenção", "error");

                }

            }, true);

    },

    CreateListShare: function (CONTROLID, arrObj) {
        var htmlLine = "";
        var ctFoto = 0;
        if (arrObj !== null) {
            for (var i = 0; i < arrObj.length; i++) {
                ctFoto = i + 1;
                htmlLine = htmlLine + "<li class='list-group-item list-group-item-warning' style=''>";
                htmlLine = htmlLine + "<div class='controls'>&nbsp;";
                htmlLine = htmlLine + "<a onclick='JSERVICE.OpenViewer(\"" + arrObj[i].Path + arrObj[i].Name + "\")' href='javascript:;'  class='btn btn-inline btn-secondary'><span class='glyphicon glyphicon-save-file' aria-hidden='true' ></span>Visualizar</a>";
                //htmlLine = htmlLine + "&nbsp;<button type='button' class='btn btn-icon btn-danger' onclick='this.RemoveFile(" + i + ")'><small>Remover</small></button>";
                htmlLine = htmlLine + "&nbsp;<button type='button' class='btn btn-inline btn-danger btnFileRemove' onclick='FILECONTROL" + CONTROLID + ".RemoveFile(" + i + ")'><span class='glyphicon glyphicon-floppy-remove' aria-hidden='true'></span> Excluir</button>";

                htmlLine = htmlLine + "</div>";
                htmlLine = htmlLine + "</li>";
            }
        }
        return htmlLine;
    },

    customItemRendererShare: function (item) {
        const itemElement = document.createElement('span');

        itemElement.classList.add('custom-item');
        itemElement.id = `mention-list-item-id-${item.userId}`;
        itemElement.textContent = `${item.name} `;

        const usernameElement = document.createElement('span');

        usernameElement.classList.add('custom-item-username');
        usernameElement.textContent = item.id;

        itemElement.appendChild(usernameElement);

        return itemElement;
    },

    getFeedItemsShare: function (queryText) {
        return new Promise(resolve => {
            setTimeout(() => {
                const itemsToDisplay = variaveis
                    .filter(isItemMatching)
                    .slice(0, 10);

                resolve(itemsToDisplay);
            }, 100);
        });

        function isItemMatching(item) {
            const searchString = queryText.toLowerCase();

            return (
                item.name.toLowerCase().includes(searchString) ||
                item.id.toLowerCase().includes(searchString)
            );
        }
    },
    BuildTextArea: function (id, data) {

        $('#areaCampoShare').html("");
        $('#areaCampoShare').append("<textarea name='editorCorpoShare' id='editorCorpoShare' class='editorCorpoShare'></textarea>");

        ClassicEditor.create(document.querySelector('.' + id), {
            mention: {
                feeds: [
                    {
                        marker: '#',
                        feed: JSERVICE.getFeedItemsShare,
                        itemRenderer: JSERVICE.customItemRendererShare
                    }
                ]
            },

            toolbar: {
                viewportTopOffset: 79,
                items: [

                    'heading',
                    '|',
                    'alignment',
                    '|',
                    'bold',
                    'italic',
                    'link',
                    'bulletedList',
                    'numberedList',
                    '|',
                    'indent',
                    'outdent',
                    '|',
                    //'imageUpload',
                    'blockQuote',
                    'insertTable',
                    'undo',
                    'redo'
                ]
            },
            language: 'pt-br',
            image: {
                toolbar: [
                    'imageTextAlternative',
                    'imageStyle:full',
                    'imageStyle:side'
                ]
            },
            table: {
                contentToolbar: [
                    'tableColumn',
                    'tableRow',
                    'mergeTableCells'
                ]
            },
            licenseKey: '',
        })
            //    .then(editor => {
            //        editor.setData(retorno.Data.ModeloCorpo);
            //    });
            .then(editor => {


                editor.setData(data);


                editor.model.document.on('change', () => {
                    $("#" + id).html(editor.getData());
                    //console.log(editor.getData());
                });
                editor.editing.view.change(writer => {
                    writer.setStyle('height', '700px', editor.editing.view.document.getRoot());
                });

                MODULO.EditorBody = editor;
            })
            .catch(error => {
                console.error('Oops, something gone wrong!');
                console.error('Please, report the following error in the https://github.com/ckeditor/ckeditor5 with the build id and the error stack trace:');
                console.warn('Build id: sb5wsw8zunnh-8o65j7c6blw0');
                console.error(error);
            });


    },
    CarregarUltimoModelo: function (id) {

        JSERVICE.Ajax.GetData("GenericControl/CarregarUltimoModelo",
            {
                id: id,
            }, function (retorno) {
                if (ClassicEditor.Data !== null) {
                    JSERVICE.BuildTextArea("editorCorpoShare", retorno.Data.Data.ModeloCorpo);
                    $("#editorCorpoShare").html(retorno.Data.Data.ModeloCorpo);
                    $("#btnCarregarUltimoShare").hide();
                }
            }, true);
    },
    CarregarModeloSetup: function (id) {

        var tid = $("#hdnTipoModalidadeShare").val();
        JSERVICE.Ajax.GetData("GenericControl/CarregarModeloContratoShare",
            {
                id: id,
                tipomoduloid: tid,
                empresaid: $("#hdnEmpresaId").val(),
            }, function (retorno) {
                if (ClassicEditor.Data !== null) {
                    JSERVICE.BuildTextArea("editorCorpoShare", retorno.Data.Data.ModeloCorpo);
                    $("#editorCorpoShare").html(retorno.Data.Data.ModeloCorpo);
                    if (retorno.Data.DataVersao !== null && retorno.Data.DataVersao.Id > 0) {
                        $("#btnCarregarUltimoShare").show();
                        $("#btnCarregarUltimoShare").attr("onclick", "JSERVICE.CarregarUltimoModelo(" + retorno.Data.DataVersao.Id + ")");
                    }
                    else {
                        $("#btnCarregarUltimoShare").hide();
                    }
                }
            }, true);

        $("#areaTemplateSetupShare").show();
    },

    AbrirModeloContrato: function (obj, index, tid, mid) {

        // alert($(obj).attr("tp"));

        var max = $("#listImages" + index + " li").length;

        if (max === 0) {
            $("#areaTemplateSetupShare").hide();
            $("#hdnTipoModalidadeShare").val(tid);
            $("#hdnFileControlIdShare").val(index);
            $("#hdnClienteEmailShare").val($("#hdnClienteEmail").val());
            var jsonData = {
                tipopessoa: $(obj).attr("tp"),
                empresaid: $("#hdnEmpresaId").val(),
                tid: tid,
                locatarioct: $("#hdnLocatarioCount").val(),
                fiadorct: $("#hdnFiadorCount").val()
            };

            //alert(JSON.stringify(jsonData))

            JSERVICE.Ajax.GetData("GenericControl/AbrirModeloContratosShare", jsonData, function (retorno) {
                if (retorno.Data !== null) {
                    var html = "<option value=''>:: Selecione ::</option>";
                    $(retorno.Data.List).each(function (e, k) {
                        html = html + "<option value='" + k.ID + "'>" + k.NOME + "</option>";
                    });

                    $("#drpModeloShare").html(html);
                }
                $("#modalModeloShare").modal("show");
            }, true);

        }
        else {
            JSERVICE.Mensagem("Já existe documento anexado!", "Atenção", "error");
        }
    },
    openHelpChat: function () {
        //alert($("#md-app-widget").children().attr("class"));

        if ($("#md-app-widget").children().hasClass("minimized")) {
            movideskChatWidgetChangeWindowState('maximized');
        } else {
            movideskChatWidgetChangeWindowState('minimized');
        }
    },
    stringToDate: function (_date, _format, _delimiter) {
        var formatLowerCase = _format.toLowerCase();
        var formatItems = formatLowerCase.split(_delimiter);
        var dateItems = _date.split(_delimiter);
        var monthIndex = formatItems.indexOf("mm");
        var dayIndex = formatItems.indexOf("dd");
        var yearIndex = formatItems.indexOf("yyyy");
        var month = parseInt(dateItems[monthIndex]);
        month -= 1;
        var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
        return formatedDate;
    },
    DialogCadastraImovel: function (codigoImovel, primeiraModal) {
        //$("#btnNegacaoCadastroImovel").unbind("click");
        //$("#btnNegacaoCadastroImovel").click(function () {
        //    $(primeiraModal).modal("show");
        //});

        if (primeiraModal !== null && primeiraModal !== undefined) {
            $(primeiraModal).modal("hide");
        }

        $("#dialogCadastraImovel").modal("show");
    },
    CarregarFotoCorretor: function (codigo) {
        JSERVICE.Ajax.GetData("GenericControl/CarregarFotoCorretor", { "codigo": codigo, "empresaid": $("#hdnCurrentEmpresaId").val() }, function (retorno) {
            if (retorno.Data !== "" && retorno.Data !== null) {
                $("#imgFotoCorretor").attr("src", "https://sohtec.com.br/upload/user/" + retorno.Data);
                $("#areaFotoCorretor").show();
            } else {
                $("#areaFotoCorretor").hide();
            }
        }, true);
    },
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
    ConfirmSetProfile: function (pid) {
        //alert($("#drpSelecioneEmpresa").val());

        if ($("#drpSelecioneEmpresa").val() === "") {

            $("#drpSelecioneEmpresa").addClass("form-control-error");
            $("#drpSelecioneEmpresa").next().addClass("form-control-error");
        }
        else {


            $("#drpSelecioneEmpresa").removeClass("form-control-error");
            $("#drpSelecioneEmpresa").next().removeClass("form-control-error");
            var json = $("form").serializeArray();
            json.push({ "name": "pid", "value": pid });
            json.push({ "name": "uid", "value": $("#hdnCurrentUsuarioId").val() });
            json.push({ "name": "eid", "value": $("#drpSelecioneEmpresa").val() });
            JSERVICE.Ajax.GetData("/Dashboard/SetProfileEmp", json, function (retorno) {


                if (retorno.Data) {
                    window.location.href = JSERVICE.rootApplication + "/Dashboard";
                }
            }, true);

        }

    },

    SetEmpresa: function (eid) {


        var json = $("form").serializeArray();
        json.push({ "name": "pid", "value": $("#hdnPerfilCodigo").val() });
        json.push({ "name": "uid", "value": $("#hdnCurrentUsuarioId").val() });
        json.push({ "name": "eid", "value": eid });

        JSERVICE.Ajax.GetData("/Dashboard/SetProfileEmp", json, function (retorno) {            
            if (retorno.Data) {
                window.location.href = JSERVICE.rootApplication + "Dashboard";
            }
        }, true);

    },
    SetProfileBKP: function (pid) {



        if ($("#hdnListEmpresa").val() !== "") {

            var jsonData = JSON.parse($("#hdnListEmpresa").val());

            if (jsonData.length > 1) {

                if (parseInt(pid) === parseInt($("#hdnProfileIdAdministrador").val())) {
                    var json = $("form").serializeArray();
                    json.push({ "name": "pid", "value": pid });
                    JSERVICE.Ajax.GetData("/Dashboard/SetProfile", json, function (retorno) {
                        if (retorno.Data) {
                            window.location.href = JSERVICE.rootApplication + "/Dashboard";
                        }
                    }, true);
                }
                else {


                    $("#drpSelecioneEmpresa").removeClass("form-control-error");
                    $("#drpSelecioneEmpresa").next().removeClass("form-control-error");
                    var htmlItem = "<option value=''>:: Selecione ::</option>";
                    $(jsonData).each(function (t, k) {
                        htmlItem += "<option value='" + k.Value + "'>" + k.Text + "</option>";
                    });
                    $("#drpSelecioneEmpresa").html(htmlItem);
                    $("#btnSelecioneEmpresa").attr("onclick", "JSERVICE.ConfirmSetProfile(" + pid + ");");
                    $("#modalSelecaoEmpresa").modal("show");
                }
            }
            else {
                if (jsonData.length === 0) {
                    JSERVICE.Mensagem("Nenhuma imobiliária vinculada!", "Atenção", "error");
                } else {
                    var json = $("form").serializeArray();
                    json.push({ "name": "pid", "value": pid });
                    JSERVICE.Ajax.GetData("/Dashboard/SetProfile", json, function (retorno) {
                        if (retorno.Data) {
                            window.location.href = JSERVICE.rootApplication + "/Dashboard";
                        }
                    }, true);
                }
            }

        }
    },


    //ListProfiles: function () {

    //    var json = $("form").serializeArray();
    //    var usuarioid = $("#hdnCurrentUsuarioId").val();
    //    var pid = parseInt($("#hdnPerfilCodigo").val());

    //    json.push({ "name": "uid", "value": usuarioid });
    //    JSERVICE.Ajax.GetData("/Dashboard/ListProfiles", json, function (retorno) {
    //        if (retorno.Data != null) {
    //            $(retorno.Data).each(function () {


    //            });
    //        }
    //    }, true);
    //},
    SetProfileAngular: function (obj) {
        var pid = $(obj).attr("data-pid");

        if ($("#hdnListEmpresa").val() !== "") {
            var jsonData = JSON.parse($("#hdnListEmpresa").val());

            if (jsonData.length === 0) {
                JSERVICE.Mensagem("Nenhuma imobiliária vinculada!", "Atenção", "error");
            } else {
                var json = $("form").serializeArray();
                json.push({ "name": "pid", "value": pid });
                JSERVICE.Ajax.GetData("/Dashboard/SetProfile", json, function (retorno) {
                    if (retorno.Data) {
                        window.location.href = JSERVICE.rootApplication + "/Dashboard";
                    }
                }, true);
            }

        }
    },
    SetProfile: function (pid) {



        if ($("#hdnListEmpresa").val() !== "") {

            var jsonData = JSON.parse($("#hdnListEmpresa").val());

            if (jsonData.length === 0) {
                JSERVICE.Mensagem("Nenhuma imobiliária vinculada!", "Atenção", "error");
            } else {
                var json = $("form").serializeArray();
                json.push({ "name": "pid", "value": pid });
                JSERVICE.Ajax.GetData("/Dashboard/SetProfile", json, function (retorno) {
                    if (retorno.Data) {
                        window.location.href = JSERVICE.rootApplication + "/Dashboard";
                    }
                }, true);
            }

        }
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
        var resp_id = $(obj).attr("data-resp-id");
        var resp_tipo = $(obj).attr("data-resp-tipo");
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

        var resp = "";

        if (resp_id > 0) {
            resp = resp_tipo + "_" + resp_id;
        }


        //$("#drpPrioridadeChange").val("");
        //$("#drpPrioridadeChange").select2();
        //$("#drpPrioridadeChange").select2().val("").trigger("change");
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

                var htmlFull = "";

                $(retorno.Data).each(function (k, e) {

                    var htmlOpt = "<span class='label label-custom label-pill icon-circle-prior-" + e.Prioridade + "'><small>" + e.PrioridadeTexto + "</small></span>";

                    htmlFull += "<div class='row'>";


                    htmlFull += "<div class='col-md-2'><small><strong>Prioridade</strong></small><br/>" + htmlOpt + "</div>";


                    htmlFull += "<div class='col-md-3'><small><strong>Usuário</strong></small><br/>";
                    htmlFull += "<span class='color-blue-grey-lighter'><small>" + e.ClienteNome + "</small></span>";
                    htmlFull += "<br/><small>" + e.ClienteEmail + "</small>";

                    if (e.RespAdmin !== null && e.RespAdmin !== "") {
                        htmlFull += "<br/><span class='color-blue-grey-lighter'><small>Resp.Admin: " + e.RespAdmin + "</small></span>";
                    }
                    htmlFull += "</div>";


                    htmlFull += "<div class='col-md-5'><small><strong>Motivo</strong></small><br/>";
                    htmlFull += "<small>";
                    htmlFull += e.Motivo;
                    htmlFull += "</small>";
                    htmlFull += "</div>";

                    htmlFull += "<div class='col-md-2'><small><strong>Criado/Alterado</strong></small><br/>";

                    if (e.AdminUserNome !== null)
                        htmlFull += "<span class='color-blue-grey-lighter'><small>" + e.AdminUserNome + "</small></span><br/>";

                    if (e.AdminUserEmail !== null)
                        htmlFull += "<small>" + e.AdminUserEmail + "</small><br/>";

                    htmlFull += "<small>" + e.DataHoraFmt + "</small>";


                    htmlFull += "</div>";


                    htmlFull += "</div>";


                    htmlFull += "<div class='row'><div class='col-md-12'><hr></div></div>";
                });

                $("#modalBodyDisparoPrioridade").html(htmlFull);

            }
        }, true);


        $("#btnConfirmPrioridade").attr("onclick", "JSERVICE.SavePrioridade(" + opt + "," + code + ",'" + acao + "', " + mid + ")");
        $("#modalPrioridadeHistorico").modal("show");


        $("#drpPrioridadeChange").val(opt).trigger('change');


        if (resp !== null && resp !== "") {
            $("#drpPrioridadeRespAdm").val(resp).trigger('change');
        } else {
            $("#drpPrioridadeRespAdm").val("").trigger('change');
        }


        //  HOTFIX: Abrir Select2 em Popup
        $('#modalPrioridadeHistorico #drpPrioridadeRespAdm').each(function () {
            //$('#modalPrioridadeHistorico .select2').each(function () {
            var $p = $(this).parent();
            $(this).select2({
                dropdownParent: $p
            });
        });

    },
    EditPrioridadeVisita: function (opt, code, mid, resp) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Visita', mid, resp);


        //  HOTFIX: Abrir Select2 em Popup
        $('#modalPrioridadeHistorico #drpPrioridadeRespAdm').each(function () {
            //$('#modalPrioridadeHistorico .select2').each(function () {
            var $p = $(this).parent();
            $(this).select2({
                dropdownParent: $p
            });
        });
    },
    EditPrioridadeModalidade: function (opt, code, mid, resp) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Modalidade', mid, resp);
    },
    EditPrioridadeLead: function (opt, code) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Lead', 0);
    },
    EditPrioridadeProposta: function (opt, code, mid, resp) {
        $("#hdnModalLembreteId").val("");
        $("#hdnModalLembreteData").val("");
        JSERVICE.EditPrioridade(opt, code, 'Proposta', mid, resp);


        //  HOTFIX: Abrir Select2 em Popup
        $('#modalPrioridadeHistorico #drpPrioridadeRespAdm').each(function () {
            //$('#modalPrioridadeHistorico .select2').each(function () {
            var $p = $(this).parent();
            $(this).select2({
                dropdownParent: $p
            });
        });
    },
    EditPrioridadeId: function (opt, code, acao, mid, id, dt) {
        $("#hdnModalLembreteId").val(id);
        $("#hdnModalLembreteData").val(dt);
        JSERVICE.EditPrioridade(opt, code, acao, mid);
    },
    EditPrioridade: function (opt, code, acao, mid, resp) {
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

        // HOTFIX: Abrir Select2 em Popup
        /*
        $('#modalPrioridadeHistorico .select2').each(function () {
            var $p = $(this).parent();
            $(this).select2({
                dropdownParent: $p
            });
        });
        */

        $("#drpPrioridadeChange").val(opt).trigger('change');


        if (resp !== null && resp !== "") {
            $("#drpPrioridadeRespAdm").val(resp).trigger('change');
        } else {
            $("#drpPrioridadeRespAdm").val("").trigger('change');
        }

        if ($("#hdnModalLembreteId").val() !== "") {
            $("#chkLembrete").attr('checked', 'checked');
            JSERVICE.RefreshDtLembrete();
            $("#txtLembreteChange").val($("#hdnModalLembreteData").val());
            $("#areaDtLembrete").show();
            //       $("#areaDtLembrete").hide();
        }
    },
    SavePrioridade: function (optold, code, acao, mid) {

        var respadmin = $("#drpPrioridadeRespAdm").val();
        var respadminName = "";

        if (respadmin !== "") {
            respadminName = $("#drpPrioridadeRespAdm option:selected").text();
        }
        //alert("ANTES:" + opt + " : " + code);
        //alert("DEPOIS:" + $("#drpPrioridadeRespAdm").val() + " : " + code);

        var opt = $("#drpPrioridadeChange").val();
        // if ($("#txtMotivoPrioridade").val() === "") {
        //    $("#txtMotivoPrioridade").addClass("form-control-error");
        // }
        //else {

        var lembreteid = $("#hdnModalLembreteId").val();

        // $("#txtMotivoPrioridade").removeClass("form-control-error");
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
            json.push({ "name": "respadmin", "value": respadmin });
            json.push({ "name": "respadminname", "value": respadminName });
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
        //}
    },
    OpenViewer: function (url) {

        window.open(JSERVICE.rootApplication + '/DocViewer?p=' + url, "_blank");

    },
    OpenViewerED: function (url) {
        var a = document.createElement('a');
        a.target = "_blank";
        a.href = url;
        a.click();

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
    ClickRegerarAssinantesED: function () {

        var json = $("form").serializeArray();
        json.push({ "name": "eid", "value": $("#hdnEmpresaId").val() });
        JSERVICE.Ajax.GetData("/GenericControl/PopularGrupoAssinantes", json, function (retorno) {

            if (retorno.Data !== null) {
                var htmlItems = "<option value=''>:: Selecione ::</option>";
                $(retorno.Data.List).each(function (k, e) {
                    htmlItems += "<option value='" + e.ID + "'>" + e.NOME + "</option>";
                });

                $("#drpShareGrupoAssinantes").html(htmlItems);
                $("#drpShareGrupoAssinantes").select2();

            }
        });
        $("#btnRegerarAssinantes").attr("onclick", "JSERVICE.ClickAssinantesRegerarED();");
        $("#modalRegerarGrupoAssinantes").modal("show");
    },
    ClickAssinantesRegerarED: function () {

        if ($("#drpShareGrupoAssinantes").val() === "") {
            $("#drpShareGrupoAssinantes").next().addClass("form-control-error");
            JSERVICE.Mensagem("Selecione um  Grupo de Assinantes!", "", "error");
        }
        else {
            $("#drpShareGrupoAssinantes").next().removeClass("form-control-error");
            JSERVICE.AssinantesRegerarED($("#drpShareGrupoAssinantes").val(), $("#hdnId").val(), $("#hdnTipoModuloId").val(), $("#hdnEmpresaId").val())
        }
    },
    MaxGridItemsED: 16,
    AssinantesRegerarED: function (grupoid, id, moduloid, empresaid) {

        JSERVICE.AssinanteModuloId = moduloid;
        JSERVICE.AssinanteId = id;
        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        //json.push({ "name": "moduloId", "value": moduloid });
        json.push({ "name": "empresaId", "value": empresaid });
        json.push({ "name": "grupoId", "value": grupoid });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesRegerarED", json, function (retorno) {

            if (retorno.Error === false) {
                $("#hdnAssinantesTotalMin").val(retorno.Data.totalMin);
                $("#hdnAssinantesTotalMax").val(retorno.Data.totalMax);
                JSERVICE.AssinantesBuildGridED(retorno.Data.list, id);//, id, moduloid);
                $("#modalRegerarGrupoAssinantes").modal("hide");

                $("#drpShareGrupoAssinantes").val("");
                $("#drpShareGrupoAssinantes").select2();
            }

        }, true);
    },

    ClickRegerarAssinantes: function () {

        var json = $("form").serializeArray();
        json.push({ "name": "eid", "value": $("#hdnEmpresaId").val() });
        JSERVICE.Ajax.GetData("/GenericControl/PopularGrupoAssinantes", json, function (retorno) {

            if (retorno.Data !== null) {
                var htmlItems = "<option value=''>:: Selecione ::</option>";
                $(retorno.Data.List).each(function (k, e) {
                    htmlItems += "<option value='" + e.ID + "'>" + e.NOME + "</option>";
                });

                $("#drpShareGrupoAssinantes").html(htmlItems);
                $("#drpShareGrupoAssinantes").select2();

                //if (code > 0) {
                //    MODULO.PopularGrupoAssinantesItens(code);
                //}
            }
        });

        $("#btnRegerarAssinantes").attr("onclick", "JSERVICE.ClickAssinantesRegerar();");
        $("#modalRegerarGrupoAssinantes").modal("show");
        //        //JSERVICE.AssinantesRegerar($("#hdnId").val(), $("#hdnTipoModuloId").val(), $("#hdnEmpresaId").val());
    },
    ClickAssinantesRegerar: function () {

        if ($("#drpShareGrupoAssinantes").val() === "") {
            $("#drpShareGrupoAssinantes").next().addClass("form-control-error");
            JSERVICE.Mensagem("Selecione um  Grupo de Assinantes!", "", "error");
        }
        else {
            $("#drpShareGrupoAssinantes").next().removeClass("form-control-error");
            //alert($("#drpShareGrupoAssinantes").val() + " : " + $("#hdnId").val() + ", " + $("#hdnTipoModuloId").val() + ", " + $("#hdnEmpresaId").val());
            JSERVICE.AssinantesRegerar($("#drpShareGrupoAssinantes").val(), $("#hdnId").val(), $("#hdnTipoModuloId").val(), $("#hdnEmpresaId").val())
        }
    },
    AssinantesRegerar: function (grupoid, id, moduloid, empresaid) {

        JSERVICE.AssinanteModuloId = moduloid;
        JSERVICE.AssinanteId = id;
        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "moduloId", "value": moduloid });
        json.push({ "name": "empresaId", "value": empresaid });
        json.push({ "name": "grupoId", "value": grupoid });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesRegerar", json, function (retorno) {

            if (retorno.Error === false) {
                $("#hdnAssinantesTotalMin").val(retorno.Data.totalMin);
                $("#hdnAssinantesTotalMax").val(retorno.Data.totalMax);
                JSERVICE.AssinantesBuildGrid(retorno.Data.list, id, moduloid);
                $("#modalRegerarGrupoAssinantes").modal("hide");

                $("#drpShareGrupoAssinantes").val("");
                $("#drpShareGrupoAssinantes").select2();
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
                            html += "<small>" + k.Message + "</small>";
                        }
                        else {
                            if (k.StatusDescricao === "") {
                                html += "<small>" + k.Message + "</small>";
                            }
                            else {
                                html += "<small><strong>" + k.PerfilNome + "</strong> alterou status para <strong>" + k.StatusDescricao + "</strong></small>";//Message;
                            }
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
                        "Principal": $(this).find(".codePrincipal").is(":checked"),
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



    EventUpdateGridED: function (id) {

        var jsonData = new Array();
        var jsonDataIDs = new Array();
        var ct = 1;
        $("#gridAssinantes tbody tr").each(function () {

            if ($(this).find(".codeID").val() !== undefined) {


                if (jQuery.inArray($(this).find(".codeID").val(), jsonDataIDs) !== -1) {
                    //// console.log("JA EXISTE:" + $(this).find(".codeID").val());
                } else {
                    ///console.log("NOVO:" + $(this).find(".codeID").val());
                    jsonDataIDs.push($(this).find(".codeID").val());
                    var code = $(this).find(".codeID").val();
                    jsonData.push({
                        "Id": $(this).find(".codeID").val(),
                        "Campo": $(this).find("#txtAlias_" + code).val(), //(".codeAlias").val(),
                        "Nome": $(this).find("#txtValor_" + code).val(), //.find(".codeNome").val(),
                        "Email": $(this).find("#txtEmail_" + code).val(), //.find(".codeEmail").val(),
                        "Selfie": $(this).find("#chkSelfieED_" + code).is(":checked"), //.codeCheck").is(":checked"),
                        "Ordem": ct
                    });
                    ct++;
                }
            }
        });

        //  alert(JSON.stringify(jsonData));

        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "json", "value": JSON.stringify(jsonData) });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesUpdateAllED", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGridED(retorno.Data, id);
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
            if (item.Principal === true) {
                htmlBody += "<input type='radio' id='chkPrincipal_" + item.Ordem + "' name='chkPrincipal_" + id + "' value='' checked='checked' class='form-control codePrincipal' onclick='JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");' />";
            }
            else {
                htmlBody += "<input type='radio' id='chkPrincipal_" + item.Ordem + "' name='chkPrincipal_" + id + "' value='' class='form-control codePrincipal'  onclick='JSERVICE.EventUpdateGrid(" + id + "," + moduloid + ");'/>";
            }

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
            if (item.Ordem > 1) {//minAssinantes) {
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

    AssinantesBuildGridED: function (array, id) {

        $("#gridAssinantes tbody").html("");

        var habilitaEventos = false;

        if ($("#HabilitaManual").val() === "1") {
            habilitaEventos = true;
        }
        var htmlFull = "";
        var ctNum = 1;
        $(array).each(function (index, data) {

            htmlFull += "<tr>";
            htmlFull += "<td style='cursor:pointer'><i class='fa fa-bars'></i><small>&nbsp;" + ctNum + "º</small></td>";

            // htmlFull += "<td><label>" + data.Ordem + "</label></td>";
            htmlFull += "<td>";
            htmlFull += "<input type='hidden' id='hdnAssinanteID_" + ctNum + "' name='hdnAssinanteID_" + ctNum + "' value='" + ctNum + "' class='codeID'  />";

            htmlFull += "<div class='input-group'>";
            htmlFull += "<input type='text' id='txtAlias_" + ctNum + "' name='txtAlias_" + ctNum + "' value='" + data.Campo + "' class='form-control field-trim codeAlias' maxlength='100'  onChange='return JSERVICE.EventUpdateGridED(" + id + ");'  />";
            htmlFull += "</div>";
            htmlFull += "</td>";
            htmlFull += "<td>";
            htmlFull += "<div class='input-group'>";
            htmlFull += "<input type='text' id='txtValor_" + ctNum + "' name='txtValor_" + ctNum + "' value='" + data.Nome + "' class='form-control field-trim codeNome' maxlength='100'  onChange='return JSERVICE.EventUpdateGridED(" + id + ");'  />";
            htmlFull += "</div>";
            htmlFull += "</td>";
            htmlFull += "<td>";
            htmlFull += "<div class='input-group'>";
            htmlFull += "<input type='text' id='txtEmail_" + ctNum + "' name='txtEmail_" + ctNum + "' value='" + data.Email + "' class='form-control field-email codeEmail' maxlength='100'  onChange='return JSERVICE.EventUpdateGridED(" + id + ");'  />";
            htmlFull += "</div>";
            htmlFull += "</td>";
            htmlFull += "<td>";
            htmlFull += "<div class='input-group'>";

            if (habilitaEventos) {
                if (data.Selfie)
                    htmlFull += "<input type='checkbox' id='chkSelfieED_" + ctNum + "' name='chkSelfieED_" + ctNum + "' value='1'  onclick='JSERVICE.EventUpdateGridED(" + id + ");' checked='checked'  class='form-control codeCheck' />";
                else
                    htmlFull += "<input type='checkbox' id='chkSelfieED_" + ctNum + "' name='chkSelfieED_" + ctNum + "' value='1'  onclick='JSERVICE.EventUpdateGridED(" + id + ");' class='form-control codeCheck' />";
            }
            htmlFull += "</div>";
            htmlFull += "</td>";
            htmlFull += "<td>";

            if (habilitaEventos) {

                if (array.length > 1 && ctNum > 1) {
                    htmlFull += "<span style='cursor:pointer;' onclick='JSERVICE.AssinantesRemoveItemED(" + ctNum + "," + id + ");'>";
                    htmlFull += "<span class='glyphicon glyphicon-minus' aria-hidden='true'></span>";
                    htmlFull += "</span>";
                }
            }
            htmlFull += "</td>";
            htmlFull += "<td>";

            if (habilitaEventos) {

                if (ctNum === array.length) {
                    htmlFull += "<span style='cursor:pointer;' onclick='JSERVICE.AssinantesAddItemED(" + id + ");'>";
                    htmlFull += "<span class='glyphicon glyphicon-plus' aria-hidden='true'></span>";
                    htmlFull += "</span>";
                }
            }
            htmlFull += "</td>";
            htmlFull += "</tr>";

            ctNum++;
        });

        $("#gridAssinantes tbody").html(htmlFull);

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
                "Principal": $(this).find(".codePrincipal").is(":checked"),
                "Selfie": $(this).find("#chkSelfie_" + ct).is(":checked")
            });
            ct++;
        });

        return json;



    },

    AssinantesRemoveItemED: function (ordem, id) {
        var json = $("form").serializeArray();
        json.push({ "name": "ordem", "value": ordem });
        json.push({ "name": "codigo", "value": id });

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesRemoveED", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGridED(retorno.Data, id);
            }

        }, true);
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

        JSERVICE.Ajax.GetData("/GenericControl/AssinantesAdd", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGrid(retorno.Data, id, moduloid);
            }

        }, true);
    },

    AssinantesAddItemED: function (id) {
        var json = $("form").serializeArray();
        json.push({ "name": "codigo", "value": id });
        json.push({ "name": "max", "value": JSERVICE.MaxGridItemsED });


        JSERVICE.Ajax.GetData("/GenericControl/AssinantesAddED", json, function (retorno) {
            if (retorno.Error === false) {
                JSERVICE.AssinantesBuildGridED(retorno.Data, id);
            }

        }, true);
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
    BuscarByColAngular: function (orderNumber, orderSort) {
        if (orderNumber !== undefined && orderSort !== undefined) {
            JSERVICE.OrderNumber = orderNumber;
            JSERVICE.OrderSort = orderSort;
        }
        //MODULO.Buscar();
        MODULO.scope.FiltrarOrder(orderNumber, orderSort);
    },
    BuscarByCol: function (orderNumber, orderSort) {
        if (orderNumber !== undefined && orderSort !== undefined) {
            JSERVICE.OrderNumber = orderNumber;
            JSERVICE.OrderSort = orderSort;
        }

        MODULO.Buscar();
    },
    GenerateAngularSort: function (tableid) {
        var ct = 0;
        var colorSelected = "#000000";
        var colorNoSelected = "#cecece";

        $("#" + tableid + " thead .col_sort").each(function () {
            ct++;

            //console.log(ct);
            if ($(this).find("span").length > 0) {
                $(this).find("span").remove();
            }

            var order = $(this).attr("col_order");
            var columnName = $(this).html();
            var htmlColumnOrder = "";
            if (JSERVICE.OrderNumber !== 0 && JSERVICE.OrderNumber === ct) {
                if (JSERVICE.OrderSort === 'DESC') {
                    //  htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorSelected + ";' class='fa fa-arrow-up'></i></span>";
                    htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByColAngular(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorSelected + ";'></i></span>";

                } else {
                    htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByColAngular(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorSelected + ";'></i></span>";
                    //  htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorNoSelected + ";' class='fa fa-arrow-up'></i></span>";
                }
            }
            else {
                htmlColumnOrder = columnName + "<span style='float:right;'><i class='fa fa-arrows-v' onclick='JSERVICE.BuscarByColAngular(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i></span>";
                // htmlColumnOrder = columnName + " <span style='float:right;'><i class='fa fa-arrow-down' onclick='JSERVICE.BuscarByCol(" + ct + ", \"ASC\");' style='cursor:pointer;color:" + colorNoSelected + ";'></i>  <i onclick='JSERVICE.BuscarByCol(" + ct + ", \"DESC\");' style='cursor:pointer;color:" + colorNoSelected + ";' class='fa fa-arrow-up'></i></span>";
            }
            $(this).html(htmlColumnOrder);

        });

    },
    GenerateSort: function (tableid) {
        var ct = 0;
        var colorSelected = "#000000";
        var colorNoSelected = "#cecece";

        $("#" + tableid + " thead .col_sort").each(function () {
            ct++;

            // console.log(ct);
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

    MensagemTimer: function (message, titulo, tipo) {

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
            timer: 20000,
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
            delay: 8000,
            sound: false,
            position: 'center top',
            title: "Mensagem do Sistema",
            icon: true,
            msg: retorno.Message
        });
    },

    Mensagem3: function (retorno, fncCallback) {
        swal({
            title: "Mensagem do Sistema",
            text: retorno.Message,
            type: retorno.MessageTipo,
            confirmButtonClass: "btn-success",
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
        $('.cpfMask').mask('000.000.000-00', { placeholder: "___.___.___-__" });
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
        if (JSERVICE.stopAutoLoading === false) {
            var regionBlock = "bodyLayout";
            var msg = "Processando. <br> Por favor aguarde.";

            if (show) {
                $.blockUI({
                    message: '<div class="blockui-default-message"><i class="fa fa-circle-o-notch fa-spin"></i><h6 id="msgLoading">' + msg + '</h6></div>',
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
                $('#' + regionBlock).unblock();
            }
        }
    },

    LoadingArea: function (show, area) {
        if (JSERVICE.stopAutoLoading === false) {
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
    ChangePageVENDAS: function (obj) {

        if ($(obj).val() !== "") {
            MODULO.PaginationVENDAS($(obj).val());
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

        var text = $("#areaNameLi a").html();
        $("#ltlTitle").html(text);
        $("#actionName").html(title);
        if (controller === "") {
            $("#areaNameLi").hide();
        }
        $("#controllerName").html(controller);
        $(".breadcrumb").fadeIn();
    },
    TransferModalidade: function () {
        var codeid = $("#hdnTransferId").val();
        var typeid = $("#hdnTransferType").val();
        var typetoid = $("#drpModalidadeTransfer").val();
        var empresaid = $("#hdnCurrentEmpresaId").val();
        JSERVICE.Ajax.GetData("GenericControl/TransferModalidade", { "empresaid": empresaid, "codeid": codeid, "typeid": typeid, "typetoid": typetoid }, function (retorno) {
            if (retorno.Data.OK) {
                JSERVICE.Mensagem(retorno.Data.Mensagem, "", "success");
                $("#modalTransfer").modal("hide");
            } else {
                JSERVICE.Mensagem(retorno.Data.Mensagem, "", "error");

            }
        }, true);
    },
    Transfer: function (code, typeid) {

        $("#hdnTransferId").val(code);
        $("#hdnTransferType").val(typeid);
        var json = JSON.parse($("#hdnJsonModalidadeEnum").val());
        var htmlItem = "";

        $(json).each(function (e, k) {
            if (parseInt(k.Value) !== parseInt(typeid))
                htmlItem += "<option value='" + k.Value + "'>" + k.Text + "</option>";
        });

        $("#drpModalidadeTransfer").html(htmlItem);
        $("#drpModalidadeTransfer").select2();
        $("#modalTransfer").modal("show");
    },
    ConfirmAlterarConsultorDash: function () {

        if ($("#drpConsultorDash").val() === "") {
            $("#drpConsultorDash").addClass("form-control-error");
            $("#drpConsultorDash").next().addClass("form-control-error");
        }
        else {
            $("#drpConsultorDash").removeClass("form-control-error");
            $("#drpConsultorDash").next().removeClass("form-control-error");

            var json = $("form").serializeArray();
            json.push({ "name": "clienteEmail", "value": $("#txtClienteEmailDash").val() });
            json.push({ "name": "consultorId", "value": $("#drpConsultorDash").val() });

            json.push({ "name": "modulo", "value": $("#hdnModuloClienteEdicaoDash").val() });

            JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/AlterarConsultor", json, function (retorno) {

                if (retorno.Data) {

                    $("#modalAlterarConsultorDash").modal("hide");
                    MODULO.scope.CarregaGridAcoes();

                    Lobibox.notify('success', {
                        size: 'mini',
                        rounded: true,
                        delayIndicator: false,
                        sound: false,
                        position: 'center top', //or 'center bottom'
                        //title: 'Informação',
                        icon: false,
                        msg: retorno.Message
                    });

                }
            }, true);
        }
    },
    AlterarConsultorDash: function (userEmail, modulo) {

        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/ListarConsultores", json, function (retorno) {

            var htmlItem = "<option value=''>:: Selecione ::</option>";
            $(retorno.Data).each(function (r, k) {

                htmlItem += "<option value='" + k.Value + "'>" + k.Text + "</option>";

            });

            $("#drpConsultorDash").html(htmlItem);

            //$("#drpConsultorDash").val("");
            $("#drpConsultorDash").select2();
            $("#hdnModuloClienteEdicaoDash").val(modulo);
            $("#txtClienteEmailDash").val(userEmail);
            $("#modalAlterarConsultorDash").modal("show");

        }, true);

    },
    Ajax: function () {
        return {
            Name: '',
            GetDataHome: function (url, jsonData, callbackSucesso, async) {


                var browser = "";
                var ua = navigator.userAgent.toLowerCase();
                if (ua.indexOf("safari") !== -1) {
                    if (ua.indexOf("chrome") > -1) {
                        browser = "chrome";
                    }
                    else {
                        browser = "safari";
                    }
                }
                else {
                    browser = "geral";
                }

                var asyncTemp = true;
                if (async !== null && async !== undefined) {
                    asyncTemp = async;
                }
                console.log("browser : " + browser);
                console.log("rootApplication : " + JSERVICE.rootApplication);
                if (browser === "safari") {
                    $.ajax({
                        type: "POST",
                        //async: asyncTemp,
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
                }
                else {
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
                }

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

                $.ajax({
                    type: "POST",
                    async: asyncTemp,
                    data: jsonData,
                    url: JSERVICE.rootApplication + url,
                    // withCredentials: true,
                    success: function (resposta) {
                        if (resposta.Authenticated === false) {
                            console.log("ERRO 2:" + JSON.stringify(resposta));
                            swal({
                                title: "Atenção",
                                text: "Autenticação expirou :( (ERRO 2)",
                                type: "error",
                                confirmButtonClass: "btn-danger"
                            });
                            return;
                        }
                        else if (resposta.ErrorMessage !== "") {
                            console.log("ERRO 3:" + resposta.ErrorMessage);
                            swal({
                                title: "Atenção",
                                text: "Não foi possível realizar sua requisição :( Tente mais tarde!! (ERRO 3)",
                                type: "error",
                                confirmButtonClass: "btn-danger"
                            });
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
                            text: "Não foi possível realizar sua requisição :( Tente mais tarde!! (ERRO)",
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
                    }
                });
            }
        }
    }()

};