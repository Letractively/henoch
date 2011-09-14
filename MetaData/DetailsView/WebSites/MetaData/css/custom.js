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

$(document).ready(function() {
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

            //Get the Id of the record to delete  
            var record_id = $(this).attr("id");

            //Get the GridView Row reference  
            var tr_id = $(this).parents("#.record");

            //add  GridView row by cloning.
            var cloned = tr_id.clone(true);

            var $lables = cloned.children().find('.label').css('background-color', 'red');
            var $buttons = cloned.children().find('.button').css('background-color', 'red');
            cloned.insertAfter(tr_id);

            cloned.find('.inputtextbox').append("<input id='trefwoordInsert' type='text' />");
            cloned.find('.savebutton').append("<img alt='' src='../Images/save_32.png' />&nbsp;<img alt='' src='../Images/stop_32.png' />");
            $lables.hide();
            $buttons.hide();
            
            $("#test tr:last").after('<td>WORLD!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td> 
            $('#test2 > tbody:last').after('<td>WORLD!!</td><td></td><td></td>');   //<td>WORLD!</td><td></td><td></td>   

            //$('.label').hide("slow"); $('.button').hide("slow");
            //alert('done');
        }
        catch (Error) {
            $("#ObservableByErrorHandler").append("<div>" + Error.description + "</div>");
        }
    });

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
            ('.result').load('ajax/missing.html');
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
