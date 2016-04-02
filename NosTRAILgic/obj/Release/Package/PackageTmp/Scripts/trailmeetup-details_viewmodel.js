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
            
            var content =
                '<table style="width:70%">' +

            '<tr>' +
                 '<td><div><img src="' + location.ImageLink + '"/></div> ' +
                 '<div><p>Postal Code: ' + location.PostalCode + '</p></div></td>' +
                 '<td></td><td><div><h3> ' + location.Name + '</h3></div>' +
                     '<div><p>Description: ' + location.Description + '</p></div>' +
                     '<div><p>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></p></div></td>' +

            '</tr></table>';
                //    '<div style="width:70%;">'+
                //    '<h4>' + location.Name + '</h4>' +
                //    '<p>Description: ' + location.Description + '</p>' +
                //     '<p>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></p>' +
                //     '<p>Postal Code: ' + location.PostalCode + '</p></div>' +

                //'<div style="width:30%; "><p><img src="' + location.ImageLink + '"/></p></div>' ;
            //'<div><h4>Name: Hello World </h4></div>' +
            //'<p>Description: Hello</p>' +
            //'<div>Link: Hi</div>' +
            //'<div>Postal Code: hello</div>';
            //'<div><h4>Name: ' + location.Name + '</h4></div>' +
            //'<p>Description: ' + location.Description + '</p>' +
            //'<div>Link: <a href="' + location.HyperLink + '">' + location.HyperLink + '</a></div>' +
            //'<div>Image: <img src="' + location.ImageLink + '"/></div>' +
            //'<div>Postal Code: ' + location.PostalCode + '</div>';
            
            
            //Remove empty image
            var str = content;
            var res = str.split("<div>");
            if (!res[1].includes("jpg")) {
                delete res[1];
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