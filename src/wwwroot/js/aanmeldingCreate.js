function Toggle16(checkbox)
{
    targetDiv = document.getElementById("VoogdAanmelding");

    naamvoogdInput = document.getElementById("NaamVoogd");
    emailvoogdInput = document.getElementById("EmailVoogd");
    telefoonvoogdInput = document.getElementById("TelefoonVoogd");

    if(checkbox.checked == true)
    {
        targetDiv.classList.remove("d-none")
        naamvoogdInput.disabled = false;
        emailvoogdInput.disabled = false;
        telefoonvoogdInput.disabled = false;
    }
    
    else
    {
        targetDiv.classList.add("d-none")
        naamvoogdInput.value = "";
        naamvoogdInput.disabled = true;
        emailvoogdInput.value = "";
        emailvoogdInput.disabled = true;
        telefoonvoogdInput.value = "";
        telefoonvoogdInput.disabled = true;
    }

}