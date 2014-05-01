var margin = { top: 150, right: 100, bottom: 200, left: 200 };
var width = 1500 - margin.left - margin.right;
var height = 1500 - margin.top - margin.bottom;

var chart = d3.select("body").append("svg")
        .attr("width", width + margin.left + margin.right)
		.attr("height", height + margin.top + margin.bottom)
	.append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

var tooltip = chart.append("text")
        .style("visibility", "hidden");

var rows = d3.csv("outputD3.csv", function (d) {
    var freq = d.frequency;
    var freqArr = freq.split(" ");
    freqArr.splice(0, 1);
    return {
        basketCount: +d.basketCount,
        frequency: freqArr,
        item1: +d.item1,
        item2: +d.item2,
        item3: +d.item3,

    };
}, function (error, rows) {
    console.log(error);
    console.log(rows);

});
