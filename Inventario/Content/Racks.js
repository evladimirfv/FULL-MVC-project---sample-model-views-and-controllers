



function drawNow(_altura,_equipos) {
    
    drawRack(_altura);
     // drawEquipos(_equipos);
    drawEquiposDesdeU(_altura, _equipos);
}

function drawLine() {
    var c = document.getElementById("myCanvas");
    var ctx = c.getContext("2d");
    ctx.moveTo(0, 0);
    ctx.lineTo(200, 100);
    ctx.stroke();
}


// Una U   4,445 cm  = 10 pixels.   =>   1 px = 0,4445 cm
// Ancho estandard  60, 80 cm  (Tomo 80 como estandard del sistema)
// 80 cm a pixels =>   179,97 (180 pixels)


// Armario rack:
// Tienen una anchura estándar de 600, 800 mm
// Fondo de 600, 800, 900, 1000 y 1200mm.
// Se suelen fabricar con alturas comprendidas entre 12U y 47U  o mas pequeñas.

function drawRack(_altura) {

  
    var scale = 1.8;
    var u = 10;  // One U  en pixels
    var altura = _altura;

    if (altura == '' || altura == undefined || altura == null) {
        window.alert('Para graficar el Rack, la altura debe estar definida.');
        return;
    }

    var intUs = parseInt(altura, 10);

    var intAlturaGraficar = parseInt(intUs, 10) * u;

    // window.alert(intAlturaGraficar);

    var c = document.getElementById("myCanvas");
    var context = c.getContext("2d");
    context.fillStyle = '#000000';
    context.strokeStyle = '#000105';
    context.lineWidth = 5;
    // context.fillRect(10, 10, 40, 40);
    context.strokeRect(20 * scale, 10 * scale, 190 * scale, intAlturaGraficar * scale);    // start start , ancho , alto

    // Us
    context.lineWidth = 2;
    context.fillStyle = '#000000';
    context.strokeStyle = '#cccccc';
    var i;
    for (i = 2; i <= intUs + 1; i++) {
        // window.alert(10 * i);
        context.moveTo(20 * scale, 10 * i * scale);
        context.lineTo(15 * scale, 10 * i * scale);
        context.stroke();
        ////////////////////////
        context.moveTo(210 * scale, 10 * i * scale);
        context.lineTo(215 * scale, 10 * i * scale);
        context.stroke();

        context.fillStyle = "black";
        context.font = "7px Arial";
        context.fillText(i - 1, (u * scale) - 5, (u * i * scale) + 2);



    }

    //  context.clearRect(20, 20, 20, 20);
}




function findUStart(_altura, _startU) {


    var scale = 1.8;
    var u = 10;  // One U  en pixels
    var altura = _altura;
    var intUs = parseInt(altura, 10);


    if (altura == '' || altura == undefined || altura == null) {
        window.alert('Para graficar el Rack, la altura debe estar definida.');
        return;
    }

    var intStartU = parseInt(_startU, 10);

   
    var i;
    for (i = 0; i <= intUs; i++) {

        if (intStartU == i) {
            return u * (i + 1) * scale;
        }
          
    }

    window.alert('No se encuentra U inicial');
    return u * 1 * scale;

    //  context.clearRect(20, 20, 20, 20);
}



function drawEquiposDesdeU(_altura, _equipos) {

   

    // var colorArray =  [  '#4fbb80', '#4f62bb', '#bb6280', '#4f6ff0'];
    var colorArray = ['#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459'];

    var scale = 1.8;
    var c = document.getElementById("myCanvas");
    var context = c.getContext("2d");
    context.fillStyle = '#000000';
    context.strokeStyle = '#4f6280';
    context.lineWidth = 2;

    var lastX = 20 * scale + 5;
    var lastY = 10 * scale;


    var u = 10;  // One U  en pixels
    var equipos = _equipos;
    // Total equipos por rack
    var numEquipos = equipos.length;

    if (numEquipos == 0)
    {
        window.alert('No hay equipos en el rack, no se graficará equipos.');
    }
   

    var equipoIniciaU = 1

    var j = 0;
    for (j = 0; j < numEquipos; j++) {
        // window.alert(j);
        var altura = equipos[j].altura_u;

        equipoIniciaU = equipos[j].comentarios;


        if (altura == '' || altura == undefined || altura == null) {
            window.alert('Para graficar el Rack, la altura debe estar definida.');
            continue;
        }

        var intUs = parseInt(altura, 10);
        var intAlturaGraficar = parseInt(intUs, 10) * u;

        var intAlturaGraficarScale = intAlturaGraficar * scale;

        lastX = 20 * scale + 5;
        lastY = findUStart(_altura, equipoIniciaU);

        // window.alert(lastY);

        // window.alert(intAlturaGraficar);

        // context.fillRect(10, 10, 40, 40);
        // context.strokeStyle = colorArray[j];
        // context.strokeRect(lastX, lastY, 180 * scale, intAlturaGraficarScale);    // start start , ancho , alto

        context.strokeStyle = '#000000';
        context.strokeRect(lastX, lastY, 184 * scale, intAlturaGraficarScale);

        context.fillStyle = colorArray[j];
        context.fillRect(lastX, lastY, 184 * scale, intAlturaGraficarScale);    // start start , ancho , alto

        //  context.clearRect(lastX + 15, lastY + 15, , );  
        context.fillStyle = "white";
        context.font = "10px Arial";

        context.fillText(equipos[j].s_equipo_numero_serie, lastX + 15, lastY + 15);

     

    }



}



function drawEquipos(_equipos) {



    // var colorArray =  [  '#4fbb80', '#4f62bb', '#bb6280', '#4f6ff0'];
    var colorArray = ['#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459', '#415b85', '#3a404a', '#395c5e', '#5c529e', '#3f2259', '#664e1b', '#403a2e', '#966503', '#425b63', '#515f63', '#15404d', '#360917', '#573e45', '#8a8821', '#4f4e08', '#1a0459'];

    var scale = 1.8;
    var c = document.getElementById("myCanvas");
    var context = c.getContext("2d");
    context.fillStyle = '#000000';
    context.strokeStyle = '#4f6280';
    context.lineWidth = 2;

    var lastX = 20 * scale + 5;
    var lastY = 10 * scale;

    var initY = 20 * scale;

    var u = 10;  // One U  en pixels
    var equipos = _equipos;
    // Total equipos por rack
    var numEquipos = equipos.length;

    if (numEquipos == 0) {
        window.alert('No hay equipos en el rack, no se graficará equipos.');
    }


    var equipoIniciaU = 1

    var j = 0;
    for (j = 0; j < numEquipos; j++) {
        // window.alert(j);
        var altura = equipos[j].altura_u;

        equipoIniciaU = equipos[j].comentarios;


        if (altura == '' || altura == undefined || altura == null) {
            window.alert('Para graficar el Rack, la altura debe estar definida.');
            continue;
        }

        var intUs = parseInt(altura, 10);
        var intAlturaGraficar = parseInt(intUs, 10) * u;

        var intAlturaGraficarScale = intAlturaGraficar * scale;



        // window.alert(intAlturaGraficar);

        // context.fillRect(10, 10, 40, 40);
        // context.strokeStyle = colorArray[j];
        // context.strokeRect(lastX, lastY, 180 * scale, intAlturaGraficarScale);    // start start , ancho , alto

        context.strokeStyle = '#000000';
        context.strokeRect(lastX, lastY, 184 * scale, intAlturaGraficarScale);

        context.fillStyle = colorArray[j];
        context.fillRect(lastX, lastY, 184 * scale, intAlturaGraficarScale);    // start start , ancho , alto

        //  context.clearRect(lastX + 15, lastY + 15, , );  
        context.fillStyle = "white";
        context.font = "10px Arial";

        context.fillText(equipos[j].s_equipo_numero_serie, lastX + 15, lastY + 15);

        lastY = intAlturaGraficarScale + lastY;

    }



}


