let currentDate = new Date()


let initialDate
let finalDate
let workDays = 0
let workedDays = 0
let remainingDays = 0
let hollydays = [];
GetHollidaysYear()

async function GetHollidaysYear() {
    currentYear = currentDate.getFullYear() - 1
    for (var j = 0; j <= 2; j++) {
       

        let url = "https://brasilapi.com.br/api/feriados/v1/" + currentYear
        let hollydaysInYear = await fetch(url)
            .then(response => response.json())
            .then(data => data)
            .catch(err => error = err)
        for (var i = 0; i < hollydaysInYear.length; i++) {
            let [year, month, day] = hollydaysInYear[i].date.split('-')
            const fomratedDate = `${day}/${month}/${year}`
             hollydays.push(fomratedDate.toString())
        }
        currentYear++

    }
    AddDays()
    
}


const totalSales = parseFloat(document.getElementById('totalSales').innerText)
let salesToday = document.getElementById('salesToday')
let salesPrevision = document.getElementById('salesPrevision')

if (currentDate.getDate() > 20) {
    initialDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 21)
    finalDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 20)
}
else {
    initialDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 21)
    finalDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 20)
}


function AddDays() {
    
    /*currentDate.setDate(currentDate.getDate() - 1)*/
    currentDate.setHours(0)
    currentDate.setMinutes(0)
    currentDate.setSeconds(0)
    while (initialDate <= finalDate) {
       
        if (initialDate.getDay() != 0 && initialDate.getDay() != 6 )  {
            workDays++

            if (initialDate <= currentDate) {
                workedDays++
            }
            
        }
        for (var h = 0; h < hollydays.length; h++) {
            if (hollydays[h] == initialDate.toLocaleDateString()) {
                workDays--
            }

        }
        initialDate.setDate(initialDate.getDate() + 1)


    }
    remainingDays = workDays - workedDays
    salesToday.innerText = `Média dia: ${(parseFloat(totalSales) / workedDays).toLocaleString('pt-br', { style: 'currency', currency: 'BRL' })}`
    salesPrevision.innerText = `Prev. mensal: ${(parseFloat(totalSales) / workedDays * workDays).toLocaleString('pt-br', { style: 'currency', currency: 'BRL' })}`


}
