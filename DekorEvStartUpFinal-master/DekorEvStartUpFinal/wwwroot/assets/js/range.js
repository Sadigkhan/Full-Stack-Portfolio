function getVals(){
    let parent = this.parentNode;
    let slides = parent.getElementsByTagName("input");
    let slide1 = parseFloat( slides[0].value );
    let slide2 = parseFloat( slides[1].value );
    if( slide1 > slide2 ){ 
        let tmp = slide2; slide2 = slide1; slide1 = tmp; 
    }
    
    let displayElement = parent.getElementsByClassName("rangeValues")[0];
        displayElement.innerHTML =  slide1 + " -     " + slide2;
  }
  
  window.onload = function(){
    let sliderSections = document.getElementsByClassName("range-slider");
        for( let x = 0; x < sliderSections.length; x++ ){
          let sliders = sliderSections[x].getElementsByTagName("input");
          for( let y = 0; y < sliders.length; y++ ){
            if( sliders[y].type ==="range" ){
              sliders[y].oninput = getVals;
              sliders[y].oninput();
            }
          }
        }
  }


  // Filter-toggle-shop-wide
$('.btn_filter').click(()=> {
	$('.sw-filter .left').toggle();
})
