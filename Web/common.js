// Creates the menu inside the menu DIV tag in the template
function CreateMenu() {
    var strMenu;
    
    strMenu = '<p class="nav">' + '\n';
    strMenu += '<a id="l_index" href="index.html" title="Home">Home</a> · ';
    strMenu += '<a id="l_schedule" href="schedule.htm" title="Schedule">Schedule</a> · ';
    strMenu += '<a id="l_boardgames" href="boardgames.htm" title="Board Games">Board Games</a> · '
    strMenu += '<a id="l_warhammer" href="warhammer.html" title="Warhammer 40K">Warhammer 40K</a> · ';
    //strMenu += '<a id="l_warmahorde" href="warmahorde.html" title="Warmahordes">Warmahordes</a> · ';
    strMenu += '<a id="l_directions" href="accommodations.html" title="Directions/Accommodations">Directions/Accommodations</a> · ';
    strMenu += '<a id="l_vendors" href="vendors.html" title="Vendors/Sponsors">Vendors/Sponsors</a>\n';
    strMenu += '<br>\n';
    strMenu += '<a id="l_reg" href="registration.html" title="Registration">Registration</a> · \n';
    strMenu += '<a id="l_game" href="game.html" title="Run a Game!">Run a Game!</a> · \n';
    strMenu += '<a id="l_whatis" href="whatis.html" title="What is Roleplaying?">What is Roleplaying?</a> · \n';
    strMenu += '<a id="l_why" href="why.html" title="Why come to a gaming convention?">Why come?</a> · \n';
    strMenu += '<a id="l_inclusion" href="inclusion.html" title="Inclusion">Inclusion</a> · \n';
    strMenu += '<a id="l_links" href="links.html" title="Thanks">Thanks</a> · \n';
    strMenu += '<a id="l_contact" href="mailto:dwatson@nexicom.net" title="Contact Us">Contact Us</a>\n';
    strMenu += '</p>';

    // Add the menu to the DOM
    $("#menu").append(strMenu);
}

// Creates the Footer on each page

function CreateFooter() {

    var strFooter;
    
    strFooter = '<div align="center"><img src="images/imprint.gif"></div>' + '\n';
	strFooter += '<p class="class">© Phantasm</p>' + '\n';
    strFooter += '<p class="class">Statically Obfuscated Design by Sara Manning &amp; Bryan McKellar<br />' + '\n';
    strFooter += 'Web Page hosted by the <a href="http:\\www.pfga.ca" target="_blank">Peterborough Fish &amp; Game Association</a>' + '\n';        
    strFooter += '</p>' + '\n';

    // Add the Footer to the DOM
    $("#footer").append(strFooter);
}

function init() {
    CreateMenu();
    CreateFooter();
    $($("#pageRef").val()).css('color', 'orange');
}
$(document).ready(init);