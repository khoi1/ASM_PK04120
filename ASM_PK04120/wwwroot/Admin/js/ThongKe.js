// --- DỮ LIỆU MẪU ---
var topProductsData = {
    week: [
        { name: 'iPhone 14 Pro', quantity: 45, revenue: 540000000 },
        { name: 'Samsung Galaxy S23', quantity: 38, revenue: 380000000 },
        { name: 'MacBook Air M2', quantity: 25, revenue: 625000000 },
        { name: 'iPad Pro 11"', quantity: 32, revenue: 288000000 },
        { name: 'AirPods Pro 2', quantity: 67, revenue: 167500000 }
    ],
    month: [
        { name: 'iPhone 14 Pro', quantity: 185, revenue: 2220000000 },
        { name: 'Samsung Galaxy S23', quantity: 156, revenue: 1560000000 },
        { name: 'MacBook Air M2', quantity: 98, revenue: 2450000000 },
        { name: 'iPad Pro 11"', quantity: 142, revenue: 1278000000 },
        { name: 'AirPods Pro 2', quantity: 278, revenue: 695000000 }
    ],
    year: [
        { name: 'iPhone 14 Pro', quantity: 2145, revenue: 25740000000 },
        { name: 'Samsung Galaxy S23', quantity: 1823, revenue: 18230000000 },
        { name: 'MacBook Air M2', quantity: 1156, revenue: 28900000000 },
        { name: 'iPad Pro 11"', quantity: 1687, revenue: 15183000000 },
        { name: 'AirPods Pro 2', quantity: 3421, revenue: 8552500000 }
    ]
};

var categoryData = {
    revenue: [
        { name: 'Điện thoại', value: 5450000000 },
        { name: 'Laptop', value: 3250000000 },
        { name: 'Máy tính bảng', value: 1850000000 },
        { name: 'Phụ kiện', value: 1100000000 },
        { name: 'Thiết bị khác', value: 550000000 }
    ],
    quantity: [
        { name: 'Điện thoại', value: 450 },
        { name: 'Laptop', value: 120 },
        { name: 'Máy tính bảng', value: 210 },
        { name: 'Phụ kiện', value: 850 },
        { name: 'Thiết bị khác', value: 95 }
    ]
};

var chartColors = [
    'rgba(255, 99, 132, 0.8)',
    'rgba(54, 162, 235, 0.8)',
    'rgba(255, 206, 86, 0.8)',
    'rgba(75, 192, 192, 0.8)',
    'rgba(153, 102, 255, 0.8)',
    'rgba(255, 159, 64, 0.7)'
];

// Biến toàn cục cho các biểu đồ
var topProductsChart, categoryPieChart, overviewChart;

// --- CÁC HÀM TIỆN ÍCH ---
function formatCurrency(number) {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(number);
}

// --- HÀM CẬP NHẬT CHO CÁC BIỂU ĐỒ ---

// Hàm cập nhật cho biểu đồ tổng quan doanh thu (Ngày/Tháng/Năm)
function updateOverviewChart(period) {
    $('#overviewChart').parent().parent().find('.period-tabs .btn').removeClass('active');
    if (period === 'day') $('#btn-day').addClass('active');
    if (period === 'month') $('#btn-month-overview').addClass('active');
    if (period === 'year') $('#btn-year-overview').addClass('active');
    if (overviewChart) {
        overviewChart.destroy();
    }
    var ctx = document.getElementById('overviewChart').getContext('2d');
    var config = {};
    switch (period) {
        case 'day':
            var trendData = [];
            var trendLabels = [];
            for (var i = 29; i >= 0; i--) {
                var date = new Date();
                date.setDate(date.getDate() - i);
                trendLabels.push(date.getDate() + '/' + (date.getMonth() + 1));
                trendData.push(Math.floor(Math.random() * 5000000) + 5000000);
            }
            config = {
                type: 'line',
                data: { labels: trendLabels, datasets: [{ label: 'Doanh thu', data: trendData, backgroundColor: 'rgba(75, 192, 192, 0.2)', borderColor: 'rgba(75, 192, 192, 1)', borderWidth: 2, fill: true, tension: 0.4 }] },
                options: { responsive: true, scales: { yAxes: [{ ticks: { beginAtZero: false, callback: function (value) { return (value / 1000000).toFixed(1) + 'M'; } } }] }, legend: { display: false } }
            };
            break;
        case 'month':
            config = {
                type: 'bar',
                data: { labels: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'], datasets: [{ label: 'Doanh thu (triệu VNĐ)', data: [85, 92, 78, 105, 98, 115, 125.5, 130, 142, 155, 150, 165], backgroundColor: 'rgba(54, 162, 235, 0.7)', borderColor: 'rgba(54, 162, 235, 1)', borderWidth: 1 }] },
                options: { responsive: true, scales: { yAxes: [{ ticks: { beginAtZero: true, callback: function (value) { return value + 'M'; } } }] }, legend: { display: false } }
            };
            break;
        case 'year':
            config = {
                type: 'bar',
                data: { labels: ['2022', '2023', '2024', '2025 (dự kiến)'], datasets: [{ label: 'Doanh thu (tỷ VNĐ)', data: [12.5, 15.2, 18.1, 22.5], backgroundColor: 'rgba(153, 102, 255, 0.7)', borderColor: 'rgba(153, 102, 255, 1)', borderWidth: 1 }] },
                options: { responsive: true, scales: { yAxes: [{ ticks: { beginAtZero: true, callback: function (value) { return value + ' Tỷ'; } } }] }, legend: { display: false } }
            };
            break;
    }
    overviewChart = new Chart(ctx, config);
}

// Hàm cập nhật top sản phẩm
function updateTopProducts(period) {
    $('#topProductsChart').parent().parent().parent().parent().find('.period-tabs .btn').removeClass('active');
    $('#btn-' + period).addClass('active');
    var data = topProductsData[period];
    var tableBody = $('#topProductsTableBody');
    tableBody.empty();
    data.forEach(function (product, index) {
        var row = `<tr><td>${index + 1}</td><td>${product.name}</td><td>${product.quantity.toLocaleString('vi-VN')}</td><td>${formatCurrency(product.revenue)}</td></tr>`;
        tableBody.append(row);
    });
    topProductsChart.data.labels = data.map(p => p.name);
    topProductsChart.data.datasets[0].data = data.map(p => p.revenue);
    topProductsChart.update();
}

// --- Hàm cập nhật cho biểu đồ Danh mục ---
function updateCategoryChart(period) {
    $('#categoryPieChart').parent().parent().parent().parent().find('.period-tabs .btn').removeClass('active');
    if (period === 'revenue') $('#btn-cat-revenue').addClass('active');
    if (period === 'quantity') $('#btn-cat-quantity').addClass('active');

    var data = categoryData[period];
    var tableBody = $('#categoryTableBody');
    var tableHeader = $('#category-table-header-value');

    tableBody.empty();

    if (period === 'revenue') {
        tableHeader.text('Doanh Thu');
        data.forEach(function (cat, index) {
            var row = `<tr><td>${index + 1}</td><td>${cat.name}</td><td>${formatCurrency(cat.value)}</td></tr>`;
            tableBody.append(row);
        });
    } else {
        tableHeader.text('Số Lượng Bán');
        data.forEach(function (cat, index) {
            var row = `<tr><td>${index + 1}</td><td>${cat.name}</td><td>${cat.value.toLocaleString('vi-VN')}</td></tr>`;
            tableBody.append(row);
        });
    }

    categoryPieChart.data.labels = data.map(c => c.name);
    categoryPieChart.data.datasets[0].data = data.map(c => c.value);
    categoryPieChart.update();
}

// --- KHỞI TẠO CÁC BIỂU ĐỒ KHI TẢI TRANG ---
$(document).ready(function () {

    // 1. Biểu đồ danh mục (khởi tạo)
    var ctx3 = document.getElementById('categoryPieChart').getContext('2d');
    categoryPieChart = new Chart(ctx3, {
        type: 'doughnut',
        data: {
            labels: [], // Dữ liệu sẽ được điền bởi hàm update
            datasets: [{
                data: [],
                backgroundColor: chartColors
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                position: 'right'
            }
        }
    });

    // 2. Biểu đồ và bảng Top sản phẩm
    var ctx6 = document.getElementById('topProductsChart').getContext('2d');
    topProductsChart = new Chart(ctx6, {
        type: 'doughnut',
        data: { labels: [], datasets: [{ data: [], backgroundColor: chartColors }] },
        options: { responsive: true, maintainAspectRatio: false, legend: { position: 'bottom', labels: { boxWidth: 12 } }, cutoutPercentage: 40 }
    });

    // 3. Khởi tạo DataTable cho bảng doanh thu ngày
    var dailyRevenueTableBody = $('#dailyRevenueTable tbody');
    var dailyData = [
        ['20/07/2025', 12000000, 15, 800000, '<span class="badge badge-success">Doanh thu cao nhất tuần</span>'],
        ['19/07/2025', 8500000, 10, 850000, ''],
        ['18/07/2025', 9200000, 12, 766667, ''],
        ['17/07/2025', 7800000, 9, 866667, ''],
        ['16/07/2025', 10500000, 14, 750000, ''],
        ['15/07/2025', 6200000, 8, 775000, '<span class="badge badge-warning">Cuối tuần</span>'],
        ['14/07/2025', 5800000, 7, 828571, '<span class="badge badge-warning">Cuối tuần</span>']
    ];

    dailyData.forEach(function (row) {
        dailyRevenueTableBody.append(`<tr><td>${row[0]}</td><td>${formatCurrency(row[1])}</td><td>${row[2]}</td><td>${formatCurrency(row[3])}</td><td>${row[4]}</td></tr>`);
    });

    $('#dailyRevenueTable').DataTable({
        "order": [[0, "desc"]],
        "language": { "url": "//cdn.datatables.net/plug-ins/1.10.21/i18n/Vietnamese.json" }
    });

    // --- TẢI DỮ LIỆU MẶC ĐỊNH ---
    updateOverviewChart('day');
    updateTopProducts('week');
    updateCategoryChart('revenue');
});