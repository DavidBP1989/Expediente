var validateForm =
{
    input: null
    , IsEmail: function () {
        var reg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/igm;
        if (reg.test(this.input.value)) {
            this.Success();
            return true;
        } else {
            this.Error();
            return false;
        }
    }

    , IsEmpty: function () {
        if (/^[a-zñÑ](?:\s?[a-zñÑ]+)*$/i.test(this.input.value.trim())) {
            this.Success();
            return true;
        } else {
            this.Error();
            return false;
        }
    }

    , IsOnlyLettersAndNumbers() {
        if (/^[a-zñáéíóúA-ZÑñÁÉÍÓÚ0-9][a-zñáéíóúA-ZÑñÁÉÍÓÚ0-9\s]*$/.test(this.input.value))
        {
            this.Success();
            return true;
        } else
        {
            this.Error();
            return false;
        }
    }

    , IsOnlyLetters: function () {
        if (/^[a-zñáéíóúA-ZÑñÁÉÍÓÚ][a-zñáéíóúA-ZÑñÁÉÍÓÚ\s]*$/.test(this.input.value))
        {
            this.Success();
            return true;
        } else
        {
            this.Error();
            return false;
        }
    }

    , IsOnlyEmpty: function () {
        if (this.input.value.trim() !== '') {
            this.Success();
            return true;
        } else {
            this.Error();
            return false;
        }
    }

    , IsNumberAndSpaces: function () {
        if (/^[a-z0-9ñÑ](?:\s+[a-z0-9ñÑ]+)*$/i.test(this.input.value.trim())) {
            this.Success();
            return true;
        } else {
            this.Error();
            return false;
        }
    }

    , IsNumber: function () {
        if (/^\d[0-9]*$/.test(this.input.value)) {
            this.Success();
            return true;
        } else {
            this.Error();
            return false;
        }
    }



    , Success: function () {
        this.input.style.backgroundColor = '';
        this.input.style.color = '';
        this.input.style.border = '';
    }
    , Error: function () {
        this.input.style.backgroundColor = '#F2DEDE';
        this.input.style.color = '#B94A48';
        this.input.style.border = '1px solid';
    }
}