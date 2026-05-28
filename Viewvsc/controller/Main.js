const sidebar = document.querySelector('.sidebar ul ');
const items = sidebar.querySelectorAll('li');

items.forEach(item => {
    item.addEventListener('click', function (e) {
        e.stopPropagation();
        items.forEach(li => li.classList.remove('active'));
        this.classList.add('active');
        if (this.dataset.hash) {
            window.location.hash = this.dataset.hash;
        }
    });
});

document.getElementById('avatar').onclick = function (event) {
    event.stopPropagation();
    var dropdownAvatar = document.getElementById('dropdownAvatar');
    var dropdownBell = document.getElementById('dropdownBell');

    dropdownBell.style.display = 'none';
    dropdownAvatar.style.display = dropdownAvatar.style.display === 'block' ? 'none' : 'block';
};

document.getElementById('bell').onclick = function (event) {
    event.stopPropagation();
    var dropdownBell = document.getElementById('dropdownBell');
    var dropdownAvatar = document.getElementById('dropdownAvatar');

    dropdownAvatar.style.display = 'none';
    dropdownBell.style.display = dropdownBell.style.display === 'block' ? 'none' : 'block';
};

document.addEventListener('click', function () {
    document.getElementById('dropdownAvatar').style.display = 'none';
    document.getElementById('dropdownBell').style.display = 'none';
});
app.controller("Home", function ($scope, $timeout) {
    $scope.$on('$viewContentLoaded', function () {
        document.querySelectorAll('.filter-btn').forEach(btn => {
            btn.addEventListener('click', function () {
                document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
                this.classList.add('active');
            });
        });

    });
    $scope.products1 = [
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Balen Grey 2023 - Tím - Bạc - Đế nhựa - Giày lưới - Balenciaga", qty: 18, price: "550,000 ₫", size: "40,41,42"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Converse Venom - Tím - Da - Đế nhôm - Giày tây - Converse", qty: 18, price: "950,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Balen Grey 2023 - Xanh dương - Bạc - Đế nhựa - Giày lưới - Balenciaga", qty: 15, price: "490,000 ₫", size: "40,42"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Xanh dương - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "100,000 ₫", size: "40"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},

    ];
    $scope.pageSizes1 = [5, 10];
    $scope.pageSize1 = 5;
    $scope.currentPage1 = 0;

    $scope.updatePaging1 = function() {
        $scope.totalPages1 = Math.ceil($scope.products1.length / $scope.pageSize1);
        $scope.pagedProducts1 = [];
        for (let i = 0; i < $scope.totalPages1; i++) {
            $scope.pagedProducts1[i] = $scope.products1.slice(i * $scope.pageSize1, (i + 1) * $scope.pageSize1);
        }
        if ($scope.currentPage1 >= $scope.totalPages1) $scope.currentPage1 = 0;
    };

    $scope.setPage1 = function(page) {
        if (page >= 0 && page < $scope.totalPages1) {
            $scope.currentPage1 = page;
        }
    };

    $scope.updatePaging1();

    $scope.products2 = [
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Balen Grey 2023 - Tím - Bạc - Đế nhựa - Giày lưới - Balenciaga", qty: 18, price: "550,000 ₫", size: "40,41,42"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Converse Venom - Tím - Da - Đế nhôm - Giày tây - Converse", qty: 18, price: "950,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Balen Grey 2023 - Xanh dương - Bạc - Đế nhựa - Giày lưới - Balenciaga", qty: 15, price: "490,000 ₫", size: "40,42"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Xanh dương - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "100,000 ₫", size: "40"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
        {img: "https://i.imgur.com/1Q9Z1Zm.png", name: "Kkkk - Tím - Sắt - Đế sắt - Giày cao cổ - Converse", qty: 9, price: "700,000 ₫", size: "41"},
    ];
    $scope.pageSizes2 = [5, 10];
    $scope.pageSize2 = 5;
    $scope.currentPage2 = 0;

    $scope.updatePaging2 = function() {
        $scope.totalPages2 = Math.ceil($scope.products2.length / $scope.pageSize2);
        $scope.pagedProducts2 = [];
        for (let i = 0; i < $scope.totalPages2; i++) {
            $scope.pagedProducts2[i] = $scope.products2.slice(i * $scope.pageSize2, (i + 1) * $scope.pageSize2);
        }
        if ($scope.currentPage2 >= $scope.totalPages2) $scope.currentPage2 = 0;
    };

    $scope.setPage2 = function(page) {
        if (page >= 0 && page < $scope.totalPages2) {
            $scope.currentPage2 = page;
        }
    };

    $scope.updatePaging2();

});