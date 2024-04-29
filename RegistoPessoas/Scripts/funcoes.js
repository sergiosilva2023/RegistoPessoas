




function somar(valor1, valor2) {
    var resultado = valor1 + valor2;
    return resultado;
}

function mostrarResultado(){
    debugger
    var valor1 = document.getElementById('valor1').value;
    var valor2 = document.getElementById('valor2').value;
    
    valor1 = parseInt(valor1);
    valor2 = parseInt(valor2);

    var resultado = somar(valor1, valor2);

    document.getElementById('resultado').innerHTML = resultado;
}