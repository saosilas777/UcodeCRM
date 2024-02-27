const token = document.URL.split('?Token=')

localStorage.setItem("Token", JSON.stringify(token[1]))