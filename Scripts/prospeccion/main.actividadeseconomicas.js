var actividadesInMem;

function compareActividad(a, b) {
    if (a.Codigo < b.Codigo)
        return -1;
    if (a.Codigo > b.Codigo)
        return 1;
    return 0;
}

var onGetActividadesEconomicasSuccess = function(json) {

    $.each(json, function (i, v) {
        v.CodigoX = v.Codigo.replace(".", "_");
    });

    actividadesInMem = json;

    for (var i = 1; i <= 6; i++) {
        $("#ddlActividades" + i).kendoMultiSelect({
            dataTextField: "Descripcion",
            dataValueField: "Codigo",
            itemTemplate: '<input id="chk#: data.CodigoX #" type="checkbox"/> <span class="k-state-default">#: data.Descripcion #</span>',
            dataSource: {
                data: []
            },
            change: onActividadesChange
        });
    }

    var ddlActividades = $("#ddlActividades1").data("kendoMultiSelect");
    ddlActividades.setDataSource(json.sort(compareActividad));
};

var onActividadesChange = function (e) {
    var values = this.value();
    var data = this.dataSource.data();

    $.each(data, function (i, v) {
        if (values.includes(v.Codigo))
            $("#chk" + v.CodigoX).prop('checked', true);
        else
            $("#chk" + v.CodigoX).prop('checked', false);
    });

    var elId = e.sender.element[0].id;
    var elIndex = parseInt(elId[elId.length - 1]);
    var items = this.dataItems();
    var allItems = new Array();

    $.each(items, function (i, v) {
        $.each(v.Subactividades.toJSON(), function (i2, v2) {
            v2.CodigoX = v2.Codigo.replace(".", "_");
            allItems.push(v2);
        });
    });

    var ddlActividades = $("#ddlActividades" + (elIndex + 1)).data("kendoMultiSelect");

    if (typeof ddlActividades === "undefined") {
        $("#ddlActividades" + (elIndex + 1)).kendoMultiSelect({
            dataTextField: "Descripcion",
            dataValueField: "Codigo",
            dataSource: {
                data: allItems.sort(compareActividad)
            },
            change: onActividadesChange
        });
    } else if (ddlActividades !== null) {
        ddlActividades.setDataSource(allItems.sort(compareActividad));
    }

    onInvestmentChange();
};
