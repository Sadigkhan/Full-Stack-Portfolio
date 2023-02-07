$('.search-icon').click(()=>{
    $('#Search-area').slideToggle();
})

$('.ham-menu').click(()=>{
    $('#Sidebar').addClass('side-active');
})

$('.close i').click(()=>{
    $('#Sidebar').removeClass('side-active');
})



let stock_status = document.querySelectorAll('.stock-status');
for(let status of stock_status) {
	if(status.innerHTML === 'Bəli') {
		status.style.backgroundColor = 'green';
	}
	else{
		status.style.backgroundColor = 'red';
	}
}

var navItems = document.querySelectorAll(".mobile-bottom-nav__item");
navItems.forEach(function(e, i) {
	e.addEventListener("click", function(e) {
		navItems.forEach(function(e2, i2) {
			e2.classList.remove("mobile-bottom-nav__item--active");
		})
		this.classList.add("mobile-bottom-nav__item--active");
	});
});






