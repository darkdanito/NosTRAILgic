var map
var markers = [];
var contents = [];
var infowindows = [];

$(function () {
    initMap();
    initMarker();
});

function initMap() {

    //map properties
    var mapProp = {
        center: new google.maps.LatLng(1.3538, 103.7941),
        zoom: 11,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }

    //Get Map ID from view .......getElementById
    map = new google.maps.Map(document.getElementById("map_canvas"), mapProp);
}

function initMarker() {
    var homeViewModelData = document.getElementById("homeViewModelData");    
    var homeViewModel = JSON.parse(homeViewModelData.innerText);

    //homeViewModel is a JSON obj
    // i = counter, item = enumerableAllLocation obj
    $.each(homeViewModel.enumerableAllLocation, function (i, location) {
        markers[i] = new google.maps.Marker({
            position: new google.maps.LatLng(location.Latitude, location.Longitude),
            map: map,
            title: location.Name
        });

        markers[i].index = i;
        contents[i] = '<div class="popup_container">' + location.Name + '</div>';

        infowindows[i] = new google.maps.InfoWindow({
            content: contents[i] ,
            maxWidth: 300
        });

        google.maps.event.addListener(markers[i], 'click', function () {

            //convert for last updated
            var re = /-?\d+/;
            var m = re.exec(homeViewModel.enumerableAllWeather[i].LastUpdated);
            var d = new Date(parseInt(m[0]));

            var content =
            '<div>Name: ' + location.Name + '</div>' +
            '<div>Description: ' + location.Description + '</div>' +
            '<div>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></div>' +
            '<div>Image: <img src="' + location.ImageLink + '"/></div>' +
            '<div>Postal Code: ' + location.PostalCode + '</div>' +
            '<div>Forecast: ' + homeViewModel.enumerableAllWeather[i].Forecast + ' </div>' +
            '<div>Region: ' + homeViewModel.enumerableAllWeather[i].Region + ' </div>' +
            '<div>Area: ' + homeViewModel.enumerableAllWeather[i].Area + ' </div>' +
            '<div>Icon: <img src="' + homeViewModel.enumerableAllWeather[i].Icon + '"/></div>' +
            '<div>Last Updated: ' + d + ' </div>';

            document.getElementById("locationDetails").innerHTML = content;

            infowindows[this.index].open(map, markers[this.index]);
            map.panTo(markers[this.index].getPosition());
        });
    })
}

$(document).ready(function () {
    $('#searchKeyword').autocomplete({
        source: $('#autoCompleteURL').data('url')
    });
});