const customerDataEdit = document.querySelector('#customerDataEdit')
const customerDataEditForm = document.querySelector('.customerDataEditForm')
const customerDataEditFormCancel = document.querySelector('#customerDataEditFormCancel')
const customerDataEditFormSave = document.querySelector('#customerDataEditFormSave')



customerDataEdit.addEventListener('click', () => {

    customerDataEditForm.style.display = 'flex'
})
customerDataEditFormCancel.addEventListener('click', () => {

    customerDataEditForm.style.display = 'none'
})
customerDataEditFormSave.addEventListener('click', () => {

    customerDataEditForm.submit()
})
































//(function () {
//    'use strict'


//    const inputs = document.querySelectorAll('.editable')

//    const btnEdit = document.querySelector('#btnEdit')

//    const editOrDelete = document.querySelector('#editOrDelete')
//    const cancelOrSave = document.querySelector('#cancelOrSave')

//    const btnEditCancelEdit = document.getElementById('cancelEdit')
//    btnEditCancelEdit.addEventListener('click', ButtonsShow)

//    const deletarBtn = document.getElementById('deletarBtn')
//    deletarBtn.addEventListener('click', confirmDelete);

//    const cancelDeleteBtn = document.getElementById('cancelDelete')
//    cancelDeleteBtn.addEventListener('click', cancelDelete)


//    const deleteConfirm = document.getElementById('deleteConfirm')

//    const btnconfirmDelete = document.getElementById('confirmDelete')

//    const btnSubmitEdit = document.querySelector('#btnSubmitEdit')
//    const btnSubmitDelete = document.querySelector('#btnSubmitDelete')

//    const btnSave = document.getElementById('card_cta-saveChanges')

//    const spanAlert = document.getElementById('spanAlertContainer')



//    btnEdit.addEventListener('click', disable)

//    btnSave.addEventListener('click', SubmitEdit)
//    btnconfirmDelete.addEventListener('click', SubmitDelete)


//    function SubmitEdit() {
//        btnSubmitEdit.disabled = !btnSubmitEdit.disabled

//        btnSubmitEdit.click();
//    }
//    function SubmitDelete() {
//        btnSubmitDelete.disabled = !btnSubmitDelete.disabled
//        btnSubmitDelete.click();
//    }

//    function disable() {
//        inputs.forEach(function (item) {
//            item.disabled = !item.disabled
//        })
//        ButtonsHidden()
//    }



//    function ButtonsShow() {
//        disable()
//        editOrDelete.style.display = 'flex'
//        cancelOrSave.style.display = 'none'
//    }
//    function ButtonsHidden() {
//        editOrDelete.style.display = 'none'
//        cancelOrSave.style.display = 'block'

//    }

//    function confirmDelete() {
//        editOrDelete.style.display = 'none'
//        deleteConfirm.style.display = 'flex'
//        spanAlert.style.display = 'flex'

//    }

//    function cancelDelete() {
//        spanAlert.style.display = 'none'
//        editOrDelete.style.display = 'flex'
//        deleteConfirm.style.display = 'none'
//    }



//    const insertBtn = document.getElementById('insertBtn')
//    const cancelInsertBtn = document.getElementById('cancelInsertBtn')
//    const saveInsertBtn = document.getElementById('saveInsertBtn')
//    const editableInsert = document.querySelectorAll('.editableInsert')


//    function InsertRegistration() {
//        editableInsert.forEach(function (item) {
//            item.disabled = !item.disabled
//        })
//    }

//    const ContactRecordsAnotation = document.getElementById('ContactRecordsAnotation')
//    ContactRecordsAnotation.addEventListener('focus', function () {
//        const ContactRecordsDate = document.getElementById('ContactRecordsDate')
//        ContactRecordsDate.value = '';
//    })


//    const lastPurchaseValue = document.getElementById('lastPurchaseValue')
//    const value = lastPurchaseValue.value
//    lastPurchaseValue.addEventListener('focus', () => {

//        lastPurchaseValue.value = ''
//    })

//    lastPurchaseValue.addEventListener('blur',()=>{
//        if (lastPurchaseValue.value == '') {
//            lastPurchaseValue.value = value
//        }
//    })
//})()

