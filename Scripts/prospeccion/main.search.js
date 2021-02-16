
// DEPLOY PUBLISH
// var pathInit = '/PLAVALPRO';

var pathInit = '';
var pageSizeParam = 500;    //  Tested as 40


var jsonResponse;
var reportCols;
var reportCols2;
var defaultCols = [
    { headerText: "#", template: "<span></span>", textAlign: ej.TextAlign.Center, width: 15 },
    { field: "Ruc", headerText: "RUC", width: 30, textAlign: ej.TextAlign.Center },
    { field: "NombreCompania", headerText: "Nombre Compañía", width: 150, textAlign: ej.TextAlign.Center },
    { field: "FechaConstitucion", headerText: "Fecha Const.", width: 30, textAlign: ej.TextAlign.Center, format: "{0:MM/dd/yyyy}" },
    { field: "EstadoLegal", headerText: "Estado", width: 65, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel1", headerText: "Actividad Económica 1", width: 140, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel2", headerText: "Actividad Económica 2", width: 120, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel3", headerText: "Actividad Económica 3", width: 120, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel4", headerText: "Actividad Económica 4", width: 120, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel5", headerText: "Actividad Económica 5", width: 120, textAlign: ej.TextAlign.Center },
    { field: "ActividadEconomicaNivel6", headerText: "Actividad Económica 6", width: 450, textAlign: ej.TextAlign.Center },
    { field: "CapitalSuscrito", headerText: "Capital Suscrito", width: 100, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "Provincia", headerText: "Provincia", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Canton", headerText: "Cantón", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Ciudad", headerText: "Ciudad", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Calle", headerText: "Calle", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Numero", headerText: "Número", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Telefono", headerText: "Teléfono", width: 100, textAlign: ej.TextAlign.Center },
    { field: "Email", headerText: "E-mail", width: 100, textAlign: ej.TextAlign.Center },
    { field: "NumeroEmpleados", headerText: "# Total Empleados", width: 80, textAlign: ej.TextAlign.Center },
    { field: "TotalActivos", headerText: "Total Activos", width: 100, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "TotalPasivos", headerText: "Total Pasivos", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "Patrimonio", headerText: "Patrimonio", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "Ventas", headerText: "Ventas", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "UtilidadNeta", headerText: "Utilidad Neta", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "Antiguedad", headerText: "Antigüedad", width: 30, textAlign: ej.TextAlign.Center },
    { field: "Calificacion", headerText: "Calificación", width: 30, textAlign: ej.TextAlign.Center },
    { field: "ActivoCorriente", headerText: "Act. Corriente", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "ActivoNoCorriente", headerText: "Act. No Corriente", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "PasivoCorriente", headerText: "Pas. Corriente", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
    { field: "PasivoNoCorriente", headerText: "Pas. No Corriente", width: 80, textAlign: ej.TextAlign.Center, format: "{0:C2}" },
];
var defaultCols2 = [
    { headerText: "#", template: "<span></span>", textAlign: ej.TextAlign.Center, width: 15 },
    { field: "NombreCompania", headerText: "Nombre Compañía", width: 150, textAlign: ej.TextAlign.Center }
];

var onSearchClicked = function () {
    var provincias = $("#ddlProvincias").data("kendoMultiSelect").value();
    var ciudades = $("#ddlCiudades").data("kendoMultiSelect").value();
    var actividades1 = $("#ddlActividades1").data("kendoMultiSelect").value();
    var actividades2 = $("#ddlActividades2").data("kendoMultiSelect").value();
    var actividades3 = $("#ddlActividades3").data("kendoMultiSelect").value();
    var actividades4 = $("#ddlActividades4").data("kendoMultiSelect").value();
    var actividades5 = $("#ddlActividades5").data("kendoMultiSelect").value();
    var actividades6 = $("#ddlActividades6").data("kendoMultiSelect").value();
    var rangoNumEmpleados = [$("#NumEmpleadosMinSelectedHdn").val(), $("#NumEmpleadosMaxSelectedHdn").val()];
    var rangoAntiguedad = [$("#AntiguedadMinSelectedHdn").val(), $("#AntiguedadMaxSelectedHdn").val()];
    var rangoActivo = [$("#ActivoMinSelectedHdn").val(), $("#ActivoMaxSelectedHdn").val()];
    var rangoActivoCorriente = [$("#ActivoCorrienteMinSelectedHdn").val(), $("#ActivoCorrienteMaxSelectedHdn").val()];
    var rangoActivoNoCorriente = [$("#ActivoNoCorrienteMinSelectedHdn").val(), $("#ActivoNoCorrienteMaxSelectedHdn").val()];
    var rangoPasivo = [$("#PasivoMinSelectedHdn").val(), $("#PasivoMaxSelectedHdn").val()];
    var rangoPasivoCorriente = [$("#PasivoCorrienteMinSelectedHdn").val(), $("#PasivoCorrienteMaxSelectedHdn").val()];
    var rangoPasivoNoCorriente = [$("#PasivoNoCorrienteMinSelectedHdn").val(), $("#PasivoNoCorrienteMaxSelectedHdn").val()];
    var rangoPatrimonio = [$("#PatrimonioMinSelectedHdn").val(), $("#PatrimonioMaxSelectedHdn").val()];
    var rangoVentas = [$("#VentasMinSelectedHdn").val(), $("#VentasMaxSelectedHdn").val()];
    var rangoUtilidad = [$("#UtilidadMinSelectedHdn").val(), $("#UtilidadMaxSelectedHdn").val()];
    var filtrosArray = new Array();

    $(".additionalFilterId").each(function (i) {
        var rangeId = $(this).val();
        filtrosArray.push({
            Min: $("#" + rangeId + "MinSelectedHdn").val(),
            Max: $("#" + rangeId + "MaxSelectedHdn").val(),
            Id: rangeId
        });
    });

    $(".additionalBooleanFilterId").each(function (i) {
        var elemId = $(this).val();
        filtrosArray.push({
            BoolValue: $("input:radio[name='" + elemId + "']:checked").val(),
            Id: elemId
        });
    });

    $("#dlgShowPreResults").dialog({
        height: 655,
        width: 500,
        autoOpen: false,
        modal: true,
        buttons: {
            "Cancelar": function () {
                toastr.clear();
                $(this).dialog("close");
            },
            "Finalizar Prospección": function () {
                var userTypeMessage = "Al proceder a la prospección se deducirá de su saldo actual. ¿Confirma esta acción?";

                var currUserId = jsonResponse.Stats.UserId;
                var currUserName = jsonResponse.Stats.UserName;
                var currBalance = jsonResponse.Stats.Balance;
                var currAmountUsed = jsonResponse.Stats.AmountUsed;

                var jsonStatsTotal = jsonResponse.Stats.Total;

               // userTypeMessage = "Usuario: " + currUserName + "  " + userTypeMessage;
                var tempBool = false;

                if (tempBool) {     // currBalance < jsonStatsTotal
                    var errorUserTypeMessage = "El saldo actual es insuficiente para el número de RUCs a consultar. Por favor redefina la consulta o solicite un nuevo crédito de consultas.";

                    alert(errorUserTypeMessage);


                }
                else
                { 
                    if (confirm(userTypeMessage)) {

                        var req = {
                            UserId: jsonResponse.Stats.UserId,
                            UserName: jsonResponse.Stats.UserName,
                            AmountUsed: jsonResponse.Stats.Shown,     //  jsonResponse.Stats.Shown,
                            Balance: jsonResponse.Stats.Balance
                        };
                        $.ajax({
                            cache: true,
                            type: 'POST',
                            url: pathInit + '/api/prospeccion/UpdateAvailable',
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            success: function (js) {
                                $("#dlgShowPreResults").dialog("close");
                                jsonResponse = js;

                                $("#dlgShowResults").dialog({
                                    height: 670,
                                    width: 1040,
                                    title: "Resultados",
                                    autoOpen: false,
                                    modal: true,
                                    buttons: {
                                        "Cancelar": function() {
                                            toastr.clear();
                                            $("#dlgShowResults").dialog("close");
                                        },
                                        "Exportar a Excel": function () {

                                            var grid = $("#spreadsheet").ejGrid("instance");
                                            var pageNumber = grid.model.pageSettings.currentPage;

                                            $("#spreadsheet").ejGrid('export', pathInit + '/api/prospeccion/ExcelExport?pageNum=' + pageNumber);
                                        }
                                    },
                                    open: function (event, ui) {
                                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                                        $("#xlTitle").text("Datos financieros al año: " + $("#selYear").children("option:selected").val());
                                        $(".ui-dialog-titlebar", ui.dialog | ui).hide();
                                        $("button").each(function (xi, xv) {
                                            if (xv.innerHTML === "Exportar a Excel") {
                                                $(xv)[0].className = 'btn btn-aval';
                                                $(xv).append('<img src="/imgs/excel-button.png" alt="">');
                                            }
                                            if (xv.innerHTML === "Cancelar") {
                                                $(xv)[0].className = 'btn btn-aval btn-disabled';
                                            }
                                        });
                                    }
                                });

                                onGoToNextStage(jsonResponse);
                            },
                            data: JSON.stringify(req),
                            error: function () {
                                console.log("Error calling Prospeccion API: Search");
                            }
                        });
                    }
                }

            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
            $(".ui-dialog-titlebar", ui.dialog | ui).hide();
            $("button").each(function (xi, xv) {
                if (xv.innerHTML === "Finalizar Prospección") {
                    $(xv)[0].className = 'btn btn-aval';
                }
                if (xv.innerHTML === "Cancelar") {
                    $(xv)[0].className = 'btn btn-aval btn-disabled';
                }
            });
        }
    });

    var request = {
        Provincias: provincias,
        Ciudades: ciudades,
        ActividadesEconomicas1: actividades1,
        ActividadesEconomicas2: actividades2,
        ActividadesEconomicas3: actividades3,
        ActividadesEconomicas4: actividades4,
        ActividadesEconomicas5: actividades5,
        ActividadesEconomicas6: actividades6,
        NumeroEmpleados: {
            Min: rangoNumEmpleados[0],
            Max: rangoNumEmpleados[1]
        },
        Antiguedad: {
            Min: rangoAntiguedad[0],
            Max: rangoAntiguedad[1]
        },
        Activo: {
            Min: rangoActivo[0],
            Max: rangoActivo[1]
        },
        ActivoCorriente: {
            Min: rangoActivoCorriente[0],
            Max: rangoActivoCorriente[1]
        },
        ActivoNoCorriente: {
            Min: rangoActivoNoCorriente[0],
            Max: rangoActivoNoCorriente[1]
        },
        Pasivo: {
            Min: rangoPasivo[0],
            Max: rangoPasivo[1]
        },
        PasivoCorriente: {
            Min: rangoPasivoCorriente[0],
            Max: rangoPasivoCorriente[1]
        },
        PasivoNoCorriente: {
            Min: rangoPasivoNoCorriente[0],
            Max: rangoPasivoNoCorriente[1]
        },
        Patrimonio: {
            Min: rangoPatrimonio[0],
            Max: rangoPatrimonio[1]
        },
        Ventas: {
            Min: rangoVentas[0],
            Max: rangoVentas[1]
        },
        Utilidad: {
            Min: rangoUtilidad[0],
            Max: rangoUtilidad[1]
        },
        CalificacionEmpresa: $("#ddlCalificacion").data("kendoMultiSelect").value(),
        Year: $("#selYear").children("option:selected").val(),
        FiltrosAdicionales: filtrosArray
    };

    $.ajax({
        cache: true,
        type: 'POST',
        url: pathInit + '/api/prospeccion/Search',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: onSearchSuccess,
        data: JSON.stringify(request),
        error: function () {
            console.log("Error calling Prospeccion API: Search");
        }
    });
};

var onSearchSuccess = function (json) {
    jsonResponse = json;
    reportCols2 = defaultCols2.slice();

    toastr.clear();
    toastr.options.timeOut = 0;
    toastr.options.extendedTimeOut = 0;
    toastr.options.closeButton = true;
    toastr.options.positionClass = "toast-top-right";


    var saldoInsuficiente = 'Total Mostrado:';
    var varTotalMostrado = json.Stats.Shown;
    if (json.Stats.Available == 0)
    {
        saldoInsuficiente = 'Saldo no disponible, no se muestra resultados.';
        varTotalMostrado = '';
    }

    //var toShowHTML = '<table><tr><td><b>Total Encontrado:</b></td><td>' + json.Stats.Total +
    //    '</td></tr><tr><td><b>Disponible para prospección:</b></td><td>' + json.Stats.Available +
    //    '</td></tr><tr><td><b>Total Mostrado:</b></td><td>' + json.Stats.Shown + '</td></tr></table>');

    //     if (json.Stats.Available == 0)
    //     {
    //         toShowHTML = '<table><tr><td><b>Total Encontrado:</b></td><td>' + json.Stats.Total +
    //             '</td></tr><tr><td style="color:orange"><b>Disponible para prospección:</b></td><td>' + json.Stats.Available +
    //             '</td></tr><tr><td><b>Total Mostrado:</b></td><td>' + json.Stats.Shown + '</td></tr></table>');

    //     }

    toastr.success(

       // toShowHTML;
       
        '<table><tr><td><b>Total Encontrado:</b></td><td>' + json.Stats.Total +
        '</td></tr><tr><td><b> ' + 'Disponible para prospección:' + '</b></td><td>' + json.Stats.Available +
        '</td></tr><tr><td><b>' +  saldoInsuficiente + '</b></td><td>' + varTotalMostrado + '</td></tr></table>');

    if ($("#spreadsheet2").data("ejGrid") !== undefined) {
        $("#spreadsheet2").ejGrid("destroy");
    }
    $("#spreadsheet2").ejGrid({
        dataSource: ej.parseJSON(json.Items),
        allowPaging: true,
        allowSorting: true,
        allowScrolling: true,
        scrollSettings: { width: 465, height: 345 },
        columns: reportCols2,
        pageSettings: { pageSize: pageSizeParam },
        templateRefresh: "templateRefresh"
    });

    $("#dlgShowPreResults").dialog({ title: "Previsualización" });
    $("#dlgShowPreResults").dialog("open");
};

var onGoToNextStage = function (json) {
    toastr.clear();
    //toastr.options.timeOut = 0;
    //toastr.options.extendedTimeOut = 0;
    //toastr.options.closeButton = true;
    //toastr.options.positionClass = "toast-top-right";
    //toastr.success(
    //    '<table><tr><td><b>Disponible Actualizado:</b></td><td>' + (json.Stats.Available - json.Stats.Shown) +
    //    '</td></tr></table>');

    $.each(json.Items, function (i, v) {
        $.each(json.ExtraData, function (_i, _v) {
            $.each(_v, function (__i, __v) {
                if (__v[0] === v.Ruc) {
                    v["Extra" + _i] = __v[1];
                    return false;
                }
            });
        });
    });

    reportCols = defaultCols.slice();
    var baseWidth = 7500;
    $.each(json.ExtraData, function (i, v) {
        baseWidth += 30;

        var sFormat = "{0:N2}";
        if (v[0][1] === "Capital de trabajo")
            sFormat = "{0:C2}";

        reportCols.push({
            field: "Extra" + i,
            headerText: v[0][1],
            width: 80,
            textAlign: ej.TextAlign.Center,
            format: sFormat
        });
    });

    if ($("#spreadsheet").data("ejGrid") !== undefined) {
        $("#spreadsheet").ejGrid("destroy");
    }
    $("#spreadsheet").ejGrid({
        dataSource: ej.parseJSON(json.Items),
        allowPaging: true,
        allowSorting: true,
        allowScrolling: true,
        scrollSettings: { width: baseWidth, height: 345 },
        exportToExcelAction: pathInit + '/api/prospeccion/ExcelExport',     /* ?pageNum */
        toolbarSettings: {
            showToolbar: false,
            toolbarItems: [ej.Grid.ToolBarItems.ExcelExport]
        },
        columns: reportCols,
        pageSettings: { pageSize: pageSizeParam, enableQueryString: true  },   /* VF */
        templateRefresh: "templateRefresh"
    });

    $("#dlgShowResults").dialog({ title: json.Message });
    $("#dlgShowResults").dialog("open");
};

function templateRefresh(args) {
    $(args.cell).text((args.rowIndex + 1) + ((args.model.pageSettings.currentPage - 1) * pageSizeParam));
}
