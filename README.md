# EduHub — Quản Lý Câu Lạc Bộ & Sự Kiện (Ch01)

Dự án bài tập môn **Phát triển phần mềm hướng đối tượng (IN4211)** — Trường Đại học Đồng Tháp.

---

## 1. Mô tả thiết kế Domain

Hệ thống mô hình hóa 5 thực thể cốt lõi:

- **`Club`**: Quản lý câu lạc bộ và danh sách thành viên nội bộ qua `AddMember()` (kiểm tra không trùng MSSV).
- **`Member`**: Lưu thông tin sinh viên (Họ tên, Email, MSSV) và trạng thái hoạt động (`Active` / `Inactive`).
- **`Event`**: Sự kiện trực thuộc một CLB (`ClubId`), quản lý sức chứa tối đa và hàm kiểm tra còn chỗ `HasCapacity()`.
- **`Registration`**: Phiếu đăng ký tham dự (bảng nối N-N giữa `Member` và `Event`), có trạng thái `Pending`, `Confirmed`, `Cancelled`.
- **`CheckIn`**: Bản ghi điểm danh tại sự kiện (quan hệ 1 - 0..1 với `Registration`), hỗ trợ hình thức `QR` và `Manual`.

> **Nguyên tắc OOP:** Thuộc tính dùng `private set`, constructor `private`, khởi tạo qua Static Factory `Create(...)` có kiểm tra tính hợp lệ dữ liệu. Sơ đồ lớp chi tiết xem tại [`docs/class-diagram.md`](docs/class-diagram.md).

---

## 2. Cách chạy ứng dụng

Mở terminal tại thư mục dự án và chạy lệnh:
```bash
dotnet run
```
Chương trình cho phép nhập trực tiếp thông tin từ bàn phím (hoặc nhấn **Enter** để chọn nhanh giá trị mẫu).

---

## 3. Kết quả chạy thực tế

```text
==========================================================
      EDUHUB - HỆ THỐNG QUẢN LÝ CÂU LẠC BỘ & SỰ KIỆN     
==========================================================

--- [BƯỚC 1] NHẬP THÔNG TIN CÂU LẠC BỘ ---
Nhập tên CLB: Tech Club
Nhập mô tả CLB: CLB Lập trình & Đổi mới sáng tạo - DThU
-> Đã tạo CLB: Tech Club (Mã: 3fad9e47-4a44-40ce-a308-c5fd2c77731e)

--- [BƯỚC 2] NHẬP THÀNH VIÊN VÀO CLB ---
Nhập số lượng thành viên muốn thêm: 2

Thành viên #1:
  - Họ và tên: Nguyễn Văn An
  - Email: an.nguyen@dthu.edu.vn
  - Mã số sinh viên (MSSV): DH24001
  -> Đã thêm thành công: Nguyễn Văn An (DH24001)

Thành viên #2:
  - Họ và tên: Trần Thị Bình
  - Email: binh.tran@dthu.edu.vn
  - Mã số sinh viên (MSSV): DH24002
  -> Đã thêm thành công: Trần Thị Bình (DH24002)

--- [BƯỚC 3] TẠO SỰ KIỆN MỚI CHO CLB ---
Nhập tên sự kiện: Workshop Lập trình C# hướng đối tượng
Nhập sức chứa tối đa (Capacity): 50
-> Đã tạo sự kiện: Workshop Lập trình C# hướng đối tượng | Sức chứa: 50 chỗ

--- [BƯỚC 4] ĐĂNG KÝ THAM GIA SỰ KIỆN ---
-> Sinh viên Nguyễn Văn An (DH24001) đã đăng ký thành công [Trạng thái: Confirmed]
-> Sinh viên Trần Thị Bình (DH24002) đã đăng ký thành công [Trạng thái: Confirmed]

--- [BƯỚC 5] ĐIỂM DANH SỰ KIỆN (CHECK-IN) ---
Tiến hành điểm danh cho: Nguyễn Văn An (DH24001)
Chọn phương thức (1. Quét mã QR, 2. Thủ công) [Mặc định 1]: 1
-> [CheckIn #c11d01ed] Lúc 16:39:52 12/10/2026 qua [QR] (Phiếu #72a2a9e2)

==========================================================
   BÁO CÁO THAM DỰ SỰ KIỆN: WORKSHOP LẬP TRÌNH C# HƯỚNG ĐỐI TƯỢNG
   Đơn vị: Tech Club | Sức chứa: 50 chỗ
   Số đăng ký: 2 | Đã check-in: 1
==========================================================
MSSV       | Họ tên             | Trạng thái   | Điểm danh
----------------------------------------------------------
DH24001    | Nguyễn Văn An      | Confirmed    | Đã check-in (QR)
DH24002    | Trần Thị Bình      | Confirmed    | Chưa check-in
==========================================================
Chương trình hoàn tất!
```
