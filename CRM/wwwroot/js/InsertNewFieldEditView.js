const addPhoneNumberBtn = document.getElementById('addPhoneNumber')
const phoneNumber = document.getElementById('phoneNumber')
const addEmail = document.getElementById('addEmail')
const addEmailBtn = document.getElementById('addEmailBtn')

addPhoneNumberBtn.addEventListener('click', () => {
	InsertPhoneField()
})
addEmailBtn.addEventListener('click', () => {
	InsertEmailField()
})


let countEmail = 0;
let countPhone = 0;
function InsertPhoneField() {

	if (countEmail < 2) {
		phoneNumber.insertAdjacentHTML('beforeend', `<label class="newField">
												Telefone
												<input class="editable" value="" name="Phones" disabled />
											</label>`)
	}
	countEmail++
}

function InsertEmailField() {
	if (countPhone < 2) {
		addEmail.insertAdjacentHTML('beforeend', `<label class="newField">
												Email:
												<input class="editable" value="" name="Emails" disabled />
											</label>`)
	}
	countPhone++
}