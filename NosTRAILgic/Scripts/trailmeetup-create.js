$(document).ready(function () {
    var $fieldCount = 0;
    $('#addField').on('click', function () {
        $fieldCount++;;

        var $node = '<div class="form-group" id="locationField"><label class="control-label col-md-2" for="text">Location</label><div class="col-md-10"><input type="text" placeholder="Location" style="width:100%" name="text" id="text"/><span class="removeField glyphicon glyphicon-remove"></span></div></div>';

        $(dynamicDivInput).last().after($node);

        $('#text').autocomplete({
            source: $('#autoCompleteURL').data('url')
        });
    });

    $('form').on('click', '.removeField', function () {
        $('#locationField').remove();
    });
});