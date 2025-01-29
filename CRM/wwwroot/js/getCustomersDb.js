myTable = document.getElementById('myTable')

const trs = myTable.querySelectorAll("tr")
let customers = []

for (var i = 1; i < trs.length; i++) {

    let customerCode = trs[i].querySelector(".customerCode").textContent.replace(/\s/g, '')
    let customerName = trs[i].querySelector(".customerName").innerText
    let customerId = trs[i].querySelector(".customerId").textContent.replace(/\s/g, '')
    let nextContactDate = trs[i].querySelector(".data_de_contato").value
    customers.push({ customerCode, customerName, customerId, nextContactDate })

    localStorage.setItem("Customers", JSON.stringify(customers))
}