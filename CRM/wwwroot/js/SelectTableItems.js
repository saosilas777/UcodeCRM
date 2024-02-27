setTimeout(function () {
    clickChange()
}, 200)

function clickChange() {
    const ultimoContato = document.getElementById('proximoContato')
    ultimoContato.click()
   /* setTimeout(function () {
        ultimoContato.click()
    }, 400)*/
}
const tbody = document.getElementById('tbody')
let trs = tbody.querySelectorAll('tr')


const selection = document.getElementById('selection')

selection.addEventListener('change', function () {
    removeStyleAttribute()

    if (selection.value == 'Ativo') {

        trs.forEach(function (item) {
            if (item.textContent.includes('Inativo')) {
                item.style.display = 'none'
            }
        })
    }
    else if (selection.value == 'Todos') {
        removeStyleAttribute()
    }
    else if (selection.value == 'Inativo') {
        trs.forEach(function (item) {
            if (item.textContent.includes('Ativo')) {
                item.style.display = 'none'
            }

        })
    }


})



function removeStyleAttribute() {
    trs.forEach(function (item) {
        item.removeAttribute('style')

    })
}