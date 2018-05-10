var urlFillState;
var urlFillCitie;

function Init(data, url_fillState, url_fillCities)
{
    urlFillState = url_fillState;
    urlFillCitie = url_fillCities;

    var showEducation = $('#showEducation');
    if (data.education == '9')
        showEducation.show();
    else showEducation.hide();

    $('#education').change(function () {
        if (this.value == 9)
            showEducation.show();
        else showEducation.hide();
    })


    $('#ShowNewCity').hide();
    $('#city').change(function () {
        if (this.value == -2)
            $('#ShowNewCity').show();
        else $('#ShowNewCity').hide();
    })
}

function FillStates() {
    var IdCountry = $('#country').val();

    $.ajax({
        url: urlFillState
        , type: 'GET'
        , dataType: 'json'
        , data: { idCountry: IdCountry }
        , success: function (response) {
            var state = $('#state');
            state.empty();
            $.each(response, function (i, v) {
                state.append($('<option></option>').val(v.Value).html(v.Text));
            })
        }
    })
}


function FillCities() {
    var IdState = $('#state').val();
    var IdCountry = $('#country').val();

    $.ajax({
        url: urlFillCitie
        , type: 'GET'
        , dataType: 'json'
        , data: { idState: IdState, idCountry: IdCountry }
        , success: function (response) {
            var city = $('#city');
            city.empty();
            $.each(response, function (i, v) {
                city.append($('<option></option>').val(v.Value).html(v.Text));
            })
        }
    })
}


function SendForm()
{

    var band = true;
    if ($('#showEducation').is(':visible'))
    {
        validateForm.input = $('#typeEducation')[0];
        band = validateForm.IsOnlyEmpty();
        if (!band)
        {
            ScrollError($('#typeEducation'));
            return;
        }
    }

    if ($('#ShowNewCity').is(':visible'))
    {
        validateForm.input = $('#OtherCity')[0];
        band = validateForm.IsOnlyEmpty();
        if (!band)
        {
            ScrollError($('#OtherCity'));
            return;
        }
    }

    var ind = -1;
    $('.letter').each(function (index) {
        validateForm.input = this;
        band = (band && validateForm.IsOnlyEmpty());
        if (!band && ind == -1)
            ind = index;
    })
    
    if (!band)
    {
        ScrollError($('.letter').eq(ind));
        return;
    }


    $('#infoForm').submit();     
}

function ScrollError($Tag)
{
    $('html, body').animate({ scrollTop: $Tag.parent().offset().top }, 500);
}