# EduHub — Quản Lý Câu Lạc Bộ & Sự Kiện (Chương 1)

Dự án bài tập môn **Phát triển phần mềm hướng đối tượng (IN4211)** — Trường Đại học Đồng Tháp.

---

## 1. Mô tả thiết kế Domain

Hệ thống gồm 5 thực thể cốt lõi:

- **`Club`**: Quản lý thông tin câu lạc bộ và danh sách thành viên nội bộ qua phương thức `AddMember()` (chặn trùng MSSV).
- **`Member`**: Lưu trữ thông tin sinh viên (Họ tên, Email, MSSV) và trạng thái hoạt động (`Active` / `Inactive`).
- **`Event`**: Sự kiện trực thuộc một CLB (`ClubId`), quản lý số lượng sức chứa và hàm kiểm tra còn chỗ `HasCapacity()`.
- **`Registration`**: Phiếu đăng ký tham dự (bảng nối N-N giữa `Member` và `Event`), có trạng thái `Pending`, `Confirmed`, `Cancelled`.
- **`CheckIn`**: Bản ghi điểm danh thực tế (quan hệ 1 - 0..1 với `Registration`), hỗ trợ phương thức quét `QR` hoặc thủ công `Manual`.

### Các nguyên tắc OOP áp dụng:
- **Đóng gói (Encapsulation):** Tất cả properties dùng `{ get; private set; }`, constructor để `private`.
- **Factory Method:** Khởi tạo đối tượng qua phương thức tĩnh `Create(...)` có kiểm tra hợp lệ dữ liệu (tên trống, email thiếu `@`, sức chứa $\le 0$).
- **An toàn kiểu dữ liệu:** Dùng enum `MemberStatus`, `RegistrationStatus` và hằng số `CheckInMethod` thay vì chuỗi tự do.
- **Sơ đồ lớp:** Xem chi tiết tại [`docs/class-diagram.md`](docs/class-diagram.md) hoặc mở tệp vẽ [`docs/class-diagram.drawio`](docs/class-diagram.drawio).

---

## 2. Hướng dẫn chạy ứng dụng

### Yêu cầu:
- Máy đã cài **.NET 10 SDK** (hoặc .NET 8 trở lên).

### Lệnh chạy:
```bash
cd EduHub.Ch01
dotnet run
```

Chương trình cho phép nhập trực tiếp thông tin từ bàn phím (hoặc nhấn **Enter** để dùng giá trị mẫu có sẵn).

---

## 3. Kết quả chạy thực tế (Console Output)

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
