/// <reference path="redis.demo.cache.js" />

var inMemoryItems;

$(document).ready(function () {
    $("#searchByCompanyName").click(searchByCompanyName);
    $("#searchByExpediente").click(searchByExpediente);
    $("#searchByRuc").click(searchByRuc);
    $("#searchByName").click(searchByName);
    $("#searchByConstitucion").click(searchByConstitucion);
    $("#searchByEstadoLegal").click(searchByEstadoLegal);

    $("#txtConstitucionDesde").datepicker();
    $("#txtConstitucionHasta").datepicker();
    $("#selEstadoLegal").selectmenu();

    if (isStorageSupported()) {
        loadCompanyNameCache();
        loadCcExpedienteCache();
        loadCcRucCache();
    }
});

var isStorageSupported = function () {
    return (typeof (Storage) !== "undefined");
};

var searchByCompanyName = function (evt, handler) {
    var companyName = $("#txtCompanyName").val().toUpperCase();

    if (checkCompanyNameCache())
        sendGetRequest(evt, companyName, "CompanyByName", "Company-Template", "companiesTable");
    else {
        var items = new Array();
        $.each(inMemoryCompanies, function (i, v) {
            if (v.Name.indexOf(companyName) !== -1)
                items.push(v);
        });

        var source = $("#Company-Template").html();
        var template = Handlebars.compile(source);
        var html = template({ Items: items });
        $("#companiesTable").html("");
        $("#companiesTable").append(html);
//        searchInCompanyNameCache(companyName, "Company-Template", "companiesTable");
    }
};

var searchByExpediente = function (evt, handler) {
    var expediente = $("#txtExpediente").val();

    if (checkCcExpedienteCache())
        sendGetRequest(evt, expediente, "CcPorExpediente", "CC-Template", "ccTable");
    else {
        var ids = searchInCcExpedienteCache(expediente);
        sendPostRequest(evt, ids, "CcPorId", "CC-Template", "ccTable");
    }
};

var searchByRuc = function (evt, handler) {
    var ruc = $("#txtRuc").val();

    if (checkCcRucCache())
        sendGetRequest(evt, ruc, "CcPorRuc", "CC-Template", "ccTable");
    else {
        var ids = searchInCcRucCache(ruc);
        sendPostRequest(evt, ids, "CcPorId", "CC-Template", "ccTable");
    }
};

var searchByName = function (evt, handler) {
    var nombre = $("#txtName").val();

    if (checkCcNombreCache())
        sendGetRequest(evt, nombre, "CcPorNombre", "CC-Template", "ccTable");
    else {
        var ids = searchInCcNombreCache(nombre);
        sendPostRequest(evt, ids, "CcPorId", "CC-Template", "ccTable");
    }
};

var searchByConstitucion = function (evt, handler) {
    var desde = $("#txtConstitucionDesde").datepicker("getDate");
    var hasta = $("#txtConstitucionHasta").datepicker("getDate");
    sendGetRequest(evt, desde.getDate() + "_" + (desde.getMonth() + 1) + "_" + desde.getFullYear() + "|" + hasta.getDate() + "_" + (hasta.getMonth() + 1) + "_" + hasta.getFullYear(), "CcPorFechaConstitucion", "CC-Template", "ccTable");
};

var searchByEstadoLegal = function (evt, handler) {
    var estado = $("#selEstadoLegal").find(":selected").text();
    sendGetRequest(evt, estado, "CcPorEstadoLegal", "CC-Template", "ccTable");
};

var sendGetRequest = function (evt, parameter, apiMethod, hbTemplate, hbTarget) {
    if (parameter !== "" && parameter !== undefined) {
        $.ajax({
            cache: true,
            type: "GET",
            url: "/api/search/" + apiMethod + "/" + parameter,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            beforeSend: function () {
                evt.target.value = "Searching...";
                evt.target.disabled = true;
            },
            success: function (json) {
                inMemoryItems = json;

                var source = $("#" + hbTemplate).html();
                var template = Handlebars.compile(source);
                var html = template({ Items: json });
                $("#" + hbTarget).html("");
                $("#" + hbTarget).append(html);
            },
            error: function () {
                console.log("Error calling Redis API: Search/" + apiMethod);
            },
            complete: function () {
                evt.target.value = "Search";
                evt.target.disabled = false;
            }
        });
    }
};

var sendPostRequest = function (evt, parameter, apiMethod, hbTemplate, hbTarget) {
    $.ajax({
        cache: true,
        type: "POST",
        url: "/api/search/" + apiMethod,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(parameter),
        beforeSend: function () {
            evt.target.value = "Searching...";
            evt.target.disabled = true;
        },
        success: function (json) {
            inMemoryItems = json;

            var source = $("#" + hbTemplate).html();
            var template = Handlebars.compile(source);
            var html = template({ Items: json });
            $("#" + hbTarget).html("");
            $("#" + hbTarget).append(html);
        },
        error: function () {
            console.log("Error calling Redis API: Search/" + apiMethod);
        },
        complete: function () {
            evt.target.value = "Search";
            evt.target.disabled = false;
        }
    });
};

var showDetails = function (id) {
    $.each(inMemoryItems, function (i, v) {
        if (v.Id === id) {
            var source = $("#Dialog-Template").html();
            var template = Handlebars.compile(source);
            var html = template(v);
            $("#dialog").html("");
            $("#dialog").append(html);
            $("#dialog").dialog({
                width: 1000,
                height: 665
            });
            return;
        }
    });
};