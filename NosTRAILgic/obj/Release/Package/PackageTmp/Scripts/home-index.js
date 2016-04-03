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
    var loginStatus = document.getElementById("loginStatus");

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
            var date = d.getDate();
            var month = d.getMonth()+1; // returns the month (from 0 to 11)
            var year = d.getFullYear();
            var hour = d.getHours();
            var updateArray = [date.toString(), "/", month.toString(), "/", year.toString(), "   ", hour.toString(), ":00"];
            var update = updateArray.join('');

            var content1 =
            '<hr/>' +
            '<table style="width:70%;" cellpadding="10" id="detailtable">' +
                '<tr>' +
                     '<td rowspan="2" width:"20%" align="center">' +
                        '<div><img src="' + location.ImageLink + '" style="width:150px; height:150px;" onError="this.onerror=null;this.src=\'/images/no_image_available.jpg\';"/></div><br>';

            var encodeName = encodeURI(location.Name);
            var checkInButton =
                    '<div><input class="btn btn-default" value="I am Here!" type="button" onclick="window.location.href=\'/Home/CheckIn?LocationName=' + encodeName + '\';"/></div>';
            
            var content2 =
                     '</td>' +
                     '<td style="vertical-align: top;" width:"60%" >' +
                        '<h4>' + location.Name + '</h4>' +
                        '<i> Postal: ' + location.PostalCode + '</i>' +
                     '</td>' +
                     '<td rowspan="2" align="center" width:"20%">' +
                        '<div><img src="' + homeViewModel.enumerableAllWeather[i].Icon + '"/></div>' +
                         '<div>' + homeViewModel.enumerableAllWeather[i].Forecast + ' </div><br>' +
                        '<div><i>Updated:</i><br><i>' + update + '</i></div>' +
                     '</td>' +
                '</tr>' +

                '<tr>' +
                    '<td style="vertical-align: top;">' +
                        '<p>' + location.Description + '</p>' +
                        '<a href="' + location.HyperLink + '"> Read More </a>' +
                    '</td>' +
                '</tr>' +
            '</table>';

            //Validate to in checkin button or not            
            if (loginStatus.innerHTML == "Login") {
                var contentArray = [content1,checkInButton, content2];
            }else{
                var contentArray = [content1, content2];
            }

            var content = contentArray.join('');

            document.getElementById("locationDetails").innerHTML = content;

            infowindows[this.index].open(map, markers[this.index]);
            map.panTo(markers[this.index].getPosition());
        });
    })
}