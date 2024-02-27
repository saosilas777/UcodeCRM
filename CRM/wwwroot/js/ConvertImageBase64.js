const btn_sendFile = document.getElementById('btn-send-file')
btn_sendFile.addEventListener('click', function () {
    document.getElementById('sendImgFile').removeAttribute("disabled")
})
let redimensionar = $('#apresentar').croppie({

    enableExif: true,
    enableOrientation: true,

    viewport: {
        width: 300,
        height: 300,
        type: 'square'
    },
    boundary: {
        width: 400,
        height: 400
    }




})
$('#btn-send-file').on('change', function () {

    var reader = new FileReader();
    
    reader.onload = function (e) {
        redimensionar.croppie('bind', {
            url: e.target.result,
        })

    }

    reader.readAsDataURL(this.files[0])
})

$('#sendImgFile').on('click', function () {
    redimensionar.croppie('result', {
        type: 'canvas',
        size: 'viewport'
    }).then(function (img) {
        sendImg(img)
    })
})


const sendImgFile = document.getElementById('sendImgFile')

sendImgFile.addEventListener('click', function () {
    document.getElementById('sendImg').removeAttribute("disabled")
})


function sendImg(url) {
    let send = document.querySelector('#btn-send-text')

    send.value = url
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