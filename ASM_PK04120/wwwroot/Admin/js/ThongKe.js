// Dữ liệu từ server được đẩy vào đây
var dashboardData = window.dashboardData || { /* ... */ };

// Các biến toàn cục cho biểu đồ để có thể cập nhật
var overviewChartInstance;
var topProductsChartInstance;
var categoryPieChartInstance;

var chartColors = [
    'rgba(54, 162, 235, 0.8)', 'rgba(255, 99, 132, 0.8)',
    'rgba(255, 206, 86, 0.8)', 'rgba(75, 192, 192, 0.8)',
    'rgba(153, 102, 255, 0.8)'
];

function formatCurrency(number) {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(number);
}

// === KHỞI TẠO DASHBOARD BAN ĐẦU ===
function initDashboard(data) {
    // Khởi tạo với dữ liệu mặc định
    updateOverviewChart('day');
    updateTopProducts('week');
    updateCategoryChart('revenue');
    initDailyRevenueTable(data.dailyRevenue);
}

// === BIỂU ĐỒ DOANH THU TỔNG QUAN ===
function updateOverviewChart(period) {
    var dataForPeriod = dashboardData.overview[period] || [];
    var labels = [];
    var values = [];

    // --- SỬA LOGIC LẤY NHÃN VÀ GIÁ TRỊ ---
    if (period === 'day') {
        labels = dataForPeriod.map(item => new Date(item.ngay).toLocaleDateString('vi-VN'));
        values = dataForPeriod.map(item => item.tongDoanhThu);
    } else { // period === 'month' hoặc 'year'
        labels = dataForPeriod.map(item => item.thoiGian); // Lấy trực tiếp "Tháng X" hoặc "YYYY"
        values = dataForPeriod.map(item => item.tongDoanhThu);
    }
    // ------------------------------------

    const chartData = {
        labels: labels,
        datasets: [{
            label: 'Doanh thu (VNĐ)',
            data: values,
            backgroundColor: 'rgba(54, 162, 235, 0.7)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
        }]
    };

    // Phần còn lại của hàm giữ nguyên (cập nhật hoặc tạo mới biểu đồ)
    if (overviewChartInstance) {
        overviewChartInstance.data = chartData;
        overviewChartInstance.update();
    } else {
        const ctx = document.getElementById('overviewChart').getContext('2d');
        overviewChartInstance = new Chart(ctx, { type: 'bar', data: chartData, options: { responsive: true, scales: { y: { beginAtZero: true } } } });
    }

    // Cập nhật nút active
    $('#btn-day-overview, #btn-month-overview, #btn-year-overview').removeClass('active');
    $(`#btn-${period}-overview`).addClass('active');
}

// === TOP SẢN PHẨM BÁN CHẠY ===
function updateTopProducts(period) {
    // period là 'week', 'month', hoặc 'year'
    var dataForPeriod = dashboardData.topProducts[period] || [];

    // Cập nhật biểu đồ tròn
    const chartData = {
        labels: dataForPeriod.map(p => p.tenSanPham),
        datasets: [{ data: dataForPeriod.map(p => p.doanhThu), backgroundColor: chartColors }]
    };

    if (topProductsChartInstance) {
        topProductsChartInstance.data = chartData;
        topProductsChartInstance.update();
    } else {
        const ctx = document.getElementById('topProductsChart').getContext('2d');
        topProductsChartInstance = new Chart(ctx, { type: 'doughnut', data: chartData, options: { responsive: true, legend: { position: 'bottom' } } });
    }

    // Cập nhật bảng dữ liệu
    const tbody = $('#topProductsTableBody');
    tbody.empty();
    if (dataForPeriod.length === 0) {
        tbody.append('<tr><td colspan="4" class="text-center">Không có dữ liệu cho khoảng thời gian này.</td></tr>');
    } else {
        dataForPeriod.forEach((p, i) => {
            tbody.append(`<tr>
                <td>${i + 1}</td>
                <td>${p.tenSanPham}</td>
                <td>${p.soLuongBan.toLocaleString('vi-VN')}</td>
                <td>${formatCurrency(p.doanhThu)}</td>
            </tr>`);
        });
    }
    // Cập nhật trạng thái active cho button
    $('#btn-week, #btn-month, #btn-year').removeClass('active');
    $(`#btn-${period}`).addClass('active');
}

// === THỐNG KÊ DANH MỤC ===
function updateCategoryChart(type) {
    // type là 'revenue' hoặc 'quantity'
    var dataForType = dashboardData.categories[type] || [];
    var isRevenue = type === 'revenue';

    // Cập nhật biểu đồ tròn
    const chartData = {
        labels: dataForType.map(c => c.tenDanhMuc),
        datasets: [{ data: dataForType.map(c => c.giaTri), backgroundColor: chartColors }]
    };
    if (categoryPieChartInstance) {
        categoryPieChartInstance.data = chartData;
        categoryPieChartInstance.update();
    } else {
        const ctx = document.getElementById('categoryPieChart').getContext('2d');
        categoryPieChartInstance = new Chart(ctx, { type: 'doughnut', data: chartData, options: { responsive: true, legend: { position: 'right' } } });
    }

    // Cập nhật bảng và header
    $('#category-table-header-value').text(isRevenue ? 'Doanh thu' : 'Số lượng bán');
    const tbody = $('#categoryTableBody');
    tbody.empty();
    dataForType.forEach((c, i) => {
        tbody.append(`<tr>
            <td>${i + 1}</td>
            <td>${c.tenDanhMuc}</td>
            <td>${isRevenue ? formatCurrency(c.giaTri) : c.giaTri.toLocaleString('vi-VN')}</td>
        </tr>`);
    });

    // Cập nhật trạng thái active cho button
    $('#btn-cat-revenue, #btn-cat-quantity').removeClass('active');
    $(`#btn-cat-${type}`).addClass('active');
}

// --- BẢNG DOANH THU NGÀY ---
function initDailyRevenueTable(dailyData) {
    const tbody = $('#dailyRevenueTable tbody');
    tbody.empty(); // Xóa dữ liệu cũ

    // 1. Đổ dữ liệu vào bảng
    dailyData.forEach(row => {
        const dateObj = new Date(row.ngay);
        // Định dạng ngày theo dd/MM/yyyy
        const formattedDate = dateObj.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });

        tbody.append(`<tr>
            <td>${formattedDate}</td>
            <td>${formatCurrency(row.tongDoanhThu)}</td>
            <td>${row.soDonHang}</td>
            <td>${formatCurrency(row.giaTriTrungBinh)}</td>
            <td>${row.ghiChu || ''}</td>
        </tr>`);
    });

    // 2. Dạy DataTables cách đọc định dạng ngày Việt Nam (CHỈ CẦN GỌI MỘT LẦN KHI KHỞI TẠO)
    // Kiểm tra xem DataTables đã được khởi tạo cho bảng này chưa
    if (!$.fn.DataTable.isDataTable('#dailyRevenueTable')) {
        // Chỉ cần gọi $.fn.dataTable.moment một lần duy nhất trước khi khởi tạo DataTable lần đầu
        $.fn.dataTable.moment('DD/MM/YYYY');

        // 3. Khởi tạo DataTables SAU KHI đổ dữ liệu
        $('#dailyRevenueTable').DataTable({
            order: [[0, 'desc']], // Sắp xếp theo cột Ngày (index 0) giảm dần
            language: { url: '//cdn.datatables.net/plug-ins/1.10.21/i18n/Vietnamese.json' }
            // Thêm các tùy chọn khác nếu bạn muốn (paging, searching...)
            // "paging": true,
            // "searching": true
        });
    } else {
        // Nếu DataTable đã tồn tại, chỉ cần vẽ lại bảng với dữ liệu mới
        $('#dailyRevenueTable').DataTable().clear().rows.add(tbody.find('tr')).draw();
        // Lưu ý: Đoạn else này thường không cần thiết vì chúng ta chỉ gọi init một lần.
        // Nhưng để đây phòng trường hợp bạn muốn cập nhật bảng sau này.
    }
}
// --- KHỞI TẠO ---
$(function () {
    initDashboard(dashboardData);
});
