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

// code juste copié à adapter à la balise //
//$(document).ready(function() {
//    var progressbar = $('#progressbar'),
//      max = progressbar.attr('max'),
//      time = (1000/max)*5,  
//        value = progressbar.val();
 
//    var loading = function() {
//        value += 1;
//        addValue = progressbar.val(value);
       
//        $('.progress-value').html(value + '%');
 
//        if (value == max) {
//            clearInterval(animate);                
//        }
//    };
 
//    var animate = setInterval(function() {
//        loading();
//    }, time);
//};