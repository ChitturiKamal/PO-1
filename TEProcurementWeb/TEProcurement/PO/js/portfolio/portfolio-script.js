 $(document).ready(function () {
	 
$('.inventorylist .item-list').click(function(){
	seleproject=$('.inventorylist .item-list').index(this);
	$('.portfolio-list-page').hide();
	$('.portfolio-page').fadeIn();
});	 
$('.backtolist').click(function(){
	$('.portfolio-list-page').fadeIn();
	$('.portfolio-page').hide();
});
	 
     
			
			$('.terms-tab , .agreement-tab').hide();
$('.spec').click(function(){
	$('.specification-tab').fadeIn();
	$('.terms-tab , .agreement-tab').hide();
});
$('.tcs').click(function(){
	$('.terms-tab').fadeIn();
	$('.specification-tab , .agreement-tab').hide();
});
$('.ags').click(function(){
	$('.agreement-tab').fadeIn();
	$('.terms-tab , .specification-tab').hide();
});

 $("#faqs dd").hide();
    $("#faqs dt").click(function () {
        $(this).next("#faqs dd").slideToggle(500);
        $(this).toggleClass("expanded");
    });
$('.showdet').click(function(){
	$("#faqs dd").slideDown();
	$('#faqs dt').addClass('expanded');
});
$('.hidedet').click(function(){
	$("#faqs dd").slideUp();
	$('#faqs dt').removeClass('expanded');
});
$('.mastplan').click(function(){
	$('.masterplancontainer').fadeIn();
	$('.productplancontainer , .unitplancontainer').hide();
});
$('.prodplan').click(function(){
	$('.productplancontainer').fadeIn();
	$('.masterplancontainer , .unitplancontainer').hide();
});
$('.unplan').click(function(){
	$('.unitplancontainer').fadeIn();
	$('.productplancontainer , .masterplancontainer').hide();
});

        });