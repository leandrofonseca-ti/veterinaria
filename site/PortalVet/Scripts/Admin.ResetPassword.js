$(document).ready(function () {

    $('#txtEmail').keypress(function (e) {
        if (e.which === 13) {
            MODULO.Reset();
        }
    });

    $("#btnOK").click(function (event) {
        MODULO.Reset();
    });

    window.setTimeout(function () {
        $("#txtEmail").show();
    }, 100);
});

var MODULO = {

    ModuloNome: "Admin",

    Reset: function () {

        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();
        //json.push({ "name": "txtEmail", "value": $("#txtEmail").val() });
        //json.push({ "name": "txtPassword", "value": $("#txtPassword").val() });
        //json.push({ "name": "chkRemember", "value": "0" });

        JSERVICE.Ajax.GetDataHome(MODULO.ModuloNome + "/JsonResetPassword", json, function (retorno) {
            if (retorno.Criticas.length > 0) {
                $(retorno.Criticas).each(function () {
                    $("#" + this.FieldId).addClass("form-control-error");
                    //JSERVICE.AddToolTip($("#" + this.FieldId), this.Message);

                });
                if (retorno.Message.length > 0) {
                    //JSERVICE.ShowMessage(retorno.Message, "error");
                    $("#msg_feedback").html(retorno.Message);

                    $("#msg_feedback").fadeIn("slow");

                    setTimeout(function () {
                        $('#msg_feedback').fadeOut('slow');
                    }, 4000);
                }
                //JSERVICE.Loading(false);
            }
            else {

                if (retorno.Data) {
                    //JSERVICE.ShowMessage("Recuperação de senha realizado com sucesso.", "success");                    
                    $("#msg_feedback").html("Verifique sua caixa de entrada para recuperar a senha.");
                    $("#msg_feedback").attr("style", "background-color: #fff !important;border: dashed 1px green!important;padding: 12px 14px 3px;color: green!important;margin: 0 0 1rem;font-size: 1rem;");
                    $("#txtEmail").hide();
                    $("#txtEmail").removeClass("form-control-error");
                    $("#msg_feedback").fadeIn("slow");
                    $("#txtEmail").val("");
                    setTimeout(function () {
                        $('#msg_feedback').fadeOut('slow');

                        window.location.href = JSERVICE.rootApplication + MODULO.ModuloNome;
                        //window.location.href = JSERVICE.rootApplication;
                    }, 4000);
                    //$("#hdnId").val(retorno.Data);

                }
            }


            //var html = "";
            //alert(JSON.stringify(retorno.Data));
            // if (retorno.Data.Sucesso) {
            //}

        }, true);


        //JSERVICE.Loading(false);
    },

};

