$(document).ready(function () {
    $('#searchKeyword').autocomplete({
        source: $('#autoCompleteURL').data('url')
    });
});