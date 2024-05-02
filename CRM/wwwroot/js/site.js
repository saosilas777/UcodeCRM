let alerta = document.querySelector('.alert');

let _alert = document.getElementById('alert');


const customer = document.getElementById('customers')
const analytics = document.getElementById('analytics')
const home = document.getElementById('home')
const sales = document.getElementById('sales')
const keyBtn = document.getElementById('keyBtn')

const url = window.location.pathname

if (url.includes('Home')) {
    home.classList.add('bgImageLink')
}
if (url.includes('Analytics')) {
    analytics.classList.add('bgImageLink')
}
if (url.includes('Customer')) {
    customer.classList.add('bgImageLink')
}
if (url.includes('ChangePassword')) {
    keyBtn.classList.add('keyBtnColor')
}
if (url.includes('Seller')) {
    sales.classList.add('bgImageLink')
}

$(document).ready(function () {
    const myTable = "#myTable"
    const myTable2 = "#myTable2"
    getTable(myTable)
    getTable(myTable2)
    SortingDates()
    ConvertDates()

    function getTable(id) {

            new DataTable(id, {
                lengthMenu: [
                    [-1, 5, 10, 15, 20],
                    ['Todos', 5, 10, 15, 20]
                ],

                language: {
                    lengthMenu: "Listar _MENU_ clientes",
                    search: "Procurar ",
                    processing: "Processando...",
                    emptyTable: "Nenhum registro encontrado",
                    paginate: {
                        first: "Primeira página",
                        previous: "Anterior",
                        next: "Próximo",
                        last: "Última página"
                    }
                },
                columnDefs: [

                    { orderable: false, targets: [2] }


                ]
            });
        
    }
});

let today = new Date()
let todayBr = new Date(today).toLocaleDateString('pt-BR')

function SortingDates() {
    let datas = document.querySelectorAll('.lastPurchase')

    datas.forEach(function (data) {
        let date = data.innerText
        const p = data.querySelector('p')
        const id = p.getAttribute('id')
        let _newdate = new Date(date).toLocaleDateString('pt-BR')
        const dateLong = new Date(date)
        if (today > dateLong) {
            data.innerHTML = `<p id="${id}" class="lastPurchaseRed">${_newdate}</p>`
        }
        else {
            data.innerHTML = `<p class="lastPurchase">${_newdate}</p>`
        }
    })
}
function ConvertDates() {
    let datas = document.querySelectorAll('.dateConvert')
    datas.forEach(function (data) {
        let date = data.innerText
        let _newdate = new Date(date).toLocaleDateString('pt-BR')
        const dateLong = new Date(date)
        data.innerHTML = `<p>${_newdate}</p>`
        
    })
}

customer.addEventListener('click', () => {
    const token = localStorage.getItem("Token")
    const customersInput = document.getElementById('customersInput')
    const customersFormBtn = document.getElementById('customersFormBtn')
    customersInput.setAttribute("value", token)
    customersFormBtn.click()
})

const btnSendFile = document.getElementById('btn-send-file')

btnSendFile.addEventListener('change', () => {
    document.getElementById('inputFileName').innerText = btnSendFile.files[0].name
})