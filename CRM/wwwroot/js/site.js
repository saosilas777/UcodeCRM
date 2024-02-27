let alerta = document.querySelector('.alert');
/*
setTimeout(function () {
    alerta.style.transform = 'translateY(-10rem)';
}, 3000)*/


let _alert = document.getElementById('alert');


const customer = document.getElementById('Customers')
const analytics = document.getElementById('Analytics')
const home = document.getElementById('Home')
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


/*const insertDataBtn = document.getElementById('insertDataBtn')
const insert = document.getElementById('insert')

insertDataBtn.addEventListener('click', function () {
    insert.style.display = "block";
})*/

const menuHideBtn = document.getElementById('menuHideBtn')
$(document).ready(function () {
    const myTable = "#myTable"
    const myTable2 = "#myTable2"
    getTable(myTable)
    getTable(myTable2)
    SortingDates()

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
        let _newdate = new Date(date).toLocaleDateString('pt-BR')
        const dateLong = new Date(date)
        if (today > dateLong) {
            data.innerHTML = `<p class="lastPurchaseRed">${_newdate}</p>`
        }
        else {
            data.innerHTML = `<p class="lastPurchase">${_newdate}</p>`
        }

    })


}


customer.addEventListener('click', () => {
    const token = localStorage.getItem("Token")

    const customersInput = document.getElementById('customersInput')
    const customersFormBtn = document.getElementById('customersFormBtn')
    customersInput.setAttribute("value", token)
    customersFormBtn.click()


    //$.ajax({
    //    type: 'POST',
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    url: '/Customer/GetCustomers/',
    //    cache: false,
    //    data: token
    //    //    //success: (e) => {
    //    //    //    console.log('success')
    //    //    //},
    //    //    //error: (e) => {
    //    //    //    console.log('error')
    //    //    //},
    //    //    //complete: (e) => {
    //    //    //    console.log('complete');
    //    //    //}
    //    //});


    //})
})

const btnSendFile = document.getElementById('btn-send-file')

btnSendFile.addEventListener('change', () => {
    document.getElementById('inputFileName').innerText = btnSendFile.files[0].name
})


/*menuHideBtn.addEventListener('click', function () {
    const main = document.querySelector('.main')
    const aside = document.querySelector('.aside')
    const headerNav = document.querySelector('.header_nav')
 
    headerNav.style.opacity = '0';
 
    aside.style.marginLeft = '-10.525rem';
    main.style.marginLeft = '3.1rem';
})*/