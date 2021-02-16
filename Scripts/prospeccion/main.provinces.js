var provsInMem;
var totalEmpresas = 0;

var onGetCountByProvinceSuccess = function (json) {
    provsInMem = json;
    var data = new Array();
    var provs = new Array();

    $.each(json,
        function(i, v) {
            totalEmpresas += v.Cantidad;
            data.push([v.Provincia, v.Cantidad]);
            provs.push(
                {
                    'Provincia': v.Provincia,
                    'Hashcode': hashCode(v.Provincia)
                });
        }
    );

    fillMapData(data);

    $("#spanTotalEmpresas").html(totalEmpresas.toLocaleString("es-EC"));
    $("#ddlProvincias").kendoMultiSelect({
        dataTextField: "Provincia",
        dataValueField: "Provincia",
                dataSource: {
            data: provs
        },
        itemTemplate: '<input id="chk#: data.Hashcode #" type="checkbox"/> <span class="k-state-default">#: data.Provincia #</span>',
        change: onProvinceChange
    });
    $("#ddlCiudades").kendoMultiSelect({
        dataTextField: "Ciudad",
        dataValueField: "Ciudad",
        itemTemplate:
            '<input id="chk#: data.Hashcode #" type="checkbox"/> <span class="k-state-default">#: data.Ciudad # <span style="font-size: xx-small">#: data.Provincia # (#:data.Cantidad#)</span></span>',
        tagTemplate: '<span class="selected-value">#:data.Ciudad#</span> <b>(#:data.Cantidad#)</b>',
        dataSource: {
            data: []
        },
        height: 400,
        change: function () {
            var values = this.value();
            var data = this.dataSource.data();

            $.each(data, function (i, v) {
                if (values.includes(v.Ciudad))
                    $("#chk" + hashCode(v.Ciudad)).prop('checked', true);
                else
                    $("#chk" + hashCode(v.Ciudad)).prop('checked', false);
            });

            onInvestmentChange();
        }
    });
};

var fillMapData = function (data) {
    Highcharts.mapChart('container',
        {
            chart: {
                map: 'countries/ec/ec-all',
                height: 370
            },

            title: {
                text: ''
            },

            mapNavigation: {
                enabled: true,
                buttonOptions: {
                    verticalAlign: 'bottom'
                }
            },

            colorAxis: {
                min: 0
            },

            series: [
                {
                    data: data,
                    name: '# Companias',
                    states: {
                        hover: {
                            color: '#BADA55'
                        }
                    },
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    },
                    events: {
                        click: onProvinceClicked
                    }
                }, {
                    name: 'Separators',
                    type: 'mapline',
                    data: Highcharts.geojson(Highcharts.maps['countries/ec/ec-all'], 'mapline'),
                    color: 'red',
                    showInLegend: false,
                    enableMouseTracking: false
                }
            ]
        });
};

function compareCiudad(a, b) {
    if (a.Ciudad < b.Ciudad)
        return -1;
    if (a.Ciudad > b.Ciudad)
        return 1;
    return 0;
}

var onProvinceChange = function (e) {
    var values = this.value();
    var ciudades = new Array();

    $.each(provsInMem, function (i, v) {
        if (values.includes(v.Provincia)) {
            ciudades = ciudades.concat(v.Ciudades);
            $("#chk" + hashCode(v.Provincia)).prop('checked', true);
        } else
            $("#chk" + hashCode(v.Provincia)).prop('checked', false);
    });
    ciudades.sort(compareCiudad);

    $.each(ciudades, function (i, v) {
        v.Hashcode = hashCode(v.Ciudad);
    });

    var ddlCiudades = $("#ddlCiudades").data("kendoMultiSelect");

    if (typeof ddlCiudades === "undefined") {
        $("#ddlCiudades").kendoMultiSelect({
            dataTextField: "Ciudad",
            dataValueField: "Ciudad",
            itemTemplate:
                '<input id="chk#: data.Hashcode #" type="checkbox"/> <span class="k-state-default">#: data.Ciudad # <span style="font-size: xx-small">#: data.Provincia # (#:data.Cantidad#)</span></span>',
            tagTemplate: '<span class="selected-value">#:data.Ciudad#</span> <b>(#:data.Cantidad#)</b>',
            dataSource: {
                data: ciudades
            },
            height: 400,
            change: function () {
                var values = this.value();
                var data = this.dataSource.data();

                $.each(data, function (i, v) {
                    if (values.includes(v.Ciudad))
                        $("#chk" + hashCode(v.Ciudad)).prop('checked', true);
                    else
                        $("#chk" + hashCode(v.Ciudad)).prop('checked', false);
                });
            }
        });
    } else if (ddlCiudades !== null) {
        ddlCiudades.setDataSource(ciudades);
    }

    onInvestmentChange();
};

var onProvinceClicked = function (item) {
    var itemInMap = item.point["hc-key"];
    var multiSelect = $("#ddlProvincias").data("kendoMultiSelect");
    var currentValues = multiSelect.value();

    $("#chk" + itemInMap).prop('checked', !$("#chk" + itemInMap).is(":checked"));

    if (currentValues.includes(itemInMap)) {
        var index = currentValues.indexOf(itemInMap);
        if (index > -1)
            currentValues.splice(index, 1);
    } else {
        currentValues.push(itemInMap);
    }

    multiSelect.value(currentValues);
    multiSelect.trigger("change");
};