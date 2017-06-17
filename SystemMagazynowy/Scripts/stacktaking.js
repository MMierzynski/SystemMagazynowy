var tmpSelectedAssortment;


$(function () {
    

    $("#open-modal").click(function (event) {

        $("#submit-stocktaking").attr("disabled", "disabled");

    });

    //ID wybranego magazynu
    var warehouseID = $("#WarehouseID").val();
    console.log("ID: " + warehouseID);
    $("input[type='text']#barcode").focus(function (event)
    {
        $(this).val("");
    });

    //====================================================
    // pobranie nowego ID magazynu   
    // po wyborze nowego magazynu z listy
    //====================================================
    $("#WarehouseID").change(function (event)
    {
        warehouseID = $(this).val();
        ClearAssortmentTable();
        console.log("ID: " + warehouseID);
    });

    //=============================================================
    // pobranie danych asortymentu z magazynu o określonym ID 
    // oraz danym kodzie kreskowym
    // przy pomocy AJAX 
    // oraz zwrócenie wyników w formacie JSON
    //============================================================
    $("button#search-assortment").click(function (event)
    {
        GetAssortmentFromDB(warehouseID, $("input[type='text']#barcode").val());

        return false;
    });


    ////====================================================================
    //// obsługa kliknięcia przycisku 'Dodaj' w oknie dialogowym 
    //// polegająca na dodaniu wybraanego asortymentu do tabeli i pola hidden 
    //// oraz wyczyszczeniu okna dialogowego i jego zamknięcie
    ////===================================================================
    $("button#btn-add-assortment").click(function (event)
    {
        if (!Number.isNaN(Number.parseFloat($("input[type='text']#after-quantity").val())))
        {
            tmpSelectedAssortment.AfterQuantity = $("input[type='text']#after-quantity").val();
            AddAssortmentToTable(tmpSelectedAssortment);
            AddJSONToHiddenField(tmpSelectedAssortment);
            tmpSelectedAssortment = null;
            ClearModalData();
            $("div.modal").modal("hide");
        }
        else {
            $("input[type='text']#after-quantity").addClass("bad-input-value").val("0").val("").attr("placeholder", "Musisz wpisać poprawną ilość");
        }
    });

    //====================================================================
    // obsługa kliknięcia przycisku 'Dodaj' w oknie dialogowym 
    // polegająca na dodaniu wybraanego asortymentu do tabeli i pola hidden 
    // oraz wyczyszczeniu okna dialogowego bez zamykania 
    // umożliwiająca dodanie kolejnego elementu
    //===================================================================
    $("button#btn-add-next-assortment").click(function (event) {
       
        if (!Number.isNaN(Number.parseFloat($("input[type='text']#after-quantity").val())))
        {
            tmpSelectedAssortment.AfterQuantity = $("input[type='text']#after-quantity").val();
            AddAssortmentToTable(tmpSelectedAssortment);
            AddJSONToHiddenField(tmpSelectedAssortment);
            tmpSelectedAssortment = null;
            ClearModalData();
        }
        else
        {
            $("input[type='text']#after-quantity").addClass("bad-input-value").val("").attr("placeholder","Musisz wpisać poprawną ilość");
        }

        
    });

    //====================================================================
    // obsługa kliknięcia przycisku 'Zaamknij' w oknie dialogowym 
    // realizująca wyczyszczenie danych okna dialogowego i jego zamknięcie
    //===================================================================
    $("button#btn-close").click(function (event) {
        $("#submit-stocktaking").removeAttr("disabled");
        tmpSelectedAssortment = null;
        ClearModalData();
       
    });
  

    $("button#btn-close-stocktaking").click(function (event)
    {
      //  $("div.btn-back").hide();
        $("input[name='IsOpen']").val('false');//false

        
        var d = new Date();
        $("input[name='EndDate']").val(d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate());

        $("input#open-modal[type='button']").attr("disabled", "disabled");
        $("input#btn-open-confirmation-modal[type='button']").attr("disabled", "disabled");
        $("tr button").attr("disabled", "disabled");
        $("#submit-stocktaking").removeAttr("disabled");
        $("#close-confirm").modal("hide");
    });
});




//===========================================================
// Funkcja pobierająca z bazy asortment
// o określonym kodzie kreskowym
// @param barecode - kod kreskowy
//==============================================================

function GetAssortmentFromDB(id,barcode)
{

    console.log("PATHNAME: " + window.location.pathname);
    var action = window.location.pathname.slice(14, 18);
    console.log("ACTION: " + action);
    var url = "GetAssortmentFromWarehouseByCode";
    
    if (action == "Edit")
    {
        var url = "../GetAssortmentFromWarehouseByCode";
        
    }
    console.log("URL: " + url);
    console.log("Warehouse: " + id + " || barcode: " + barcode);
    $.getJSON(url, "id=" + id + "&barcode=" + barcode, function (data) {
        AddModalData(data);

        tmpSelectedAssortment = data;
        //  console.log(tmpSelectedAssortment.Name);
    }).fail(function (jqXHR, textStatus, errorThrown)
    {

        console.log('getJSON request failed! ' + textStatus);
        var modal_body = "<hr /><h3 class='text-danger text-center'>Asortyment o kodzie kreskowym: " + barcode + " nie istnieje w wybranym magazynie.</h3>";

       // var modal_body = "<hr /><h3 class='text-danger text-center'>FAIL request: " + textStatus + "error thrown: "+errorThrown+"</h3>";
        
        $(".assortment-details").html(modal_body);
    });

}



function DisableEditForm()
{
    $("input[name='Number']").attr("disabled", "disabled");
    $("input[name='StartDate']").attr("disabled", "disabled");
    $("input#open-modal[type='button']").attr("disabled", "disabled");
    $("input#btn-open-confirmation-modal[type='button']").attr("disabled", "disabled");
}



function AddAssortmentToTable(data)
{

    var row = "<tr>" +
            "<td>" + data.Name + "</td>" +
            "<td>" + data.Barcode + "</td>" +
            "<td>" + data.BeforeQuantity + "</td>" +
            "<td><input type=\"text\" class=\"form-control\" data=" + data.AssortmentID + " name=\"after-quantity\" id=\"after-quantity\" value=\"" + data.AfterQuantity + "\" disabled /></td>" +
            "<td>"+
            "<button class=\"btn btn-default\" type=\"button\" data=" + data.AssortmentID + " onclick=\"UpdateAssortmentFromTable(" + data.AssortmentID + ") \">Edytuj</button>" +
            "<button class=\"btn btn-default\" data=" + data.AssortmentID + " onclick=\"DeleteAssortmentFromTable(" + data.AssortmentID + ")\">Usuń</button></td>" +
        "</tr>";

    $("#assortment-list-table tr:last").after(row);
}

function UpdateAssortmentFromTable(id)
{
    
    $("tr input#after-quantity").each(function ()
    {
        
        if($(this).attr("data")==id)
        {
            console.log("---->" + $(this).attr("data") + " ----> " + $("tr input#after-quantity[data='" + id + "']").attr("disabled"));
            
        }
        
    });


    if ($("tr input#after-quantity[data='" + id + "']").attr("disabled") == "disabled")
    {
        
        $("tr input#after-quantity[data='" + id + "']").removeAttr("disabled");
        $("button#update-assortment[data='" + id + "']").text("Zapisz");
    }
    else
    {
        $("tr input#after-quantity[data='" + id + "']").attr("disabled", "disabled");
        $("button#update-assortment[data='" + id + "']").text("Edytuj");

        var afterValue = $("input#after-quantity[data=" + id + "]").val();

        //console.log()

        var jsonArray = GetAssortmentFromDB();

        for(item in jsonArray)
        {
            if(item.AssortmentID==id)
            {
                item.AfterQuantity = afterValue;
                break;
            }
        }

        UpdateJSONFromHiddenField(id, afterValue);
    }
    return false;
    
    
    
    
}

function DeleteAssortmentFromTable(id)
{
    DeleteJSONFromHiddenFieldByID(id);

    $("table#assortment-list-table tr").has("button[data='" + id + "']").remove();
    return false;
}

function ClearAssortmentTable()
{
    ClearJSONFromHiddenField();
    $("table#assortment-list-table tr").has("td").remove();
}
function AddModalData(data)
{
    var modal_body = "<hr /><h3 class='text-danger text-center'>Asortyment został już dodany do listy.</h3>";

    if (!IsAddedAssortmentToJSON(data.AssortmentID))
    {
        modal_body = "<hr />" +
                            "<h3 class='text-center'>" + data.Name + "</h3>" +
                            "<h4 class='text-center'>Magazyn: " + data.WarehouseName + "</h4>" +
                            "<div class='row'>" +
                                    "<p class='col-sm-4 control-label' >Ilość w systemie</p>" +
                                    "<p class='col-sm-8'>" + data.BeforeQuantity + " " + data.Unit + "</p>" +
                                "</div>" +
                                "<div class='row'>" +
                                    "<label class='col-sm-4 control-label'>Ilość wg spisu</label>" +
                                    "<input type='text' id='after-quantity' class='form-control' name='after-quantity' placeholder='Wpisz ilość' />" +
                                    "</div>";
    }
   
    $(".assortment-details").html(modal_body);
}

function ClearModalData()
{
    $("input[type='text']#barcode").val("");
    $(".assortment-details").html("");
}


//================================================
// metody obsługująca zarządzanie zawartością 
// pola hidden zawierającego 
// wybrany asortment
//=================================================
function AddJSONToHiddenField(data)
{
    var jsonArray = GetJSONFormHiddenField();
    jsonArray[jsonArray.length] = data;//tmpSelectedAssortment;

    var json = JSON.stringify(jsonArray);
    console.log(json);
    $("input[type='hidden']#AssortmentInStocktaking").val(json);

}

function GetJSONFormHiddenField()
{
    var json = $("input[type='hidden']#AssortmentInStocktaking").val();

    var jsonArray = [];
    if (json.length > 2) {
        jsonArray = JSON.parse(json);
    }
    else
        jsonArray = [];

    return jsonArray;
}

function UpdateJSONFromHiddenField(id,afterQty) {
    var jsonArray = GetJSONFormHiddenField();
    for (a = 0; a < jsonArray.length; a++) {
        if (jsonArray[a].AssortmentID == id) {
            jsonArray[a].AfterQuantity = afterQty;
            break
        }

    }

    var json = JSON.stringify(jsonArray);
    console.log(json);
    $("input[type='hidden']#AssortmentInStocktaking").val(json);
}

function DeleteJSONFromHiddenFieldByID(id)
{
    var jsonArray = GetJSONFormHiddenField();
    var tmp= [];
    for(a=0; a<jsonArray.length;a++)
    {
        if (jsonArray[a].AssortmentID == id)
        {
            console.log("FOUND");
            continue;
        }
        else
            tmp[tmp.length] = jsonArray[a];
    }

    var json = JSON.stringify(tmp);
    console.log(json);
    $("input[type='hidden']#AssortmentInStocktaking").val(json);
}

function ClearJSONFromHiddenField()
{
    $("input[type='hidden']#AssortmentInStocktaking").val("");
}

function IsAddedAssortmentToJSON(id)
{
    var jsonArray = GetJSONFormHiddenField();

    for(i=0;i<jsonArray.length;i++)
    {
        if (jsonArray[i].AssortmentID == id) return true;
    }
    return false;
}