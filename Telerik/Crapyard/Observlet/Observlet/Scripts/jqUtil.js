
/// <reference path="jquery-1.4.1-vsdoc.js" />


/// <reference path="jquery-1.4.4.min.js" />

$(document).ready(function () {
    $('.async').click(function () {
        $(this).attr('disabled', true);
        $('.interactasync').attr('disabled', false);
        setTimeout('location.href = document.location', 3000)
        __doPostBack('Button1', '');

    });$.re
    $('.interactasync').click(function () {
        $(this).attr('disabled', true);
        $('.async').attr('disabled', false);
        __doPostBack('Button2', '');
    });
});
