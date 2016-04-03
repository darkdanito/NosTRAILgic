google.charts.load('current', { packages: ['corechart', 'bar', 'geochart', 'line'] });

$(document).ready(function () {
    google.charts.setOnLoadCallback(drawChartTopTrail);
});


function drawChartTopTrail() {
    var statsByTopTrailContributorData = document.getElementById("statsByTopTrailContributorData");    
    var statsByTopTrailContributor = JSON.parse(statsByTopTrailContributorData.innerText);
    var array = [];
    array.push(['Creator Name', 'Number of Participants']);
    for (var i = 0; i < statsByTopTrailContributor.length; i++) {
        var temp = [];
        temp.push(item[i].Name);
        temp.push(item[i].Number);
        array.push(temp);
    }
    var data = new google.visualization.arrayToDataTable(array);
    //data.addColumn('Creator Name', 'Number of Participants');
        //$.each(statsByTopTrailContributor, function (i, item) {
        //    data.addColumn(item[i].Name, item[i].Number);
        //});

    var options = {
        title: 'Top Trail Contributor (Top 5)',
        chartArea: { width: '75%' },
        hAxis: {
            title: 'Total Participants',
            minValue: 0
        },
        vAxis: {
            title: 'Trail Creator Name'
        }
    };
    var chart = new google.visualization.BarChart(document.getElementById('ChartTopTrail'));
    chart.draw(data, options);
}

//google.charts.setOnLoadCallback(drawChartSearchLocation);
//function drawChartSearchLocation() {
//    var data = new google.visualization.DataTable();
//    data.addColumn('number', 'Day');
//    data.addColumn('number', 'Number Of Visitors');

//    data.addRows([
//    @for (var i = 1; i < 32; i++)
//    {
//        var skip = 0;
//        foreach (var item in @ViewBag.searchLocationDay)
//        {
//            if (i.Equals(item.Date))
//            {
//                @Html.Raw("[" + Json.Encode(item.Date) + ", " + Json.Encode(item.Number) + "],")
//                skip = 1;
//            }

//            continue;
//        }
//        if(skip.Equals(1))
//        {

//        }
//        else
//        {
//            @Html.Raw("[" + i.ToString() + ", " + 0 + "],")
//        }
//    }
//]);

//    var options = {
//        axes: {
//            x: {
//                all: {
//                    range: {
//                        max: 31,
//                        min: 0
//                    }
//                }
//            },
//            y: {
//                all: {
//                    range: {
//                        min: 0
//                    }
//                }
//            }
//        }
//    };

//    var chart = new google.charts.Line(document.getElementById('ChartSearchLocation'));

//    chart.draw(data, options);
//}

//google.charts.setOnLoadCallback(drawChart);
//function drawChart() {
//    var data = google.visualization.arrayToDataTable([
//      ['Year', 'Hours per Day'],
//      @foreach (var item in @ViewBag.statsByCheckInCurrentYear)
//    {
//        @Html.Raw("['" + Json.Encode(item.Date) + "'," + Json.Encode(item.Number) + "],");
//    }
//    @foreach (var item in @ViewBag.statsByCheckInPreviousYear)
//    {
//      @Html.Raw("['" + Json.Encode(item.Date) + "'," + Json.Encode(item.Number) + "],");
//    }
//]);

//    var options = {
//        title: 'Visitor based on current and previous year',
//        pieHole: 0.4,
//        width: "100%",
//        height: "500",
//    };

//    var chart = new google.visualization.PieChart(document.getElementById('ChartCategory'));
//    chart.draw(data, options);
//}
