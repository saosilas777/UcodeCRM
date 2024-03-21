const analytics_totalSalaries = document.getElementById('analytics_totalSalaries')
const analytics_totalSalariesBtn = document.querySelector('.analytics_totalSalariesBtn')

analytics_totalSalariesBtn.addEventListener('click', () => {
    analytics_totalSalaries.classList.toggle('totalSalariesShow')
})