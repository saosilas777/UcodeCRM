const dataFindBtn = document.querySelector('.fa-magnifying-glass')
const initialDate = document.querySelector('#initialDate')
const finalDate = document.querySelector('#finalDate')


function getDate(){
   
    const _initialDate = localStorage.getItem('initialDate', initialDate)
    const _finalDate = localStorage.getItem('finalDate', finalDate)

    if (_initialDate != "" && _initialDate != null && _finalDate != "" && _finalDate != null) {
        initialDate.value = _initialDate
        finalDate.value = _finalDate
    }
   
}


dataFindBtn.addEventListener('click', (e) => {
    const btn = e.target.closest('.dataFindBtn')
    if (btn) {
        if (initialDate != "" && initialDate != null && finalDate != "" && finalDate != null) {
            localStorage.setItem('initialDate', initialDate.value)
            localStorage.setItem('finalDate', finalDate.value)
        }
       
        
    }


})
$(document).ready(function() {
    getDate()
})
