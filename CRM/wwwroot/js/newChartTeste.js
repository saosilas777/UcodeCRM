
	let backgroundColor = ""
	const ctx1 = document.getElementById('myChart');
	const ctx2 = document.getElementById('myChart2');
	let _data = [];
	_data[0] = @januarySales
	_data[1] = @feb
	_data[2] = @mar
	_data[3] = @apr
	_data[4] = @may
	_data[5] = @jun
	_data[6] = @jul
	_data[7] = @aug
	_data[8] = @sep
	_data[9] = @oct
	_data[10] = @nov
	_data[11] = @dec
	new Chart(ctx1, {

		type: 'bar',
	data: {
		labels: ['jan', 'fev', 'mar', 'abr', 'maio', 'jun', 'jul', 'ago', 'set', 'out', 'nov', 'dez'],
	datasets: [{
		label: 'Vendas Mês',
	data: _data,
	borderWidth: 0,

																backgroundColor: color => {
		// console.log(color)
		let colors = []
	for (let i = 0; i < _data.length; i++) {
																		if (_data[i] >= '100000') {

		colors[i] = "rgba(21,168,21,0.7)"
	}
	else {
		colors[i] = "rgba(227, 52, 102, 0.7)"
	}
																	}
	return colors
																},
																borderColor: color => {
		// console.log(color)
		let colors = []
	for (let i = 0; i < _data.length; i++) {
																		if (_data[i] > 10) {

		colors[i] = "rgba(227, 52, 102,1)"
	}
	else {
		colors[i] = "rgba(232, 116, 155)"
	}
																	}
	return colors
																},
															}]
														},

	options: {
		maintainAspectRatio: false,
	aspectRatio: 1,
	responsive: true,
	barPercentage: 0.7,
	categoryPercentage: 0.6,
	scales: {
		x: {
		grid: {
		display: false,
																	},
	ticks: {
		display: true,
	color: 'rgb(0, 0, 0)',
	padding: 10,
	font: {
		size: 14,
	weight: 300,
	family: "Open-Sans",
	style: 'normal',
	lineHeight: 0,


																		},
																	},

																},

															},
	plugins: {

		tooltip: {
		display: true,
	borderWidth: 0,
	backgroundColor: 'rgba(0, 0, 255, 0.6)',
	intersect: false
																},
	legend: {
		display: false
																}
															},
														},

													});

	let _data2 = [];
	_data2[0] = @Model.CustomersServedPerMonth.January
	_data2[1] = @Model.CustomersServedPerMonth.February
	_data2[2] = @Model.CustomersServedPerMonth.March
	_data2[3] = @Model.CustomersServedPerMonth.April
	_data2[4] = @Model.CustomersServedPerMonth.May
	_data2[5] = @Model.CustomersServedPerMonth.June
	_data2[6] = @Model.CustomersServedPerMonth.July
	_data2[7] = @Model.CustomersServedPerMonth.August
	_data2[8] = @Model.CustomersServedPerMonth.September
	_data2[9] = @Model.CustomersServedPerMonth.Octuber
	_data2[10] = @Model.CustomersServedPerMonth.November
	_data2[11] = @Model.CustomersServedPerMonth.December

	new Chart(ctx2, {

		type: 'bar',
	data: {
		labels: ['jan', 'fev', 'mar', 'abr', 'maio', 'jun', 'jul', 'ago', 'set', 'out', 'nov', 'dez'],
	datasets: [{
		label: 'CLientes atendidos no mês',
	data: _data2,
	borderWidth: 0,
																backgroundColor: color => {
		let colors = []
	for (let i = 0; i < _data2.length; i++) {
																		if (_data2[i] > '50') {

		colors[i] = "rgba(21,168,21,0.7)"
	}
	else {
		colors[i] = "rgba(227, 52, 102, 0.7)"
	}
																	}
	return colors
																},
																borderColor: color => {
		let colors = []
	for (let i = 0; i < _data2.length; i++) {
																		if (_data[i] > '50') {

		colors[i] = "rgba(227, 52, 102,1)"
	}
	else {
		colors[i] = "rgba(232, 116, 155)"
	}
																	}
	return colors
																},
															}]
														},

	options: {
		maintainAspectRatio: false,
	aspectRatio: 1,
	responsive: true,
	barPercentage: 0.5,
	categoryPercentage: 0.5,
	scales: {
		x: {
		grid: {
		display: false,
																	},
	ticks: {
		display: true,
	color: 'rgb(0, 0, 0)',
	padding: 10,
	font: {
		size: 14,
	weight: 300,
	family: "Open-Sans",
	style: 'normal',
	lineHeight: 0,
																		},
																	},

																},
															},
	plugins: {

		tooltip: {
		display: true,
	borderWidth: 0,
	backgroundColor: 'rgba(0, 0, 255, 0.6)',
	intersect: false
																},
	legend: {
		display: false
																}
															},
														},

													});
