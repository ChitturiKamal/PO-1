  $(function() { // document ready
        var sdat= "2016-04-10 12:30:00"; 
        var edat= '2016-04-10 12:40:50';

        $('#calendarnew').fullCalendar({
            header: {
                left: '',
                center: '',
                right:'prev,next'
            },
            defaultView: 'agendaDay',
            editable: true,
            nowIndicator:true
        });
        $('.addnote').summernote({
            height: 120,
            toolbar: [
					  ['style', ['bold', 'italic', 'underline', 'clear']],
					  ['fontsize', ['fontsize']],
					  ['color', ['color']],
					  ['para', ['ul', 'ol', 'paragraph']],
					 ]
        });
		 $('.popovernote').summernote({
            height: 60,
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

        var selectdate;
        $('#datepicker').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
            todayHighlight:true,
        }).on('changeDate', function (ev) {
            // /console.log("Date changed: ", ev.target.value);
            selectdate = ev.target.value;
  
            $(this).datepicker('hide');
        });

        /*$('#datepicker').on('change',function(){
            var vale = $('.date-pick').data("datepicker");
            console.log(vale.date);
            var date = vale.date;
            var formatted =date.getDate()+ "/" + (date.getMonth() + 1) + "/" +  date.getFullYear()  ;
            alert(formatted);
        
        });*/
$('.prjlsttable tr').click(function(){
	$('.prjlsttable tr').removeClass('selectrow');
	$(this).addClass('selectrow');
	$('.notfolup').hide();
	$('.detailpgtermsheet').fadeIn(1000);
	
    var customerId = $(this).find("td").eq(4).html();  
console.log(customerId);	
if(customerId=='Draft')
{
	$('.watermark').show();
	
}
else{
	$('.watermark').hide();
}	

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
$('.savesplnote').click(function(){
	var spltextareaval =$('.specialcondition').summernote('code');
	var formattextareaval= $.parseHTML(spltextareaval);
	var splnotevalue = $('.displaycontent').html(formattextareaval);
	$('.special_entries').toggle();
	$('.specialdata').append($('.displaycontent').html());

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
	$('.discountview').click(function(){
		$('#costspt').hide();
				$('#discont').toggle('slide', {direction: 'right'});
				$('#discont').animate({ right: '50px', height:'380px' }, 50);  
			});
			
			$('.costsplit').click(function(){
				$('#discont').hide();
				$('#costspt').toggle('slide', {direction: 'right'});
				$('#costspt').animate({ right: '50px', height:'260px' }, 50);  
			
				
			});
			$('.specialconditionview').slideUp();
			$('.specialview').click(function(){
				$('.specialconditionview').slideToggle();
			});
        $('#demo-htmlselect').ddslick({
            showSelectedHTML: true,
            onSelected: function(selectedData){
                var str = $('.dd-selected-value').val();
                $('.followups-selct').val(str);
            }   
        });
        $('.usericondate .clear').click(function(){
            $('.date-pick').val("");
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
        $('.selectionentry').hide();
        $('.addnoteinitialscreen').show();
        $('.tab-list li').click(function (e) {
            $('.addnoteinitialscreen').hide();
  
            $('.tab-list li a').removeClass('selt');
            $('.selectionentry').hide();
            var tabindexval = $('.tab-list li').index(this);
            $('.selectionentry').eq(tabindexval).fadeIn();
            // alert(tabindexval);
            $('.tab-list li a').eq(tabindexval).addClass('selt'); 
  
            $('#calendarnew').fullCalendar('render');
   
 

        });
		$('.addmilestone').on('click',function(event){
	event.stopPropagation();
	 $('.pdf-format tr.milestoneadding').after('<tr><td class="txtalgncenter inline-edit" width="20" contenteditable="true">&nbsp;</td>'+ 
	 '<td class="inline-edit" style="text-align:left; padding:3px 0 3px 15px;" contenteditable="true"></td>'+
	   '<td class="txtalgncenter inline-edit" style="padding:3px 2px 3px 3px;" contenteditable="true"></td>'+
	   	   '<td class="txtalgnright inline-edit" style="position:relative;padding:3px 2px 3px 3px;"><span contenteditable="true">&nbsp;</span><span class="removemilestone pull-right"><i class="fa fa-times" aria-hidden="true"></i></span></td></tr>');
		    var $div=$('.slnoedit'), isEditable=$div.is('.editable');
    $('.slnoedit').prop('contenteditable',!isEditable).toggleClass('editable');
	   $('.removemilestone').on('click',function(event){
	event.stopPropagation();
	 $(this).parent().parent().remove();
    });	
});	
        $('.addnoteinitialscreen').show();
        $('.addnoteinitialscreen').click(function(e){
            e.preventDefault();
            $('.addnoteinitialscreen').hide();
            $('.selectionentry').eq(0).fadeIn();
            $('.tab-list li a').eq(0).addClass('selt'); 
	 
        });
          $('.enqcurrentinfo,.assigninfo,.hideinfoenq,.hideinfoass,.terminfo,.hideterminfo,.editinfoass').hide();
        $('.editinfoassign').on('click', function (e) {
			 e.preventDefault();
            $('.assinfodetails').hide();
            $('.editinfoass').slideDown();
        });
        $('.showinfoenq').click(function(e){
			 e.preventDefault();
	        $('.enqcurrentinfo').slideDown();
            $('.showinfoenq').hide();
            $('.hideinfoenq').show();
        });
        $('.hideinfoenq').click(function(e){
	 e.preventDefault();
            $('.enqcurrentinfo').slideUp();
            $('.showinfoenq').show();
            $('.hideinfoenq').hide();
        });
        $('.showinfoass').click(function(e){
	 e.preventDefault();
            $('.assigninfo').slideDown();
            $('.showinfoass').hide();
            $('.hideinfoass').show();
        });
        $('.hideinfoass').click(function(e){
	 e.preventDefault();
            $('.assigninfo').slideUp();
            $('.showinfoass').show();
            $('.hideinfoass').hide();
        });
		
		 $('.showterminfo').click(function(e){
	 e.preventDefault();
            $('.terminfo').slideDown();
            $('.showterminfo').hide();
            $('.hideterminfo').show();
        });
        $('.hideterminfo').click(function(e){
	 e.preventDefault();
            $('.terminfo').slideUp();
            $('.showterminfo').show();
            $('.hideterminfo').hide();
        });
        $('.subassbtn').click(function(e){
			 e.preventDefault();
            $('.assinfodetails').show();
            $('.editinfoass').hide();
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
	    $(".contelcode").intlTelInput({
            allowExtensions: true,
            autoFormat: false,
            autoHideDialCode: false,
            autoPlaceholder: true,
            defaultCountry: "auto",
            ipinfoToken: "yolo",
            nationalMode: false,
            numberType: "MOBILE",
           
            preventInvalidNumbers: true
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
$('[data-toggle="tooltip"]').tooltip(); 
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
$("input[type*=radio]").each(function(i,value){ 
    $(this).attr("title");
    });
    $('label').tooltip({
        placement : 'top'
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

$('.addcarpark').on('click',function(event){
	 event.stopPropagation();
	 $('.pdf-format tr.carparkadding').after('<tr><td class="txtalgncenter" width="20">&nbsp;</td>'+ 
	 '<td style="text-align:left; padding:3px 0 3px 15px;"><select class="selectpicker"><option>Regular Independent Car Park/s</option>'+
	'<option>Large Independent Car Park/s</option><option>Small Independent Car Park/s</option><option>Large Twin Car Park/s</option>'+
	'<option>Regular Twin Car Park/s</option><option>Small Twin Car Park/s</option><option>Large Special Car Park/s</option>'+	'<option>Regular Special Car Park/s</option><option>Small Special Car Park/s</option><option>Large Open to Sky Car Park/s</option></select></td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;"><span class="inline-edit" contenteditable="true">1</span></td>'+
	   '<td class="txtalgnright" style="padding:3px 2px 3px 3px;">1</td>'+
	   '<td class="txtalgnright inline-edit" style="position:relative;padding:3px 2px 3px 3px;"><span contenteditable="true">&nbsp;</span><span class="removecarpark pull-right"><i class="fa fa-times" aria-hidden="true"></i></span></td></tr>');
$('.removecarpark').on('click',function(event){
	event.stopPropagation();
	 $(this).parent().parent().remove();
    });	
});
$('.rating > label.full').click(function(){
	  var $this = $(this);
    if ($this.hasClass('clicked')){
        $(this).css('color','#b12525');
        return;
    }/* else{
        $this.addClass('clicked');
        $(this).css('color','#ffd203');
        setTimeout(function() { 
        $this.removeClass('clicked'); },500);
    }//end of else */
	
});

 /* $("[data-toggle=popover]").popover({
        html : true,
		container: 'body',
        content: function() {
          var content = $(this).attr("data-popover-content");
          return $(content).children(".popover-body").html();
        },
        title: function() {
          var title = $(this).attr("data-popover-content");
          return $(title).children(".popover-heading").html();
        }
    });*/
    });

 	
function printDivNew(elem)
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
setTimeout(function(){   mywindow.print();  mywindow.close();}, 100);
}