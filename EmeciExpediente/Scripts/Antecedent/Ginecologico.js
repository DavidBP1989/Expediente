
function Init()
{
    $('.calendar').each(function (c) {
        var picker = new Pikaday({
            field: this
        })
    })

    $('* .checkbox').map(function (index, ck)
    {
        var input = $(ck).find('input:checkbox');
        if (input.length > 0 && !input.is(':checked')) {
            $(ck).find('i').toggleClass('glyphicon-ok');
            $('* #cnt_' + input[0].id).hide();
        }
    })

    $('.glyphi').click(function (e) {

        var jE = $(e.target);
        if (jE.is(':checkbox')) {

            $(this).find('i').toggleClass('glyphicon-ok');

            if ($(this).find('i').hasClass('glyphicon-ok')) {
                jE.val(true);
                $('* #cnt_' + jE[0].id).show();
            } else {
                jE.val(false);
                $('* #cnt_' + jE[0].id).hide();
            }
        }
    })

    $('.numbersOnly').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
}

function Post()
{
    if (Validate())
        $('#formGine').submit();
}


function Validate()
{
    var _continue = true;
    if ($('#leucorrea').prop('checked'))
    {
        validateForm.input = $('#leucorreaTxt')[0];
        _continue = (_continue && validateForm.IsOnlyEmpty());
        if (!_continue)
        {
            ScrollError($('#leucorreaTxt'));
            return _continue;
        }
    }

    if ($('#sickness').prop('checked'))
    {
        validateForm.input = $('#sicknessTxt')[0];
        _continue = (_continue && validateForm.IsOnlyEmpty());
        if (!_continue)
        {
            ScrollError($('#sicknessTxt'));
            return _continue;
        }
    }

    if ($('#mamografia').prop('checked'))
    {
        validateForm.input = $('#dateMamografia')[0];
        _continue = (_continue && validateForm.IsOnlyEmpty());
        if (!_continue)
        {
            ScrollError($('#dateMamografia'));
            return _continue;
        }
        validateForm.input = $('#nextMamografia')[0];
        _continue = (_continue && validateForm.IsOnlyEmpty());
        if (!_continue)
        {
            ScrollError($('#nextMamografia'));
            return _continue;
        }
    }

    if ($('#spQuirurgico').prop('checked'))
    {
        validateForm.input = $('#spQuirurgicoTxt')[0];
        _continue = (_continue && validateForm.IsOnlyEmpty());
        if (!_continue)
        {
            ScrollError($('#spQuirurgicoTxt'));
            return _continue;
        }
    }

    return _continue;
}

function ScrollError($Tag)
{
    $('html, body').animate({ scrollTop: $Tag.parent().offset().top }, 500);
}