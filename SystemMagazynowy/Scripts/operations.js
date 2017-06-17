
$(function ()
{
    $("#open-modal").click(function (event)
    {
       
        $("#submit-operation").attr("disabled", "disabled");

    });

    $("#btn-close-modal").click(function (event) {
        console.log("Clicked close btn");
        $("#submit-operation").removeAttr("disabled");
        ClearModalData();
        $("div.modal").modal("hide");
    });


    $("button#search-operation-assortment").click(function (event)
    {

        var barcode = $('input#barcode').val();
        

        if ($("input[type='hidden']#Type").val() == "Pz" || $("input[type='hidden']#Type").val() == "PzK")
        {
            var id = $("#ContractorID").find(":selected").val();
            
            loadAssortment("GetAssortmentByContractor", "id=" + id + "&barcode=" + barcode)
            loadAssortmentByContractor(id, barcode);

        }
        else if ($("input[type='hidden']#Type").val() == "Pw" || $("input[type='hidden']#Type").val() == "PwK")
        {
            loadAssortment("GetAllAssortment", "barcode="+barcode);
        }
        else
        {
            var id = $("#FromWarehouseID").find(":selected").val();
            loadAssortment("GetAssortmentByWarehouse", "warehouseId=" + id + "&barcode=" + barcode);
        }
        return false;
    });

    $("button#btn-add-operation-assortment").click(function (event)
    {
        addAssortmentBtn_Clicked();
        $("#submit-operation").removeAttr("disabled");
        ClearModalData();
        $("div.modal").modal("hide");
    });

    $("button#btn-add-next-operation-assortment").click(function (event) {
        addAssortmentBtn_Clicked();
        ClearModalData();
        
    });
    ////jeżeli został wybrany kontrahent załaduj odpowiadający mu asortyment
    ////który można dodać do operacji
    //if ($("#ContractorID").find(":selected").val() != undefined)
    //{

    //    alert("");
    //    if ($("input[type='hidden']#Type").val() == "Pz" || $("input[type='hidden']#Type").val() == "PzK")
    //    {

    //        var contractorID = $("#ContractorID").find(":selected").val();
            

    //      //  loadAssortmentByContractor(contractorID);
    //    }
    //}

    
    //if($("#FromWarehouseID").find(":selected").val()!= undefined)
    //{
    //    if ($("input[type='hidden']#Type").val() == "Wz" || $("input[type='hidden']#Type").val() == "WzK"
    //        || $("input[type='hidden']#Type").val() == "Mm" || $("input[type='hidden']#Type").val() == "MmK"
    //        || $("input[type='hidden']#Type").val() == "Rw" || $("input[type='hidden']#Type").val() == "RwK")
    //    {
    //        var warehouseID = $("#FromWarehouseID").find(":selected").val();
    //       // loadAssortmentInWarehouse(warehouseID);
    //    }
    //}

    //obsługa zmiany wyboru kontrahenta w formularzu
    $("#ContractorID").change(function (event) {

        
        if ($("input[type='hidden']#Type").val() == "Pz" || $("input[type='hidden']#Type").val() == "PzK") {
            console.log("Contractor changed");
            //usunięcie asortymentu z listy, który nie należy 
            //do aktualnego dostawcy
             $("#assortment-list-table td").parent("tr").remove();
            clearSelectedAssortment();
        }
    });

    $("#FromWarehouseID").change(function (event)
    {
        if ($("input[type='hidden']#Type").val() == "Wz" || $("input[type='hidden']#Type").val() == "WzK"
            || $("input[type='hidden']#Type").val() == "Mm" || $("input[type='hidden']#Type").val() == "MmK"
            || $("input[type='hidden']#Type").val() == "Rw" || $("input[type='hidden']#Type").val() == "RwK")
        {
            $("#assortment-list-table td").parent("tr").remove();
            clearSelectedAssortment();
        }
    });



    //if ($("input[type='hidden']#Type").val() == "Wz")
    //{
    //    var warehouseID = $("#FromWarehouseID").find(":selected").val();
    //    loadAssortmentInWarehouse(warehouseID);
    //}
    //else 




    //if ($("input[type='hidden']#Type").val() == "Pw" || $("input[type='hidden']#Type").val() == "PwK") {
    //    $.getJSON(prepareActionForAjax("GetAllAssortment"), null,
    //        function (data) {
    //            var rows = "<table class=\"table\" id=\"table-in-modal\">"
    //                        + "<tr>"
    //                            + "<th>Nazwa</th>"
    //                            + "<th>Kod kreskowy</th>"
    //                           + "<th></th>"
    //                        + "</tr>";
                
    //            for (i = 0; i < data.length; i++) {
                    
    //                rows += "<tr>"
    //                        + "<td>" + data[i].Name + "</td>"
    //                        + "<td>" + data[i].BarCode + "</td>"
    //                        + "<td><button type=\"button\" value=\"" + data[i].ID + "\" id=\"add-assortment-to-list\" class=\"btn btn-default\" onclick=\"addAssortmentBtn_Clicked(" + (i + 1) + "," + data[i].ID + ")\">Dodaj</button></td>"
    //                    + "</tr>"
    //            }
    //            rows += "</table>";
    //            //dodanie tabeli do okna dialogowego
    //            $("div.modal-body").html(rows);
    //        });
    //}
});


function prepareActionForAjax(action)
{
    var actionName = window.location.pathname.slice(11, 15);
    console.log("ACTION: " + actionName);

    

    if (actionName == "Edit") 
    {
        console.log("URL: " +"../"+action);
        return "../"+action;
    }
        return action;

}

function loadAssortment(actionName, parameters)
{
    $("div.modal-body .assortment-details").html("");


    //zapytanie ajax pobierające dane z db
    $.getJSON(prepareActionForAjax(actionName), parameters, function (data) {
        console.log(data);
        tmpSelectedAssortment = data;
        addDataToModal(data);
    });
}

    //=====================================================================
    //=====================================================================
    // Funkcja ładująca asortyment poczodzący od kontrahenta
    // o id podanym w parametrze funkcji
    //=====================================================================
    function loadAssortmentByContractor(contractorId,barcode)
    {
        //usunięcie tabeli z załądowanymi wcześniej danymi
        
        $("div.modal-body .assortment-details").html("");

    
        //zapytanie ajax pobierające dane z db
        $.getJSON(prepareActionForAjax("GetAssortmentByContractor"), "id=" + contractorId + "&barcode=" + barcode, function (data)
        {
            console.log(data);
            tmpSelectedAssortment = data;
            addDataToModal(data);
        });
    }

    function loadAssortmentInWarehouse(warehouseId)
    {
        $("div.modal-body").remove("table");
        $.getJSON(prepareActionForAjax("GetAssortmentByWarehouse"), "warehouseId=" + warehouseId, function(data){addTableInToModalFromJSON(data)});

    }

    function addDataToModal(data)
    {
        var modal_body = "<hr /><h3 class='text-danger text-center'>Asortyment został już dodany do listy.</h3>";
        var addedAssortment = false;
        var jsonArray = getSelectedAssortment();
        console.log(jsonArray);
        for (a = 0; a < jsonArray.length; a++)
        {
            console.log("a.ID: " + jsonArray[a]);
            if(jsonArray[a].ID == data.ID)
            {
                addedAssortment = true;
                break;
            }
        }
        
        console.log("added: " + addedAssortment);

        if (!addedAssortment) {
            modal_body = "<hr />" +
                                "<h3 class='text-center'>" + data.Name + "</h3>" +
                                "<h4 class='text-center'>Magazyn: " + data.BarCode + "</h4>" +
                                "<div class='row'>" +
                                    "<label class='col-sm-4 control-label'>Ilość </label>" +
                                    "<input type='text' id='quantity' class='form-control' name='quantity' placeholder='Wpisz ilość' />" +
                                    "</div>";
        }

        $(".assortment-details").html(modal_body);
    }

    //function addTableInToModalFromJSON(data)
    //{
    //    alert(data);
    //    var rows = "<table class=\"table\" id=\"table-in-modal\">"
    //                            + "<tr>"
    //                                + "<th>Nazwa</th>"
    //                                + "<th>Kod kreskowy</th>"
    //                               + "<th></th>"
    //                            + "</tr>";
        
    //    for (i = 0; i < data.length; i++) {

    //        rows += "<tr>"
    //                + "<td>" + data[i].Name + "</td>"
    //                + "<td>" + data[i].BarCode + "</td>"
    //                + "<td><input type='hidden' id='max-quantity' value='"+data[i].Quantity+"'/><button type=\"button\" value=\"" + data[i].ID + "\" id=\"add-assortment-to-list\" class=\"btn btn-default\" onclick=\"addAssortmentBtn_Clicked(" + (i + 1) + "," + data[i].ID + ")\">Dodaj</button></td>"
    //            + "</tr>"
    //    }
    //    rows += "</table>";
    //    //dodanie tabeli do okna dialogowego
    //    $("div.modal-body").html(rows);
    //}

    function getSelectedAssortment()
    {
        var jsonArray = [];
        if ($("#SelectedAssortment").val().length > 2) {
            jsonArray = JSON.parse($("#SelectedAssortment").val());
        }
        else
            jsonArray = [];

        return jsonArray;
    }
    
   

    //=================================================================================
    // Obsługa kliknięcia przycisku w oknie dialogowym
    // pozwalająca dodać asortyment z listy 
    // do operacji
    //=================================================================================
    function addAssortmentBtn_Clicked()
    {

        //pobranie wszystkich td z wybranego wiersza
        //var cols = $("#table-in-modal tr").eq(pos).children();


        //var maxQuantity = cols.eq(2).children().eq(0).val();
        ////utworzenie obiektu z danych pobranych z wiersza tabeli
        ////obiekt jest wykorzystywany do oznaczania wybranych elementów 
        //var jsonObject = new Object();
        //jsonObject.ID = id;
        //jsonObject.Name = cols.eq(0).text();
        //jsonObject.BarCode = cols.eq(1).text();
        //jsonObject.Quantity = 1;
        if (!Number.isNaN(Number.parseFloat($("input[type='text']#quantity").val()))) {
            tmpSelectedAssortment.Quantity = $("input[type='text']#quantity").val();


            //dodanie wcześniej utworzonego obiktu do listy wcześniej wybranych elementów
            var jsonArray = getSelectedAssortment();

            var arrayLen = jsonArray.length;
            jsonArray[jsonArray.length] = tmpSelectedAssortment;


            var jsonString = JSON.stringify(jsonArray);
            console.log(jsonString);
            $("#SelectedAssortment").val(jsonString);
          //  $("button[value='" + id + "']").attr("disabled", "disabled");

            //pobranie tabeli i dodanie wiersza na końcu
            var table = $("#assortment-list-table");
            var selector = "<tr><td>" + tmpSelectedAssortment.Name + "</td>"
                + "<td>" + tmpSelectedAssortment.BarCode + "</td>"
                + "<td><input type=\"number\" min=\"1\" class = \"form-control\" id='quantity_" + tmpSelectedAssortment.ID + "'  name=\"quantity\" value=\""+tmpSelectedAssortment.Quantity+"\"  onchange=\"onchangeAssortmentQuantity(" + tmpSelectedAssortment.ID + ")\"/></td>"
                + "<td><button type=\"button\" id=\"delete_" + tmpSelectedAssortment.ID + "\" class=\"btn btn-default\" onclick=\"deleteAssortmentFromList_Clicked(" + tmpSelectedAssortment.ID + ")\">Usuń</button></td></tr>";

            $("#assortment-list-table tr:last").after(selector);
        }

    }


    //=======================================================================
    // Usunięcie wszystkich elementów z listy 
    // wybranych elementów w operacji
    //========================================================================
    function clearSelectedAssortment()
    {
        var jsonSelected = "";
        //jsonSelected = JSON.stringify(jsonSelected);
        $("#SelectedAssortment").val(jsonSelected);

        $("#assortment-list-table td").parent("tr").remove();
    }

    //=======================================================================
    // Obsługa kliknięcia przycisku Usuń w liście asortymentu
    // dodanego do operacji.
    //=======================================================================
    function deleteAssortmentFromList_Clicked(id)
    {
        
        //pobranie listy wybranych elementów w formacie JSON 
        var jsonSelected = $("#SelectedAssortment").val();

        //tablica wybranego asortymentu
        var items = [];
        //jeśli wartość jsonSelect jest różna od '[]' deserializuj liste 
        if (jsonSelected.length > 2)
        {
            items = JSON.parse(jsonSelected);
        }
    
        //tablica elementów po usunięciu wybranego
        var jsonArray = []
    
        //wyszukanie i usunięcie elementu z listy o określinym id
        for (i = 0; i < items.length;i++)
        {
            if (items[i].ID != id) {
                jsonArray[jsonArray.length] = items[i];
            }
        }

        //seralizacja tablicy elementó
        var jsonString = JSON.stringify(jsonArray);

        //zaoisanie listy w polu hidden formualrza 
        $("#SelectedAssortment").val(jsonString);

        //usunięcie elementu z tabeli 
        $("#assortment-list-table tr").has("button[id=\"delete_" + id + "\"]").remove();

        //aktywacja przycisku 'Dodaj' w oknie dialogowym
        //który odpowiada usuniętemu elementowi 
        //aby móc go ponownie wybrać
        $("#table-in-modal button[value='" + id + "']").removeAttr("disabled");
    
    }

    //================================================================
    //Obsługa zdarzenie change generowanego 
    //podczas zmiany ilości asortymentu  
    //================================================================
    function onchangeAssortmentQuantity(id)
    {
        //pobranie listy wybranych elementów
        var jsonSelected = $("#SelectedAssortment").val();
        var items = [];
        //deserializacja listy 
        if (jsonSelected.length > 2) {
            items = JSON.parse(jsonSelected);
        }

        //zmiana wartości wybranego elementu
        for(i = 0; i<items.length;i++)
        {
            var item = items[i];
            if (item.ID == id)
                items[i].Quantity = $("input[id='quantity_" + id + "']").val();
        }

        //ponowna serializacja tablicy oraz zapisanie do pola hidden
        var jsonString = JSON.stringify(items);
        $("#SelectedAssortment").val(jsonString); 
    }
