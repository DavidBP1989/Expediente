var urlDelete;
var urlSaveSurgery;
var urlOthers;

function Init(url_Delete, url_SaveSurgery, url_Others)
{
    urlDelete = url_Delete;
    urlSaveSurgery = url_SaveSurgery;
    urlOthers = url_Others;

    $('* .checkbox').map(function (index, ck) {
        var input = $(ck).find('input:checkbox');
        if (input.length > 0 && !input.is(':checked'))
        {
            $(ck).find('i').toggleClass('glyphicon-ok');
            $('#cnt_' + input[0].id).hide();
        }
    })

    $('.glyphi').click(function (e) {

        var jE = $(e.target);
        if (jE.is(':checkbox'))
        {
            $(this).find('i').toggleClass('glyphicon-ok');

            if ($(this).find('i').hasClass('glyphicon-ok')) {
                jE.val(true);
                $('#cnt_' + jE[0].id).show();
            } else {
                jE.val(false);
                $('#cnt_' + jE[0].id).hide();
            }
        }
    })

    $('.calendar').each(function (c) {
        var picker = new Pikaday({
            field: this
        })
    })
}


function Delete(idPatologia, elm)
{
    $.ajax({
        url: urlDelete
        , type: 'GET'
        , dataType: 'json'
        , data: { idPatologia: idPatologia }
        , success: function (response) {
            if (response.status)
                $(elm).parent().parent().remove();
        }
    })
}


function SaveSurgery_Or_Trauma(category)
{
    var name = '';
    var date = '';
    var observations = '';
    var band = true;

    if (category == 1) {
        //Cirugia
        validateForm.input = $('#nameCirugia')[0];
        band = (band && validateForm.IsOnlyEmpty());
        name = validateForm.input.value;
        date = $('#dateCirugia').val();
        validateForm.input = $('#obsCirugia')[0];
        band = (band && validateForm.IsOnlyEmpty());
        observations = validateForm.input.value;
    }

    if (category == 2) {
        //Traumatismo
        validateForm.input = $('#nameTrauma')[0];
        band = (band && validateForm.IsOnlyEmpty());
        name = validateForm.input.value;
        date = $('#dateTrauma').val();
        validateForm.input = $('#obsTrauma')[0];
        band = (band && validateForm.IsOnlyEmpty());
        observations = validateForm.input.value;
    }

    if (band) {
        $.ajax({
            url: urlSaveSurgery
            , type: 'GET'
            , dataType: 'json'
            , data: { category: category, name: name, date: date, observations: observations }
            , success: function (response) {
                console.log(response);
                var tr = '<tr> \
                                <td>' + response.Fecha + '</td> \
                                <td>' + response.Nombre + '</td> \
                                <td>' + response.Observaciones + '</td> \
                                <td> \
                                    <a style="cursor:pointer" onclick="Delete(' + response.idpatologia + ', this)" class="text-danger"> \
                                        <i class="glyphicon glyphicon-remove"></i> \
                                            &nbsp;Eliminar \
                                   </a> \
                                </td> \
                             </tr>';
                var row;
                if (category == 1) {
                    $('#nameCirugia').val('');
                    $('#dateCirugia').val('');
                    $('#obsCirugia').val('');
                    row = $('#rowSurgeries');
                    $('#modalCirugia').modal('toggle');
                }
                if (category == 2) {
                    $('#nameTrauma').val('');
                    $('#dateTrauma').val('');
                    $('#obsTrauma').val('');
                    row = $('#rowTrauma');
                    $('#modalTrauma').modal('toggle');
                }

                row.append(tr);
            }
        })
    }
}


function SaveDiseases_Or_Others(category)
{
    var name = '';
    var alergeno = '';
    var age = 0;
    var manejo = '';
    var evolution = '';
    var band = true;

    if (category == 5) {
        //Enfermedades alergias o atopicas
        validateForm.input = $('#nameDisease')[0];
        band = (band && validateForm.IsOnlyEmpty());
        name = validateForm.input.value;
        validateForm.input = $('#alergenoDisease')[0];
        band = (band && validateForm.IsOnlyEmpty());
        alergeno = validateForm.input.value;
        validateForm.input = $('#ageDisease')[0];
        band = (band && validateForm.IsNumber());
        age = validateForm.input.value;
        validateForm.input = $('#manejoDisease')[0];
        band = (band && validateForm.IsOnlyEmpty());
        manejo = validateForm.input.value;
        validateForm.input = $('#evoDisease')[0];
        band = (band && validateForm.IsOnlyEmpty());
        evolution = validateForm.input.value;
    }

    if (category == 6) {
        //Otros tipos de enfermedades
        validateForm.input = $('#nameOther')[0];
        band = (band && validateForm.IsOnlyEmpty());
        name = validateForm.input.value;
        validateForm.input = $('#ageOther')[0];
        band = (band && validateForm.IsNumber());
        age = validateForm.input.value;
        validateForm.input = $('#manejoOther')[0];
        band = (band && validateForm.IsOnlyEmpty());
        manejo = validateForm.input.value;
        validateForm.input = $('#evoOther')[0];
        band = (band && validateForm.IsOnlyEmpty());
        evolution = validateForm.input.value;
    }

    if (band) {
        $.ajax({
            url: urlOthers
            , type: 'GET'
            , dataType: 'json'
            , data: { category: category, name: name, alergeno: alergeno, age: age, manejo: manejo, evolution: evolution }
            , success: function (response) {
                var tr = '<tr>';
                tr += '<td>' + response.Nombre + '</td>';
                tr += (category == 5 ? ('<td>' + response.Alergeno + '</td>') : '');
                tr += '<td>' + response.EdadAdquirida + '</td>';
                tr += '<td>' + response.Manejo + '</td>';
                tr += '<td>' + response.Evoluciones + '</td>';
                tr += '<td> \
                                <a style="cursor:pointer" onclick="Delete(' + response.idpatologia + ', this)" class="text-danger"> \
                                    <i class="glyphicon glyphicon-remove"></i> \
                                        &nbsp;Eliminar \
                                </a> \
                            </td>';
                tr += '</tr>'

                var row;
                if (category == 5) {
                    $('#nameDisease').val('');
                    $('#alergenoDisease').val('');
                    $('#ageDisease').val('');
                    $('#manejoDisease').val('');
                    $('#evoDisease').val('');
                    row = $('#rowDisease');
                    $('#modalDiseases').modal('toggle');
                }
                if (category == 6) {
                    $('#nameOther').val('');
                    $('#ageOther').val('');
                    $('#manejoOther').val('');
                    $('#evoOther').val('');
                    row = $('#rowOthers');
                    $('#modalOtherDiseases').modal('toggle');
                }

                row.append(tr);
            }
        })
    }
}


function Post() {
    $('#formPath').submit();
}