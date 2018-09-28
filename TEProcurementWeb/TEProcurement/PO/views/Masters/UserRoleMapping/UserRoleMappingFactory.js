angular.module('TEPOApp')
.factory('UserRoleMappingFactory', function($sessionStorage, $http) {
    var result = {};
    result.GetPagenation=function(data){return $http.post($sessionStorage.PortfolioAPI+'TEUsersRoleAPI/GetAllTEUsersRole_Pagenation',data,heads);}
    result.Save=function(data){return $http.post($sessionStorage.PortfolioAPI+'TEUsersRoleAPI/SaveTEUserRole',data,heads);}
    result.Delete=function(data){return $http.post($sessionStorage.PortfolioAPI+'TEUsersRoleAPI/DeleteTEUserRole',data,heads);}
    result.UsersList=function(data){return $http.post($sessionStorage.PortfolioAPI+'TEUserProfileAPI/GetAllTEUserProfileData',data,heads);}
    result.RolesList=function(data){return $http.post($sessionStorage.PortfolioAPI+'TEWebpagesRolesAPI/GetWebpagesRolesByUserIDNotAssigned',data,heads);}

    return result;
})