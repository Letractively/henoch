/// <reference path="jquery-1.3.2.js" />

/// <reference path="jquery-1.3.2-vsdoc.js" />

$(document).ready(function () {
    $('.async').click(function () {
        $(this).attr('disabled', true);
        __doPostBack('Button1', ''); 
    });
});
