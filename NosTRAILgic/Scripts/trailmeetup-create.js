$(document).ready(function () {
    var $fieldCount = 0;
    $('#text').autocomplete({
        source: $('#autoCompleteURL').data('url')
    });

    $('#addField').on('click', function () {
        $fieldCount++;;

        var $node = '<div class="form-group" id="locationField"><label class="control-label col-md-2" for="text">Location</label><div class="col-md-10"><input type="text" placeholder="Location" style="width:100%" name="text" id="textF"/><span class="removeField glyphicon glyphicon-remove"></span></div></div>';

        $(dynamicDivInput).last().after($node);
        $('#textF').autocomplete({
            source: $('#autoCompleteURL').data('url')
        });
        
    });

    $('form').on('click', '.removeField', function () {
        $('#locationField').remove();
    });

    $('form').on('submit', function () {

        var formLocation = $('#text').val();
        var jSLocation = $('#textF').val();
        if (jSLocation == '') {

            //this means the input value was empty
            alert("addtional LOCATION field is required")
            return false;
        };
        if (formLocation == '') {

            //this means the input value was empty
            alert("LOCATION field is required")
            return false;
        };
        if (formLocation == jSLocation) {
            alert("Location field cannot be the same!")
            return false;
        };

    });

});