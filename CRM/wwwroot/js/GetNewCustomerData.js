const form = document.getElementById('formCustomerCreate')
form.addEventListener('submit', (e) => {
    e.preventDefault()
    e.stopPropagation()
})

let inputs = form.querySelectorAll('input')
const resetForm = document.querySelector('#resetForm')
resetForm.addEventListener('click', () => {

    inputs.forEach((item) => {
        item.value = ''
    })
    for (let i = 0; i <= inputs.length; i++) {
        inputs[1].value = ""
    }
})

const dataSearchBtn = document.querySelector('#dataSearchBtn')
dataSearchBtn.addEventListener('click', () => {
    GetDataCnpj(cnpj.value)
})

const saveBtn = document.querySelector('#saveBtn')
saveBtn.addEventListener('click', () => {
    document.getElementById('formCustomerCreate').submit()
})




const cnpj = document.querySelector('#cnpj')
const urlApi = 'https://brasilapi.com.br/api/cnpj/v1/'
cnpj.addEventListener('keyup', (e) => {
    if (e.keyCode == 13) {

        GetDataCnpj(cnpj.value)
    }
})



async function GetDataCnpj(cnpj) {

    if (cnpj.length >= 14) {


        try {
            cnpj = cnpj.replace(/[.\-\/]/g, '')
            let response = await fetch(urlApi + cnpj)
            if (response.ok != true) {

                dataSearchError()
                throw new Error(`Erro HTTP: ${response.ok}`)
            }

            let data = await response.json().then(data => data)
            AddInputValues(data)
        }
        catch (error) {

            dataSearchError()
            console.log("Ocorreu um erro", error)


        }
    }
}
function dataSearchError() {
    document.querySelector('#cnpj').style.background = '#d92139'
    setTimeout(() => {
        document.querySelector('#cnpj').style.background = '#fff'
    }, 1000)

}


function AddInputValues(data) {
    document.querySelector('#razaoSocial').value = data.razao_social
    document.querySelector('#zipCodeInput').value = data.cep
    document.querySelector('#city').value = data.municipio
    document.querySelector('#uf').value = data.uf


}