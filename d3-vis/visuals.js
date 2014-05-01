var margin = { top: 50, right: 100, bottom: 200, left: 100 };
var width = 700 - margin.left - margin.right;
var height = 500 - margin.top - margin.bottom;

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

    var sentimentScale = d3.scale.linear()
        .range([-100, height])
        .domain([-100, 100]);

    var dayScale = d3.scale.ordinal()
        .rangePoints([1, 7])
        .domain(["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]);

    var xAxis = d3.svg.axis()
        .scale(dayScale)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(sentimentScale)
        .orient("left");

    chart.append("g")
	    .attr("class", "axis")
	    .attr("transform", "translate(0, " + 100 + ")")
	    .call(xAxis);


    chart.selectAll("circle")
        .data(rows)
    .enter().append("circle")
        .attr("cx", function (d) { return dayScale(d.frequency[0]); })
        .attr("cy", function (d) { return sentimentScale(d.frequency[1]) })
        .attr("r", 3);
    /*   .on("mouseover", function (d, i) {
           var tipy = d3.select(this).attr("cy");
           d3.select(this).style("fill", "#f60");
           var tipx = d3.select(this).attr("cx");
           tooltip.attr("x", tipx);
           tooltip.attr("y", tipy);
           tooltip.attr("dx", 50);
           tooltip.attr("dy", -20);
           tooltip.style("font-size", "40px");
           tooltip.style("visibility", "visible");
           tooltip.style("fill", "black");
           tooltip.text(d.state + ": " + Math.round((d.gdp / d.population) * 1000) / 1000);
           chart.append("tooltip");
       })*/
});
