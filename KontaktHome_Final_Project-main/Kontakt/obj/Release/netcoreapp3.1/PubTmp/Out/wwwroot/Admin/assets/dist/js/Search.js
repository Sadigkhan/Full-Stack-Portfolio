$(document).ready(function () {

    $(document).on("change", ".searchBrandBtn", function (e) {
        e.preventDefault()
        
        let id = $(this).find(':selected').val()
       
        if (id != null && id.length > 0) {
            fetch('/admin/product/GetBrand/' + id).then(response => response.text()).then(data => {

                $(".BrandList").html(data);
                
               
                //fetch(`/admin/product/GetDetail/?id=${id}&brandId=${25}`).then(res => res.text()).then(data => {

                //    $(".DetailList").html(data);
                //    $(".static-column").removeClass("d-none");
                //})
            });
        } else {

            $(".BrandList").html('');
            $(".DetailList").html('');
            $(".static-column").addClass("d-none");
        }

    })
    $(document).on("change", ".searchDetailBtn", function (e) {
        e.preventDefault()
        
        let Brandid = $(this).find(':selected').val()
        let id = $(".searchBrandBtn").find(':selected').val()

        if (id != null && id.length > 0) {
            


            fetch(`/admin/product/GetDetail/?id=${id}&brandId=${Brandid}`).then(res => res.text()).then(data => {

                    $(".DetailList").html(data);
                    $(".static-column").removeClass("d-none");
                });
            
        } else {

            $(".BrandList").html('');
            $(".DetailList").html('');
            $(".static-column").addClass("d-none");
        }

    })

    $(document).on("change", ".detailInput", function (e) {
        e.preventDefault()
        

        if ($(this).val().length > 0) {
            $(this).addClass("active")
            $(this).removeClass("deactive")

        }
        else {
            $(this).removeClass("active")
            $(this).addClass("deactive")
        }

        

    })


    $(document).on("change", ".detailInput2", function (e) {
        e.preventDefault()


        if ($(this).val().length > 0) {
            $(this).addClass("change")
            $(this).removeClass("deactive")

        }
        else {
            $(this).removeClass("change")
            $(this).addClass("deactive")
        }



    })

    $(document).on("change", ".searchDetailKeyBtn", function (e) {
        e.preventDefault()
        
        let id = $(this).find(':selected').val()

        if (id != null && id.length > 0) {
            fetch('/admin/Detail/GetDetailKeyList/' + id).then(response => response.text()).then(data => {

                $(".DetailKeyList").html(data);
                $(".static-group").removeClass("d-none")
            });
        } else {

            
            $(".DetailKeyList").html('');
            $(".static-group").addClass("d-none")
            
        }

    })
    $(document).on("mouseenter", ".searchDetailKeyBtn", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()
        let detId = $(".detailkeylist").find(':selected').val()

        if (id != null && id.length > 0) {

            if (!(detId != null && detId.length > 0)) {
                fetch('/admin/Detail/GetDetailKeyList/' + id).then(response => response.text()).then(data => {

                    $(".DetailKeyList").html(data);
                    $(".static-group").removeClass("d-none")
                });
            }

            
        } else {


            $(".DetailKeyList").html('');
            $(".static-group").addClass("d-none")

        }

    })

    $(document).on("click", ".AddNewVal", function (e) {
        e.preventDefault();
        let valueInp = `<div class="form-group ">
                        <label>please enter value</label>
                        <div class="input d-flex align-items-end justify-content-between">
                            <div class="left col-lg-11">
                                <input class="form-control" placeholder="Enter Value" type="text" id="Names" name="Names" value="">
                            </div>
                            <div class="right col-lg-1">
                                <button class="InpDeleteBtn btn btn-danger btn-circle mb-2 mr-1"><i class="fas fa-trash-alt"></i></button>
                            </div>
                        </div>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Names" data-valmsg-replace="true"></span>
                    </div>`
        
        $(".ValueListdinamic").append(valueInp)

    })
    $(document).on("click", ".InpDeleteBtn", function (e) {
        e.preventDefault();

        $(this).parent().parent().parent().remove();

    })

    $(document).on("change", ".searchSubCtgBtn", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()

        if (id != null && id.length > 0) {
            fetch('/admin/Category/GetSubCtg/' + id).then(response => response.text()).then(data => {

                $(".SubCtgList").html(data);

            });
        } else {

            $(".SubCtgList").html('');
            $(".BrandList").html('');
            $(".DetailList").html('');
            $(".static-column").addClass("d-none");
        }

    })


    $(document).on("change", ".searchParentSubCtgBtn", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()

        if (id != null && id.length > 0) {
            fetch('/admin/Category/GetParentSubCtg/' + id).then(response => response.text()).then(data => {

                $(".SubCtgList").html(data);

            });
        } else {

            $(".SubCtgList").html('');
            $(".BrandList").html('');
            $(".DetailList").html('');
            $(".static-column").addClass("d-none");
        }

    })

    $(document).on("change", ".parentselect", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()

        if (id != null && id.length > 0) {

            $(".searchParentSubCtgBtn").removeAttr("id");
            $(".searchParentSubCtgBtn").removeAttr("name");


        } else {

            $(".searchParentSubCtgBtn").attr("id", "ParentId")
            $(".searchParentSubCtgBtn").attr("name", "ParentId");
        }

    })


    $(document).on("change", ".searchDetailBtn", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()
        let ctgId = $(".searchBrandBtn").find(':selected').val();

        
        let url = "/admin/product/GetSimilarProduct/";

        if (id != null && id.length > 0) {
            fetch(url + "?id="+id +"&ctgId="+ctgId).then(response => response.text()).then(data => {

                $(".Similar-pro-list").html(data);


            });
        } else {

            $(".BrandList").html('');
            $(".DetailList").html('');
            $(".Similar-pro-list").html('');
            $(".static-column").addClass("d-none");
        }

    })

    $(document).on("change", ".similarproduct", function (e) {
        e.preventDefault()

        let id = $(this).find(':selected').val()
        
       

        let url = "/admin/product/GetSimilarDeatil/";

        if (id != null && id.length > 0) {
            fetch(url + "?id=" + id).then(response => response.text()).then(data => {

                $(".DetailList").html(data);

                
            });

            let url2 = "/admin/product/GetProMainDetail/";
            fetch(url2 + "?id=" + id).then(response => response.text()).then(data => {

                $(".static-column-db").html(data);


            });
        } else {
            ctgId = $(".searchBrandBtn").find(':selected').val();
            Brandid = $(".searchDetailBtn ").find(':selected').val();
            url = "/admin/product/GetDetail/" + "?brandId=" + Brandid + "&id=" + ctgId;
            console.log(url);
            fetch(url).then(res => res.text()).then(data => {

                $(".DetailList").html(data);
                
            });
            
        }

    })
})