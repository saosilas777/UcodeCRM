const selection = document.getElementById('selection')
const statusValue = document.getElementById('statusValue')
const statusSend = document.getElementById('statusSend')

selection.addEventListener('change', () => {
	let trs = tbody.querySelectorAll('tr')

	trs.forEach(function (item) {
		item.classList.remove('hide')
	})

	if (selection.value != 'Todos') {
		trs.forEach(function (item) {

			let status = item.querySelector('.status').innerText


			if (status.toLowerCase() != selection.value.toLowerCase()) {
				item.classList.add('hide')
			}
			else {

			}

		})
	}

})