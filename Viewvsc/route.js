var app = angular.module("Admin",["ngRoute"])
app.config(function($routeProvider){
    $routeProvider
    .when("/Home",{
        templateUrl: "view/Home.html",
    })
    .when("/BanHang", {
        templateUrl: "view/BanHang.html",
    })

    .otherwise({
        redirectTo:"/Home"
    })
})