app.controller("indexController", function ($scope, $http) {
		$scope.page;
		$scope.domain;
		$scope.localdirectory;
		$scope.indexedFiles = [];

		$scope.crawl = function() {
			if(!$scope.page || !$scope.domain || !$scope.localdirectory) {
				return false;
			}
			if(!$scope.validateURL($scope.page)) {
				return false;
			}
			$http.post('/crawl', {page: $scope.page, domainToIndex: $scope.domain, indexedDirectory: $scope.localdirectory }).success(function(data, status, headers, config){
				$scope.indexedFiles.push(data.path);
			});
		};

		$scope.validateURL = function(url) {
			var checkDot = checkIfPartOfString(url, ".");
			var checkHttp = checkIfPartOfString(url, "http://");
			if(checkDot && checkHttp) {
				return true;
			}
			return false;
		};


		function checkIfPartOfString(string, text){
			if(string.indexOf(text) === -1){
				return false;
			}
			return true;
		}
});