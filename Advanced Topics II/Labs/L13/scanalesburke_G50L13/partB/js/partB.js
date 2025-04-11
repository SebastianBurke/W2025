const margin = {top: 20, right: 30, bottom: 50, left: 60},
width = 800 - margin.left - margin.right,
height = 500 - margin.top - margin.bottom;

const svg = d3.select("body")
.append("svg")
.attr("width", width + margin.left + margin.right)
.attr("height", height + margin.top + margin.bottom)
.append("g")
.attr("transform", `translate(${margin.left},${margin.top})`);

d3.csv("data/auto-mpg.csv").then(data => {
data.forEach(d => {
d.mpg = +d.mpg;
d.weight = +d.weight;
});

const x = d3.scaleLinear()
.domain(d3.extent(data, d => d.mpg))
.range([0, width]);

const y = d3.scaleLinear()
.domain(d3.extent(data, d => d.weight))
.range([height, 0]);

svg.append("g")
.attr("transform", `translate(0,${height})`)
.call(d3.axisBottom(x));

svg.append("g")
.call(d3.axisLeft(y));

// X Axis Label
svg.append("text")
  .attr("text-anchor", "middle")
  .attr("x", width / 2)
  .attr("y", height + 40)
  .style("font-weight", "bold")
  .text("Miles Per Gallon");

// Y Axis Label
svg.append("text")
  .attr("text-anchor", "middle")
  .attr("transform", `rotate(-90)`)
  .attr("x", -height / 2)
  .attr("y", -40)
  .style("font-weight", "bold")
  .text("Weight");
  svg.append("g")
  .attr("transform", `translate(0,${height})`)
  .call(d3.axisBottom(x));

svg.append("g")
  .call(d3.axisLeft(y));

const tooltip = d3.select("body")
.append("div")
.style("opacity", 0)
.attr("class", "tooltip")
.style("background-color", "#fff")
.style("border", "1px solid black")
.style("padding", "5px")
.style("position", "absolute");

svg.selectAll("circle")
.data(data)
.enter()
.append("circle")
.attr("cx", d => x(d.mpg))
.attr("cy", d => y(d.weight))
.attr("r", 5)
.style("fill", "#4682B4")
.on("mouseover", function(event, d) {
    tooltip.style("opacity", 1)
      .html(`The exact value of<br>the Miles per Gallon is: ${d.mpg} and the car weighs ${d.weight} pounds`)
      .style("left", (event.pageX + 10) + "px")
      .style("top", (event.pageY - 28) + "px");
  })
  
  .on("mouseout", function() {
    tooltip.style("opacity", 0);
  });
  
});
