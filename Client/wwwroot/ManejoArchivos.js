window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}
window.GenerarPDF = async (nombre, FechaInicio, FechaFinal, Tov, Gsv, Nsv, AguaNeta, TovProd, GsvProd, NsvProd, AguaNetaProd, GsvDesp, NsvDesp, AguaDesp, ApiDesp, CantidadVehiculos,
    TovNaf, GsvNaf, NsvNaf, AguaNetaNaf,
    NombrePDF) => {
    const doc = new jsPDF();

    //CAMBIAR TIPO DE LETRA
    doc.setFontSize(12); //Tamaño de letra
    doc.setFont("times");
    doc.setFontType("italic");
    doc.text(50, 25, "INFORME PRODUCCION NUEVA ESPERANZA");
    doc.text(19, 40, nombre);
    doc.text(50, 35, "Fecha Inicial:");
    doc.text(50, 40, "Fecha Final:");
    doc.text(75, 35, FechaInicio);
    doc.text(75, 40, FechaFinal);
    doc.text(45, 60, "Volumenes Actuales en Campo");
    doc.text(45, 70, "Crudo");
    doc.text(50, 80, "TOV :" + Tov + " Bls");
    doc.text(50, 85, "GSV :" + Gsv + " Bls");
    doc.text(50, 90, "NSV :" + Nsv + " Bls");
    doc.text(50, 95, "AGUA NETA :" + AguaNeta + " Bls");

    doc.text(115, 70, "Nafta");
    doc.text(120, 80, "TOV :" + TovNaf + " Bls");
    doc.text(120, 85, "GSV :" + GsvNaf + " Bls");
    doc.text(120, 90, "NSV :" + NsvNaf + " Bls");
    doc.text(120, 95, "AGUA NETA :" + AguaNetaNaf + " Bls");


    doc.text(45, 105, "Prod Campo Nueva Esperanza");
    doc.text(50, 115, "TOV :" + TovProd + " Bls");
    doc.text(50, 120, "GSV :" + GsvProd + " Bls");
    doc.text(50, 125, "NSV :" + NsvProd + " Bls");
    doc.text(50, 130, "AGUA NETA :" + AguaNetaProd + " Bls");

    doc.text(45, 140, "Despachos Realizados");
    doc.text(50, 150, "GSV :" + GsvDesp + " Bls");
    doc.text(50, 155, "NSV :" + NsvDesp + " Bls");
    doc.text(50, 160, "AGUA NETA :" + AguaDesp + " Bls");
    doc.text(50, 165, "API @60 F :" + ApiDesp + " °");


    var Salto = 50;
    //recuadro de color
    doc.setDrawColor(0);
    doc.setFillColor(60, 117, 66);
    doc.rect(180, 0, 30, 300, 'F'); // filled red square(X iicial, y final, ancho, alto)


    // IMAGEN PASAR A URL
    var img = new Image;
    img.src = "favicon.png";
    img.crossOrigin = "";  // for demo as we are at different origin than image


    img.onload = function () {
        doc.addImage(this, 20, 20, 15, 15);

        doc.save(NombrePDF);//GRACIAS SEÑOR QUE ESTO FUNCIONA
    };

}
