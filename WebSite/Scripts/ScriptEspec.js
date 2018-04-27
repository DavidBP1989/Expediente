var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};


var urlBase = "http://www.emeci.com/site/Emeci/";
var emeciApp = angular
.module("emeciModule", [])

  .controller("emeciController", function ($scope, $http) {
    $scope.loading = true;
    $scope.especialistas = null;

    console.log('controller');
   
    var id = getUrlParameter("idespec")
    //Trae especialista
    $scope.Especialista = null;
    $http.get(urlBase + '/GetEspecialista?idespec=' + id).then(function (response) {

        $scope.Especialista = response.data;
        $scope.loading = false;
    });

   
    
});

// controller de Especialista


