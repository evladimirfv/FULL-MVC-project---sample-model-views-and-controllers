var companiesKey = "CompaniesCache";
var companiesExpirationKey = "CompaniesCache.Expiration";
var ccExpedienteKey = "CcExpedienteCache";
var ccExpedienteExpirationKey = "CcExpedienteCache.Expiration";
var ccRucKey = "CcRucCache";
var ccRucExpirationKey = "CcRucCache.Expiration";

var loadCompanyNameCache = function () {
    commonAjaxFileRequest(companiesKey, companiesExpirationKey, "AllCompanies");
};

var loadCcExpedienteCache = function () {
    commonAjaxRequest(ccExpedienteKey, ccExpedienteExpirationKey, "AllCcExpediente");
};

var loadCcRucCache = function () {
    commonAjaxRequest(ccRucKey, ccRucExpirationKey, "AllCcRuc");
};

var commonAjaxRequest = function(key, expirationKey, method) {
    if (commonCheckCache(key, expirationKey)) {
        $.ajax({
            cache: true,
            type: "GET",
            url: "/api/search/" + method,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (json) {
                localStorage.setItem(key, JSON.stringify(json));
                localStorage.setItem(expirationKey, Date.now() + (1000 * 3600));
            },
            error: function () {
                console.log("Error calling Redis API: Search/" + method);
            }
        });
    }
};

var commonAjaxFileRequest = function (key, expirationKey, method) {
    if (commonCheckCache(key, expirationKey)) {
        $.ajax({
            cache: true,
            type: "GET",
            url: "/api/search/" + method,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (readFile) {

                var blob = null;
                var xhr = new XMLHttpRequest();
                xhr.open("GET", readFile);
                xhr.responseType = "blob";//force the HTTP response, response-type header to be blob
                xhr.onload = function () {
                    blob = xhr.response;//xhr.response is now a blob object
                    var reader = new FileReader();
                    reader.readAsText(blob, "UTF-8");
                    reader.onload = loaded;
                }
                xhr.send();
            },
            error: function () {
                console.log("Error calling Redis API: Search/" + method);
            }
        });
    }
};

var inMemoryCompanies;
function loaded(evt) {
    // Obtain the read file data
    var json = evt.target.result;
    // Handle UTF-16 file dump
    inMemoryCompanies = JSON.parse(json);
}

var checkCompanyNameCache = function () {
    return !(inMemoryCompanies !== undefined && inMemoryCompanies !== null);
    //return commonCheckCache(companiesKey, companiesExpirationKey);
};

var checkCcExpedienteCache = function () {
    return commonCheckCache(ccExpedienteKey, ccExpedienteExpirationKey);
};

var checkCcRucCache = function () {
    return commonCheckCache(ccRucKey, ccRucExpirationKey);
};

var commonCheckCache = function (key, expirationKey) {
    var cache = localStorage.getItem(key);
    var cacheExpiration = localStorage.getItem(expirationKey);

    return cache === null || cache === undefined || cacheExpiration <= Date.now();
};

var searchInCompanyNameCache = function(toSearch, templateName, elem) {
    var cache = localStorage.getItem(companiesKey);

    var items = new Array();
    $.each(JSON.parse(cache), function (i, v) {
        if (v.Name.indexOf(toSearch) !== -1)
            items.push(v);
    });

    var source = $("#" + templateName).html();
    var template = Handlebars.compile(source);
    var html = template({ Items: items });
    $("#" + elem).html("");
    $("#" + elem).append(html);
};

var searchInCcExpedienteCache = function (toSearch) {
    var cache = localStorage.getItem(ccExpedienteKey);

    var items = new Array();
    $.each(JSON.parse(cache), function (i, v) {
        if (v.indexOf(toSearch) !== -1)
            items.push(v);
    });

    return items;
};

var searchInCcRucCache = function (toSearch) {
    var cache = localStorage.getItem(ccRucKey);

    var items = new Array();
    $.each(JSON.parse(cache), function (i, v) {
        if (v.Ruc.indexOf(toSearch) !== -1)
            items.push(v.Id);
    });

    return items;
};
