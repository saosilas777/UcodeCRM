const analytics_totalSalaries = document.getElementById('analytics_totalSalaries')
const showCash = document.getElementById('showCash')
const analytics_totalSalariesBtn = document.querySelector('.analytics_totalSalariesBtn')

analytics_totalSalariesBtn.addEventListener('click', () => {

    const ticket_content = analytics_totalSalaries.querySelectorAll('.ticket_content')

    ticket_content.forEach(function (item) {
        const valueHide = item.querySelector('.valueHide').textContent
        const reciveValue = item.querySelector('.showValue')
        if (reciveValue.textContent == 'R$ ******') {
            reciveValue.innerHTML = `${valueHide}`
        }
        else {
            reciveValue.innerHTML = 'R$ ******'
        }
    })
})

