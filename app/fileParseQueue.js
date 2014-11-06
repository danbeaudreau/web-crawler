var async 			  = require('async');
var request 		  = require('request');
var domainInformation = require('./domainInformation');
var queue 	= async.queue(function (url) {
	request.post('http://localhost:3000/crawl', {form:{page: url}});
}, 3);

module.exports = queue;