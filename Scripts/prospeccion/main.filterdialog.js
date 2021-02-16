var filtersInMem;
var filterSectionSource = $("#FilterSection-Template").html();
var filterSectionSourceTemplate = Handlebars.compile(filterSectionSource);
var tboxesInFilters = new Array();

var onGetFiltrosSuccess = function (json) {
    filtersInMem = json;

    var html = filterSectionSourceTemplate(json);
    $("#panelbar").html("");
    $("#panelbar").append(html);

    $.each(json.Categorias, function (i, v) {
        $("#div" + v.Id + " .chk-accordion").click(function () {
            $("#count" + v.Id).html($("#div" + v.Id + " input.chk-accordion:checked").length);
        });
    });

    $("#dlgAddFilters").dialog({
        height: 550,
        width: 1040,
        autoOpen: false,
        modal: true,
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
            },
            "Actualizar filtros": function () {
                refreshFilters();
                $(this).dialog("close");
            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
            $(".ui-dialog-titlebar", ui.dialog | ui).hide();
            $("button").each(function (xi, xv) {
                if (xv.innerHTML === "Actualizar filtros") {
                    $(xv)[0].className = 'btn btn-aval';
                }
                if (xv.innerHTML === "Cancelar") {
                    $(xv)[0].className = 'btn btn-aval btn-disabled';
                }
            });
        }
    });

    $("#panelbar").ejAccordion({
        collapsible: true
    });
    $("#panelbar").ejAccordion("collapsePanel", 0);

    $(".subpanel").ejAccordion({
        collapsible: true
    });
    $(".subpanel").ejAccordion("collapsePanel", 0);
};

var openDialog = function () {
    $("#panelbar").ejAccordion("collapsePanel", 0);
    $(".subpanel").ejAccordion("collapsePanel", 0);

    $("#dlgAddFilters").dialog("open");
};

var selectedFilters;
var selectedBoolFilters;

var refreshFilters = function () {
    selectedFilters = new Array();
    selectedBoolFilters = new Array();

    $("input.chk-accordion:checked").each(function (i, v) {
        var selCheckboxName = v.id.substring(3);
        $.each(filtersInMem.Categorias, function (_i, category) {
            $.each(category.Opciones, function (__i, option) {
                if (option.Id === selCheckboxName) {
                    selectedFilters.push(option);
                    return false;
                }
            });
            $.each(category.Booleanos, function (__i, booleano) {
                if (booleano.Id === selCheckboxName) {
                    selectedBoolFilters.push(booleano);
                    return false;
                }
            });
            $.each(category.Subcategorias, function (__i, subcategory) {
                $.each(subcategory.Opciones, function (___i, option) {
                    if (option.Id === selCheckboxName) {
                        selectedFilters.push(option);
                        return false;
                    }
                });
            });
        });
    });

    var source = $("#AdditionalFilter-Template").html();
    var template = Handlebars.compile(source);
    $("#divAdditionalFilters").html("");

    var boolSource = $("#AdditionalBooleanFilter-Template").html();
    var boolTemplate = Handlebars.compile(boolSource);

    renderFilters(template, selectedFilters);
    renderBoolFilters(boolTemplate, selectedBoolFilters);
};

var renderBoolFilters = function (template, selectedFilters) {
    for (var i = 0, j = selectedFilters.length; i < j; i += 3) {
        var tempArray = selectedFilters.slice(i, i + 3);

        var html = template({ Items: tempArray });
        $("#divAdditionalFilters").append(html);
    }

    $.each(selectedFilters,
        function (i, v) {
            $("#cancel-" + v.Id).click(function () {
                // remove item
                $.each(selectedFilters,
                    function (_i, _v) {
                        if (_v.Id === v.Id) {
                            selectedFilters.splice(_i, 1);
                            return false;
                        }
                    });
                $("#cell-" + v.Id).remove();

                //// reorganize items
                //var remainingCells = $('div[id^="cell-"]').toArray();
                //var containers = new Array();

                //for (var i = 0, j = remainingCells.length; i < j; i += 3) {
                //    var tempArray = remainingCells.slice(i, i + 3);

                //    $.each(tempArray,
                //        function (_i, _v) {
                //            containers.push(_v);
                //        });
                //}

                //$("#divAdditionalFilters").empty();
                //$.each(containers,
                //    function (_i, _v) {
                //        $("#divAdditionalFilters").append(_v);
                //    });

                //// uncheck checkbox
                $.each(filtersInMem.Categorias,
                    function (_i, category) {
                        $.each(category.Booleanos,
                            function (__i, option) {
                                if (option.Id === v.Id) {
                                    console.log(v.Id);
                                    $("#chk" + v.Id).prop("checked", false);
                                    return false;
                                }
                            });
                    });
            });
        });
};

var clearTextBoxInArray = function (id) {
    $.each(tboxesInFilters, function (x, y) {
        if (y.startsWith(id)) {
            tboxesInFilters.splice(x, 1);
            return false;
        }
    });
};

var renderFilters = function (template, selectedFilters) {
    for (var i = 0, j = selectedFilters.length; i < j; i += 3) {
        var tempArray = selectedFilters.slice(i, i + 3);

        var html = template({ Items: tempArray });
        $("#divAdditionalFilters").append(html);
    }

    $.each(selectedFilters,
        function (i, v) {
            $("#reset-" + v.Id).click(function () {
                $("#" + v.Id + "MinSelected").val("");
                $("#" + v.Id + "MaxSelected").val("");
                $("#" + v.Id + "MinSelectedHdn").val(v.Min);
                $("#" + v.Id + "MaxSelectedHdn").val(v.Max);

                clearTextBoxInArray(v.Id + "MinSelected-");
                clearTextBoxInArray(v.Id + "MaxSelected-");
            });
            $("#cancel-" + v.Id).click(function () {
                clearTextBoxInArray(v.Id + "MinSelected-");
                clearTextBoxInArray(v.Id + "MaxSelected-");

                // remove item
                $.each(selectedFilters,
                    function (_i, _v) {
                        if (_v.Id === v.Id) {
                            selectedFilters.splice(_i, 1);
                            return false;
                        }
                    });
                $("#cell-" + v.Id).remove();

                //// reorganize items
                //var remainingCells = $('div[id^="cell-"]').toArray();
                //var containers = new Array();

                //for (var i = 0, j = remainingCells.length; i < j; i += 3) {
                //    var tempArray = remainingCells.slice(i, i + 3);

                //    $.each(tempArray,
                //        function (_i, _v) {
                //            containers.push(_v);
                //        });
                //}

                //$("#divAdditionalFilters").empty();
                //$.each(containers,
                //    function (_i, _v) {
                //        $("#divAdditionalFilters").append(_v);
                //    });

                //// uncheck checkbox
                $.each(filtersInMem.Categorias,
                    function (_i, category) {
                        $.each(category.Opciones,
                            function (__i, option) {
                                if (option.Id === v.Id) {
                                    console.log(v.Id);
                                    $("#chk" + v.Id).prop("checked", false);
                                    return false;
                                }
                            });
                        $.each(category.Subcategorias,
                            function (_i, subcategory) {
                                $.each(subcategory.Opciones,
                                    function (__i, option) {
                                        if (option.Id === v.Id) {
                                            $("#chk" + v.Id).prop("checked", false);
                                            return false;
                                        }
                                    });
                            });
                    });
            });

            $("#" + v.Id + "MinSelected").change(function () {
                var parsed = parseFloat($(this).val());

                if ($(this).val() !== "" && !isNaN(parsed)) {
                    var newMinValue = parsed;

                    $(this).val(newMinValue.toLocaleString("es-EC"));
                    $("#" + v.Id + "MinSelectedHdn").val(newMinValue);
                    tboxesInFilters.push(v.Id + "MinSelected-" + newMinValue.toLocaleString("es-EC"));
                } else {
                    $(this).val("");
                    $("#" + v.Id + "MinSelectedHdn").val(v.Min);
                    clearTextBoxInArray(v.Id + "MinSelected-");
                }
            });
            $("#" + v.Id + "MaxSelected").change(function () {
                var parsed = parseFloat($(this).val());

                if ($(this).val() !== "" && !isNaN(parsed)) {
                    var newMaxValue = parsed;

                    $(this).val(newMaxValue.toLocaleString("es-EC"));
                    $("#" + v.Id + "MaxSelectedHdn").val(newMaxValue);
                    tboxesInFilters.push(v.Id + "MaxSelected-" + newMaxValue.toLocaleString("es-EC"));
                } else {
                    $(this).val("");
                    $("#" + v.Id + "MaxSelectedHdn").val(v.Max);
                    clearTextBoxInArray(v.Id + "MaxSelected-");
                }
            });
        });

    $.each(tboxesInFilters, function (i, v) {
        var pair = v.split("-");
        $("#" + pair[0]).val(pair[1]);
    });
};

var resetAllFilters = function () {
    $('[id^="reset-"]').trigger("click");
    $('[id^="ddl"]').each(function (i, v) {
        var mid = this.id.replace("_taglist", "");
        var ddl = $("#" + mid).data("kendoMultiSelect");
        ddl.value([]);

        if (mid !== "ddlProvincias" && mid !== "ddlActividades1" && mid !== "ddlCalificacion") ddl.setDataSource([]);
        $('[id^="chk"]').prop("checked", false);
    });
    tboxesInFilters = new Array();
};