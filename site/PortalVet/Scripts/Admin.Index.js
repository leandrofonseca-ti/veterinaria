
$(document).ready(function () {

    $('#txtPassword, #txtEmail').keypress(function (e) {
        if (e.which === 13) {
            MODULO.Login($("#hdnAba").val());
        }
    });

    $("#btnLogin").click(function (event) {
        MODULO.Login($("#hdnAba").val());
    });

    $("#btnLogin2").click(function (event) {
        MODULO.Login($("#hdnAba").val());
    });

    $("#btnLoginTelefone").click(function (event) {
        MODULO.Login($("#hdnAba").val());
    });

    
    $("#abaTab1").click(function (event) {
        $("#hdnAba").val("CPF_CNPJ");
    });

    $("#abaTab2").click(function (event) {
        $("#hdnAba").val("TELEFONE");
    });
    $("#abaTab3").click(function (event) {
        $("#hdnAba").val("EMAIL_SENHA");
    });

    $("#areaBody").fadeIn(500);
 
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
});

var MODULO = {

    ModuloNome: "Admin",

    Login: function (opcao) {
        //alert($("#hdnAba").val() + " - " + opcao);
        /* var browser = "";
         var ua = navigator.userAgent.toLowerCase();
         if (ua.indexOf("safari") !== -1) {
             if (ua.indexOf("chrome") > -1) {
                 browser = "chrome";            }
             else {
                 browser = "safari";
             }
         }
         else {
             browser = "geral";
         }*/
        var json = $("form").serializeArray();
        json.push({ "name": "opcao", "value": opcao });
        json.push({ "name": "perfilNome", "value": $("#hdnProfile").val() });
        
        JSERVICE.Ajax.GetDataHome(MODULO.ModuloNome + "/JsonLogin", json, function (retorno) {
            console.log("home.login:" + JSON.stringify(retorno));
            if (retorno.Criticas.length > 0) {
                var message = "";
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");

                    if (this.FieldId === "MESSAGE") {
                        message = this.Message;
                    }
                });

                if (message.length > 0) {
                    $("#msg_feedback").html(message);
                    $("#msg_feedback").fadeIn("slow");
                    $("#msg_feedback").show();
                    setTimeout(function () {
                        $('#msg_feedback').fadeOut('slow');
                        $("#msg_feedback").hide();
                    }, 4000);
                } else {
                    $("#msg_feedback").html("Autenticação incorreta. Favor tente novamente.");
                    $("#msg_feedback").fadeIn("slow");
                    $("#msg_feedback").show();
                    setTimeout(function () {
                        $('#msg_feedback').fadeOut('slow');
                        $("#msg_feedback").hide();
                    }, 4000);
                }
            }
            else {
                if (retorno.Data) {
                    setTimeout(function () {
                        JSERVICE.Loading(true);
                        //console.log(JSERVICE.rootApplication + "Dashboard");

                        window.location.href = JSERVICE.rootApplication + "Dashboard";

                    }, 100);
                }
            }
        }, true);
    },
};

