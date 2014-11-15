var express        = require('express');
var app            = express();
var bodyParser     = require('body-parser');
var ejs 		   = require('ejs');

app.set('view engine', 'ejs');
app.use( bodyParser.json() );
app.use( bodyParser.urlencoded({extended:true}) ); 

app.use(express.static(__dirname + '/public'));

var port = process.env.PORT || 3000; 


var router = express.Router();
require('./app/routes')(router);
app.use('/', router);


app.listen(port);                                
console.log('Node server started on port ' + port);     
exports = module.exports = app;                         
