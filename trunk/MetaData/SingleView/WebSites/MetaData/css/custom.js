/// <reference path="../js/query-1.4.2.min.js" />

 $(function() {
 $("#orderedlist").find("li").each(function(i) {
     $(this).append( " BAM! " + i );
   });

   $("a").click(function() {
     $("form").each(function() {
       this.reset();
     });

   });

});

$(document).ready(function($) {
    $(".visibility").hide();
    $("#RowId").hide();
    
    $.noConflict();
    $.ajaxSetup({
        error: function(x, e) {
            if (x.status == 0) {
                alert('You are offline!!\n Please Check Your Network.');
            } else if (x.status == 4014) {
                //alert('Requested URL not found.');
                $("#msg").ajaxComplete(function(request, settings) {
                    $(this).append("<li>Requested URL not found.</li>");
                });
            } else if (x.status == 500) {
                alert('Internel Server Error.');
            } else if (e == 'parsererror') {
                alert('Error.\nParsing JSON Request failed.');
            } else if (e == 'timeout') {
                alert('Request Time out.');
            } else {
                $("#msg").ajaxComplete(function(request, settings) {
                    $(this).append("<li>Unknow Error:\n" + x.responseText + "</li>");
                });
            }
        }

    });

    $("#.insertbutton").click(function() {
        try {
            //$(".visibility").show();
            //$("#RowId").show();
            //Get the Id of the record to delete  
            var record_id = $(this).attr("id");
            //alert('' + record_id);            
            
            //Get the GridView Row reference  
            var tr_id = $(this).parents("#.record");

            //add  GridView row
            tr_id.after('<tr class = "newrow"><td></td><td></td><td><div class="buttonarea" /></td><td><div class="inputarea" /></td></tr>');

            //add controls //value = '<%= IsPostBack ? Request.Form[" + '"trefwoordInsert"' + "]:null %>'
            $('.inputarea').append("<input id='trefwoordInsert' class = 'inputtextbox' type='text' /> ");
            $('.buttonarea').append('<input id="submit" type="image" name = "submit" alt="" src="../Images/save_32.png" class = "savebutton" />&nbsp;');            
            $('.buttonarea').append('<input id="cancel" type="image" name = "cancel" alt="" src="../Images/stop_32.png" class = "cancel" />');
            
            $('.inputarea').removeClass();
            $('.buttonarea').removeClass();

            $('.savebutton').click(function() {                
                var inputtextbox = $('.inputtextbox').val();
                $("#InlineInsert").val(inputtextbox);
                var row_index = $(this).closest("tr").prevAll("tr").length;
                $("#RowId").val(row_index);
                $(".savebutton").submit();                
            });
            $('.cancel').click(function() {
                $(".newrow").empty();
            });
            
  
        }
        catch (Error) {
            $("#ObservableByErrorHandler").append("<div>" + Error.description + "</div>");
        }
    });
    
    $(".inputtextbox").keyup(function() {
        var value = $(this).val();
        $("#InlineInsert").val(value);                
    }).keyup();
        
    $("#ObservableByErrorHandler").click(function() {
        $(this).hide("slow");
    });
    $("#Button1").click(function() {

        $(this).hide("slow");
        alert('done');

    });

    $("#Button1").click(function() {
        try {
            $("#ctl00_DefaultContent_TrefwoordView_ctl02_TrefwoordTextBox").hide("slow");
            $('.result').load('ajax/missing.html');
        }
        catch (Error) {
            $("#ObservableByErrorHandler").append("<li>" + Error.description + "</li>");
        }


    });

    $("#ObservableByErrorHandler").click(function() {
        $(this).hide("slow");

    });


    function test() {
        //            alert('hello ... ajax/test.html');
        //            $.get('ajax/test.html', function(data) {
        //              $('.result').html(data);
        //              alert('Load was performed.');
        //            });

    }

    $('.log').ajaxError(function() {
        $(this).text('Triggered ajaxError handler.');
    });

    $('.trigger').click(function() {
        $('.result').load('ajax/missing.html');
    });

    $("#msg").ajaxError(function(event, request, settings) {
        $(this).append("<li>Error requesting page " + settings.url + "</li>");
    });

    $("#msg").ajaxComplete(function(request, settings) {
        $(this).append("<li>Request Complete.</li>");
        alert('request completed');
    });

});
