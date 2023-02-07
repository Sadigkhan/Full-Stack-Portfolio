$(document).ready(function () {

    $(document).on("click", ".decrease", function (e) {
        e.preventDefault()
        let url = $(this).attr("href")

        fetch(url).then(res => {
            return res.text()

        }).then(data => {
            $("#Cart").html(data)
        })
    })



    $(document).on("click", ".increase", function (e) {
        e.preventDefault()
        let url = $(this).attr("href")

        fetch(url).then(res => {
            return res.text()

        }).then(data => {
            $("#Cart").html(data)
        })
    })



    $(document).on("click", ".deletecard", function (e) {
        e.preventDefault()
        let url = $(this).attr("href")

        fetch(url).then(res => {
            return res.text()
        }).then(data => {
            $("#Cart").html(data)
            let count = parseInt($(".count-basket").text());
            count--;
            $(".count-basket").text(count)
        })
    })


            $('#input-search').keyup(function () {
                let val = $(this).val().trim();
                if (val.length > 1) {
                    $(".search-list").removeClass("d-none")
                    $.ajax({
                        type: "GET",
                        url: "/Product/SearchPartial?query=" + val,
                        success: function (response) {
                            $('#prod-search-list').html("");
                            $('#prod-search-list').html(response);
                        }
                    })
                }
                else {
                    $(".search-list").addClass("d-none")
                    $('#prod-search-list').html("");
                }
            })



    $(document).on("click", ".deletecompare", function (e) {
        e.preventDefault()
        let url = $(this).attr("href")

        fetch(url).then(res => {
            return res.text()
        }).then(data => {
            $("#Compare").html(data)
            let count = parseInt($(".count-compare").text());
            count--;
            $(".count-compare").text(count)
        })
    })

    $(document).on('click', '.add-basket-btn', function (e) {
        e.preventDefault();

        let url = $(this).attr("href")

        fetch(url).then(res => {
            return res.json();
        }).then(data => {

            console.log(data.count)
            $(".count-basket").html(data.count)
        })
    })

    $(document).on('click', '.add-compare-btn', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.json())
            .then(data => {
                if (data.status == 201) {
                    console.log(data.status)
                    $('.count-compare').html(data.count);
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'success',
                        title: 'Müqayisə üçün əlavə olundu'
                    })
                } else if (data.status == 215) {
                    console.log(data.status)
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'warning',
                        title: 'Müqayisə etmək istədiyiniz məhsul artıq əlavə olunub'
                    })
                } else if (data.status == 121212) {
                    console.log(data.status)
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top-end',
                        showConfirmButton: false,
                        timer: 3000,
                        timerProgressBar: true,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })

                    Toast.fire({
                        icon: 'warning',
                        title: 'Müqayisə üçün sadəcə üç məhsul əlavə oluna bilər'
                    })
                }
            })
    })

    function alertio(data) {
        if (data.status == 215) {


            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'warning',
                title: 'Müqayisə etmək istədiyiniz məhsul artıq əlavə olunub'
            })


        }
        else if (data.status == 121212) {

            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'warning',
                title: 'Müqayisə üçün sadəcə üç məhsul əlavə oluna bilər'
            })

        } else {

            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'success',
                title: 'Müqayisə üçün əlavə olundu'
            })
        }
    }

})