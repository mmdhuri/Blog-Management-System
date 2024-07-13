function onClickDelete(element) {
    var Id = $(element).closest("table").attr("id");
    console.log(Id);
    var url = $(element).attr("data-url");
    Swal.fire({
        title: "Do you want to delete?",
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: "Delete",
        denyButtonText: `Don't save`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type:'POST',
                success: function (result) {
                    if (result.response) {
                        Swal.fire({
                            title: "Deleted Successfully",
                            icon: "success",
                            showCancelButton: false,
                            confirmButtonText: "OK",
                            backdrop: false,
                        }).then((result) => {
                            location.reload(true);
                        })
                    }
                }
            });
        } else if (result.isDenied) {
            Swal.fire("Changes are not saved", "", "info");
        }
    });
}

function onFavouriteClick(element, blogFavouriteId, action) {
    var isUserAuthenticated = $(element).attr("data-userauthenticated");
    var blogId = $(element).attr("data-blogid");
    if (isUserAuthenticated == "True") {
        var isFavourite = $(element).attr("data-isfavourite") === "True";
        var isLike = $(element).attr("data-islike") === "True";

        var Favourite = action == "Favourite" ? !isFavourite : isFavourite;
        var Like = action == "Like" ? !isLike : isLike;

        if (action == "Favourite")
            $(element).attr("data-isfavourite", Favourite ? "True" : "False");
        if (action == "Like")
            $(element).attr("data-islike", Like ? "True" : "False");

        var container = $(element).find('.love-button-container');
        var containerLove = $(element).find('.like-button-container');

        for (var i = 0; i < 5; i++) {
            var anim
            if (action == "Favourite") {
                anim = $('<div class="like-animation"></div>').html(`
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="${Favourite ? 'red' : 'none'}" class="${Favourite ? 'blog-favourite' : ''}" stroke="#000" stroke-width="2" stroke-linecap="round">
                                <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path>
                            </svg>
                        `);
                container.append(anim);
            }
            if (action == "Like") {
                anim = $('<div class="like-animation"></div>').html(`
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="${Like ? 'red' : 'none'}" class="${Like ? 'blog-like' : ''}" stroke="#000" stroke-width="2" stroke-linecap="round">
                               <path d="M8 10V20M8 10L4 9.99998V20L8 20M8 10L13.1956 3.93847C13.6886 3.3633 14.4642 3.11604 15.1992 3.29977L15.2467 3.31166C16.5885 3.64711 17.1929 5.21057 16.4258 6.36135L14 9.99998H18.5604C19.8225 9.99998 20.7691 11.1546 20.5216 12.3922L19.3216 18.3922C19.1346 19.3271 18.3138 20 17.3604 20L8 20" stroke="#000000" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                            </svg>
                        `);
                containerLove.append(anim);
            }

            // Random position for animation
            var x = (Math.random() - 0.5) * 200;
            var y = (Math.random() - 0.5) * 200;
            anim.css({
                '--x': x + 'px',
                '--y': y + 'px'
            });

            // Remove the animation element after the animation ends
            anim.on('animationend', function () {
                $(this).remove();
            });
        }

        $.ajax({
            cache: false,
            url: '/BlogPost/AddBlogIntoFavourite?BlogFavouriteId=' + blogFavouriteId + '&BlogId=' + blogId + "&IsFavourite=" + Favourite + "&IsLike=" + Like + "&ButtonAction=" + action,
            type: 'POST',
            success: function (response) {
                if (response.response == true) {
                    console.log(response);
                    if (response.islike == true) {
                        $(element).find(".like-btn-icon").addClass("blog-like");
                    } else {
                        $(element).find(".like-btn-icon").removeClass("blog-like");
                    }

                    if (response.isfavourite == true) {
                        $(element).find(".fav-btn-icon").addClass("blog-favourite");
                    } else {
                        $(element).find(".fav-btn-icon").removeClass("blog-favourite");
                    }

                    $(element).closest(".icon-wrapper").find(".like-count").text(response.likeCount);
                    $(element).closest(".icon-wrapper").find(".favourite-count").text(response.favouriteCount);
                    //$(".button-wrapper").load(location.href + " .favourite-count");
                    //$(".button-wrapper").load(location.href + " .like-count ");
                    setTimeout(function () {
                        //$(".blogWrapper").load(location.href + " .blogWrapper");
                        location.reload();
                    }, 500);
                }
            },
            error: function (error) {
                toastr.error("Something Went Wrong.....");
            }
        });
    } else {
        Swal.fire({
            title: "Please Login?",
            text: "If you want to add blog into your favourite list. Please login or Register",
            icon: "warning",
            showCancelButton: true,
        })
    }
}