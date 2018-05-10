
function Init()
{
    $('.calendar').each(function (c) {
        var picker = new Pikaday({
            field: this
        })
    })

    $('* .checkbox').map(function (index, ck) {
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
}

function Post() {
    $('#formObs').submit();
}