function modif(val) {
    var ava = document.getElementById("avancement");
    if ((ava.value + val) <= ava.max && (ava.value + val) > 0) {
        ava.value += val;
    }
    avancement();
}

function avancement() {
    var ava = document.getElementById("avancement");
    var prc = document.getElementById("pourcentage");
    prc.innerHTML = ava.value + "%";
}

avancement(); //Initialisation