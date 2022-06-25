PORTALVETAPP.controller('DashBoardController', function ($scope) {

    $scope.dashboard = {};
    $scope.dashboard.totalClientes = 0;
    $scope.dashboard.totalExamesEmAndamento = 0;
    $scope.dashboard.totalExamesConcluidos = 0;
    $scope.dashboard.totalExamesCancelados = 0;
    $scope.dashboard.totalGerentes = 0;

    $scope.dashboard.listCompany = {};


    $scope.DashReportAdmin = function () {
        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/DashReportAdmin", {}, function (retorno) {
            if (retorno.Data !== null) {
                $scope.dashboard.listCompany = retorno.Data;
                $scope.$apply();
            }
        }, true);
    };


    $scope.RefreshList = function () {
        var jsonData = new Array();
        jsonData.push({ "name": "txtFilterDtInicio", "value": $("#txtFilterDtInicio").val() });
        jsonData.push({ "name": "txtFilterDtFim", "value": $("#txtFilterDtFim").val() });
        jsonData.push({ "name": "txtFilterCodigo", "value": $("#txtFilterCodigo").val() });
        jsonData.push({ "name": "txtFilterCPFCNPJ", "value": $("#txtFilterCPFCNPJ").val() });
        jsonData.push({ "name": "drpFilterSituacao", "value": $("#drpFilterSituacao").val() });
        jsonData.push({ "name": "pageIndex", "value": MODULO.PageIndex });
        jsonData.push({ "name": "pageSize", "value": MODULO.PageSize });

        $scope.ListAcoes(jsonData);
    };

    $scope.ArquivarExame = function (exameid) {

        var json = [];
        json.push({ "name": "exameid", "value": exameid });
        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/ArquivarExame", json, function (retorno) {
            if (retorno.Data === true) {
                $scope.RefreshList();
                //$scope.$apply();
            }
        }, true);

    };

    //$scope.DashReport = function () {
    //    JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/DashReport", {}, function (retorno) {
    //        if (retorno.Data !== null) {

    //            $scope.dashboard.totalClientes = retorno.Data.totalClientes;
    //            $scope.dashboard.totalExamesEmAndamento = retorno.Data.totalExamesEmAndamento;
    //            $scope.dashboard.totalExamesConcluidos = retorno.Data.totalExamesConcluidos;
    //            $scope.dashboard.totalExamesCancelados = retorno.Data.totalExamesCancelados;
    //            $scope.dashboard.totalGerentes = retorno.Data.totalGerentes;
    //            $scope.$apply();
    //        }
    //    }, true);
    //};


    $scope.ListAcoes = function (json) {

        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/ListAcoes", json, function (retorno) {
            $scope.acoes = retorno.Data;
            $scope.$apply();
            JSERVICE.BuildPagination(retorno);
            $("#areaGrid").show();
        }, true);
    };

    if ($("#hdnPerfilCodigo").val() === "1") {
        $scope.DashReportAdmin();
    }

    MODULO.scopeDash = $scope;
});

/*PORTALVETAPP.controller('MasterController', function ($scope) {
    $scope.Ciente = false;

    $scope.Fechar = function () {

        if ($scope.Ciente) {
            JSERVICE.Ajax.GetData("/BaseAdmin/DesabilitaNovidadesUsuario", {}, function (retorno) {
                //
            }, true);
        }

        $("#modalReleaseNotes").modal("hide");
    }

    $scope.Abrir = function () {
        JSERVICE.Ajax.GetData("/BaseAdmin/Novidades", {}, function (retorno) {
            if (retorno.Data) {
                switch ($("#hdnPerfilCodigo").val()) {
                    case JSERVICE.GetProfileId("Corretora"):
                        $("#modalReleaseNotes").modal("show");
                        break;
                    case JSERVICE.GetProfileId("Corretor"):
                        $("#modalReleaseNotes").modal("show");
                        break;

                    case JSERVICE.GetProfileId("Imobiliaria"):
                        $("#modalReleaseNotes").modal("show");

                        break;
                    case JSERVICE.GetProfileId("Consultor"):
                    case JSERVICE.GetProfileId("ConsultorTotal"):
                        $("#modalReleaseNotes").modal("show");
                        break;
                }
            }
        }, true);
    }

    $scope.Abrir();
});
*/



/*
* 
* Credits to https://css-tricks.com/long-dropdowns-solution/
*
*/

var maxHeight = 400;

$(function () {

    $(".dropdownX > li").hover(function () {

        var $container = $(this),
            $list = $container.find("ul"),
            $anchor = $container.find("a"),
            height = $list.height() * 1.1,       // make sure there is enough room at the bottom
            multiplier = height / maxHeight;     // needs to move faster if list is taller

        // need to save height here so it can revert on mouseout            
        $container.data("origHeight", $container.height());

        // so it can retain it's rollover color all the while the dropdown is open
        $anchor.addClass("hover");

        // make sure dropdown appears directly below parent list item    
        $list
            .show()
            .css({
                paddingTop: $container.data("origHeight")
            });

        // don't do any animation if list shorter than max
        if (multiplier > 1) {
            $container
                .css({
                    height: maxHeight,
                    overflow: "hidden"
                })
                .mousemove(function (e) {
                    var offset = $container.offset();
                    var relativeY = ((e.pageY - offset.top) * multiplier) - ($container.data("origHeight") * multiplier);
                    if (relativeY > $container.data("origHeight")) {
                        $list.css("top", -relativeY + $container.data("origHeight"));
                    };
                });
        }

    }, function () {

        var $el = $(this);

        // put things back to normal
        $el
            .height($(this).data("origHeight"))
            .find("ul")
            .css({ top: 0 })
            .hide()
            .end()
            .find("a")
            .removeClass("hover");

    });

});





$(function () {


    $('.panel-collapse').on('show.bs.collapse', function () {
        $(this).siblings('.panel-heading').addClass('active');

    });

    $('.panel-collapse').on('hide.bs.collapse', function () {
        $(this).siblings('.panel-heading').removeClass('active');

    });

    $("#btnConfirmEnvioPDF").click(function () {

        var exameId = $(this).attr("data-id");
        var email = "";

        if ($("#drpExameEnvio").val() !== "") {
            $("#drpExameEnvio").next().removeClass("error-field");
            if ($("#drpExameEnvio").val() === "2") {
                if ($("#txtOutroEmail").val() === "") {
                    $("#txtOutroEmail").addClass("error-field");
                    Lobibox.notify('error', {
                        size: 'mini',
                        msg: "Informe um e-mail!"
                    });
                } else {
                    $("#txtOutroEmail").removeClass("error-field");
                    email = $("#txtOutroEmail").val();
                }
            }
            else {
                email = $("#drpExameEnvio").val();
            }
        }
        else {
            $("#drpExameEnvio").next().addClass("error-field");
        }


        if (email !== "") {

            var json = [];
            json.push({ "name": "exameid", "value": exameId });
            json.push({ "name": "email", "value": email });

            JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/EnviarEmailExame", json, function (retorno) {

                if (retorno.Data === true) {

                    Lobibox.notify('success', {
                        size: 'mini',
                        msg: "Exame enviado com sucesso!"
                    });
                } else {
                    Lobibox.notify('error', {
                        size: 'mini',
                        msg: "Erro ao enviar exame!"
                    });
                }

                $("#modalEnviarPDF").modal("hide");
            }, true);

        }

    });
    $("#drpExameEnvio").change(function () {

        if ($(this).val() === "2") {
            $("#areaOutroEmail").show();
        }
        else {
            $("#areaOutroEmail").hide();
        }
    });


/*
       Administrador = 1,
       Gerente = 2,
       Clinica = 3,
       Cliente = 4,
       Laudador = 5
    */
    if ($("#hdnPerfilCodigo").val() === "4") {
        $(".campoFiltro").hide();
    }


    $('.data').mask('00/00/0000');
    $('.codigo').mask('00000');

    $("#txtFilterDtInicio, #txtFilterDtFim").datepicker({

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





    $("#txtFilterCPFCNPJ").keydown(function () {
        try {
            $("#txtFilterCPFCNPJ").unmask();
        } catch (e) { }

        var tamanho = $("#txtFilterCPFCNPJ").val().length;

        if (tamanho < 11) {
            $("#txtFilterCPFCNPJ").mask("999.999.999-99");
        } else {
            $("#txtFilterCPFCNPJ").mask("99.999.999/9999-99");
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

    $("#btnFilter").click(function () {
        MODULO.CarregarAcoesRecentes();
    });

    $("#btnClear").click(function () {
        $("#txtFilterDtInicio").val("");
        $("#txtFilterDtFim").val("");
        $("#txtFilterCodigo").val("");
        $("#txtFilterCPFCNPJ").val("");
        $("#drpFilterSituacao").val("");

        MODULO.CarregarAcoesRecentes();
    });
    window.setTimeout(function () {
        if ($("#hdnPerfilCodigo").val() !== "1") {
            MODULO.CarregarAcoesRecentes();
        }
    }, 100);
});


var MODULO = {
    scopeDash: null,
    PageIndex: 1,
    PageSize: 50,
    ControllerNome: "Dashboard",
    CarregarAcoesRecentes: function () {

        MODULO.PageSize = 50;// $("#drpFilterSize").val();
        var jsonData = new Array();
        //jsonData.push({ "name": "drpFilterTipo", "value": $("#drpFilterTipo").val() });
        jsonData.push({ "name": "txtFilterDtInicio", "value": $("#txtFilterDtInicio").val() });
        jsonData.push({ "name": "txtFilterDtFim", "value": $("#txtFilterDtFim").val() });
        //jsonData.push({ "name": "txtFilterNome", "value": $("#txtFilterNome").val() });
        //jsonData.push({ "name": "txtFilterEmail", "value": $("#txtFilterEmail").val() });

        //if ($("#hdnPerfilCodigo").val() === $("#hdnProfileIdImobiliaria").val()) {
        //    jsonData.push({ "name": "drpFilterConsultor", "value": $("#drpFilterConsultor").val() });
        //}
        //else {
        //    jsonData.push({ "name": "drpFilterConsultor", "value": "" });
        //}

        //jsonData.push({ "name": "txtFilterTelefone", "value": $("#txtFilterTelefone").val() });
        jsonData.push({ "name": "txtFilterCodigo", "value": $("#txtFilterCodigo").val() });
        jsonData.push({ "name": "txtFilterCPFCNPJ", "value": $("#txtFilterCPFCNPJ").val() });
        jsonData.push({ "name": "drpFilterSituacao", "value": $("#drpFilterSituacao").val() });
        //jsonData.push({ "name": "drpFilterRespAdm", "value": $("#drpFilterRespAdm").val() });
        jsonData.push({ "name": "pageIndex", "value": MODULO.PageIndex });
        jsonData.push({ "name": "pageSize", "value": MODULO.PageSize });

        MODULO.scopeDash.ListAcoes(jsonData);

    },
    DashReport: function () {

        var json = $("form").serializeArray();
        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/DashReport", json, function (retorno) {


            var htmlGridRobo = "";
            $(retorno.Data.listStatusRobos).each(function (k, v) {

                htmlGridRobo += "<tr>";
                htmlGridRobo += "<td><small>" + v.Nome + "</small></td>";
                htmlGridRobo += "<td>";
                htmlGridRobo += "<span class='nav-link-in'>";

                switch (v.Status) {
                    case "ok":
                        htmlGridRobo += "<i class='font-icon font-icon-check-circle' style='color:forestgreen'></i>";
                        break;
                    case "?":
                        htmlGridRobo += "<i class='font-icon font-icon-question' style='color:yellow'></i>";
                        break;
                    case "nok":
                        htmlGridRobo += "<i class='font-icon font-icon-question' style='color:red'></i>";
                        break;
                    default:
                }


                htmlGridRobo += "</span>";
                htmlGridRobo += "</td>";

                htmlGridRobo += "<td class='color-blue-grey-lighter'><small>" + v.Descricao + "</small></td>";
                htmlGridRobo += "<td class='color-blue-grey-lighter'><small>" + v.DtExecuxao + "</small></td>";
                htmlGridRobo += "</tr>";
            });

            $("#tbServicesRobo tbody").html(htmlGridRobo);


            var htmlGridLeads = "";
            $(retorno.Data.listGroupLeads).each(function (k, v) {

                htmlGridLeads += "<tr>";
                htmlGridLeads += "<td><small>" + v.Nome + "</small></td>";
                htmlGridLeads += "<td>";
                htmlGridLeads += "<span class='nav-link-in'>";

                switch (v.Status) {
                    case "ok":
                        htmlGridLeads += "<i class='font-icon font-icon-check-circle' style='color:forestgreen'></i>";
                        break;
                    case "?":
                        htmlGridLeads += "<i class='font-icon font-icon-question' style='color:yellow'></i>";
                        break;
                    case "nok":
                        htmlGridLeads += "<i class='font-icon font-icon-question' style='color:red'></i>";
                        break;
                    default:
                }


                htmlGridLeads += "</span>";
                htmlGridLeads += "</td>";

                htmlGridLeads += "<td class='color-blue-grey-lighter'><small>" + v.Descricao + "</small></td>";
                htmlGridLeads += "<td class='color-blue-grey-lighter'><small>" + v.DtExecuxao + "</small></td>";
                htmlGridLeads += "</tr>";
            });

            $("#tbServicesLeads tbody").html(htmlGridLeads);
            ///$(".clsAreaBtnRobo").hide();
            $(".clsAreaRobos").show();
        }, true);
    },
    ModalEnviarPDF: function (obj) {

        var exameid = $(obj).attr("data-id");
        var json = [];
        json.push({ "name": "exameid", "value": exameid });

        JSERVICE.Ajax.GetData(MODULO.ControllerNome + "/CarregarEmailExame", json, function (retorno) {
            var html = "<option value=''>:: Selecione ::</option>";

            if (retorno.Data !== null && retorno.Data !== "" && retorno.Data !== undefined) {
                html += "<option value='" + retorno.Data + "'>" + retorno.Data + "</option>";
                html += "<option value='2'>Outro e-mail</option>";
                $("#drpExameEnvio").html(html);
                $("#drpExameEnvio").select2();
            } else {
                html += "<option value='2'>Outro e-mail</option>";
                $("#drpExameEnvio").html(html);
                $("#drpExameEnvio").select2();
            }
        }, true);


        $("#btnConfirmEnvioPDF").attr("data-id", exameid);
        $("#areaOutroEmail").hide();
        $("#modalEnviarPDF").modal("show");
    }

};
