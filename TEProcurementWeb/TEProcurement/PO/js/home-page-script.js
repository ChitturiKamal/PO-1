$(document).ready(function () {  
 
$('.companydetails').hide();
$('.next-step').click(function (e) {
		$('.tab-content #step1').hide();
		$('.tab-content #step2').show();
}); 
$('.prev-step').click(function (e) {
	$('.tab-content #step2').hide();
      $('.tab-content #step1').show();
});
$(".selectbtn").on('change', function (e) {
e.preventDefault();
	if ( $(this).val() == "M/S" ) { 
	$('.comdetail').hide();
		$(this).closest('div.firstandlast').hide();
    	$('.companydetails').show();
    	return false;
    }
	else{
		// if(( $(this).val() == "Mr" )||( $(this).val() == "Mrs" )||( $(this).val() == "Ms" )||( $(this).val() == "Dr." )){
	//alert($(this).val()); 
		$('.firstandlast').show();
   		$('.companydetails').hide();
   		return false;
    }
});
$('.matchinfodetails').hide();
$('.matchinfo').click(function (e) {
	$('.matchinfodetails').show();
});
$('.personaleditinfo').hide();
$('.enqeditinfo').hide();
$('.editinfoper').on('click', function (e) {
    $('.personalinfodetails').hide();
    $('.personaleditinfo').slideDown();
});

$('.submitperbtn').on('click', function (e) {
    $('.personalinfodetails').show();
    $('.personaleditinfo').hide();
});

$('.personaleditinfo').hide();
$('.editinfoper').click(function(){
    $('.personalinfodetails').hide();
    $('.personaleditinfo').slideDown();
});

$('.editinfoenq').on('click', function (e) {
  $('.enqinfodetails').hide();
  $('.enqeditinfo').slideDown();
});


$('.submitbtn').on('click', function (e) {
    $('.enqinfodetails').show();
    $('.enqeditinfo').hide();
});

$('.project_more_details').hide();
$('.accordion-group .panel').hide();
$('.projects_list_selection').click(function () {
                $count = 0;
				var indexpro = $('.projects_list_selection').index(this);
                if ($(this).find("i").length < 1) 
                {
                    $(this).find('.projects_selection').append('<i class="fa fa-check-circle-o" style="float:right;"></i>');
					var projectsid=$(this).attr('id');
					
					$('.accordion-group .panel').eq(indexpro).show();
					$('.accordion-group .panel-collapse').eq(indexpro).addClass('in');
                }
                else if ($(this).find("i").length < 2) 
                {
                	  $('.project_more_details').show();
                	 $(this).find('img').addClass('sel');
                    $('.projects_list_selection').each(function () 
                    {
                        if ($(this).find('i').length == 2) {

                           $('.project_more_details').show();
                           $count++;
						   $('.accordion-group .panel').eq(indexpro).show();
					$('.accordion-group .panel-collapse').eq(indexpro).eq(indexpro).addClass('in');
                        }
                    });
                    if ($count == 0)
					  $(this).find('.projects_selection').append('<i class="fa fa-check-circle-o" style="float:right;"></i>');
			
					
                    else 
                    {
	
                        $(this).find("i").eq(0).remove();
                        $('.project_more_details').hide();
						$('.accordion-group .panel').eq(indexpro).hide();
                        $('.accordion-group .panel-collapse').eq(indexpro).removeClass('in');
                    }
                }
                else {
					console.log('nothingselct');
                	 $(this).find('img').removeClass('sel');
                    	$(this).find("i").remove();
							
                     	$('.project_more_details').hide();
						$('.accordion-group .panel').eq(indexpro).hide();
                      $('.accordion-group .panel-collapse').eq(indexpro).removeClass('in');
						
                }
});


 



$('.form-group .phoneno').on('keyup change', function() {
        if (this.value.length > 0) {
            $('.potentialmatch').show();
        } else {
            $('.potentialmatch').hide();
        }
    });    
 //$('.tab-list li a').removeClass('selt');
 $('.enqcurrentinfo').hide();
$('.assigninfo').hide();
$('.hideinfoenq').hide();
$('.hideinfoass').hide();
$('.showinfoenq').click(function(){
	$('.enqcurrentinfo').slideDown();
	$('.showinfoenq').hide();
	$('.hideinfoenq').show();
});
$('.hideinfoenq').click(function(){
	
	$('.enqcurrentinfo').slideUp();
	$('.showinfoenq').show();
	$('.hideinfoenq').hide();
});
$('.showinfoass').click(function(){
	
	$('.assigninfo').slideDown();
	$('.showinfoass').hide();
	$('.hideinfoass').show();
});
$('.hideinfoass').click(function(){
	
	$('.assigninfo').slideUp();
	$('.showinfoass').show();
	$('.hideinfoass').hide();
});

 $('.fc-right .fc-button-group .fc-button').addClass('fc-btn-sm');
$('#accordion').on('hidden.bs.collapse', toggleChevron);
$('#accordion').on('shown.bs.collapse', toggleChevron);
$('.addnote').summernote({
        height: 120,
        toolbar: [
          ['style', ['bold', 'italic', 'underline', 'clear']],
           ['fontsize', ['fontsize']],
    ['color', ['color']],
    ['para', ['ul', 'ol', 'paragraph']],
        ]
      });
       $('.followupnote').summernote({
        height: 150,
        toolbar: [
          ['style', ['bold', 'italic', 'underline']],
         
    ['para', ['ul', 'ol', 'paragraph']],
        ]
      });

      var save = function() {
  var makrup = $('.addnote').summernote('code');
  $('.addnote').summernote('destroy');
};


	   });
	   function nextTab(elem) {
    $(elem).next().find('a[data-toggle="tab"]').click();
}
function prevTab(elem) {
    $(elem).prev().find('a[data-toggle="tab"]').click();
}


function toggleChevron(e) {
    $(e.target)
        .prev('.panel-heading')
        .find("i.indicator")
        .toggleClass('glyphicon-chevron-down glyphicon-chevron-right');
}