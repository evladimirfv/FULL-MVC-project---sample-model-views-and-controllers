/// <reference path="../handlebars.min.js" />

var rangesInMem;
var calificaciones = [
    { Name: "A1" },
    { Name: "A2" },
    { Name: "A3" },
    { Name: "B1" },
    { Name: "B2" },
    { Name: "C1" },
    { Name: "C2" },
    { Name: "D" },
    { Name: "E" }
];

var onGetRangesSuccess = function (json) {
    rangesInMem = json;

    var source = $("#RangeSlider-Template").html();
    var template = Handlebars.compile(source);
    $("#divRangeSliders").html("");

    var jRange = json.slice(0, 2);
    var html = template({ Items: jRange });
    $("#divRangeSliders").append(html);

    // BEGIN Calificaciones
    var source2 = $("#CalificacionEmpresa-Template").html();
    var template2 = Handlebars.compile(source2);
    $("#divRangeSliders").append(template2({}));

    $("#ddlCalificacion").kendoMultiSelect({
        dataTextField: "Name",
        dataValueField: "Name",
        itemTemplate: '<input id="chk#: data.Name #" class="chkCalificacion" type="checkbox"/> <span class="k-state-default">#: data.Name #</span>',
        dataSource: {
            data: calificaciones
        },
        change: onCalificacionesChange
    });
    $("#reset-calificaciones").click(
        function () {
            var ddl = $("#ddlCalificacion").data("kendoMultiSelect");
            ddl.value([]);
            $('.chkCalificacion').prop("checked", false);
        });
    // END Calificaciones

    jRange = json.slice(2, json.length);
    html = template({ Items: jRange });
    $("#divRangeSliders2").append(html);

    $.each(json, function (i, v) {
        $("#reset-" + v.Name).click(function () {
            $("#" + v.Name + "MinSelected").val("");
            $("#" + v.Name + "MaxSelected").val("");
            $("#" + v.Name + "MinSelectedHdn").val(v.Min);
            $("#" + v.Name + "MaxSelectedHdn").val(v.Max);
        });

        $("#span" + v.Name + "Min").html(v.Min.toLocaleString("es-EC"));
        $("#span" + v.Name + "Max").html(v.Max.toLocaleString("es-EC"));

        $("#" + v.Name + "MinSelectedHdn").val(v.Min);
        $("#" + v.Name + "MaxSelectedHdn").val(v.Max);

        $("#" + v.Name + "MinSelected").change(function () {
            var parsed = parseFloat($(this).val());

            if ($(this).val() !== "" && !isNaN(parsed)) {
                var newMinValue = parsed;

                $(this).val(newMinValue.toLocaleString("es-EC"));
                $("#" + v.Name + "MinSelectedHdn").val(newMinValue);
            } else {
                $(this).val("");
                $("#" + v.Name + "MinSelectedHdn").val(v.Min);
            }
        });
        $("#" + v.Name + "MaxSelected").change(function () {
            var parsed = parseFloat($(this).val());

            if ($(this).val() !== "" && !isNaN(parsed)) {
                var newMaxValue = parsed;

                $(this).val(newMaxValue.toLocaleString("es-EC"));
                $("#" + v.Name + "MaxSelectedHdn").val(newMaxValue);
            } else {
                $(this).val("");
                $("#" + v.Name + "MaxSelectedHdn").val(v.Max);
            }
        });
    });
};

var onCalificacionesChange = function () {
    var values = this.value();
    var data = this.dataSource.data();

    $.each(data, function (i, v) {
        $("#chk" + v.Name).prop('checked', values.includes(v.Name));
    });

    onInvestmentChange();
};