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
    var detailsViewModelData = document.getElementById("detailsViewModelData");
    var detailsViewModel = JSON.parse(detailsViewModelData.innerText);

    //detailsViewModel is a JSON obj
    // i = counter, item = enumerableAllLocation obj
    $.each(detailsViewModel.enumerableAllLocationFromTrail, function (i, location) {
        markers[i] = new google.maps.Marker({
            position: new google.maps.LatLng(location.Latitude, location.Longitude),
            map: map,
            title: location.Name
        });

        markers[i].index = i;
        contents[i] = '<div class="popup_container">' + location.Name + '</div>';

        infowindows[i] = new google.maps.InfoWindow({
            content: contents[i],
            maxWidth: 300
        });

        google.maps.event.addListener(markers[i], 'click', function () {
            

         
            document.getElementById("locationDetails").innerHTML = content;

            infowindows[this.index].open(map, markers[this.index]);
            map.panTo(markers[this.index].getPosition());
        });
    })
}