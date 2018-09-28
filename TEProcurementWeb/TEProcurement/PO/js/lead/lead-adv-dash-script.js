$(document).ready(function() { // document ready
$('[data-toggle="tooltip"]').tooltip();   
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
		
		 $('.specialcondition').summernote({
            height: 50,
			placeholder: 'Special Condition here...',
            toolbar: [
              ['style', ['bold', 'italic', 'underline', 'clear']],
               ['fontsize', ['fontsize']],
       
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

        var selectdate;
        $('#followupdatepicker').datepicker();

        /*$('#datepicker').on('change',function(){
            var vale = $('.date-pick').data("datepicker");
            console.log(vale.date);
            var date = vale.date;
            var formatted =date.getDate()+ "/" + (date.getMonth() + 1) + "/" +  date.getFullYear()  ;
            alert(formatted);
        
        });*/

        $('#demo-htmlselect').ddslick({
            showSelectedHTML: true,
            onSelected: function(selectedData){
                var str = $('.dd-selected-value').val();
                $('.followups-selct').val(str);
            }   
        });
       
        $('#time-select').ddslick({
            onSelected: function(selectedData){
                //callback function: do something with selectedData;
            }   
        });
        $('#dur-select').ddslick({
            onSelected: function(selectedData){
                //callback function: do something with selectedData;
            }   
        });
        $('#datepicker').datepicker();
	 
$(".horzontalscroll").mCustomScrollbar({
					axis:"x",
				
					advanced:{autoExpandHorizontalScroll:true}
				});
$('.prjlsttable tr').click(function(){
	$('.eventcalander').hide();
	$('.termspage').fadeIn(1000);
	
    var customerId = $(this).find("td").eq(3).html();   
if(customerId=='Draft')
{
	$('.watermark').show();
	
}
else{
	$('.watermark').hide();
}	

});

$('#row0grid1').click(function(){
		$('.eventcalander').hide();
	$('.termspage').fadeIn(1000);
	
});


//$("[data-toggle=popover]").popover({
  //  html: true, 
	//content: function() {
      //    return $('#popover-content').html();
       // }
//});
$(".pop").each(function() {
    var $pElem= $(this);
	  html: true, 
    $pElem.popover(
        {
       
          content: getPopContent($pElem.attr("id"))
        }
    );
});

	$('body').on('click', function (e) {
		$('[data-toggle="popover"]').each(function () {
			if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
				$(this).popover('hide');
			}
		});
	});
	
	
	
	$(".ebuildchangemenu li a").click(function(){
		
  $(this).parents(".dropdown").find('div').html($(this).text() + ' <span class="caret"></span>');
  $(this).parents(".dropdown").find('div').val($(this).data('value'))
  console.log($(this).data('value'));
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
	$('.nextday').click(function() {
    $('#monthlycalendar').fullCalendar('next');
});
$('.box-info .stagestatus').on('click', function() {
	var stgstatus = $(this).text();
	console.log(stgstatus);
	if(stgstatus=='Unit Selection + Offer'|| stgstatus=='Termsheet Signed + EMD Realised'|| stgstatus=='Transition to CEM')
	{
	$(this).parent().parent().next('.projectslist').fadeToggle();
	}
	//$(this).parent().parent().toggleClass('boxshadow');
});
$('.box-info .box-personname').click(function(){
	$('.eventcalander').fadeIn(1000);
	$('.termspage').hide();
});

//$('.box-info-container .box-info .tempmeter p').click(function () {
//    var index = $(this.parentNode).index();
//    alert(index);

//});

$('.box-info .tags_entry_add').on('click',function(event){
		event.stopPropagation();
		$edi=$(this).parent('.box-tags');
		$qwe=$edi.find('.tags_new');
			$qwe.show();
	//$('.tags_new').show();
	
/* 	
 */});
 $('.clearnewtag').click(function(){
	 $('.tags_new_value').val('');
 });
$('.box-info .addingnewtag').on('click',function(event){
	
	event.stopPropagation();
	
		$edi=$(this).parents('.box-tags');
		$qwe=$edi.find('.addlasttag');
		
		
		
		$qwe_val=$edi.find('.tags_new_value');
	var tagval = $qwe_val.val();
	 $qwe.after('<div class="tags_entry">'+
									'<div class="remove-tag"></div>'+
									'<div class="tags_entry_st"></div>'+
									'<div class="tags_entry_body"><a>'+tagval +'</a></div>'+
									'<div class="tags_entry_end"></div>'+
									'</div>'); 
									$('.tags_new').hide();
									 $('.tags_entry_add').show();
									
 });
$('.box-info .remove-tag').on('click',function(event){
	event.stopPropagation();

	 $(this).parent().remove();
    });

$('.tempmeter p').click(function() {
	var  tempmeterindex= $(this).index() ;
	
	$parentid = $(this).parent('.tempmeter');
	console.log($parentid);
	if(parseInt(tempmeterindex)==0)
	{
	$parentid.find('p.level1').addClass('bgcolor_f6cdcb');
	$parentid.find('p.level2').removeClass('bgcolor_f18b89');
	$parentid.find('p.level3').removeClass('bgcolor_de5855');
	$parentid.find('p.level4').removeClass('bgcolor_cd3e38');
	$parentid.find('p.level5').removeClass('bgcolor_b20700');
	}
	
	if(parseInt(tempmeterindex)==1)
	{
	
	$parentid.find('p.level1').addClass('bgcolor_f6cdcb');
	$parentid.find('p.level2').addClass('bgcolor_f18b89');
	$parentid.find('p.level3').removeClass('bgcolor_de5855');
	$parentid.find('p.level4').removeClass('bgcolor_cd3e38');
	$parentid.find('p.level5').removeClass('bgcolor_b20700');
	}
	if(parseInt(tempmeterindex)==2)
	{
	
	$parentid.find('p.level1').addClass('bgcolor_f6cdcb');
	$parentid.find('p.level2').addClass('bgcolor_f18b89');
	$parentid.find('p.level3').addClass('bgcolor_de5855');
	$parentid.find('p.level4').removeClass('bgcolor_cd3e38');
	$parentid.find('p.level5').removeClass('bgcolor_b20700');
	}
	if(parseInt(tempmeterindex)==3)
	{
	
	$parentid.find('p.level1').addClass('bgcolor_f6cdcb');
	$parentid.find('p.level2').addClass('bgcolor_f18b89');
	$parentid.find('p.level3').addClass('bgcolor_de5855');
	$parentid.find('p.level4').addClass('bgcolor_cd3e38');
	$parentid.find('p.level5').removeClass('bgcolor_b20700');
	}
	if(parseInt(tempmeterindex)==4)
	{
	
	$parentid.find('p.level1').addClass('bgcolor_f6cdcb');
	$parentid.find('p.level2').addClass('bgcolor_f18b89');
	$parentid.find('p.level3').addClass('bgcolor_de5855');
	$parentid.find('p.level4').addClass('bgcolor_cd3e38');
	$parentid.find('p.level5').addClass('bgcolor_b20700');
	}

	
	
	});
/**** TEXT VALUE CALCULATION**/
var xvalecontents = $('.pdfbordertable td span.xvalue').html();
$('.pdfbordertable td span.xvalue').blur(function() {
    if (xvalecontents!=$(this).html()){
        xvalecontents = $(this).html();
	console.log(xvalecontents);
	 //var xval = $(this).val();
	xvalecontents=xvalecontents.replace(/\,/g,'');
	var pval = $('.pvalue').html();
	pval=pval.replace(/\,/g,'');
	var sval = ((parseInt(xvalecontents))*(parseInt(pval)));
	sval = commaSeparateNumber(sval);
	$(this).parent().append('<span contenteditable="true" class="inline-edit xvalue">'+xvalecontents+'</span>');
	$('.svalue').html(sval);
     $(this).remove();
    }
});
/* $('.pdfbordertable td span.xvalue').click(function(){
console.log(xvalecontents);
    var td_value = $(this).text(); 
    var parent = $(this).parent();
	parent.html('<input type="text" style="width:50px;" value="' + $(this).html() + '">');
    $('input').on('focusout',function(){
    var xval = $(this).val();
	xval=xval.replace(/\,/g,'');
	var pval = $('.pvalue').html();
	pval=pval.replace(/\,/g,'');
	var sval = ((parseInt(xval))*(parseInt(pval)));
	sval = commaSeparateNumber(sval);
	$(this).parent().append('<span contenteditable="true" class="inline-edit xvalue">'+$(this).val()+'</span>');
	$('.svalue').html(sval);
     $(this).remove();
    });
}); */
$('.savesplnote').click(function(){
	var spltextareaval =$('.specialcondition').summernote('code');
	var formattextareaval= $.parseHTML(spltextareaval);
	var splnotevalue = $('.displaycontent').html(formattextareaval);
	$('.special_entries').show();
	$('.specialdata').append($('.displaycontent').html());

});
$('.clearsplnote').click(function(){
	$('.specialcondition').summernote('');
});
/**** TEXT VALUE CALCULATION**/

var contents = $('.unitvaledit').html();
$('.unitvaledit').blur(function() {
    if (contents!=$(this).html()){
        contents = $(this).html();
	$('.chguntval').html(contents);
    }
});
$('.addcarpark').on('click',function(event){
	 event.stopPropagation();
	 $('.pdf-format tr.carparkadding').after('<tr><td class="txtalgncenter" width="20">&nbsp;</td>'+ 
	 '<td style="text-align:left; padding:3px 0 3px 15px;"><select class="selectpicker"><option>Regular Independent Car Park/s</option>'+
	'<option>Large Independent Car Park/s</option><option>Small Independent Car Park/s</option><option>Large Twin Car Park/s</option>'+
	'<option>Regular Twin Car Park/s</option><option>Small Twin Car Park/s</option><option>Large Special Car Park/s</option>'+	'<option>Regular Special Car Park/s</option><option>Small Special Car Park/s</option><option>Large Open to Sky Car Park/s</option></select></td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;"><span class="inline-edit" contenteditable="true">1</span></td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;">1</td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;"><span class="inline-edit" >&nbsp;</span><span class="removecarpark"><i class="fa fa-times" aria-hidden="true"></i></span></td></tr>');
$('.removecarpark').on('click',function(event){
	event.stopPropagation();
	 $(this).parent().parent().remove();
    });	
});

$('.addmilestone').on('click',function(event){
	event.stopPropagation();
	 $('.pdf-format tr.milestoneadding').after('<tr><td class="txtalgncenter" width="20">&nbsp;</td>'+ 
	 '<td style="text-align:left; padding:3px 0 3px 15px;"></td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;"><span class="inline-edit" contenteditable="true"></span></td>'+
	   	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;"><span class="inline-edit" contenteditable="true">&nbsp;</span><span class="removemilestone"><i class="fa fa-times" aria-hidden="true"></i></span></td></tr>');
		    var $div=$('.slnoedit'), isEditable=$div.is('.editable');
    $('.slnoedit').prop('contenteditable',!isEditable).toggleClass('editable');
	   $('.removemilestone').on('click',function(event){
	event.stopPropagation();
	 $(this).parent().parent().remove();
    });	
});	



 

$('#fullname').click(function(){
    var name = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'name': 'fname',
            'id': 'txt_fullname',
           
            'value': name
        })
        .appendTo('#fullname');
    $('#txt_fullname').focus();
});

$(document).on('blur','#txt_fullname', function(){
    var name = $(this).val();
    //alert('Make an AJAX call and pass this parameter >> name=' + name);
    $('#fullname').text(name);
});
$('.rating > label.full').click(function(){
	  var $this = $(this);
    if ($this.hasClass('clicked')){
        $(this).css('color','#b12525');
        return;
    }else{
        $this.addClass('clicked');
        $(this).css('color','#ffd203');
        setTimeout(function() { 
        $this.removeClass('clicked'); },500);
    }//end of else
	
});
$('.masterTooltip').hover(function(){
        // Hover over code
        var title = $(this).attr('title');
        $(this).data('tipText', title).removeAttr('title');
        $('<p class="tooltip"></p>')
        .text(title)
        .appendTo('body')
        .fadeIn('slow');
}, function() {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.tooltip').remove();
}).mousemove(function(e) {
        var mousex = e.pageX + 20; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.tooltip')
        .css({ top: mousey, left: mousex })
});
/****** Inline Edit content **/


	$("input[type*=radio]").each(function(i,value){ 
    $(this).attr("title");
    });
    $('label').tooltip({
        placement : 'top'
    });
 /***** Followups scripts**/
 $('#LeadFollowupsModalBox').on('shown.bs.modal', function () {
       $("#followupcalendar").fullCalendar('render');
});
$('#followupcalendar').fullCalendar({
		header: {
                left: '',
                center: '',
                right:'prev,next'
            },
			 events: [
        {
            title  : 'event1',
            start  : '2016-05-01'
        },
        {
            title  : 'event2',
            start  : '2016-06-05',
            end    : '2016-06-07'
        },
        {
            title  : 'event3',
            start  : '2016-06-09T12:30:00',
            allDay : false // will make the time show
        }
    ],
            defaultView: 'agendaDay',
            editable: true,
            nowIndicator:true
        });
		
		$('.leadlview').click(function(){
			$('.leadgridview').hide();
			//$('.leaddashtimelineview').hide();
			$('.leadlistview').fadeIn(2000);
		});
		$('.leadgview').click(function(){
			$('.leadgridview').fadeIn(2000);
			//$('.leaddashtimelineview').hide();
			$('.leadlistview').hide();
		});
		/* $('.leadtimelineview').click(function(){
			$('.leaddashgridview').hide();
			$('.leaddashtimelineview').fadeIn(2000);
			$('.leaddashlistview').hide();
		}); */
 $('table.leaddashgrid').each(function() {
        var $table = $(this);
        $table.find('.parent').click(function() {
            $(this).nextUntil('.parent').toggle(); // must use jQuery 1.4 for nextUntil() method
			$( this ).find( "td.firstrow" ).toggleClass( "closebutton" );
        });

      
    });		
	/**** month selection **/
	$('.monboxcontainer .mon-box').click(function(){
		$('.monboxcontainer .mon-box').removeClass('active');
		var monthind = $('.monboxcontainer .mon-box').index(this);
		console.log(monthind);
		$('.monboxcontainer .mon-box').eq(monthind).addClass('active');
	});
/*** month selection*/	
		
	

         
			$('.discountview').click(function(){
				$('#first').toggle('slide', {direction: 'right'});
				$('#first').animate({ right: '30px', height:'100px' }, 2000);  
				//$('#first').css('z-index','10440', 'overflow','auto');
				
			});
			$('.specialconditionview').slideUp();
			$('.specialview').click(function(){
				$('.specialconditionview').slideToggle();
			});
 


			
			/*** Examples */
$("#funcDetail1").on("click", function(){
    // var itemget = $('#leadgridviewanother').jqxGrid('getcellvalue', 1, "leadname");
	var myGrid = $('#leadgridviewanother'),
    selRowId = myGrid.jqGrid ('getcellvalue', 1,'selrow');
   // celValue = myGrid.jqGrid ('getCell', selRowId, 'status');
     alert(selRowId);
});
			
			/*** Accordion on filters starts */
			 //toggle the component with class accordion_body
			 $(".accordion_body").eq(0).slideDown(300);
			  $(".accordion_head").eq(0).children(".plusminus").text('-');
    $(".accordion_head").click(function () {
        if ($('.accordion_body').is(':visible')) {
            $(".accordion_body").slideUp(300);
            $(".plusminus").text('+');
        }
        if ($(this).next(".accordion_body").is(':visible')) {
            $(this).next(".accordion_body").slideUp(300);
            $(this).children(".plusminus").text('+');
        } else {
            $(this).next(".accordion_body").slideDown(300);
            $(this).children(".plusminus").text('-');
        }
    });
			/*** Accordion on filters end */
 });

 function getPopContent(target) {
    return $("#" + target + "_content > div.popContent").html();
}


$(window).load( function() {
  	$('#monthlycalendar').monthly({
			mode: 'event',
			xmlUrl: 'events.xml'
		});

		/* $('#monthlycalendar').fullCalendar({
            header: {
                left: 'prev',
                center: 'title',
                right:'next'
            },
			 events: [
        {
            title  : 'event1',
            start  : '2016-05-01'
        },
        {
            title  : 'event2',
            start  : '2016-06-05',
            end    : '2016-06-07'
        },
        {
            title  : 'event3',
            start  : '2016-06-09T12:30:00',
            allDay : false // will make the time show
        }
    ],
            defaultView: 'month',
            editable: true,
            nowIndicator:true
        }); */
	 
	});
	
function commaSeparateNumber(val){
   x=val.toString();
var lastThree = x.substring(x.length-3);
var otherNumbers = x.substring(0,x.length-3);
if(otherNumbers != '')
    lastThree = ',' + lastThree;
var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    return res;
  } 
  function printDiv(elem)
{
	//console.log(data);
	var divElements = document.getElementById(elem).innerHTML;
//	console.log(divElements);
      Popup($('<div/>').append($(divElements).clone()).html());
}

function Popup(data) 
{
	
    var mywindow = window.open('', 'Term Sheets', 'height=900,width=1000');
    mywindow.document.write('<!doctype html><html moznomarginboxes mozdisallowselectionprint><head><title>Term Sheets</title>');
    mywindow.document.write('<link  href="css/lead/print.css" media="print" rel="stylesheet" type="text/css" /><link href="css/lead/lead-details-style.css" rel="stylesheet" /><link href="css/style.css" rel="stylesheet" />'+
'<link href="css/tefont.css" rel="stylesheet" /><style>ul.dropdown-menu{display:none !important;}</style>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');
setTimeout(function(){  mywindow.print(); mywindow.close();}, 200);
    //
  

    return true;
}
  
  /*** print the pdf format s
function printDiv(divID) {
	
	var divElements = document.getElementById(divID).innerHTML;
    
    var oldPage = document.body.innerHTML;
    
   document.body.innerHTML = 
   "<html moznomarginboxes mozdisallowselectionprint><head><link href='css/lead/print.css' media='print' rel='stylesheet' /></head><body>" + 
    divElements + "</body>";
    
    window.print();
    
    document.body.innerHTML = oldPage;
}
  print the pdf format */