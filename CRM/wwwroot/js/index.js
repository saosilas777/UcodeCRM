/*let carteiraTotal = document.getElementById('TotalPortfolioValue').value
document.getElementById('TotalPortfolio').innerText = carteiraTotal
let clientesAtivos = document.getElementById('clientesAtivos')
let tktMedio = document.getElementById('tktMedio')
let churns = document.getElementById('churns')





churns.innerText = `${parseInt(Math.random() * 10).toString()}`
tktMedio.innerText = `R$ ${parseInt(Math.random() * 2000).toString()},00`
*//*carteiraTotal.innerText = `R$ ${parseInt(Math.random(10000) * 200000).toString()},00`*//*
clientesAtivos.innerText = `${parseInt(Math.random() * 1000).toString()}`

let number = (Math.random() * 10) * 1000

const currencyBRL = value => {
    return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}
carteiraTotal.innerText = currencyBRL(parseInt(number) * 100)
tktMedio.innerText = currencyBRL(parseInt(number) * 10)




*/