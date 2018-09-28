(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('VendorMasterServices', VendorMasterServices).config(function ($httpProvider) {
            $httpProvider.defaults.headers.common = {};
            $httpProvider.defaults.headers.post = {};
            $httpProvider.defaults.headers.put = {};
            $httpProvider.defaults.headers.patch = {};
            //$httpProvider.defaults.headers.options = {};
          });

    /* @ngInject */
    /* function config() {}*/
    var createVendor = [];
    function VendorMasterServices($http, $window, commonUtilService) {
    var VenderMasterUrl = commonUtilService.getUrl('VendorMaster');
    var result = {
        createVendorMaster:createVendorMaster,
        CountryDropList:CountryDropList,
        getAllVendorMaster:getAllVendorMaster,
        getVendorMasterById:getVendorMasterById,
        saveVendorMaster:saveVendorMaster
    },
    createVendorCall = {
        createVendor: [VenderMasterUrl+'/api/PurchaseOrder/createTEPurchase_Vendor', 'post'],
        saveVendor: [VenderMasterUrl+'/api/PurchaseOrder/updateTEPurchase_Vendor', 'post']
    };
    return result;

     function getAllVendorMaster(){return $http.get(VenderMasterUrl+'/api/purchaseorder/GetAllVendors');}
     function getVendorMasterById(data){return $http.get(VenderMasterUrl+'/api/purchaseorder/GetVendor/'+data);}

    // result.createVendorMaster = function(data){return $http.post(VenderMasterUrl+'/api/poapi/createTEPurchase_Vendor',data);}

    function createVendorMaster(api, obj) {
        var url = createVendorCall[api][0];
        return $http({
            method: createVendorCall[api][1],
            url: url,
            //dataType: 'json',
            data: obj,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function(data) {
            return data;
        }, function(data) {
            console.log(data);
            return data;
        });
    } 

    function saveVendorMaster(api, obj) {
        var url = createVendorCall[api][0];
        return $http({
            method: createVendorCall[api][1],
            url: url,
            //dataType: 'json',
            data: obj,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function(data) {
            return data;
        }, function(data) {
            console.log(data);
            return data;
        });
    } 
    
    
   
        // return $http.get(VenderMasterUrl+'/api/poapi/GetAllVendors')
        // .then(function(data) { console.log("Success"); console.log(data);
        //     return data;
        // }, function(data) { console.log("error"); console.log(data);
        //     return data;
        // });
   
   
   
        function CountryDropList () {
            var CountryDrop = { 
                countries: [{ name: "Afghanistan", cca2: "af", "callingCode": "93" }, { name: "Albania", cca2: "al", "callingCode": "355" }, { name: "Algeria", cca2: "dz", "callingCode": "213" }, { name: "American Samoa", cca2: "as", "callingCode": "1684" }, { name: "Andorra", cca2: "ad", "callingCode": "376" }, { name: "Angola", cca2: "ao", "callingCode": "244" }, { name: "Anguilla", cca2: "ai", "callingCode": "1264" }, { name: "Antigua and Barbuda", cca2: "ag", "callingCode": "1268" }, { name: "Argentina", cca2: "ar", "callingCode": "54" }, { name: "Armenia", cca2: "am", "callingCode": "374" }, { name: "Aruba", cca2: "aw", "callingCode": "297" }, { name: "Australia", cca2: "au", "callingCode": "61" }, { name: "Austria", cca2: "at", "callingCode": "43" }, { name: "Azerbaijan", cca2: "az", "callingCode": "994" }, { name: "Bahamas", cca2: "bs", "callingCode": "1242" }, { name: "Bahrain", cca2: "bh", "callingCode": "973" }, { name: "Bangladesh", cca2: "bd", "callingCode": "880" }, { name: "Barbados", cca2: "bb", "callingCode": "1246" }, { name: "Belarus", cca2: "by", "callingCode": "375" }, { name: "Belgium", cca2: "be", "callingCode": "32" }, { name: "Belize", cca2: "bz", "callingCode": "501" }, { name: "Benin", cca2: "bj", "callingCode": "229" }, { name: "Bermuda", cca2: "bm", "callingCode": "1441" }, { name: "Bhutan", cca2: "bt", "callingCode": "975" }, { name: "Bolivia", cca2: "bo", "callingCode": "591" }, { name: "Bosnia and Herzegovina", cca2: "ba", "callingCode": "387" }, { name: "Botswana", cca2: "bw", "callingCode": "267" }, { name: "Brazil", cca2: "br", "callingCode": "55" }, { name: "Brunei Darussalam", cca2: "bn", "callingCode": "673" }, { name: "Bulgaria", cca2: "bg", "callingCode": "359" }, { name: "Burkina Faso", cca2: "bf", "callingCode": "226" }, { name: "Burundi", cca2: "bi", "callingCode": "257" }, { name: "Cambodia", cca2: "kh", "callingCode": "855" }, { name: "Cameroon", cca2: "cm", "callingCode": "237" }, { name: "Canada", cca2: "ca", "callingCode": "1" }, { name: "Cape Verde", cca2: "cv", "callingCode": "238" }, { name: "Cayman Islands", cca2: "ky", "callingCode": "1345" }, { name: "Central African Republic", cca2: "cf", "callingCode": "236" }, { name: "Chad", cca2: "td", "callingCode": "235" }, { name: "Chile", cca2: "cl", "callingCode": "56" }, { name: "China", cca2: "cn", "callingCode": "86" }, { name: "Colombia", cca2: "co", "callingCode": "57" }, { name: "Comoros", cca2: "km", "callingCode": "269" }, { name: "Congo (DRC)", cca2: "cd", "callingCode": "243" }, { name: "Congo (Republic)", cca2: "cg", "callingCode": "242" }, { name: "Cook Islands", cca2: "ck", "callingCode": "682" }, { name: "Costa Rica", cca2: "cr", "callingCode": "506" }, { name: "C�te d'Ivoire", cca2: "ci", "callingCode": "225" }, { name: "Croatia", cca2: "hr", "callingCode": "385" }, { name: "Cuba", cca2: "cu", "callingCode": "53" }, { name: "Cyprus", cca2: "cy", "callingCode": "357" }, { name: "Czech Republic", cca2: "cz", "callingCode": "420" }, { name: "Denmark", cca2: "dk", "callingCode": "45" }, { name: "Djibouti", cca2: "dj", "callingCode": "253" }, { name: "Dominica", cca2: "dm", "callingCode": "1767" }, { name: "Dominican Republic", cca2: "do", "callingCode": "1809" }, { name: "Ecuador", cca2: "ec", "callingCode": "593" }, { name: "Egypt", cca2: "eg", "callingCode": "20" }, { name: "El Salvador", cca2: "sv", "callingCode": "503" }, { name: "Equatorial Guinea", cca2: "gq", "callingCode": "240" }, { name: "Eritrea", cca2: "er", "callingCode": "291" }, { name: "Estonia", cca2: "ee", "callingCode": "372" }, { name: "Ethiopia", cca2: "et", "callingCode": "251" }, { name: "Faroe Islands", cca2: "fo", "callingCode": "298" }, { name: "Fiji", cca2: "fj", "callingCode": "679" }, { name: "Finland", cca2: "fi", "callingCode": "358" }, { name: "France", cca2: "fr", "callingCode": "33" }, { name: "French Polynesia", cca2: "pf", "callingCode": "689" }, { name: "Gabon", cca2: "ga", "callingCode": "241" }, { name: "Gambia", cca2: "gm", "callingCode": "220" }, { name: "Georgia", cca2: "ge", "callingCode": "995" }, { name: "Germany", cca2: "de", "callingCode": "49" }, { name: "Ghana", cca2: "gh", "callingCode": "233" }, { name: "Gibraltar", cca2: "gi", "callingCode": "350" }, { name: "Greece", cca2: "gr", "callingCode": "30" }, { name: "Greenland", cca2: "gl", "callingCode": "299" }, { name: "Grenada", cca2: "gd", "callingCode": "1473" }, { name: "Guadeloupe", cca2: "gp", "callingCode": "590" }, { name: "Guam", cca2: "gu", "callingCode": "1671" }, { name: "Guatemala", cca2: "gt", "callingCode": "502" }, { name: "Guernsey", cca2: "gg", "callingCode": "44" }, { name: "Guinea", cca2: "gn", "callingCode": "224" }, { name: "Guinea-Bissau", cca2: "gw", "callingCode": "245" }, { name: "Guyana", cca2: "gy", "callingCode": "592" }, { name: "Haiti", cca2: "ht", "callingCode": "509" }, { name: "Honduras", cca2: "hn", "callingCode": "504" }, { name: "Hong Kong", cca2: "hk", "callingCode": "852" }, { name: "Hungary", cca2: "hu", "callingCode": "36" }, { name: "Iceland", cca2: "is", "callingCode": "354" }, { name: "India", cca2: "in", "callingCode": "91" }, { name: "Indonesia", cca2: "id", "callingCode": "62" }, { name: "Iran", cca2: "ir", "callingCode": "98" }, { name: "Iraq", cca2: "iq", "callingCode": "964" }, { name: "Ireland", cca2: "ie", "callingCode": "353" }, { name: "Isle of Man", cca2: "im", "callingCode": "44" }, { name: "Israel", cca2: "il", "callingCode": "972" }, { name: "Italy", cca2: "it", "callingCode": "39" }, { name: "Jamaica", cca2: "jm", "callingCode": "1876" }, { name: "Japan", cca2: "jp", "callingCode": "81" }, { name: "Jersey", cca2: "je", "callingCode": "44" }, { name: "Jordan", cca2: "jo", "callingCode": "962" }, { name: "Kazakhstan", cca2: "kz", "callingCode": "7" }, { name: "Kenya", cca2: "ke", "callingCode": "254" }, { name: "Kiribati", cca2: "ki", "callingCode": "686" }, { name: "Kuwait", cca2: "kw", "callingCode": "965" }, { name: "Kyrgyzstan", cca2: "kg", "callingCode": "996" }, { name: "Laos", cca2: "la", "callingCode": "856" }, { name: "Latvia", cca2: "lv", "callingCode": "371" }, { name: "Lebanon", cca2: "lb", "callingCode": "961" }, { name: "Lesotho", cca2: "ls", "callingCode": "266" }, { name: "Liberia", cca2: "lr", "callingCode": "231" }, { name: "Libya", cca2: "ly", "callingCode": "218" }, { name: "Liechtenstein", cca2: "li", "callingCode": "423" }, { name: "Lithuania", cca2: "lt", "callingCode": "370" }, { name: "Luxembourg", cca2: "lu", "callingCode": "352" }, { name: "Macao", cca2: "mo", "callingCode": "853" }, { name: "Macedonia", cca2: "mk", "callingCode": "389" }, { name: "Madagascar", cca2: "mg", "callingCode": "261" }, { name: "Malawi", cca2: "mw", "callingCode": "265" }, { name: "Malaysia", cca2: "my", "callingCode": "60" }, { name: "Maldives", cca2: "mv", "callingCode": "960" }, { name: "Mali", cca2: "ml", "callingCode": "223" }, { name: "Malta", cca2: "mt", "callingCode": "356" }, { name: "Marshall Islands", cca2: "mh", "callingCode": "692" }, { name: "Martinique", cca2: "mq", "callingCode": "596" }, { name: "Mauritania", cca2: "mr", "callingCode": "222" }, { name: "Mauritius", cca2: "mu", "callingCode": "230" }, { name: "Mexico", cca2: "mx", "callingCode": "52" }, { name: "Micronesia", cca2: "fm", "callingCode": "691" }, { name: "Moldova", cca2: "md", "callingCode": "373" }, { name: "Monaco", cca2: "mc", "callingCode": "377" }, { name: "Mongolia", cca2: "mn", "callingCode": "976" }, { name: "Montenegro", cca2: "me", "callingCode": "382" }, { name: "Montserrat", cca2: "ms", "callingCode": "1664" }, { name: "Morocco", cca2: "ma", "callingCode": "212" }, { name: "Mozambique", cca2: "mz", "callingCode": "258" }, { name: "Myanmar (Burma)", cca2: "mm", "callingCode": "95" }, { name: "Namibia", cca2: "na", "callingCode": "264" }, { name: "Nauru", cca2: "nr", "callingCode": "674" }, { name: "Nepal", cca2: "np", "callingCode": "977" }, { name: "Netherlands", cca2: "nl", "callingCode": "31" }, { name: "New Caledonia", cca2: "nc", "callingCode": "687" }, { name: "New Zealand", cca2: "nz", "callingCode": "64" }, { name: "Nicaragua", cca2: "ni", "callingCode": "505" }, { name: "Niger", cca2: "ne", "callingCode": "227" }, { name: "Nigeria", cca2: "ng", "callingCode": "234" }, { name: "North Korea", cca2: "kp", "callingCode": "850" }, { name: "Norway", cca2: "no", "callingCode": "47" }, { name: "Oman", cca2: "om", "callingCode": "968" }, { name: "Pakistan", cca2: "pk", "callingCode": "92" }, { name: "Palau", cca2: "pw", "callingCode": "680" }, { name: "Palestinian Territory", cca2: "ps", "callingCode": "970" }, { name: "Panama", cca2: "pa", "callingCode": "507" }, { name: "Papua New Guinea", cca2: "pg", "callingCode": "675" }, { name: "Paraguay", cca2: "py", "callingCode": "595" }, { name: "Peru", cca2: "pe", "callingCode": "51" }, { name: "Philippines", cca2: "ph", "callingCode": "63" }, { name: "Poland", cca2: "pl", "callingCode": "48" }, { name: "Portugal", cca2: "pt", "callingCode": "351" }, { name: "Puerto Rico", cca2: "pr", "callingCode": "1787" }, { name: "Qatar", cca2: "qa", "callingCode": "974" }, { name: "R�union", cca2: "re", "callingCode": "262" }, { name: "Romania", cca2: "ro", "callingCode": "40" }, { name: "Russian Federation", cca2: "ru", "callingCode": "7" }, { name: "Rwanda", cca2: "rw", "callingCode": "250" }, { name: "Saint Kitts and Nevis", cca2: "kn", "callingCode": "1869" }, { name: "Saint Lucia", cca2: "lc", "callingCode": "1758" }, { name: "Saint Vincent and the Grenadines", cca2: "vc", "callingCode": "1784" }, { name: "Samoa", cca2: "ws", "callingCode": "685" }, { name: "San Marino", cca2: "sm", "callingCode": "378" }, { name: "S�o Tom� and Pr�ncipe", cca2: "st", "callingCode": "239" }, { name: "Saudi Arabia", cca2: "sa", "callingCode": "966" }, { name: "Senegal", cca2: "sn", "callingCode": "221" }, { name: "Serbia", cca2: "rs", "callingCode": "381" }, { name: "Seychelles", cca2: "sc", "callingCode": "248" }, { name: "Sierra Leone", cca2: "sl", "callingCode": "232" }, { name: "Singapore", cca2: "sg", "callingCode": "65" }, { name: "Slovakia", cca2: "sk", "callingCode": "421" }, { name: "Slovenia", cca2: "si", "callingCode": "386" }, { name: "Solomon Islands", cca2: "sb", "callingCode": "677" }, { name: "Somalia", cca2: "so", "callingCode": "252" }, { name: "South Africa", cca2: "za", "callingCode": "27" }, { name: "South Korea", cca2: "kr", "callingCode": "82" }, { name: "Spain", cca2: "es", "callingCode": "34" }, { name: "Sri Lanka", cca2: "lk", "callingCode": "94" }, { name: "Sudan", cca2: "sd", "callingCode": "249" }, { name: "Suriname", cca2: "sr", "callingCode": "597" }, { name: "Swaziland", cca2: "sz", "callingCode": "268" }, { name: "Sweden", cca2: "se", "callingCode": "46" }, { name: "Switzerland", cca2: "ch", "callingCode": "41" }, { name: "Syrian Arab Republic", cca2: "sy", "callingCode": "963" }, { name: "Taiwan, Province of China", cca2: "tw", "callingCode": "886" }, { name: "Tajikistan", cca2: "tj", "callingCode": "992" }, { name: "Tanzania", cca2: "tz", "callingCode": "255" }, { name: "Thailand", cca2: "th", "callingCode": "66" }, { name: "Timor-Leste", cca2: "tl", "callingCode": "670" }, { name: "Togo", cca2: "tg", "callingCode": "228" }, { name: "Tonga", cca2: "to", "callingCode": "676" }, { name: "Trinidad and Tobago", cca2: "tt", "callingCode": "1868" }, { name: "Tunisia", cca2: "tn", "callingCode": "216" }, { name: "Turkey", cca2: "tr", "callingCode": "90" }, { name: "Turkmenistan", cca2: "tm", "callingCode": "993" }, { name: "Turks and Caicos Islands", cca2: "tc", "callingCode": "1649" }, { name: "Tuvalu", cca2: "tv", "callingCode": "688" }, { name: "Uganda", cca2: "ug", "callingCode": "256" }, { name: "Ukraine", cca2: "ua", "callingCode": "380" }, { name: "United Arab Emirates", cca2: "ae", "callingCode": "971" }, { name: "United Kingdom", cca2: "gb", "callingCode": "44" }, { name: "United States", cca2: "us", "callingCode": "1" }, { name: "Uruguay", cca2: "uy", "callingCode": "598" }, { name: "Uzbekistan", cca2: "uz", "callingCode": "998" }, { name: "Vanuatu", cca2: "vu", "callingCode": "678" }, { name: "Vatican City", cca2: "va", "callingCode": "379" }, { name: "Venezuela", cca2: "ve", "callingCode": "58" }, { name: "Viet Nam", cca2: "vn", "callingCode": "84" }, { name: "Virgin Islands (British)", cca2: "vg", "callingCode": "1284" }, { name: "Virgin Islands (U.S.)", cca2: "vi", "callingCode": "1340" }, { name: "Western Sahara", cca2: "eh", "callingCode": "212" }, { name: "Yemen", cca2: "ye", "callingCode": "967" }, { name: "Zambia", cca2: "zm", "callingCode": "260" }, { name: "Zimbabwe", cca2: "zw", "callingCode": "263" }] };
            return CountryDrop;
        }
    }
})();