const ZipCodeBtn = document.querySelector('#zipCodeBtn')
const zipCodeInput = document.querySelector('#zipCodeInput')
const city = document.querySelector('#city')
const uf = document.querySelector('#uf')
const alertZipCode = document.querySelector('.alertZipCode')

ZipCodeBtn.addEventListener('click', function (e) {
    e.preventDefault()
    e.stopPropagation()
    GetZipCode(zipCodeInput.value)
})

zipCodeInput.addEventListener('keyup', (e) => {
    if (e.key === 'Enter') {
        if (zipCodeInput.value.length == 8) {
            GetZipCode(zipCodeInput.value)
        }
        else {
            ErrorAlert()
        }
        
    }
})

async function GetZipCode(zipCode) {
    if (zipCode.length != 8) {
        ErrorAlert()
    }
    let error
    const url = "https://viacep.com.br/ws/" + zipCode + "/json/"
    let data = await fetch(url)
        .then(response => response.json())
        .then(data => data)
        .catch(err => error = err)
    if (error) {
        ErrorAlert()
    }
    if (data.localidade != undefined) {
        city.value = data.localidade;
        uf.value = data.uf
        //remover após ajustar layout da tela de cadastro
        city.classList.add('inputFocus')
        uf.classList.add('inputFocus')
        setInterval(() => {
            city.classList.remove('inputFocus')
            uf.classList.remove('inputFocus')
        },2500)

    }
    else {
        ErrorAlert()
    }
    
}
function ErrorAlert() {
    alertZipCode.style.display = 'flex'
    setTimeout(() => {
        alertZipCode.style.display = 'none'
    }, 5000)

    return
}
const saveBtn = document.querySelector('.saveBtn')
saveBtn.addEventListener('submit', (e) => {
    e.preventDefault()
    e.stopPropagation()
})