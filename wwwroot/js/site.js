function manejoClientes()
{
    let checkbox = document.getElementById("NuevoCliente");
    let boton = document.getElementById("boton-cliente");
    if (checkbox.checked == true)
    {
        // Modo existente
        checkbox.checked = false;
        boton.innerHTML = "Nuevo";
        document.getElementById("nuevo-cliente").style.display = "none";
        setTimeout(1000);
        document.getElementById("selec-cliente").style.display = "initial";
    }
    else
    {
        // Modo nuevo
        checkbox.checked = true;
        boton.innerHTML = "Existente";
        document.getElementById("nuevo-cliente").style.display = "initial";
        document.getElementById("selec-cliente").style.display = "none";
    }
}
