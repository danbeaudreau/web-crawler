var htmlparser2 = require("htmlparser2");
var queue 		= require("./fileParseQueue");
var parser = new htmlparser2.Parser({
    onopentag: function(name, attribs){
        if(attribs.href){
            queue.push(attribs.href);
        }
        if(attribs.src){
        	queue.push(attribs.src);
        }
    }
});

module.exports = parser;