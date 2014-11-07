var request           = require('request');
var fs                = require('fs');
var parser            = require('./parser');
var domainInformation = require('./domainInformation');
var contentTypes      = require('./contentTypes');
var url               = require('url');
var mkdirp            = require('mkdirp');
module.exports = function(router) {

  router.get('/', function(req, res) {
        res.render('index');
    });

  router.post('/crawl', function(req, res) {
    var page = url.parse(req.body.page);
    if(req.body.domainToIndex) {
      domainInformation.domainToIndex = url.parse(req.body.domainToIndex).hostname;
    }
    if(req.body.indexedDirectory){
      domainInformation.indexedDirectory = req.body.indexedDirectory;
    }
    // if(page.hostname !== domainInformation.domainToIndex) { //the page is not part of the correct domain
    //   return;
    // }
    var requestPath = page.path;
    (function(requestPath){
      request({
        uri: page.href,
        method: "GET",
        timeout: 20000,
        followRedirect: true,
        maxRedirects: 5
      }, function(error, response, body){
          //going to [try to] dynamically determine extension type from response headers, or another method
          //for now all pages are saved as .html
          if(!response){
            return;
          }
          var contentType = response.headers['content-type'];  
          var extension = contentTypes[contentType];
          var localDirectory = domainInformation.indexedDirectory + requestPath.substr(0, requestPath.lastIndexOf('/') + 1);
          var fileName = requestPath.substr(requestPath.lastIndexOf('/') + 1, requestPath.length);
          var cleanFileName = fileName;
          if(cleanFileName.indexOf('.') > -1) {
            cleanFileName = cleanFileName.substr(0, cleanFileName.lastIndexOf('.')); //hopefully can remove this in the future
          }
          if(cleanFileName.indexOf('?') > -1) {
            cleanFileName = cleanFileName.substr(0, cleanFileName.lastIndexOf('?'));
          }
          if(cleanFileName === "") {
            cleanFileName = "index";
          }
          mkdirp(localDirectory);
          fs.writeFile(localDirectory + cleanFileName + '.html', body, function (err) {
            if (err) { 
              return;
            }
          });
          parser.write(body);
          parser.end();
          res.send({path: requestPath})
      })
    })(requestPath);
  });
};