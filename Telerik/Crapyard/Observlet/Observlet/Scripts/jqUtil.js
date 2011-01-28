/// <reference path="jquery-1.3.2.js" />

/// <reference path="jquery-1.3.2-vsdoc.js" />

$(document).ready(function () {
    $('.async').click(function () {
        $(this).attr('disabled', true); 
        $('.interactasync').attr('disabled', false);               
        __doPostBack('Button1', '');
    });
    $('.interactasync').click(function () {
        $(this).attr('disabled', true);
        $('.async').attr('disabled', false);        
        __doPostBack('Button2', '');
    });
});
