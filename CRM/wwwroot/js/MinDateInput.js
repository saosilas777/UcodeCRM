let nextContactDate = document.querySelectorAll('.nextContactDate')
let hoje = new Date()
hoje.setMinutes(hoje.getMinutes() - hoje.getTimezoneOffset())
let dataMinima = hoje.toISOString().slice(0, 16)
nextContactDate.forEach((item) => {
	item.min = dataMinima
})