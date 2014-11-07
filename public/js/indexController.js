app.controller("indexController", function ($scope, $http) {
		$scope.page;
		$scope.domain;
		$scope.localdirectory;
		$scope.indexedFiles = [];

		$scope.crawl = function() {
			if(!$scope.page || !$scope.domain || !$scope.localdirectory) {
				return false;
			}

			$http.post('/crawl', {page: $scope.page, domainToIndex: $scope.domain, indexedDirectory: $scope.localdirectory }).success(function(data, status, headers, config){
				$scope.indexedFiles.push(data.path);
			}
		};

		$scope.validateURL = function(url) {
			if(url.indexOf('.') === -1){
				return false;
			}
			if(url.indexOf("http://") === -1 ){
				return false;
			}
			return true;
		};

		$scope.checkDomains = function(page, domain) {
			if(page.indexOf(domain) === -1) {
				return false;
			}
			return true;
		}
});