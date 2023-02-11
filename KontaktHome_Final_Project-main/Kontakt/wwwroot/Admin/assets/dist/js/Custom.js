$(document).ready(function () {

    $(document).on("click", ".delete", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to Delete this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
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
                ).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".TableList").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restore", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
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
                ).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {

                    $(".TableList").html(data);
                })
            }
        })

    })

    $(document).on("click", "#deleteImage", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url).then(response => { return response.text() }).then(data => { $(".productupdate").html(data) });
    })

    

  
   

})