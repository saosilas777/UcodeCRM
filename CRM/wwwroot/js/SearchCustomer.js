const _customers = JSON.parse(localStorage.getItem("Customers"))
const searchIcon = document.querySelector('.searchIcon')
const reciverCustomer = document.querySelector('#reciverCustomer')
const input_search = document.getElementById("input_search")

input_search.addEventListener("keyup", (e) => {

	if (e.keyCode == '8' || e.keyCode == '46') {
		reciverCustomer.innerHTML = ''
		reciverCustomer.style.display = 'none'
	}
	if (e.keyCode == '13') {
		reciverCustomer.querySelector('.getUserList').click()
	}

	if (input_search.value.length >= 1) {
		reciverCustomer.innerHTML = ''
		
		var cust = _customers.filter((item) => item.customerName.toLowerCase().includes(input_search.value)
			|| item.customerCode.toLowerCase().includes(input_search.value)
		)
		if (cust.length == 0) {

			reciverCustomer.style.display = 'none'
		}
		else {
			reciverCustomer.style.display = 'flex'
			cust.forEach((item) =>
				reciverCustomer.innerHTML += `<a class="getUserList" href="/Customers/Editar/${item.customerId}">${item.customerCode} - ${item.customerName}</a>`
			)

		}

	}
})