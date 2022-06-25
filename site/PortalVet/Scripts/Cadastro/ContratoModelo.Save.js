const variaveis = [];

$(function () {

    $("#btnSave").click(function () {
        MODULO.Save();
    });

    $("#btnBack").click(function () {
        window.location = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Index";
    });

    window.setTimeout(function () {
      
        MODULO.CarregarVariaveis();
        MODULO.Load();

    }, 100);
   
});

var MODULO = {
    PageIndex: 1,
    PageSize: 30,
    AreaNome: "Cadastro",
    ControllerNome: "ContratoModelo",

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
    CarregarVariaveis: function () {
        //JSERVICE.Loading(true);
        var json = $("form").serializeArray();

        JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/CarregarVariaveis", json, function (retorno) {

            var html = "";

            if (!retorno.Error) {

                $.each(retorno.Data, function (i) {
                    variaveis.push({ id: "#" + retorno.Data[i].Nome.toUpperCase() + "#", userId: retorno.Data[i].Id, name: retorno.Data[i].Descricao });
                });



            }
        }, true);

    },
    InstaciaCkeditor: function () {
        var url = JSERVICE.rootApplication + MODULO.AreaNome + "/" + MODULO.ControllerNome + "/UploadImageCkeditor";

        instanciarCkeditor(url);
        
    },
    Load: function () {

        var id = $("#hdnId").val();

        if (id !== "") {
            var json = $("form").serializeArray();
            json.push({ "id": id });

            JSERVICE.Ajax.GetData(MODULO.AreaNome + "/" + MODULO.ControllerNome + "/Load", json, function (retorno) {

                if (!retorno.Error) {


                    $("#txtNomeModelo").val(retorno.Data.Nome);
                    $("#txtLocatorios").val(retorno.Data.QuantidadeLocatorio);
                    $("#txtFiadores").val(retorno.Data.QuantidadeFiador);

                    $("#editorCorpo").html(retorno.Data.ModeloCorpo);

                    //$("#drpRodape").val(retorno.Data.AssinaturaId).trigger('change');

                    if ($('#drpPerfil').find("option[value='" + retorno.Data.Perfil + "']").length) {
                        $('#drpPerfil').val(retorno.Data.Perfil).trigger('change');
                    }
 
                }

                MODULO.InstaciaCkeditor();

            }, true);
        }
        else {
            MODULO.InstaciaCkeditor();
        }
    },

}

function instanciarCkeditor(uploadEndPoint) {
    //ClassicEditor.create(document.querySelector('.editorCabecalho'), {
    //    simpleUpload: {
    //        uploadUrl: uploadEndPoint
    //    },
    //    toolbar: {
    //        viewportTopOffset: 79, 
    //        items: [
    //            'heading',
    //            '|',
    //            'alignment',
    //            '|',
    //            'bold',
    //            'italic',
    //            'link',
    //            'bulletedList',
    //            'numberedList',
    //            '|',
    //            'indent',
    //            'outdent',
    //            '|',
    //            'imageUpload',
    //            'blockQuote',
    //            'insertTable',
    //            'undo',
    //            'redo'
        
    //        ]
    //    },
    //    language: 'pt-br',
    //    image: {
    //        toolbar: [
    //            'imageTextAlternative',
    //            'imageStyle:full',
    //            'imageStyle:side'
    //        ]
    //    },
    //    table: {
    //        contentToolbar: [
    //            'tableColumn',
    //            'tableRow',
    //            'mergeTableCells'
    //        ]
    //    },
    //    licenseKey: '',
    //})
    //    .then(editor => {
    //        editor.model.document.on('change', () => {
    //            $("#editorCabecalho").html(editor.getData());
    //        });
    //        editor.editing.view.change(writer => {
    //            writer.setStyle('height', '200px', editor.editing.view.document.getRoot());
    //        });
    //    })
    //    .catch(error => {
    //        console.error('Oops, something gone wrong!');
    //        console.error('Please, report the following error in the https://github.com/ckeditor/ckeditor5 with the build id and the error stack trace:');
    //        console.warn('Build id: sb5wsw8zunnh-8o65j7c6blw0');
    //        console.error(error);
    //    });

    ClassicEditor.create(document.querySelector('.editorCorpo'), {
        mention: {
            feeds: [
                {
                    marker: '#',
                    feed: getFeedItems,
                    itemRenderer: customItemRenderer
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
        .then(editor => {
            editor.model.document.on('change', () => {
                $("#editorCorpo").html(editor.getData());
            });
            editor.editing.view.change(writer => {
                writer.setStyle('height', '700px', editor.editing.view.document.getRoot());
            });
        })
        .catch(error => {
            console.error('Oops, something gone wrong!');
            console.error('Please, report the following error in the https://github.com/ckeditor/ckeditor5 with the build id and the error stack trace:');
            console.warn('Build id: sb5wsw8zunnh-8o65j7c6blw0');
            console.error(error);
        });
    /*
    ClassicEditor.create(document.querySelector('.editorRodape'), {
        simpleUpload: {
            uploadUrl: uploadEndPoint
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
                'imageUpload',
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
        .then(editor => {
            editor.model.document.on('change', () => {
                $("#editorRodape").html(editor.getData());
            });
            editor.editing.view.change(writer => {
                writer.setStyle('height', '200px', editor.editing.view.document.getRoot());
            });
        })
        .catch(error => {
            console.error('Oops, something gone wrong!');
            console.error('Please, report the following error in the https://github.com/ckeditor/ckeditor5 with the build id and the error stack trace:');
            console.warn('Build id: sb5wsw8zunnh-8o65j7c6blw0');
            console.error(error);
        });*/
}

function MentionCustomization(editor) {
    editor.conversion.for('upcast').elementToAttribute({
        view: {
            name: 'a',
            key: 'data-mention',
            classes: 'mention',
            attributes: {
                href: true,
                'data-user-id': true
            }
        },
        model: {
            key: 'mention',
            value: viewItem => {

                const mentionAttribute = editor.plugins.get('Mention').toMentionAttribute(viewItem, {

                    userId: viewItem.getAttribute('data-user-id')
                });

                return mentionAttribute;
            }
        },
        converterPriority: 'high'
    });

    editor.conversion.for('downcast').attributeToElement({
        model: 'mention',
        view: (modelAttributeValue, viewWriter) => {

            if (!modelAttributeValue) {
                return;
            }

            return viewWriter.createAttributeElement('a', {
                class: 'mention',
                'data-mention': modelAttributeValue.id,
                'data-user-id': modelAttributeValue.userId,
            }, {

                priority: 20,

                id: modelAttributeValue.uid
            });
        },
        converterPriority: 'high'
    });
}

function getFeedItems(queryText) {
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
}

function customItemRenderer(item) {
    const itemElement = document.createElement('span');

    itemElement.classList.add('custom-item');
    itemElement.id = `mention-list-item-id-${item.userId}`;
    itemElement.textContent = `${item.name} `;

    const usernameElement = document.createElement('span');

    usernameElement.classList.add('custom-item-username');
    usernameElement.textContent = item.id;

    itemElement.appendChild(usernameElement);

    return itemElement;
}

$('#drpModalidade').on('select2:select', function (e) {
    var valorTexto = e.params.data.text;

    if (valorTexto == "Fiador") {
        $("#DivFiador").removeClass("d-none");
    }
});

$('#drpModalidade').on("select2:unselect", function (e) {
    var valorTexto = e.params.data.text;

    if (valorTexto == "Fiador") {
        $("#DivFiador").addClass("d-none");
        $("#txtFiadores").val("");
    }
});
