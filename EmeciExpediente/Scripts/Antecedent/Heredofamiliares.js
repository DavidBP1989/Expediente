var urlAddBrother;
var urlAddOtherFamily;
var urlRemove;

function Init(url_addBrother, url_addOtherFamily, url_remove)
{
    urlAddBrother = url_addBrother;
    urlAddOtherFamily = url_addOtherFamily;
    urlRemove = url_remove;

    $('.calendar').each(function (c) {
        var picker = new Pikaday({
            field: this
        })
    })

    $('* .checkbox').map(function (index, ck) {
        var input = $(ck).find('input:checkbox');
        if (input.length > 0 && !input.is(':checked')) {
            $(ck).find('i').toggleClass('glyphicon-ok');
        }
    })

    $('.glyphi').click(function (e) {

        var jE = $(e.target);
        if (jE.is(':checkbox')) {

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


    /* familiares */
    $('#showRelation').hide();
    $('#relationFam').change(function () {
        if ($(this).val() == 5)
            $('#showRelation').show();
        else $('#showRelation').hide();
    })



    /* eventos padre */
    //cuando cambie al escolaridad
    var edV = $('#f_education').val();
    $('#show_Feducation').css('display', (edV == 3 || edV == 4) ? 'block' : 'none');
    $('#f_education').change(function () {
        if ($(this).val() == 3 || $(this).val() == 4)
            $('#show_Feducation').show();
        else $('#show_Feducation').hide();
    })

    //cuando cambie del estado actual de salud
    $('#show_Fhealth').css('display', $('#f_health').val() == 1 ? 'block' : 'none');
    $('#f_health').change(function () {
        if ($(this).val() == 1)
            $('#show_Fhealth').show();
        else $('#show_Fhealth').hide();
    })

    //cuando cambie toxicomanias
    $('#show_FToxicomania').css('display', $('#f_toxicomania').val() == 1 ? 'block' : 'none');
    $('#f_toxicomania').change(function () {
        if ($(this).val() == 1)
            $('#show_FToxicomania').show();
        else $('#show_FToxicomania').hide();
    })




    /* eventos madre */
    //cuando cambie al escolaridad
    var edV = $('#m_education').val();
    $('#show_Meducation').css('display', (edV == 3 || edV == 4) ? 'block' : 'none');
    $('#m_education').change(function () {
        if ($(this).val() == 3 || $(this).val() == 4)
            $('#show_Meducation').show();
        else $('#show_Meducation').hide();
    })

    //cuando cambie del estado actual de salud
    $('#show_Mhealth').css('display', $('#m_health').val() == 1 ? 'block' : 'none');
    $('#m_health').change(function () {
        if ($(this).val() == 1)
            $('#show_Mhealth').show();
        else $('#show_Mhealth').hide();
    })

    //cuando cambie toxicomanias
    $('#show_MToxicomania').css('display', $('#m_toxicomania').val() == 1 ? 'block' : 'none');
    $('#m_toxicomania').change(function () {
        if ($(this).val() == 1)
            $('#show_MToxicomania').show();
        else $('#show_MToxicomania').hide();
    })
}



/*validar*/
function ValidatePage()
{
    var valid = true;

    /*padre*/
    if ($('#show_Feducation').is(':visible'))
    {
        validateForm.input = $('#f_educationInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#f_educationInfo'));
            return valid;
        }
    }
    if ($('#show_Fhealth').is(':visible'))
    {
        validateForm.input = $('#f_healthInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#f_healthInfo'));
            return valid;
        }
    }
    if ($('#show_FToxicomania').is(':visible'))
    {
        validateForm.input = $('#f_toxicomaniaInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#f_toxicomaniaInfo'));
            return valid;
        }
    }


    /*madre*/
    if ($('#show_Meducation').is(':visible'))
    {
        validateForm.input = $('#m_educationInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#m_educationInfo'));
            return valid;
        }
    }
    if ($('#show_Mhealth').is(':visible'))
    {
        validateForm.input = $('#m_healthInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#m_healthInfo'));
            return valid;
        }
    }
    if ($('#show_MToxicomania').is(':visible'))
    {
        validateForm.input = $('#m_toxicomaniaInfo')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid)
        {
            ScrollError($('#m_toxicomaniaInfo'));
            return valid;
        }
    }


    /*general*/
    var ind = -1;
    $('* .letter').each(function (index) {
        validateForm.input = this;
        valid = (valid && validateForm.IsOnlyEmpty());
        if (!valid && ind == -1)
            ind = index;
    })

    if (!valid)
    {
        ScrollError($('* .letter').eq(ind));
        return valid;
    }

    return valid;
}

function ScrollError($Tag)
{
    $('html, body').animate({ scrollTop: $Tag.parent().offset().top }, 500);
}





function AddBrotherHistory()
{
    var valid = true;

    validateForm.input = $('#nameBrother')[0];
    valid = (valid && validateForm.IsOnlyEmpty());
    validateForm.input = $('#dateBrother')[0];
    valid = (valid && validateForm.IsOnlyEmpty());

    if (valid) {
        $.ajax({
            url: urlAddBrother
            , type: 'GET'
            , dataType: 'json'
            , data:
                {
                    name: $('#nameBrother').val()
                    , sex: $('#sexBrother').val()
                    , date: $('#dateBrother').val()
                    , diseases: $('#diseasesBrother').val()
                }
            , success: function (response) {
                console.log(response);
                var tr = '<tr> \
                                <td>' + myDate(response.Nacimiento) + '</td> \
                                <td>' + response.Nombre + '</td> \
                                <td>' + response.Sexo + '</td> \
                                <td>' + response.Enfermedades + '</td> \
                                <td> \
                                    <a style="cursor:pointer" onclick="Delete(' + response.IdFamiliar + ', this)" class="text-danger"> \
                                        <i class="glyphicon glyphicon-remove"></i> \
                                            &nbsp;Eliminar \
                                   </a> \
                                </td> \
                             </tr>';
                $('#rowBrother').append(tr);
                $('#nameBrother').val('');
                $('#dateBrother').val('');
                $('#diseasesBrother').val('');
                $('#modalHermano').modal('toggle');

            }
        })
    }
}


function AddOtherFamily()
{
    var valid = true;

    validateForm.input = $('#nameFam')[0];
    valid = (valid && validateForm.IsOnlyEmpty());
    var relation = $('#relationFam').val();
    if (relation == 5) {
        validateForm.input = $('#otherFam')[0];
        valid = (valid && validateForm.IsOnlyEmpty());
    }


    if (valid) {
        $.ajax({
            url: urlAddOtherFamily
            , type: 'GET'
            , dataType: 'json'
            , data:
                {
                    name: $('#nameFam').val()
                    , relation: $('#relationFam').val()
                    , other: $('#otherFam').val()
                    , diseases: $('#diseasesFam').val()
                }
            , success: function (response) {
                var rel = Relations();
                var tr = '<tr> \
                                <td>' + response.Nombre + '</td> \
                                <td>' + rel[parseInt(response.Relacion)] + '</td> \
                                <td>' + response.Enfermedades + '</td> \
                                <td> \
                                    <a style="cursor:pointer" onclick="Delete(' + response.IdFamiliar + ', this)" class="text-danger"> \
                                        <i class="glyphicon glyphicon-remove"></i> \
                                            &nbsp;Eliminar \
                                   </a> \
                                </td> \
                             </tr>';
                $('#rowFam').append(tr);
                $('#nameFam').val('');
                $('#otherFam').val('');
                $('#diseasesFam').val('');
                $('#modalFamiliar').modal('toggle');

            }
        })
    }
}


function Relations()
{
    var fam = [];
    fam.push('Bisabuelo(a)');
    fam.push('Abuelo(a)');
    fam.push('Sobrino(a)');
    fam.push('Tio(a)');
    fam.push('Primo(a)');
    fam.push('Otro');
    return fam;
}


function myDate(string)
{
    var d = /\/Date\((\d*)\)\//.exec(string);
    var date = new Date(+d[1]);
    return ("0" + date.getDate()).slice(-2) + '/' +
    ("0" + (date.getMonth() + 1)).slice(-2) + '/' +
    date.getFullYear();
}


function Delete(idFamiliar, elm)
{
    $.ajax({
        url: urlRemove
        , type: 'GET'
        , dataType: 'json'
        , data: { idFamiliar: idFamiliar }
        , success: function (response) {
            if (response.status)
                $(elm).parent().parent().remove();
        }
    })
}


function Post()
{
    if (ValidatePage()) $('#formHeredo').submit();
}