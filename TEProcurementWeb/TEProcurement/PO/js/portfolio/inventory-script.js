		
		$(document).ready(function() {
		
$('.towerlist .towerentry').click(function(){
	
	//$('.towerlist .towerentry').removeClass('active');
	var towerindex =$('.towerlist .towerentry').index(this);
	$('.towerlist .towerentry').eq(towerindex).toggleClass('active');
});
$('.unitlist .unitentry').click(function(){
	//$('.unitlist .unitentry').removeClass('active');
	var unitindex =$('.unitlist .unitentry').index(this);
	$('.unitlist .unitentry').eq(unitindex).toggleClass('active');
});
$('.floor-box .floor-sec span.selection').hide();
 $('.multiselect').click(function(){
		$('.multiselect').addClass('multiapprove'); 
		
	 });
 $('.floor-box .floor-sec.favail').click(function(){
	 if($(".multiselect").is(':hidden')){
		$('.floor-box .floor-sec').removeClass('active');
	var floorindex =$('.floor-box .floor-sec.favail').index(this);
//	console.log(floorindex);
	$('.floor-box .floor-sec.favail').eq(floorindex).children('span.selection').show();
	$('.multiselect').show();
	if ( $(".floor-box .floor-sec.favail span.selection:visible").length === 1)
	{
		
		$('.inven_defult_grid').hide();
		$('.invent-unit-info').show();
	} 
	 }
	var mclass= $('.multiselect').hasClass('multiapprove');
	
	
	  if(mclass==true){
	var floorindex =$('.floor-box .floor-sec.favail').index(this);

	$('.floor-box .floor-sec.favail').eq(floorindex).children('span.selection').show();
	
	if ( $(".floor-box .floor-sec.favail span.selection:visible").length === 1)
	{
		$('.multiselect').show();
		$('.inven_defult_grid').hide();
		$('.invent-unit-info').show();
		$('.multiselect').removeClass('multiapprove'); 
	} 	 
	 }
	
		
}); 
$('.clearallsel').click(function(){
	$('.floor-box .floor-sec.favail').children('span.selection').hide();
	$('.inven_defult_grid').show();
		$('.invent-unit-info').hide();
});
$('.multisel').click(function(){
	$('.inven_multi_selc').show();
	$('.inven_defult_grid').hide();
		$('.invent-unit-info').hide();
});

$('.pro-tower').click(function(){
	$('.progresstower').show();
	$('.progressunit').hide();
});
$('.pro-unit').click(function(){
	$('.progresstower').hide();
	$('.progressunit').show();
});
$('.projdis').click(function(){
	$('.projectlist').fadeToggle();
});
$(".panel-heading").addClass("collapsed");
$('.scheduletable tr').click(function(){
	var scheduleindex = $('.scheduletable tr').index(this);
	 window.open('inventory-more-milestone.html', '_blank');
});

   $('#categories-list .item').addClass('grid-group-item');
  $('#scrollbox3').enscroll({
    showOnHover: true,
    verticalTrackClass: 'track3',
    verticalHandleClass: 'handle3'
});

 $('#slider1')
  .anythingSlider({
   toggleControls : true,
   buildNavigation:false,
     buildStartStop :false,
   enableArrows        : true,
toggleArrows        : true,
buildArrows         : true ,
hashTags            : false ,
stopAtEnd:true,
infiniteSlides      : false
  });
  $('#slider2')
  .anythingSlider({
   toggleControls : true,
   buildNavigation:false,
     buildStartStop :false,
   enableArrows        : true,
toggleArrows        : true,
buildArrows         : true ,
hashTags            : false,
stopAtEnd:true,
infiniteSlides      : false
  });
$('#morefloor')
  .anythingSlider({
   toggleControls : true,
   buildNavigation:false,
     buildStartStop :false,
   enableArrows        : true,
   
toggleArrows        : true,
buildArrows         : true ,
hashTags            : false,
stopAtEnd:true,
infiniteSlides      : false
  });
  $('table.detail').each(function() {
        var $table = $(this);
        $table.find('.parent').click(function() {
            $(this).nextUntil('.parent').toggle("slow"); // must use jQuery 1.4 for nextUntil() method
			$( this ).find( "td.firstrow" ).toggleClass( "closebutton" );
			$( this ).toggleClass( "openedrow" );
        });

        var $childRows = $table.find('tbody tr').not('.parent').hide();
        $table.find('button.hidedet').click(function() {
            $childRows.hide();
			$('tr.parent').removeClass('openedrow');
			$( '.parent' ).find( "td.firstrow" ).removeClass( "closebutton" );
        });
        $table.find('button.showdet').click(function() {
            $childRows.show();
				$('tr.parent').addClass('openedrow');
			$( '.parent' ).find( "td.firstrow" ).addClass( "closebutton" );
        });
    });
		$(".specchangemenu li a").click(function(){
	//	$('.pdftitle').removeClass('bgcolor_808080');
  $(this).parents(".dropdown").find('.btn').html($(this).text() + ' <span class="caret"></span>');
  $(this).parents(".dropdown").find('.btn').val($(this).data('value'))
  console.log($(this).data('value'));
  var colrval= $(this).data('value');
  if(colrval=='Green')
  {
	$('.specific_color, .specific_col_one').addClass('bgcolor_76913a');
	$('.specific_color, .specific_col_one').removeClass('bgcolor_f79647 bgcolor_224162 bgcolor_60477a');
	$('.specific_color').html(colrval);
	$('.specific_col_one span').html(colrval);
	$('.specchange').addClass('color_76913a');
	$('.specchange').removeClass('color_f79646 color_224162 color_60477a');
  }
  if(colrval=='Orange')
  {	
$('.specific_color, .specific_col_one').addClass('bgcolor_f79647');
	$('.specific_color, .specific_col_one').removeClass('bgcolor_76913a bgcolor_224162 bgcolor_60477a');
	$('.specific_color').html(colrval);
	$('.specific_col_one span').html(colrval);
	$('.specchange').addClass('color_f79646');
	$('.specchange').removeClass('color_76913a color_224162 color_60477a');
	
  }
  if(colrval =='Blue')
  {
	  $('.specific_color, .specific_col_one').addClass('bgcolor_224162');
	$('.specific_color, .specific_col_one').removeClass('bgcolor_f79647 bgcolor_76913a  bgcolor_60477a');
	$('.specific_color').html(colrval);
	$('.specific_col_one span').html(colrval);
	$('.specchange').addClass('color_224162');
	$('.specchange').removeClass('color_76913a color_f79646 color_60477a');

  }
   if(colrval =='Purple')
  {
	   $('.specific_color, .specific_col_one').addClass('bgcolor_60477a');
	$('.specific_color, .specific_col_one').removeClass('bgcolor_224162 bgcolor_f79647 bgcolor_76913a  ');
	$('.specific_color').html(colrval);
	$('.specific_col_one span').html(colrval);
	$('.specchange').addClass('color_60477a');
	$('.specchange').removeClass('color_76913a color_f79646 color_224162');
  }
});
});
function toggleChevron(e) {
    $(e.target)
        .prev('.panel-heading')
        .find("i.indicator")
        .toggleClass('glyphicon-chevron-down glyphicon-chevron-right');
}

 /*** print the pdf format */
function printDiv(divID) {
	//Get the HTML of div
	var divElements = document.getElementById(divID).innerHTML;
    //Get the HTML of whole page
    var oldPage = document.body.innerHTML;
    //Reset the page's HTML with div's HTML only
   document.body.innerHTML = 
   "<html moznomarginboxes mozdisallowselectionprint><head><link href='css/lead/print.css' media='print' rel='stylesheet' /></head><body>" + 
    divElements + "</body>";
    //Print Page
    window.print();
    //Restore orignal HTML
    document.body.innerHTML = oldPage;
}
  /* print the pdf format */