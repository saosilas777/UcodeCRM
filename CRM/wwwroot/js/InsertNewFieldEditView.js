const addPhoneNumberBtn = document.getElementById('addPhoneNumber')
const phoneNumber = document.getElementById('phoneNumber')
const addEmail = document.getElementById('addEmail')
const addEmailBtn = document.getElementById('addEmailBtn')

addPhoneNumberBtn.addEventListener('click', (e) => {
	e.stopPropagation()
	e.preventDefault()
	InsertPhoneField()
})
addEmailBtn.addEventListener('click', (e) => {
	e.stopPropagation()
	e.preventDefault()
	InsertEmailField()
})


let countEmail = 0;
let countPhone = 0;
function InsertPhoneField() {

	if (countPhone < 2) {
		phoneNumber.insertAdjacentHTML('beforeend', `<label class="newField">
												Telefone
												<input class="editable" value="" name="Phones"  />
											</label>`)
	}
	countPhone++
}

function InsertEmailField() {
	if (countEmail < 2) {
		addEmail.insertAdjacentHTML('beforeend', `<div id="addEmail">
										<label class="">
											Email
											<input class="editable" value="" name="email" />
										</label>


									</div>`)
	}
	countEmail++
}