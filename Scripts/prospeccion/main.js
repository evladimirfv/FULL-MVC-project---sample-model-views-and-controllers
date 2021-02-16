/// <reference path="main.filterdialog.js" />
/// <reference path="main.provinces.js" />
/// <reference path="main.search.js" />
/// <reference path="main.actividadeseconomicas.js" />
/// <reference path="main.ranges.js" />

// DEPLOY PUBLISH
// var pathInit = '/PLAVALPRO';

var pathInit = '';

Handlebars.registerHelper('replace', function (find, replace, options) { // {{#replace "&amp;" "and"}}{{title}}{{/replace}}
    var string = options.fn(this);
    return string.replace(find, replace);
});

hashCode = function (s) {
    var h = 0, l = s.length, i = 0;
    if (l > 0)
        while (i < l)
            h = (h << 5) - h + s.charCodeAt(i++) | 0;
    return h;
};

$(document)
    .ajaxStart(function () {
        $('#loadingDiv').show();
    })
    .ajaxStop(function () {
        $('#loadingDiv').hide();
    });
$(document).ready(function () {

    loadingDiv = $('#loadingDiv').hide();
    $.ajax({
        type: 'GET',
        url: pathInit + '/api/prospeccion/GetCountByProvince',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: onGetCountByProvinceSuccess,
        error: function () {
            console.log("Error calling Prospeccion API: GetCountByProvince");
        }
    });

    $.ajax({
        type: 'GET',
        url: pathInit + '/api/prospeccion/GetActividadesEconomicas',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: onGetActividadesEconomicasSuccess,
        error: function () {
            console.log("Error calling Prospeccion API: GetActividadesEconomicas");
        }
    });

    $.ajax({
        type: 'GET',
        url: pathInit + '/api/prospeccion/GetRanges',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: onGetRangesSuccess,
        error: function () {
            console.log("Error calling Prospeccion API: GetRanges");
        }
    });

    $.ajax({
        type: 'GET',
        url: pathInit + '/api/prospeccion/GetFiltrosAdicionales',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: onGetFiltrosSuccess,
        error: function () {
            console.log("Error calling Prospeccion API: GetFiltrosAdicionales");
        }
    });
});
