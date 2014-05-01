var margin = {top: 20, right: 50, bottom: 30, left: 80};
var height = 500 - margin.top - margin.bottom;
var width = 1000 - margin.left - margin.right;

var x = d3.scale.ordinal().domain(["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]).range([90, 210, 330, 450, 570, 690, 810]);				
var y = d3.scale.linear().range([height, 0]);

var chart = d3.select("svg")
        .attr("height", height + margin.top + margin.bottom)
		.attr("width", margin.right + width + margin.left);
		
var allgroup = chart.append("g")   
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    
var tooltip = chart.append("text")
        .style("visibility", "hidden");;        
	
d3.tsv("d3-output.tsv", type, function(error, data) {

	var max = d3.max(data, function(d){return d.score;});
	var min = d3.min(data, function(d){return d.score;});
	y.domain([min, max]);
	
	var xAxisPlace;
	if(min > 0){
		xAxisPlace = height;
	}
	else if(max < 0){
		xAxisPlace = 0;
	}
	else{
		xAxisPlace = (height * max / (max - min));
	}
	
	var xAxis = d3.svg.axis().scale(x).orient("bottom");
	chart.append("g").attr("class", "axis")
		.attr("transform", "translate(" + margin.left + ", " + (height + margin.top) + ")")
		.call(xAxis);
		
	var xLabel = chart.append("g")
		.attr("transform", "translate(" + (margin.left + 810 + 20) + ", " + (height + margin.top - 10) + ")")
		.append("text")
		.style("text-anchor", "end");
	xLabel.text("Day");
		
	var yAxis = d3.svg.axis().scale(y).orient("left");
	chart.append("g").attr("class", "axis")
		.attr("transform", "translate(" + margin.left + ", " + margin.top + ")")
		.call(yAxis);
		
	var yLabel = chart.append("g")
		.attr("transform", "translate(" + (margin.left - 35) + ", " + margin.top + ")")
		.append("text")
		.attr("transform", "rotate(-90)")
		.style("text-anchor", "end");
	yLabel.text("Sentiment");

	var bar = allgroup.selectAll("g")
			.data(data)
		.enter().append("g");

	bar.append("circle")
        .attr("r", 3)
        .attr("cx", function(d) { return x(d.day); })
		.attr("cy", function(d){ return y(d.score); })
        .on("mouseover", function(d, i){
            var y = d3.select(this).attr("cy");
            var x = d3.select(this).attr("cx");
            tooltip.attr("x", x); 		
            tooltip.attr("y", y);
            tooltip.attr("dx", margin.left);
            tooltip.attr("dy", 10);
            tooltip.style("visibility", "visible");
            tooltip.style("fill", "black");
			d3.select(this).attr("class", "selectedCircle");
			var text = d.itemset + " : " + d.score; 
            tooltip.text(text);})
        .on("mouseout", function(){
            tooltip.style("visibility", "hidden");
			d3.select(this).attr("class", null)}); 
});

function type(d) {
	d.score = +d.score;
	return d;
}
