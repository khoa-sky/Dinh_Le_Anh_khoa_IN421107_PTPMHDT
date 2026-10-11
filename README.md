# EDUHUB - HỆ THỐNG QUẢN LÝ CÂU LẠC BỘ & SỰ KIỆN SINH VIÊN
**Môn học:** IN4211 — Phát triển phần mềm hướng đối tượng  
**Giảng viên:** ThS. Nguyễn Tuấn Kiệt  
**Đơn vị:** Khoa Công nghệ và Kỹ thuật — Trường Đại học Đồng Tháp (DThU)  
**Bài tập:** CH01 — Phân tích yêu cầu và Domain Modeling EduHub  

---

## 1. Giới thiệu dự án
**EduHub** là dự án phần mềm xuyên suốt học kỳ của học phần **IN4211**, nhằm số hóa quy trình quản lý các Câu lạc bộ (CLB) sinh viên và các Sự kiện ngoại khóa tại Trường Đại học Đồng Tháp.

Trong **Chương 1**, dự án tập trung vào:
- Phân tích yêu cầu bài toán thực tế thành các **User Stories** chuẩn mực với các tiêu chí nghiệm thu (**Acceptance Criteria**) dạng `Given/When/Then`.
- Thiết kế **Sơ đồ lớp sơ bộ (Class Diagram)** mô hình hóa miền bài toán (Domain Model).
- Hiện thực hóa 5 lớp thực thể (Entities) bằng ngôn ngữ **C# trên nền tảng .NET 10**, tuân thủ nguyên tắc **Đóng gói (Encapsulation)**, bảo vệ toàn vẹn dữ liệu, áp dụng **Static Factory Method `Create()`**, che giấu constructor và không sử dụng public setter thô.
- Xây dựng ứng dụng **Console Demo** mô phỏng đầy đủ luồng nghiệp vụ từ khởi tạo CLB, thêm thành viên, tạo sự kiện, đăng ký, duyệt xác nhận, điểm danh (Check-in QR) đến xuất báo cáo và xử lý ngoại lệ nghiệp vụ.

---

## 2. Cấu trúc thư mục dự án
Tuân thủ chuẩn cấu trúc dự án quy định trong đề bài:
```text
EduHub.Ch01/
├── Domain/
│   ├── Member.cs         # Thực thể Sinh viên / Thành viên
│   ├── Club.cs           # Thực thể Câu lạc bộ
│   ├── Event.cs          # Thực thể Sự kiện của CLB
│   ├── Registration.cs   # Thực thể Phiếu đăng ký tham dự sự kiện
│   └── CheckIn.cs        # Thực thể Bản ghi điểm danh tại sự kiện
├── docs/
│   ├── user-stories.md   # Danh sách User Stories cho 3 Actor (Sinh viên, Ban CLB, Admin)
│   └── class-diagram.md  # Sơ đồ lớp Mermaid và phân tích khóa chính, khóa ngoại
├── Program.cs            # Kịch bản chạy Console demo đầy đủ luồng nghiệp vụ
├── EduHub.Ch01.csproj    # File cấu hình .NET 10 project (<Nullable>enable</Nullable>)
└── README.md             # Tài liệu giới thiệu, hướng dẫn sử dụng và kết quả chạy
```

---

## 3. Thiết kế kiến trúc Domain Model (Mô hình hóa nghiệp vụ)

### 3.1 Năm thực thể cốt lõi (Domain Entities)
1. **`Member` (Thành viên):**
   - *Thuộc tính:* `Id` (Guid - PK), `Name`, `Email`, `StudentCode` (MSSV), `JoinedAt`, `Status` (`MemberStatus`: Active/Inactive).
   - *Đặc điểm:* Constructor `private`. Phương thức tạo `Member.Create(...)` kiểm tra nghiêm ngặt họ tên không được rỗng, email phải chứa `@`, MSSV chuẩn hóa in hoa.
   - *Hành vi:* `IsActive()`, `Deactivate()`, `Activate()`.
2. **`Club` (Câu lạc bộ):**
   - *Thuộc tính:* `Id` (Guid - PK), `Name`, `Description`, `CreatedAt`, `Members` (`IReadOnlyList<Member>`).
   - *Đặc điểm:* Bảo vệ danh sách thành viên nội bộ `_members`, không cho phép bên ngoài tự do thêm/xóa.
   - *Hành vi:* `AddMember(Member member)` kiểm tra chặn trùng MSSV/Id và từ chối thành viên Inactive; `MemberCount()`, `HasMember(...)`.
3. **`Event` (Sự kiện):**
   - *Thuộc tính:* `Id` (Guid - PK), `ClubId` (Guid - FK liên kết Club), `Title`, `Date`, `Capacity`, `IsOnline`.
   - *Đặc điểm:* Sức chứa `Capacity` bắt buộc lớn hơn 0, `Title` không được để trống.
   - *Hành vi:* `HasCapacity(registeredCount)` kiểm tra còn chỗ để tiếp nhận đăng ký mới hay không.
4. **`Registration` (Phiếu đăng ký):**
   - *Thuộc tính:* `Id` (Guid - PK), `MemberId` (Guid - FK), `EventId` (Guid - FK), `RegisteredAt`, `Status` (`RegistrationStatus`: Pending, Confirmed, Cancelled).
   - *Vòng đời (Lifecycle):* Khởi tạo luôn ở trạng thái `Pending`.
   - *Hành vi:* `Confirm()` (chỉ duyệt khi đang Pending), `Cancel()` (huỷ phiếu khi Pending hoặc Confirmed, không cho phép xác nhận lại phiếu đã huỷ).
5. **`CheckIn` (Điểm danh):**
   - *Thuộc tính:* `Id` (Guid - PK), `RegistrationId` (Guid - FK), `CheckedInAt`, `Method` (`CheckInMethod`: QR hoặc Manual).
   - *Đặc điểm:* Tồn tại quan hệ `1 - 0..1` với Registration.
   - *Hành vi:* `CheckIn.Create(...)` bắt buộc phiếu đăng ký phải ở trạng thái `Confirmed` mới cho phép tạo lượt điểm danh; cung cấp phương thức `Summary()`.

---

## 4. Hướng dẫn cài đặt và chạy ứng dụng

### Yêu cầu hệ thống:
- Đã cài đặt **.NET 10 SDK** (kiểm tra bằng lệnh: `dotnet --version`).
- Trình soạn thảo: **Visual Studio Code**, **JetBrains Rider**, hoặc **Visual Studio 2022/2026**.

### Các bước thực hiện:
1. Mở cửa sổ dòng lệnh (Terminal / PowerShell).
2. Di chuyển vào thư mục dự án:
   ```bash
   cd EduHub.Ch01
   ```
3. Chạy ứng dụng bằng lệnh:
   ```bash
   dotnet run
   ```

---

## 5. Kết quả thực thi ứng dụng mẫu (Console Output)

```text
================================================================================
       TRƯỜNG ĐẠI HỌC ĐỒNG THÁP - KHOA CÔNG NGHỆ VÀ KỸ THUẬT
             IN4211: PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG
           BÀI TẬP CH01: DOMAIN MODELING & CONSOLE DEMO (EDUHUB)
================================================================================

>>> [BƯỚC 1] Khởi tạo Câu lạc bộ (Club)
[Thành công] Đã tạo CLB: Tech Club (Mã Id: 948ca24a-251c-43df-ae83-6a9cf186354f)
            Mô tả: CLB Lập trình & Đổi mới sáng tạo - Trường Đại học Đồng Tháp
--------------------------------------------------------------------------------

>>> [BƯỚC 2] Khởi tạo và thêm 3 thành viên (Member) vào CLB
[Thành công] Đã thêm 3 thành viên vào CLB. Tổng thành viên hiện tại: 3
  - Nguyễn Văn An | MSSV: DH24001 | Email: an.nguyen@dthu.edu.vn | Trạng thái: Active
  - Trần Thị Bình | MSSV: DH24002 | Email: binh.tran@dthu.edu.vn | Trạng thái: Active
  - Lê Hoàng Cường | MSSV: DH24003 | Email: cuong.le@dthu.edu.vn | Trạng thái: Active
--------------------------------------------------------------------------------

>>> [BƯỚC 3] Tạo sự kiện (Event) cho CLB
[Thành công] Đã tạo sự kiện: Workshop: Lập trình .NET 10 & Clean Architecture
            CLB tổ chức: Tech Club (ClubId: 948ca24a-251c-43df-ae83-6a9cf186354f)
            Thời gian: 18/10/2026 21:35 | Sức chứa: 50 chỗ
--------------------------------------------------------------------------------

>>> [BƯỚC 4] Đăng ký sự kiện (Registration) và duyệt xác nhận (Confirm)
[Tạo phiếu #1] Sinh viên Nguyễn Văn An gửi đăng ký -> Trạng thái ban đầu: Pending
              Ban tổ chức duyệt -> Trạng thái mới: Confirmed
[Tạo phiếu #2] Sinh viên Trần Thị Bình gửi đăng ký -> Trạng thái ban đầu: Pending
              Ban tổ chức duyệt -> Trạng thái mới: Confirmed
[Thống kê đăng ký] Đã xác nhận thành công 2 phiếu đăng ký.
--------------------------------------------------------------------------------

>>> [BƯỚC 5] Thực hiện Check-in điểm danh tại bàn tiếp đón
[Điểm danh thành công] Sinh viên: Nguyễn Văn An (DH24001)
                     [CheckIn #df12903b] Đã điểm danh lúc 21:35:12 11/10/2026 qua phương thức [QR] (Phiếu #7bc34e01)
--------------------------------------------------------------------------------

>>> [BƯỚC 6] Báo cáo tổng kết tham dự sự kiện
================================================================================
  BÁO CÁO THAM DỰ SỰ KIỆN: WORKSHOP: LẬP TRÌNH .NET 10 & CLEAN ARCHITECTURE
  Đơn vị tổ chức: Tech Club | Sức chứa: 50 sinh viên
  Tổng số đăng ký hợp lệ: 2 | Tổng số đã check-in: 1
================================================================================
MSSV         | Họ và Tên            | Trạng thái phiếu | Tình trạng điểm danh
--------------------------------------------------------------------------------
DH24001      | Nguyễn Văn An        | Confirmed       | [x] ĐÃ CHECK-IN (QR)
DH24002      | Trần Thị Bình        | Confirmed       | [ ] CHƯA CHECK-IN   
================================================================================

>>> [BƯỚC 7 & 8] Kiểm tra cơ chế tự bảo vệ luật nghiệp vụ (Encapsulation / Business Rules)

[Thử nghiệm 1] Cố tình thêm lại sinh viên đã có trong CLB (chặn trùng):
  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: Sinh viên 'Nguyễn Văn An' (MSSV: DH24001) đã là thành viên của CLB Tech Club.

[Thử nghiệm 2] Thử xác nhận lại một phiếu đăng ký đã bị huỷ (Cancelled):
  Tạo phiếu cho Lê Hoàng Cường -> Status: Pending
  Đã gọi Cancel() -> Status: Cancelled
  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: Không thể xác nhận phiếu đăng ký đang ở trạng thái Cancelled.

[Thử nghiệm 3] Cố tình check-in một phiếu đang ở trạng thái Pending:
  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: Chỉ phiếu đăng ký đã xác nhận (Confirmed) mới được phép check-in. Phiếu hiện tại đang ở trạng thái: Pending.

[Thử nghiệm 4] Tạo Member với email không chứa '@':
  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: Email không hợp lệ (phải chứa ký tự '@'). (Parameter 'email')

================================================================================
    HOÀN TẤT DEMO 100% CÁC BƯỚC NGHIỆP VỤ VÀ LUẬT TOÀN VẸN DỮ LIỆU!
================================================================================
```

---

## 6. Hướng dẫn quy trình Git và nộp bài Pull Request

Để đạt trọn vẹn 10 điểm phần **Nhiệm vụ 5 (Git và PR)**, hãy thực hiện theo đúng các bước sau:

### Bước 1: Khởi tạo Git và chuyển sang nhánh riêng
```bash
git init
git switch -c ch01/<MSSV_CỦA_BẠN>
# Ví dụ: git switch -c ch01/DH24001
```

### Bước 2: Tạo các commit theo chuẩn Conventional Commits
**Commit 1: Thêm tài liệu phân tích và sơ đồ lớp**
```bash
git add docs/
git commit -m "docs: add user stories and class diagram for EduHub domain"
```

**Commit 2: Hiện thực các Domain Entities tuân thủ Encapsulation**
```bash
git add Domain/ EduHub.Ch01.csproj
git commit -m "feat(domain): implement core domain entities with encapsulation"
```

**Commit 3: Hoàn thành Console Demo và kiểm thử nghiệp vụ**
```bash
git add Program.cs
git commit -m "feat(demo): implement console application workflow demo"
```

**Commit 4: Bổ sung tài liệu README**
```bash
git add README.md
git commit -m "docs: add comprehensive README with execution guide and screenshots"
```

### Bước 3: Đẩy nhánh lên kho GitHub cá nhân
```bash
git remote add origin https://github.com/<tai-khoan-github-cua-ban>/eduhub.git
git push -u origin ch01/<MSSV_CỦA_BẠN>
```

### Bước 4: Mở Pull Request về `main`
- **Tiêu đề PR:** `feat(domain): complete chapter 01 domain modeling and console demo`
- **Người đánh giá (Reviewers):** Tag tài khoản GitHub của giảng viên (ThS. Nguyễn Tuấn Kiệt).
- **Nội dung mô tả (PR Description template):**
  ```markdown
  ### What (Nội dung thay đổi)
  - Phân tích 7 User Stories chi tiết với tiêu chí nghiệm thu Given/When/Then cho 3 nhóm vai trò (Sinh viên, Ban CLB, Admin).
  - Thiết kế sơ đồ lớp sơ bộ (Class Diagram) bằng cú pháp Mermaid trong docs/class-diagram.md.
  - Hiện thực hóa 5 Domain Entities (Member, Club, Event, Registration, CheckIn) trong thư mục Domain/ theo đúng nguyên tắc Encapsulation (.NET 10).
  - Xây dựng kịch bản Console Demo hoàn chỉnh (Program.cs) qua 8 bước nghiệp vụ, có xuất báo cáo trực quan và kiểm thử vi phạm ràng buộc dữ liệu.

  ### Why (Lý do thực hiện)
  - Hoàn thành bài tập Chương 1 (HĐ1.3) học phần IN4211 - Phát triển phần mềm hướng đối tượng.
  - Đáp ứng chuẩn đầu ra CLO1, CLO4 và CLO5.

  ### How to test (Cách kiểm tra)
  1. Kiểm tra mã nguồn trong nhánh `ch01/<MSSV>`.
  2. Mở terminal tại thư mục EduHub.Ch01 và chạy lệnh: `dotnet run`
  3. Quan sát kết quả hiển thị trên màn hình console khớp 100% với kịch bản yêu cầu.
  ```
