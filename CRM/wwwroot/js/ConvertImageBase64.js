
const _fileInput = document.getElementById('photoFileInput')
const _fileInputUrl = document.getElementById('fileInputUrl')
const uploadPhotoCancel = document.getElementById('uploadPhotoCancel')
const uploadPhotoSave = document.getElementById('uploadPhotoSave')
const sendImgForm = document.querySelector('.sendImgForm')

_fileInput.addEventListener('change', function () {
    apresentar.style.display = 'block'
})
uploadPhotoCancel.addEventListener('click', function () {
    apresentar.style.display = 'none'
})
uploadPhotoSave.addEventListener('click', () => {
    setTimeout(function () {

        sendImgForm.submit()
    }, 1000)
})

let redimensionar = $('#apresentar').croppie({

    enableExif: true,
    enableOrientation: true,

    viewport: {
        width: 200,
        height: 200,
        type: 'square'
    },
    boundary: {
        width: 220,
        height: 220
    }

})
$('#photoFileInput').on('change', function () {

    var reader = new FileReader();
    reader.onload = function (e) {
        redimensionar.croppie('bind', {
            url: e.target.result,
        })

    }

    reader.readAsDataURL(this.files[0])
})

$('#uploadPhotoSave').on('click', function () {
    redimensionar.croppie('result', {
        type: 'canvas',
        size: 'viewport'
    }).then(function (img) {
        sendImg(img)
    })
})



function sendImg(url) {
    _fileInputUrl.value = url
}


/*function convertImage() {
    var receberArquivo = document.getElementById('btn-send-file').files;
    var inputText = document.getElementById('btn-send-text');

    console.log(receberArquivo);

    if (receberArquivo[0].type != "image/jpeg")
        return

    if (receberArquivo.length > 0) {

        carregarImagem = receberArquivo[0]


        var lerArquivo = new FileReader();

        lerArquivo.onload = function (arquivoCarregado) {


            let imagemBase64 = arquivoCarregado.target.result;

            var novaImagem = document.createElement('img')
            novaImagem.src = imagemBase64;
            console.log(imagemBase64);

            inputText.value = imagemBase64
            document.getElementById('apresentar').innerHTML = novaImagem.outerHTML;
        }
        lerArquivo.readAsDataURL(carregarImagem)
    }


}*/