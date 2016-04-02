﻿var map
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
            
            var content =
                '<section>' +
                    '<div class="col-md-4">'+
                    '<div>Name: ' + location.Name + '</div>' +
                    '<div>Description: ' + location.Description + '</div>' +
                     '<div>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></div>' +
                     '<div>Postal Code: ' + location.PostalCode + '</div></div>' +
                     
                '<div class="col-md-10" style="float:right;">Image: <img src="' + location.ImageLink + '"/></div></section>' ;
            

            
            //'<div>Name: ' + location.Name + '</div>' +
            //'<div>Description: ' + location.Description + '</div>' +
            //'<div>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></div>' +
            //'<div>Image: <img src="' + location.ImageLink + '"/></div>' +
            //'<div>Postal Code: ' + location.PostalCode + '</div>';

            //Remove empty image
            var str = content;
            var res = str.split("<div>");
            if (!res[4].includes("jpg")) {
                delete res[4];
                content = res.join("<div>")
            }

            //Remove for empty link
            var content = content.replace('<div>Link: <a href="&lt;Null&gt;">&lt;Null&gt;</a></div>', "")

            document.getElementById("locationDetails").innerHTML = content;

            infowindows[this.index].open(map, markers[this.index]);
            map.panTo(markers[this.index].getPosition());
        });
    })
}