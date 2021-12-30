
async function getProduct() {
    var productPageUrl = new URL(window.location.href);
    var productId = productPageUrl.searchParams.get("productId");
    console.log(productId);

    let apiUrl = 'http://localhost:61955/api/product/getProductById/' + productId;
    try {
        let res = await fetch(apiUrl);
        return await res.json();
    } catch (error) {
        console.log(error);
    }
}
async function renderProduct() {
    let product = await getProduct();
   /* let html = '';
    users.forEach(user => {
        let htmlSegment = `<div class="user">
                            <img src="${user.profileURL}" >
                            <h2>${user.firstName} ${user.lastName}</h2>
                            <div class="email"><a href="email:${user.email}">${user.email}</a></div>
                        </div>`;

        html += htmlSegment;
    });*/
    var productImagesUrl = product.productId["productImageUrls"].split('`');

    document.getElementById("priceContent").innerHTML = "EGP " + product.productId["productPrice"];
    document.getElementById("productMainImage").style.backgroundImage = "url('http://localhost:61955/images/" + productImagesUrl[0] + "')";
    var productRestOfImagesContainer = document.getElementById("productRestOfImagesContainer");

    if (productImagesUrl.length > 2) {
        var count = 0;
        for (var i = 1; count < 4; i++)
        {
            count++;
            if (count > productImagesUrl.length - 3) {
                count = 4;
            }
            var div1 = document.createElement("div");
            div1.classList.add("col-sm-2", "col-3");

            var div2 = document.createElement("div");

            div2.classList.add("img-small", "border");

            div2.style.backgroundImage = "url('http://localhost:61955/images/" + productImagesUrl[i] + "')";

            div1.appendChild(div2);

            productRestOfImagesContainer.appendChild(div1);




        }
        document.getElementById("productBrand").innerHTML = product.productId["productBrand"];
        document.getElementById("productSeller").innerHTML = product.sellerId["firstName"] + " " + product.sellerId["lastName"] ;

        document.getElementById("productDescription").innerHTML = product.productId["productDescription"];

        
    }




   /* let container = document.querySelector('.container');
    container.innerHTML = html;*/
}

renderProduct();