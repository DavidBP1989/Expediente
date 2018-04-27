var urlBase = "http://www.emeci.com/site/Emeci/";
var emeciApp = angular
.module("emeciModule", [])
.controller("emeciController", function ($scope, $http) {
    $scope.loading = true;
    $scope.especialistas = null;
    
    console.log('controller');

    $scope.btn_Click= function()
    { // click en el boton
        console.log('click..!');
        $scope.loading = true;
        var idciudad = $("#sCiudad").val();
        //idciudad = $scope.Ciudades.idciudad;
        $http.get(urlBase + '/ByCiudad?idc=' +idciudad).then(function (response) {

            $scope.especialistas = response.data;
            $scope.loading = false;
           
           
        });
    }
    //Trae lista de estados
    $scope.Estados = null;
    $http.get(urlBase + '/GetEstados').then(function (response) {

        $scope.Estados = response.data;
        $scope.loading = false;
    });

    //Trae lista de ciudades
    $scope.Ciudades = null;
    $scope.getCiudades=function(idEst)
    {
        $scope.loading = true;
        console.log("ides" + idEst)
        $http.get(urlBase+ '/GetCiudades?idest=' + idEst).then(function (response) {

            $scope.Ciudades = response.data;
            $scope.loading = false;
            

        });

    }
    
});

// controller de Especialista


