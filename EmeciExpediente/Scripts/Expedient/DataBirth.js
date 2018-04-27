var showPregnantType;
var showPatalogico;
var showTypeDistocia;
var showReasonDistocia;
var showFolio;
var showTypeLactancia;
var showFormula;

function Variables()
{
    showPregnantType = $('#showPregnantType');
    showPatalogico = $('#showPatologico');
    showTypeDistocia = $('#showTypeDistocia');
    showReasonDistocia = $('#showReasonDistocia');
    showFolio = $('#showFolio');
    showTypeLactancia = $('#showTypeLactancia');
    showFormula = $('#showFormula');
}

function Init(data)
{
    Variables();

    if (data.pregnantType == 0)
        showPregnantType.hide();
    else showPregnantType.show();

    if (data.obtainedByBirth == 0)
        showPatalogico.hide();
    else showPatalogico.show();

    if (data.typeDistocia == 2)
        showTypeDistocia.show();
    else showTypeDistocia.hide();

    if (data.reasonDistocia == 3)
        showReasonDistocia.show();
    else showReasonDistocia.hide();

    if (data.tamiz == 0)
        showFolio.hide();
    else showFolio.show();

    if (data.lactancia == 0)
        showTypeLactancia.show();
    else {
        showTypeLactancia.hide();
        showFormula.hide();
    }

    if (data.typeLactancia == 0)
        showFormula.hide();
    else showFormula.show();



    $('#pregnantType').change(function () {
        if (this.value == 0)
            showPregnantType.hide();
        else showPregnantType.show();
    })

    $('#obtainedByBirth').change(function () {
        if (this.value == 0)
            showPatalogico.hide();
        else showPatalogico.show();
    })

    $('#typeDistocia').change(function () {
        if (this.value == 2)
            showTypeDistocia.show();
        else showTypeDistocia.hide();
    })

    $('#reasonDistocia').change(function () {
        if (this.value == 3)
            showReasonDistocia.show();
        else showReasonDistocia.hide();
    })

    $('#tamiz').change(function () {
        if (this.value == 0)
            showFolio.hide();
        else showFolio.show();
    })

    $('#lactancia').change(function () {
        if (this.value == 0) {
            showTypeLactancia.show();
            if ($('#typeLactancia').val() == 1)
                showFormula.show();
        }
        else {
            showTypeLactancia.hide();
            showFormula.hide();
        }
    })

    $('#typeLactancia').change(function () {
        if (this.value == 0)
            showFormula.hide();
        else showFormula.show();
    })


    $('.calendar').each(function (c) {
        var picker = new Pikaday({
            field: this
        })
    })
}



function Post()
{
    var band = true;
    if (showTypeDistocia.is(':visible'))
    {
        validateForm.input = $('#typeDistociaInfo')[0];
        band = (validateForm.IsOnlyEmpty() && band)
        if (!band)
        {
            ScrollError($('#typeDistociaInfo'));
            return;
        }
    }


    if (showReasonDistocia.is(':visible'))
    {
        validateForm.input = $('#reasonDistociaInfo')[0];
        band = (validateForm.IsOnlyEmpty() && band);
        if (!band)
        {
            ScrollError($('#reasonDistociaInfo'));
            return;
        }
    }

    
    if (showFolio.is(':visible'))
    {
        validateForm.input = $('#folio')[0];
        band = (validateForm.IsOnlyEmpty() && band);
        if (!band)
        {
            ScrollError($('#folio'));
            return;
        }
    }


    if (showFormula.is(':visible'))
    {
        validateForm.input = $('#formula')[0];
        band = (validateForm.IsOnlyEmpty() && band);
        if (!band)
        {
            ScrollError($('#formula'));
            return;
        }
    }


    $('#BirthForm').submit();

}

function ScrollError($Tag)
{
    $('html, body').animate({ scrollTop: $Tag.parent().offset().top }, 500);
}