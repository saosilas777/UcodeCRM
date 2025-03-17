let currentDate = new Date()


let initialDate = new Date()
let finalDate = new Date()
let workDays = 0
let workedDays = 0
let remainingDays = 0

const totalSales = parseFloat(document.getElementById('totalSales').innerText)
let salesToday = document.getElementById('salesToday')
let salesPrevision = document.getElementById('salesPrevision')

if (currentDate.getDate() > 20) {
    initialDate = new Date(2025, currentDate.getMonth(), 21)
    finalDate = new Date(2025, currentDate.getMonth() + 1, 20)
}
else {
    initialDate = new Date(2025, currentDate.getMonth() - 1, 21)
    finalDate = new Date(2025, currentDate.getMonth(), 20)
}


function AddDays() {
    currentDate.setDate(currentDate.getDate() - 1)
    currentDate.setHours(0)
    currentDate.setMinutes(0)
    currentDate.setSeconds(0)
    while (initialDate <= finalDate) {

        if (initialDate.getDay() != 0 && initialDate.getDay() != 6) {
            workDays++
            if (initialDate < currentDate) {
                workedDays++
            }
            
        }
        initialDate.setDate(initialDate.getDate() + 1)
    }
    remainingDays = workDays - workedDays
    salesToday.innerText = `Média dia: ${(parseFloat(totalSales) / workedDays).toLocaleString('pt-br', { style: 'currency', currency: 'BRL' })}`
    salesPrevision.innerText = `Prev. mensal: ${(parseFloat(totalSales) / workedDays * workDays).toLocaleString('pt-br', { style: 'currency', currency: 'BRL' })}`


}
AddDays()