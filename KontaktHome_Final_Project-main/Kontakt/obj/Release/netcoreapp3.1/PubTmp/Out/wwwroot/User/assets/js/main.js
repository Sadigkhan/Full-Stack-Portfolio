



$(document).ready(function () {

    

    let skip = 4;
    let skipFilter = 6;
    let proCount = $("#productCount").val();
    
    //for menu

    //Desktop Menu btn
    $(document).on("click", "#Menubtn", function (e) {
        e.preventDefault();
        $(".side-close-btn").click();
        $(".HomeSubList").html(" ")
        $(".HomeBrandList").html(" ")
        $(".overlay").removeClass("active")
        if ($(".MainCtg").html().length > 22) {
            $(".MainCtg").html(" ")
            $(".SubCtg").html(" ")
            $(".BrandList").html(" ")
            $(".overlay").removeClass("active")
            $("#Menubtn").children().first().removeClass("selected")
            $(".sidebarHome").css("z-index", "9999");
            $("body").removeClass("disabled")
            $(".BrandList").css("display", "none");
            $(".MainCtg").addClass("d-none")
            $(".SubCtg").addClass("d-none")
            
        }
        else {
            $(".MainCtg").removeClass("d-none")
            $(".SubCtg").removeClass("d-none")
            $(".BrandList").css("display", "block");

            $(".SubCtg").html(" ");
            $(".BrandList").html(" ");

            $(".search-section").addClass("d-none");
            $(".overlay").addClass("active");
            $(".overlay").removeClass("for-search");

            $("body").addClass("disabled")
            $(".sidebarHome").css("z-index", "99");
            $("#Menubtn").children().first().addClass("selected")
            let url = $(this).attr("href");
            fetch(url
            ).then(res => {
                return res.text()
            }).then(data => {
                $(".MainCtg").html(data)
            })
        }

    })
    $(document).on("mouseover", ".overlay", function (e) {
        e.preventDefault();
       
        $(".BrandList").css("display", "none");
        

        $(".MainCtg").addClass("d-none")
        $(".SubCtg").addClass("d-none")
        
        $(".HomeSubList").addClass("d-none");
        $(".HomeBrandList").addClass("d-none");

        $(".search-section").addClass("d-none");


        $("#pro-search-input").val("")
        $(".overlay").removeClass("for-search");
        $(".main-category-item-icon").removeClass("selectedMob")
        $(".fa-angle-right").removeClass("selected")
        $(".overlay").removeClass("active");
        $(".subSidebarHome").removeClass("active");
        $(".HomeBrandList").removeClass("active");
        $(".overlayHome").removeClass("active");
        $("#Menubtn").children().first().removeClass("selected")
        $(".mobil-sidebar").removeClass("active")
        $("body").removeClass("disabled")

        $(".mobil-Brandlist").html(" ")
        $(".mobil-sublist").html(" ")
        $(".MainCtg").html(" ")
        $(".SubCtg").html(" ")
        $(".BrandList").html(" ")
        $(".HomeBrandList").html(" ")
        $(".HomeSubList").html(" ")
        $(".sidebarHome").css("z-index", "9999");
    })
    $(document).on("mouseover", ".main-ctg-btn", function (e) {
        e.preventDefault();

        var mainctgbtn = document.querySelectorAll(".main-ctg-btn");
        for (var elem of mainctgbtn) {
            if (elem.lastElementChild.classList.contains("selected")) {
                elem.lastElementChild.classList.remove("selected");
            }
        }
        this.lastElementChild.classList.add("selected");
        $(".SubCtg").removeClass("d-none")
        $(".BrandList").html(" ")
        $(".BrandList").addClass("d-none")
        let url = $(this).attr("href");
        
        fetch(url + "?keyMetod=" + "over"
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".SubCtg").html(data)
           
            if (data.length > 10) {
               
            } else {
                $(".SubCtg").addClass("d-none")
            }
        })

    })
    $(document).on("mouseover", ".sub-ctg-btn", function (e) {
        e.preventDefault();
        let id = $(this).attr("data-id")
        let url = "/Category/GetBrand/" + id
        var mainctgbtn = document.querySelectorAll(".sub-ctg-btn");
        for (var elem of mainctgbtn) {
            if (elem.lastElementChild.classList.contains("selected")) {
                elem.lastElementChild.classList.remove("selected");
            }
        }
        this.lastElementChild.classList.add("selected");

        /*let url = $(this).attr("href");*/
       
        fetch(url + "?keyMetod=" + "over"
        ).then(res => {
            return res.text()
        }).then(data => {
            console.log(data.length)
            $(".BrandList").html(data)
            $(".BrandList").removeClass("d-none")
            if (data.length > 10) {
               
            } else {
                $(".BrandList").addClass("d-none")
            }

        })
    })
    $(document).on("mouseover", ".mainHome-ctg-btn", function (e) {
        e.preventDefault();
        $(".HomeBrandList").addClass("d-none");
        $(".HomeSubList").removeClass("d-none");
        $(".HomeBrandList").html(" ")
        $(".overlayHome").addClass("active");
        $(".overlay").addClass("active");
        var mainctgbtn = document.querySelectorAll(".mainHome-ctg-btn");
        for (var elem of mainctgbtn) {
            if (elem.lastElementChild.classList.contains("selected")) {
                elem.lastElementChild.classList.remove("selected");
            }
        }
        this.lastElementChild.classList.add("selected");

        let url = $(this).attr("href");
        fetch(url + "?keyMetod=" + "over" + "&keyfrom=" + "home"
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".HomeSubList").html(data)
            if (data.length > 8) {
                
            } else {
                $(".HomeSubList").addClass("d-none")
            }
        })


    })
    $(document).on("mouseover", ".subHome-ctg-btn", function (e) {
        e.preventDefault();
        $(".HomeBrandList").removeClass("d-none");
        var mainctgbtn = document.querySelectorAll(".subHome-ctg-btn");
        for (var elem of mainctgbtn) {
            if (elem.lastElementChild.classList.contains("selected")) {
                elem.lastElementChild.classList.remove("selected");
            }
        }
        this.lastElementChild.classList.add("selected");

        let id = $(this).attr("data-id")
        let url = "/Category/GetBrand/" + id

       /* let url = $(this).attr("href");*/
        fetch(url + "?keyMetod=" + "over"
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".HomeBrandList").html(data)
            if (data.length > 8) {
                
            } else {
                
                $(".HomeBrandList").addClass("d-none");
            }
        })
    })
    $(document).on("click", ".section-tab-btn", function (e) {
        e.preventDefault();
        $(".section-tab-btn").removeClass("tabBtnActive");
        $(this).addClass("tabBtnActive");
        let url = $(this).attr("href");
        console.log(url)
        fetch(url + "?keyfrom=" + "home"
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".section-content").html(data)
            if (data.length > 10) {
                $(".section-tab-btn").removeClass("tabBtnActive");
                $(this).addClass("tabBtnActive");
            }
        })

    })

    //product filter
    $(document).on("click", ".filter-item-btn", function (e) {
        e.preventDefault();
        $(this).toggleClass("active")
        $(this).children().first().toggleClass("active");
        $(this).parent().next().toggleClass("active");
        $(this).parent().next().removeClass("max-h");
        $(this).parent().next().children().first().children().last().toggleClass("active");




    })
    $(document).on("click", ".filter-sh-more", function (e) {
        e.preventDefault();
        $(this).children().last().prev().toggleClass("d-none");
        $(this).children().last().toggleClass("d-none");
        $(this).parent().parent().toggleClass("max-h")

        $(this).parent().parent().animate({ scrollTop: 0 }, "slow");





    })
    $(document).on("click", ".ListCheckItem", function (e) {
        e.preventDefault();

        $(this).children().first().children().first().children().first().children().last().toggleClass("d-none");

        let keyValId = $(this).attr("data-val-id")
        let keyDetailId = $(this).parent().parent().parent().attr("data-key-id")
        let brandId = $("#BrandId").val()
        let ctgId = $("#CtgId").val()
        let url = "/product/FilterProList/"
        
        console.log(keyDetailId)

        fetch(url + "?keyValId=" + keyValId + "&keyDetailId=" + keyDetailId + "&brandId=" + brandId + "&ctgId=" + ctgId
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".product-list-items-list").html(data)
            let proCountFilter = $("#productCountFilter").val();
            skipFilter = 6;
            if (proCountFilter > 6)
            {
                $(".pro-load-more-btn-filter").removeClass("d-none")

            }
            else {
                $(".pro-load-more-btn-filter").addClass("d-none")
            }

        })


    })
    $(document).on("click", ".pro-load-more-btn", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        console.log(url)
        fetch(url + "&skip=" + skip
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dispro-static").append(data)
            skip += 4;

            if (skip >= proCount) {
                $(".pro-load-more-btn").remove()


            }
        })


    })
    $(document).on("click", ".section-pro-tab-btn", function (e) {
        e.preventDefault();
        $(".section-pro-tab-btn").removeClass("tabBtnActive");
        $(this).addClass("tabBtnActive");
        let url = $(this).attr("href");
        
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".section-pro-content").html(data)


        })

    })
    $(document).on("click", ".pro-load-more-btn-filter", function (e) {
        e.preventDefault();
        let brandId = $("#BrandId").val()
        let ctgId = $("#CtgId").val()
        let url = $(this).attr("href");
        let proCountFilter = $("#productCountFilter").val();
        console.log(url)
        fetch(url + "?skip=" + skipFilter + "&brandId=" + brandId + "&ctgId=" + ctgId
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".product-list-items-list").append(data)
            skipFilter += 6;

            if (skipFilter >= proCountFilter) {
                $(".pro-load-more-btn-filter").addClass("d-none")


            }
        })


    })

    //login reg
    $(document).on("click", ".side-right-auth-tab-login-btn", function (e) {
        e.preventDefault();
        $(".side-right-auth-tab-btn").removeClass("active");
        $(this).addClass("active")
        $('.styles_errorText').text("")
        $('.styles_succesText').text("")


        let url = "/Account/Login/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".form-fix").html(data)
        })


    })
    $(document).on("click", ".side-right-auth-tab-reg-btn", function (e) {
        e.preventDefault();
        $(".side-right-auth-tab-btn").removeClass("active");
        $(this).addClass("active")
        $('.styles_errorText').text("")
        $('.styles_succesText').text("")

        let url = "/Account/Register/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".form-fix").html(data)
        })


    })
    $(document).on("click", ".forgotpasswor-btn", function (e) {
        e.preventDefault();
        $(".side-right-auth-tab-btn").removeClass("active");
        $(this).addClass("active")

        let url = "/Account/ForgotPassword/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".form-fix").html(data)
        })


    })
    $(document).on("click", ".styles_passwordVisibility", function (e) {
        e.preventDefault();
        $(".bi-eye").toggleClass("active")

        if ($(".inp-pswrd").attr("type") == "text") {
            $(".inp-pswrd").attr("type", "password")
        }
        else {
            $(".inp-pswrd").attr("type", "text")
        }



    })
    $(document).on("click", ".authorizedUserBtn", function (e) {
        e.preventDefault();
        $(".search-section").addClass("d-none");
        $(".overlay").removeClass("active");
        $(".overlay").removeClass("for-search");
        $(".MainCtg").html(" ")
        $(".SubCtg").html(" ")
        $(".BrandList").html(" ")
        $(".overlay").removeClass("active")
        $("#Menubtn").children().first().removeClass("selected")
        $(".sidebarHome").css("z-index", "9999");

        $(".side-right").addClass("active")
        $(".overlay-right").addClass("active")
        $(".user-info").addClass("d-none")
        $(".user-arrowDownSVG").addClass("active")
        $("body").addClass("disabled")
        $("#pro-search-input").val("")
        $(".overlay-right").css("top", 0);

        let url = "/Account/AccountPartial/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)


        })



    })

    $(document).on("click", ".rem-btn", function (e) {
        e.preventDefault();
        $(".styles_input_check-icon").toggleClass("d-none")


    })
    $(document).on("keyup", ".login-input", function (e) {
        e.preventDefault();
        if ($(".inp-pswrd").val().length >= 8) {

            $(".next-btn-style").addClass("active")
            $(".next-btn-style").removeAttr("disabled")

        }
        else {

            $(".next-btn-style").removeClass("active")
            $(".next-btn-style").Attr("disabled")

        }


    })
    $(document).on("click", ".user-btn", function (e) {
        e.preventDefault();
        $(".overlay-right").css("top", 0);
        $(".search-section").addClass("d-none");
        $(".overlay").removeClass("active");
        $(".overlay").removeClass("for-search");
        $(".side-right").addClass("active")
        $(".overlay-right").addClass("active")
        $(".user-info").addClass("d-none")
        $("body").addClass("disabled")
        $("#pro-search-input").val("")

        $(".MainCtg").html(" ")
        $(".SubCtg").html(" ")
        $(".BrandList").html(" ")
        $(".overlay").removeClass("active")
        $("#Menubtn").children().first().removeClass("selected")
        $(".sidebarHome").css("z-index", "9999");

        let url = "/Account/LoginReg/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)
            //$(".side-right-auth-tab-login-btn").click();

        })



    })
  
    $(document).on("click", ".user-btn-mob", function (e) {
        e.preventDefault();
        $(".search-section").addClass("d-none");
        $(".overlay").removeClass("active");
        $(".overlay").removeClass("for-search");
        $("#pro-search-input").val("");

        let url = "/Account/LoginReg/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)
            $(".side-right-auth-tab-login-btn").click();

        })



    })

    $(document).on("click", ".side-close-btn", function (e) {
        e.preventDefault();
        $(".side-right").removeClass("active")
        $(".overlay-right").removeClass("active")
        $(".user-info").removeClass("d-none")
        $(".user-arrowDownSVG").removeClass("active")
        $("body").removeClass("disabled")
        $(".overlay-right").removeClass("content");
        $(".logout-section").removeClass("d-none");
        $("#mob-pro-search-input").val("");
        

    })

    $(document).on("click", ".next-btn-login", function (e) {
        e.preventDefault();
        let email = $("#Email").val()
        let password = $("#Password").val()
        console.log(email)
        //var payload = {
        //    Email: email,
        //    Password: password
        //};


        let url = "/account/login";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({ Email: email, Password: password, RememberMe: true })
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    window.location.pathname = data.message;
                    /*window.location.reload();*/
                }

            //$(".form-fix").html(data)

        })
 

        //let url = "/account/loginpost";
        //fetch(url + "?Email=" + email +"&Password=" +password
            
        //).then(res => {
        //    return res.text()
        //}).then(data => {
        //    $(".form-fix").html(data)
            
        //})

    })
    $(document).on("click", ".next-btn-reg", function (e) {
        e.preventDefault();
        
        let Name = $("#Name").val()
        let SurName = $("#SurName").val()
        let Email = $("#Email").val()
        let UserName = $("#UserName").val()
        let Password = $("#Password").val()
        let CheckPassword = $("#CheckPassword").val()
       
        


        let url = "/account/Register";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(
                {
                    Name: Name,
                    SurName: SurName,
                    Email: Email,
                    UserName: UserName,
                    Password: Password,
                    CheckPassword: CheckPassword,
                    
                })
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.styles_errorText').text(data.message)
                    return;
                } else if (data.status == 200) {
                    $('.styles_errorText').text("")
                    $('.styles_succesText').text(data.message)
                    $(".form-fix").html("")
                }

               

            })


        

    })

    $(document).on("click", ".next-btn-reset-send", function (e) {
        e.preventDefault();
        $(this).removeClass("active")
        $(this).attr("disabled","disabled")
        let email = $("#Email").val()
        
        
        

        let url = "/account/ResetPassword";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({ Email: email})
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $(this).addClass("active")
                    $(this).removeAttr("disabled")
                    $('.styles_errorText').text(data.message)
                    return;
                } else if (data.status == 200) {
                    $('.styles_errorText').text("")
                    $('.styles_succesText').text(data.message)
                    $(".form-fix").html("")
                }

                

            })


        

    })
    



    //mobil
    $(document).on("click", ".authorizedUserBtn-mob", function (e) {
        e.preventDefault();
        let url = "/Account/AccountPartial/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)


        })



    })

    $(document).on("click", ".mobil-main-ctg-btn", function (e) {
        e.preventDefault();
        if ($(this).next().html().length > 94) {
            $(this).next().html(" ")
            this.lastElementChild.classList.remove("selectedMob");

        }
        else {




            let url = $(this).attr("href");
            fetch(url + "?keyMob=" + "mobil"
            ).then(res => {
                return res.text()
            }).then(data => {
                $(this).next().html(data)

                if (data.length > 10) {
                    this.lastElementChild.classList.add("selectedMob");
                }
            })


        }



    })
    $(document).on("click", ".mobil-sub-ctg-btn", function (e) {
        e.preventDefault();
        if ($(this).next().html().length > 26) {
            $(this).next().html(" ")
            this.lastElementChild.classList.remove("selectedMob");
        }
        else {

            /* $(".mobil-Brandlist").html(" ")*/
            let url = $(this).attr("href");
            fetch(url + "?keyMob=" + "mobil"
            ).then(res => {
                return res.text()
            }).then(data => {
                $(this).next().html(data)
                if (data.length > 2) {
                    this.lastElementChild.classList.add("selectedMob");
                }
            })

        }



    })
    $(document).on("click", "#Mob-Menubtn", function (e) {
        e.preventDefault();
        //$(".overlay").toggleClass("active")
        //$(".mobil-sidebar").toggleClass("active")

        $(".side-right").addClass("active")
        $(".overlay-right").addClass("active");
        $(".overlay-right").css("top",0);
        $(".user-info").toggleClass("d-none")
        let url = "/Account/MobilMenu/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)


        })



    })
 

    $(document).on("click", ".mobAction-btn", function (e) {
        e.preventDefault();

        $(this).next().toggleClass("d-none");



    })
    $(document).on("mouseleave", ".mobAction-items", function (e) {
        e.preventDefault();

        $(this).addClass("d-none");



    })


    //my-account

    $(document).on("click", ".styles_navLink-btn", function (e) {
        e.preventDefault();
        $(".styles_navItem").removeClass("active");
        $(this).parent().addClass("active");

        let url = $(this).attr("href");
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".my-account-content-right-content").html(data)

        })



    })
    $(document).on("click", ".styles_accountBox-footer-btn-info", function (e) {
        e.preventDefault();
        $(".content-edit").addClass("active");
        $(".overlay-right").addClass("active");
        $(".overlay-right").addClass("content");
        $("body").addClass("disabled")
      

        

        let url = "/Account/EditAccountInfo/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".content-edit").html(data)

            let gender = $("#Gender").val();
            console.log(gender.length)
            if (gender.length == 4) {
                $(".styles_checkbox-male").addClass("active")
                let val = 0;
                $("#Gender").val(val);

            }
            else {
                $(".styles_checkbox-fmale").addClass("active")
                let val = 1;
                $("#Gender").val(val);
            }
        })
       
        

    })
    $(document).on("click", ".styles_accountBox-footer-btn-adress", function (e) {
        e.preventDefault();
        $(".content-edit").addClass("active");
        $(".overlay-right").addClass("active");
        $(".overlay-right").addClass("content");
        $("body").addClass("disabled")


        let url = "/Account/EditAccountAdress/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".content-edit").html(data)
            
            
        })



    })
    $(document).on("click", ".styles_accountBox-footer-btn-auth", function (e) {
        e.preventDefault();
        $(".content-edit").addClass("active");
        $(".overlay-right").addClass("active");
        $(".overlay-right").addClass("content");
        $("body").addClass("disabled")



        let url = "/Account/EditAccountAuth/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".content-edit").html(data)


        })



    })
    $(document).on("click", ".my-account-img-edit-btn", function (e) {
        e.preventDefault();
        $(".content-edit").toggleClass("active");
        $(".overlay-right").toggleClass("active");




        let url = "/Account/EditAccountImg/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".content-edit").html(data)


        })



    })


    $(document).on("click", ".styles_rightSide-btn-close", function (e) {
        e.preventDefault();
        $(".content-edit").removeClass("active");
        $(".overlay-right").removeClass("active");
        /*$(".body").toggleClass("deactive");*/
        $("body").removeClass("disabled")
        $(".overlay-right").removeClass("content");
        

    })
    $(document).on("click", ".styles_checkbox-btn", function (e) {
        e.preventDefault();
        $(".styles_input-check-svg").removeClass("active");
        $(this).children().last().addClass("active");
        let val = $(this).children().first().val();
        $("#Gender").val(val) 


    })
    $(document).on("click", ".account-info-edit", function (e) {
        e.preventDefault();
        let name = $("#Member_Name").val();
        let surName = $("#Member_SurName").val();
        let parentName = $("#Member_ParentName").val();
        let birthday = $("#Member_Birthday").val();
        let gender = $("#Gender").val();
        let genderVal = Number(gender);
        let phoneNumber = $("#Member_PhoneNumber").val();
        if (birthday=="") {
            birthday ="1950-01-01T15:15"
        }
        console.log(birthday);
       
    
        


        let url = "/account/AccountInfoUpdate";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                Name: name,
                SurName: surName,
                ParentName: parentName,
                Birthday: birthday,
                Gender: genderVal,
                PhoneNumber: phoneNumber,
                
            })
        })
            .then(res => res.json())
            .then(data => {
                
                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    /*window.location.pathname = data.message;*/
                    $(".content-edit").removeClass("active");
                    $(".overlay-right").removeClass("active");
                    $(".overlay-right").removeClass("content");
                    $("body").removeClass("disabled")
                    $(".styles_userName").text(data.message)

                    let url = "/account/myaccount?from=account";
                    fetch(url
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".my-account-content-right-content").html(data)
                        
                        
                    })
                    
                    
                }

               

            })


        

    })
    $(document).on("click", ".account-adress-edit", function (e) {
        e.preventDefault();
        let Address = $("#Address").val();
        let City = $("#City").val();
        let ZipCode = $("#ZipCode").val();
        let Country = $("#Country").val();
        let State = $("#State").val();
       




        let url = "/account/AccountAdressUpdate";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                Address: Address,
                City: City,
                ZipCode: ZipCode,
                Country: Country,
                State: State,
            })
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    /*window.location.pathname = data.message;*/
                    $(".content-edit").removeClass("active");
                    $(".overlay-right").removeClass("active");
                    $(".overlay-right").removeClass("content");
                    $("body").removeClass("disabled")
                    

                    let url = "/account/myaccount?from=account";
                    fetch(url
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".my-account-content-right-content").html(data)


                    })


                }



            })




    })
    $(document).on("click", ".account-auth-edit", function (e) {
        e.preventDefault();

        let CurrentPassword = $("#CurrentPassword").val();
        let Password = $("#Password").val();
        let ConfirmPassword = $("#ConfirmPassword").val();
        let Email = $("#Email").val();
        





        let url = "/account/AccountAuthUpdate";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                CurrentPassword: CurrentPassword,
                Password: Password,
                ConfirmPassword: ConfirmPassword,
                Email: Email,
            })
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    /*window.location.pathname = data.message;*/
                    $(".content-edit").removeClass("active");
                    $(".overlay-right").removeClass("active");
                    $(".overlay-right").removeClass("content");
                    $("body").removeClass("disabled")


                    let url = "/account/myaccount?from=account";
                    fetch(url
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".my-account-content-right-content").html(data)


                    })


                }



            })




    })
    $(document).on("click", ".styles_deletePhotoButton", function (e) {
        e.preventDefault();
       

        let url = "/Account/UserDeleteImage/"
        fetch(url
        ).then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    /*window.location.pathname = data.message;*/
                   
                    $(".user-img").attr("src", data.message)

   

                }



            })



    })
    $(document).on("change", ".img-input", function (e) {
        e.preventDefault();
        for (let file of e.target.files) {
            const reader = new FileReader();
            reader.onloadend = function (event) {
                $(".user-img").attr("src", event.target.result)

            }
            reader.readAsDataURL(file);
        }

        



    })
    $(document).on("click", ".rightSide-btn-close", function (e) {
        e.preventDefault();
        //$(".content-edit").toggleClass("active");
        //$(".overlay-right").toggleClass("active");
        let src = $(".user-img-has").attr("src")
        $("body").removeClass("disabled")
        $(".user-img").attr("src", src)

        console.log(src)


    })

    //edit account img
    $(document).on("click", ".account-img-edit1", function (e) {
        e.preventDefault();
        

        let url = "/account/AccountImgUpdate";
        fetch(url, {
            
        })
            .then(res => res.json())
            .then(data => {

                if (data.status == 400) {
                    $('.login-validation-summary').text(data.message)
                    return;
                } else if (data.status == 200) {
                    /*window.location.pathname = data.message;*/
                    $(".content-edit").removeClass("active");
                    $(".overlay-right").removeClass("active");
                    $(".overlay-right").removeClass("content");
                    $("body").removeClass("disabled")


                }



            })



    })

    //edut account img ajax
    

    $(document).on("click", ".account-img-edit", function (e) {
        e.preventDefault();
      
       
        

        
        

        //let CurrentPassword = $("#CurrentPassword").val();
        //let Password = $("#Password").val();
        //let ConfirmPassword = $("#ConfirmPassword").val();
        //let Email = $("#Email").val();






        //let url = "/account/AccountAuthUpdate";
        //fetch(url, {
        //    headers: {
        //        'Accept': 'application/json',
        //        'Content-Type': 'application/json'
        //    },
        //    method: "POST",
        //    body: JSON.stringify({
        //        CurrentPassword: CurrentPassword,
        //        Password: Password,
        //        ConfirmPassword: ConfirmPassword,
        //        Email: Email,
        //    })
        //})
        //    .then(res => res.json())
        //    .then(data => {
        //        console.log(data)
        //        if (data.status == 400) {
        //            $('.login-validation-summary').text(data.message)
        //            return;
        //        } else if (data.status == 200) {
        //            /*window.location.pathname = data.message;*/
        //            $(".content-edit").toggleClass("active");
        //            $(".overlay-right").toggleClass("active");


        //            let url = "/account/myaccount?from=account";
        //            fetch(url
        //            ).then(res => {
        //                return res.text()
        //            }).then(data => {
        //                $(".my-account-content-right-content").html(data)


        //            })


        //        }



        //    })




    })
    //kontakt tv
    $(document).on("click", ".tvlist-btn", function (e) {
        e.preventDefault();
        var link = $(this).attr("data-link");
        $(".tvlist-btn").removeClass("active")
        $(this).addClass("active")
        $(".section-content-tv-video-item").attr("src", link);

    })

    //credit clc
    $(document).on("click", ".pro-credit-cal-btn", function (e) {
        e.preventDefault();
        $(".pro-credit-cal-btn").removeClass("active");
        $(this).addClass("active");
        let mon = $(this).attr("data-mon")
        let price = $(".pro-price_val").val();
        
        if (mon > 12) {
            if (mon==14) {
                var mon_price = (Number(price) + Number(price * 15 / 100)) / mon
            }
            else if (mon==15) {
                var mon_price = (Number(price) + Number(price * 20 / 100)) / mon
            }
            else {
                var mon_price = (Number(price) + Number(price * 25 / 100)) / mon
            }
            
        }
        else {
            var mon_price = price/ mon
        }
        
        $(".pro-credit-clc-box-vall-mon").html(mon_price.toFixed(2));
        

    })

    //slider change
    $(document).on("click", ".pro-tump-slide-img", function (e) {
        e.preventDefault();
        let src = $(this).attr("src")
        $(".pro-main-slide-img.slick-slide.slick-current.slick-active").attr("src",src)
        
        

    })


    $(document).on("click", ".refbtn", function (e) {
        e.preventDefault();
        window.location.replace(window.location.pathname="/home/index/")
        


    })
    if ($("#for-reset").val() == "true") {
        
        $(".side-right").toggleClass("active")
        $(".overlay-right").toggleClass("active")
        $(".user-info").toggleClass("d-none")
        $(".user-info").addClass("refbtn")
        $(".user-btn").addClass("refbtn")
        $(".side-close-btn").addClass("refbtn")

        let url = "/Account/LoginReg/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)
           /* window.location.pathname = "/home/Index/";*/
            let url2 = "/Account/NewPassword/"
            fetch(url2
            ).then(res => {
                return res.text()
            }).then(data => {
                $(".form-fix").html(data)
                $(".side-right-auth-tab").addClass("d-none")
                /* window.location.pathname = "/home/Index/";*/


            })

        })

    }

    $(document).on("click", ".next-btn-reset-save", function (e) {
        e.preventDefault();
        
        //let Id = $("#for-reset-id").val()
        //let Password = $("#Password").val()
        //let ConfirmPassword = $("#ConfirmPassword").val()

        let Id = $("#for-reset-id").val()
        let Token = $("#for-reset-token").val()
        let Password = $("#Password").val()
        let ConfirmPassword = $("#ConfirmPassword").val()
        

        let url = "/account/NewPasswordPost";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(
                {
                    Id: Id,
                    Password: Password,
                    ConfirmPassword: ConfirmPassword,
                    Token:Token,

                })
        })
            .then(res => res.json())
            .then(data => {
                console.log(data)
                if (data.status == 400) {
                    $('.styles_errorText').text(data.message)
                    return;
                } else if (data.status == 200) {
                    $('.styles_errorText').text("")
                    $('.styles_succesText').text(data.message)
                    $(".form-fix").html("")

                    let url = "/Account/Login/"
                    fetch(url
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".form-fix").html(data)
                        /* window.location.pathname = "/home/Index/";*/
                        

                    })

                }



            })




    })


    //Basket
    $(document).on("click", ".shopping-cart-mini", function (e) {
        e.preventDefault();
        $(".overlay-right").css("top", 0);
        $(".search-section").addClass("d-none");
        $(".overlay").removeClass("active");
        $(".overlay").removeClass("for-search");
        $(".user-arrowDownSVG").removeClass("active")
        $(".side-right").addClass("active")
        $(".overlay-right").addClass("active")
        $(".user-info").addClass("d-none")
        $("body").addClass("disabled")

        $(".MainCtg").html(" ")
        $(".SubCtg").html(" ")
        $(".BrandList").html(" ")
        $(".overlay").removeClass("active")
        $("#pro-search-input").val("")

        $("#Menubtn").children().first().removeClass("selected")
        $(".sidebarHome").css("z-index", "9999");

        let url = "/Basket/GetMiniBasket/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)
            //$(".side-right-auth-tab-login-btn").click();

        })



    })
  
    $(document).on("click", ".DeleteFromCartBtn", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $(".dinamic-section").html(data)
           
            let url2 = "/basket/GetBasketCount"
            fetch(url2)
                .then(res => res.json())
                .then(data => {
                    console.log(data)
                    if (data.status == 200) {
                        $('.basketCount').text(data.message)

                        let url3 = "/basket/GetOrderBasket"
                        fetch(url3)
                            .then(resp => resp.text())
                            .then(data2 => {

                                $(".styles_checkoutSection-right").html(data2)
                               
                                let url4 = "/account/myaccount/"
                                fetch(url4 + "?from=" +"compare"
                                ).then(res => {
                                    return res.text()
                                }).then(data => {
                                    $(".my-account-content-right-content").html(data)

                                })


                            })
                    }

                    


                })


        })
    })
    $(document).on("click", ".OneEditFromCartBtn", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $(".dinamic-section").html(data)
            let url2 = "/basket/GetBasketCount"
            fetch(url2)
                .then(res => res.json())
                .then(data => {
                    console.log(data)
                    if (data.status == 200) {
                        $('.basketCount').text(data.message)
                       

                        let url3 = "/basket/GetOrderBasket"
                        fetch(url3)
                            .then(resp => resp.text())
                            .then(data2 => {

                                $(".styles_checkoutSection-right").html(data2)

                                let url4 = "/account/myaccount/"
                                fetch(url4 + "?from=" + "compare"
                                ).then(res => {
                                    return res.text()
                                }).then(data => {
                                    $(".my-account-content-right-content").html(data)

                                })
                            })
                    }

                })


        })
    })

    $(document).on("click", ".cartButton-checkout", function (e) {
        e.preventDefault()

        let user = $("#useridentity").val();
        let url = $(this).attr("href")
        if (user=="true") {
            window.location.pathname = url;
        }
        else {

            let url2 = "/Account/LoginReg/"
            fetch(url2
            ).then(res => {
                return res.text()
            }).then(data => {
                $(".dinamic-section").html(data)
                //$(".side-right-auth-tab-login-btn").click();

            })

        }
        
       


    })

    //order
    $(document).on("click", ".order-submit", function (e) {
        e.preventDefault();
        let Address = $("#Address").val();
        let City = $("#City").val();
        let ZipCode = $("#ZipCode").val();
        let Country = $("#Country").val();
        let State = $("#State").val();
        





        let url = "/order/CreateOrder";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                Address: Address,
                City: City,
                ZipCode: ZipCode,
                Country: Country,
                State: State,

            })
        })
            .then(res => res.json())
            .then(data => {

                if (data.status == 400) {
                    $('.validation-summary-form').text(data.message)
                    return;
                } else if (data.status == 200) {
                    
                    let url3 = "/basket/GetOrderBasket"
                    fetch(url3)
                        .then(resp => resp.text())
                        .then(data2 => {

                            $(".styles_checkoutSection-right").html(data2)

                            $(".order-succes").text("Sifarişiniz uğurla rəsmiləşdirildi");
                            $(".cart-empty").text("");
                            $(".basketCount").text("0");
                            $(".ordershow").removeClass("d-none");
                            $(".order-submit").addClass("d-none");

                        })
                    


                }



            })




    })

    $(document).on("mouseover", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        $(".Order-list-body").addClass("hover")
        
    })
    $(document).on("mouseout", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        $(".Order-list-body").removeClass("hover")

    })

    $(document).on("click", ".styles-order-view-btn", function (e) {
        e.preventDefault();
        $(".content-edit").addClass("active");
        $(".overlay-right").addClass("active");
        $(".overlay-right").addClass("content");
        $("body").addClass("disabled")
        let orderId = $(this).attr("data-orderId");



        let url = "/Order/GetMyOrder"
        fetch(url +"?orderId="+orderId
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".content-edit").html(data)
        })



    })

    //search

    $(document).on("keyup", "#pro-search-input", function (e) {
        e.preventDefault();

        let search =$(this).val()
        if (search.length > 0) {

            $(".MainCtg").html(" ")
            $(".SubCtg").html(" ")
            $(".BrandList").html(" ")
            $(".overlay").removeClass("active")
            $("#Menubtn").children().first().removeClass("selected")
            $(".sidebarHome").css("z-index", "9999");
            $("body").removeClass("disabled")
            $(".side-right").removeClass("active");
            $(".overlay-right").removeClass("active")
            $(".search-section").removeClass("d-none");
            let url = "/Product/SearchProduct"
            fetch(url + "?search=" + search
            ).then(res => {
                return res.text()
            }).then(data => {
                $(".search-section").html(data)
            }).then($(".overlay").addClass("active")).then($(".overlay").addClass("for-search"))
            
        }
        else {
            $(".search-section").addClass("d-none");
            $(".overlay").removeClass("active");
            $(".overlay").removeClass("for-search");
        }

       



    })
    $(document).on("keyup", "#mob-pro-search-input", function (e) {
        e.preventDefault();

        let search = $(this).val()
        if (search.length > 0) {



            
           
            

            $(".side-right").addClass("active")
            $(".user-info").addClass("d-none")
            $(".logout-section").addClass("d-none")
            let url = "/Product/SearchProduct"
            fetch(url + "?search=" + search
            ).then(res => {
                return res.text()
            }).then(data => {
                $(".dinamic-section").html(data)
                $(".side-right").css("border-top", "1px solid")
                $("body").addClass("disabled")

            })

        }
        else {
            $(".side-right").removeClass("active")
            $(".logout-section").removeClass("d-none")
            $("body").removeClass("disabled")
        }





    })

    //wishlist

    $(document).on("click", ".add-wish-list", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {

            let url2 = "/Wish/GetWishCount"
            fetch(url2)
                .then(res => res.json())
                .then(data => {
                    console.log(data)
                    if (data.status == 200) {
                        $('.wishCount').text(data.message)
                        return;
                    }

                })


        })
    })
    $(document).on("click", ".wish-cart-mini", function (e) {
        e.preventDefault();
        $(".overlay-right").css("top", 0);
        $(".search-section").addClass("d-none");
        $(".overlay").removeClass("active");
        $(".overlay").removeClass("for-search");
        $(".user-arrowDownSVG").removeClass("active")
        $(".side-right").addClass("active")
        $(".overlay-right").addClass("active")
        $(".user-info").addClass("d-none")
        $("body").addClass("disabled")

        $(".MainCtg").html(" ")
        $(".SubCtg").html(" ")
        $(".BrandList").html(" ")
        $(".overlay").removeClass("active")
        $("#pro-search-input").val("")

        $("#Menubtn").children().first().removeClass("selected")
        $(".sidebarHome").css("z-index", "9999");

        let url = "/Wish/GetMiniWish/"
        fetch(url
        ).then(res => {
            return res.text()
        }).then(data => {
            $(".dinamic-section").html(data)
            //$(".side-right-auth-tab-login-btn").click();

        })



    })
    $(document).on("click", ".DeleteFromWishtBtn", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $(".dinamic-section").html(data)

            let url2 = "/Wish/GetWishCount"
            fetch(url2)
                .then(res => res.json())
                .then(data => {
                    console.log(data)
                    if (data.status == 200) {
                        $('.wishCount').text(data.message)


                        let url4 = "/account/myaccount/"
                        fetch(url4 + "?from=" + "favorite"
                        ).then(res => {
                            return res.text()
                        }).then(data => {
                            $(".my-account-content-right-content").html(data)

                        })


                       
                    }




                })


        })
    })

    $(document).on("click", ".AddToCartBtn", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            let url2 = "/basket/GetBasketCount"
            fetch(url2)
                .then(res => res.json())
                .then(data => {
                    console.log(data)
                    if (data.status == 200) {
                        $('.basketCount').text(data.message)

                        let url3 = "/wish/DeleteWish"
                        let id = $(this).attr("data-id")

                        fetch(url3+"?id="+id).then(response => {
                            return response.text();
                        }).then(data => {
                            $(".dinamic-section").html(data)



                            let url2 = "/Wish/GetWishCount"
                            fetch(url2)
                                .then(res => res.json())
                                .then(data => {
                                    console.log(data)
                                    if (data.status == 200) {
                                        $('.wishCount').text(data.message)

                                        let url4 = "/account/myaccount/"
                                        fetch(url4 + "?from=" + "favorite"
                                        ).then(res => {
                                            return res.text()
                                        }).then(data => {
                                            $(".my-account-content-right-content").html(data)

                                        })
                                    }




                                })


                        })

                    }

                })


        })
    })

    //reviews
    $(document).on("click", ".rew-write-get-btn", function (e) {
        e.preventDefault();
       




        let url = "/Product/CheckUserForReviews/"
        fetch(url
        ).then(res => {
            return res.json()
        }).then(data => {

            if (data.status == 200) {
                
                let url = "/Product/GetFormReviews/"
                fetch(url
                ).then(res => {
                    return res.text()
                }).then(data => {
                    $(".product-rews-write-form").html(data)
                    $(this).removeClass("rew-write-get-btn");
                    $(this).addClass("rew-write-post-btn");
                })
            }
            else {
                $(".search-section").addClass("d-none");
                $(".overlay").removeClass("active");
                $(".overlay").removeClass("for-search");
                $(".side-right").addClass("active")
                $(".overlay-right").addClass("active")
                $(".user-info").addClass("d-none")
                $("body").addClass("disabled")
                $("#pro-search-input").val("")

                $(".MainCtg").html(" ")
                $(".SubCtg").html(" ")
                $(".BrandList").html(" ")
                $(".overlay").removeClass("active")
                $("#Menubtn").children().first().removeClass("selected")
                $(".sidebarHome").css("z-index", "9999");

                let url = "/Account/LoginReg/"
                fetch(url
                ).then(res => {
                    return res.text()
                }).then(data => {
                    $(".dinamic-section").html(data)
                    //$(".side-right-auth-tab-login-btn").click();

                })
            }

        })



    })

    $(document).on("mouseover", ".rev-star-btn", function (e) {
        e.preventDefault();
        
        $(this).children().children().addClass("active")
        $(this).prevUntil().children().children().addClass("active");
        let val = $(this).attr("data-rate");
        $(".rated-star").text(val);
    })
    $(document).on("mouseleave", ".rev-star-btn", function (e) {
        e.preventDefault();
        $(this).children().children().removeClass("active")
        $(this).prevUntil().children().children().removeClass("active");
        let val = $("#star-val").val()
        $(".rated-star").text(val);

        
        

       
       
        
        

    })
    $(document).on("click", ".rev-star-btn", function (e) {
        e.preventDefault();
        $(".rev-star-btn").children().children().removeClass("active-post")
        $(this).children().children().addClass("active-post")
        $(this).prevUntil().children().children().addClass("active-post");
        let val = $(this).attr("data-rate");
         $("#star-val").val(val);
        

    })

    $(document).on("click", ".rew-write-post-btn", function (e) {
        e.preventDefault();
        let star = $("#star-val").val();
        let message = $(".styles_textarea").val();
        let productId = $("#proId").val();
        productId = Number(productId);
        star = Number(star);
 


        let url = "/Product/AddReviews";
        fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({
                Star: star,
                Message: message,
                ProductId: productId,

            })
        })
            .then(res => res.json())
            .then(data => {

                if (data.status == 400) {

                    let url2 = '/product/GetProDetail/';

                    fetch(url2+"?id="+productId+"&key="+"rew"
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".section-pro-content").html(data)


                    })
                    


                    return;
                } else if (data.status == 200) {
                    
                    $(".rew-validation-summary").html(data.message);

                }



            })


    })

    $(document).on("click", ".rew-delete", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.json();
        }).then(data => {
            if (data.status == 200) {
               
                let productId = $("#proId").val();
                productId = Number(productId);
                let url2 = '/product/GetProDetail/';

                fetch(url2 + "?id=" + productId + "&key=" + "rew"
                ).then(res => {
                    return res.text()
                }).then(data => {
                    $(".section-pro-content").html(data)

                    let url4 = "/account/myaccount/"
                    fetch(url4 + "?from=" + "comment"
                    ).then(res => {
                        return res.text()
                    }).then(data => {
                        $(".my-account-content-right-content").html(data)

                    })
                })
            }
            else {
                let productId = $("#proId").val();
                productId = Number(productId);
                let url2 = '/product/GetProDetail/';

                fetch(url2 + "?id=" + productId + "&key=" + "rew"
                ).then(res => {
                    return res.text()
                }).then(data => {
                    $(".section-pro-content").html(data)


                })
            }
           
            


        })
    })

    //ctg list
    $(document).on("mouseover", ".styles_arrowWrapper", function (e) {
        e.preventDefault();
        $(this).next().addClass("active");
    })
    $(document).on("mouseleave", ".styles_productsPopup", function (e) {
        e.preventDefault();
        $(this).removeClass("active");
    })


    //add like

    $(document).on("click", ".add-like-list", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {

        })
    })
    $(document).on("click", ".add-Dislike-list", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {

        })
    })
    $(document).on("click", ".deleteFromDislike", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $(".dislikebtn").click();
        })
    })
    $(document).on("click", ".deleteFromlike", function (e) {
        e.preventDefault()


        let url = $(this).attr("formaction")


        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $(".likebtn").click();
        })
    })
})


//for mobil logo scrol hidden
window.onscroll = function() {myFunction()};
    
    var navbar = document.getElementById("navbar");
    var sticky = navbar.offsetTop;
    
    function myFunction() {
      if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
      } else {
        navbar.classList.remove("sticky");
      }
    }


