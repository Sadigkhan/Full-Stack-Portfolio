$(document).ready(function () {

    $(document).on("click", ".PageBtn", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");
        let stat = $(".Status").val();
        let Key = $(".inpName").val();
        let KeyCtg = $(".inpNameCtg").val();
        let TypeStatus = $(".TypeStatus").val();
        let KeyBrand = $(".inpNameBrand").val();
        let KeyDetailKey = $(".inpNameDetailKey").val();



        fetch(url
            + "&status=" + stat
            + "&TypeStatus=" + TypeStatus
            + "&key=" + Key
            + "&keyCtg=" + KeyCtg
            + "&keyBrand=" + KeyBrand
            + "&keyDetailKey=" + KeyDetailKey
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".TableList").html(data)
        })
    })

    $(document).on("change", ".StatusChange", function (e) {
        e.preventDefault()
        
        let url = $(".SearchBtn").attr("href");
        let stat = $(".Status").val();
        let Key = $(".inpName").val();
        let KeyCtg = $(".inpNameCtg").val();
        let TypeStatus = $(".TypeStatus").val();
        let KeyBrand = $(".inpNameBrand").val();
        let KeyDetailKey = $(".inpNameDetailKey").val();



        fetch(url
            + "?status=" + stat
            + "&TypeStatus=" + TypeStatus
            + "&key=" + Key
            + "&keyCtg=" + KeyCtg
            + "&keyBrand=" + KeyBrand
            + "&keyDetailKey=" + KeyDetailKey
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".TableList").html(data)
        })
    })


    $(document).on("keyup", ".inpVal", function (e) {
        e.preventDefault()

        let url = $(".SearchBtn").attr("href");
        let stat = $(".Status").val();
        let Key = $(".inpName").val();
        let KeyCtg = $(".inpNameCtg").val();
        let TypeStatus = $(".TypeStatus").val();
        let KeyBrand = $(".inpNameBrand").val();
        let KeyDetailKey = $(".inpNameDetailKey").val();
        let search = $(".inpSearch").val();
        let UserName = $(".UserName").val();


        fetch(url
            + "?status=" + stat
            + "&TypeStatus=" + TypeStatus
            + "&key=" + Key
            + "&keyCtg=" + KeyCtg
            + "&keyBrand=" + KeyBrand
            + "&keyDetailKey=" + KeyDetailKey
            + "&search=" + search
            + "&UserName=" + UserName
            ).then(res => {
            return res.text()
        }).then(data => {
            $(".TableList").html(data)
        })
        
    })

    //order
    $(document).on("mouseover", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        $(this).parent().addClass("hover")

    })
    $(document).on("mouseout", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        $(this).parent().removeClass("hover")

    })

    $(document).on("click", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        let url = $(this).attr("formaction")
        window.location.pathname = url;
    })
    $(document).on("click", ".styles_rightSide-btn-close", function (e) {
        e.preventDefault();
        
        let url = $(this).attr("formaction")
        window.location.pathname = url;
    })
   
})
