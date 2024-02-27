let email = document.getElementById('email')
let phone = document.getElementById('phone')
let InsertEmailFieldBtn = document.getElementById('InsertEmailField')
const InsertPhoneFieldBtn = document.getElementById('InsertPhoneField')

InsertEmailFieldBtn.addEventListener('click', InsertEmailField)
InsertPhoneFieldBtn.addEventListener('click', InsertPhoneField)

let countEmail = 0;
let countPhone = 0;
function InsertEmailField() {

    if (countEmail < 2) {
        email.insertAdjacentHTML('beforeend', '<label for="email" class="span3"><p class="newField">Email</p><input id="email" type="email" asp-for="Emails" name="Emails" value="" /></label>')
    }
    countEmail++
}

function InsertPhoneField() {
    if (countPhone < 2) {
        phone.insertAdjacentHTML('beforeend', '<label for="phone" class="span3"><p class="newField">Telefone</p><input id="phone" type="text" asp-for="Phones" name="Phones" value="" /></label>')
    }
        countPhone++
}