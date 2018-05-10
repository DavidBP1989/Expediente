
function InitPage(PatientSex, PatientAge)
{
    var Color;
    PatientSex = PatientSex.toLowerCase();

    if (PatientSex == 'm')
    {
        if (PatientAge >= 0 && PatientAge <= 18) Color = '#A8EDFF';
        else if (PatientAge >= 19 && PatientAge <= 60) Color = '#FFCC66';
        else if (PatientAge >= 61)
            Color = '#C8C8C8';
    } else if (PatientSex == 'f')
    {
        if (PatientAge >= 0 && PatientAge <= 11) Color = '#FF99FF';
        else if (PatientAge >= 12)
            Color = '#a23cdc';
    }

    $('.header-title').css('background-color', Color);
}


var MaxImage = 1024 * 2048;

function ChangeImagePerfil()
{
    var Image = $('#ImagePerfil');
    if (Image.val() != '')
    {
        if (Image[0].files[0].size <= MaxImage)
        {
            $('#ErrorMaxImage').hide();

            var fd = new FormData();
            fd.append('fileInput', Image[0].files[0]);
            $.ajax({
                url: '../Images/ChangeImgPatient'
                , type: 'POST'
                , cache: false
                , data: fd
                , processData: false
                , contentType: false
                , beforeSend: function () { waitingDialog.show('Cargando Imagen', { dialogSize: 'sm' }); }
                , success: function (json)
                {
                    switch (parseInt(json.status))
                    {
                        case 200:
                            location.reload();
                            break;
                        case 415:
                            $('#ErrorMaxImage').show();
                            break;
                        case 500:
                            break;
                    }
                }
            })
        }
    }
}


function DeleteImage(Image, Category) {
    $.ajax({
        url: 'Delete'
        , data: { Image: Image, Category: Category }
        , method: 'POST'
    }).success(function (s) {
        if (s.status)
        {
            $.fancybox.close();
            window.location.reload();
        }
    })
}