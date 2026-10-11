# BÀI TẬP CH01: TÀI LIỆU USER STORIES — HỆ THỐNG EDUHUB
**Học phần:** IN4211 — Phát triển phần mềm hướng đối tượng  
**Giảng viên:** ThS. Nguyễn Tuấn Kiệt  
**Đơn vị:** Khoa Công nghệ và Kỹ thuật — Trường Đại học Đồng Tháp  

---

## 1. Giới thiệu tổng quan
Hệ thống **EduHub** hỗ trợ quản lý Câu lạc bộ (CLB) và Sự kiện sinh viên tại Trường Đại học Đồng Tháp. Tài liệu này đặc tả các User Story (câu chuyện người dùng) đại diện cho 3 nhóm vai trò (Actor) chính:
1. **Sinh viên (Student / Member):** Tìm hiểu, tham gia CLB, đăng ký tham gia các sự kiện và check-in điểm danh.
2. **Ban Chủ nhiệm CLB (Club Manager / Organizer):** Quản lý thông tin CLB, tổ chức sự kiện, kiểm soát số lượng đăng ký và xem báo cáo tham dự.
3. **Quản trị viên hệ thống (Admin):** Quản trị danh sách thành viên, đảm bảo tuân thủ nội quy và quản lý trạng thái tài khoản.

Tất cả câu chuyện người dùng tuân thủ định dạng chuẩn:  
> **"As a [role], I want [feature], so that [value]."**

Kèm theo các **Tiêu chí nghiệm thu (Acceptance Criteria)** theo cấu trúc chuẩn **Given / When / Then**.

---

## 2. Danh sách User Stories theo Actor

### 2.1 Nhóm vai trò: Sinh viên (Student / Member)

#### US-01: Đăng ký gia nhập Câu lạc bộ
- **User Story:**
  > *As a* sinh viên trường Đại học Đồng Tháp,  
  > *I want to* đăng ký tham gia vào một Câu lạc bộ trong hệ thống EduHub,  
  > *So that* tôi có thể sinh hoạt, cập nhật các hoạt động bổ ích và giao lưu học thuật.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Đăng ký thành công (Chưa là thành viên)**
    - **Given:** Sinh viên có mã số sinh viên (MSSV) hợp lệ, tài khoản đang hoạt động (`Active`) và chưa từng gia nhập CLB này.
    - **When:** Sinh viên gửi yêu cầu tham gia CLB qua phương thức gia nhập.
    - **Then:** Hệ thống ghi nhận sinh viên vào danh sách thành viên của CLB, số lượng thành viên tăng lên 1 và trạng thái gia nhập thành công.
  - **Kịch bản 2: Từ chối đăng ký trùng (Đã là thành viên)**
    - **Given:** Sinh viên đã có tên trong danh sách thành viên của CLB.
    - **When:** Sinh viên gửi lại yêu cầu tham gia CLB đó lần thứ hai.
    - **Then:** Hệ thống từ chối yêu cầu, ném ngoại lệ thông báo thành viên đã tồn tại và không làm thay đổi số lượng thành viên.

---

#### US-02: Đăng ký tham gia sự kiện của CLB
- **User Story:**
  > *As a* sinh viên,  
  > *I want to* đăng ký tham dự một sự kiện (workshop/seminar) do CLB tổ chức,  
  > *So that* tôi có được chỗ tham gia để học tập và nhận chứng nhận hoạt động.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Đăng ký khi sự kiện còn sức chứa (Capacity)**
    - **Given:** Sự kiện có sức chứa tối đa là `Capacity`, số lượng đã đăng ký hiện tại nhỏ hơn `Capacity`.
    - **When:** Sinh viên thực hiện gửi phiếu đăng ký tham dự sự kiện.
    - **Then:** Hệ thống tạo một phiếu đăng ký (`Registration`) mới với trạng thái mặc định là `Pending` (chờ xác nhận) và liên kết đúng `MemberId` cùng `EventId`.
  - **Kịch bản 2: Báo lỗi khi sự kiện đã hết chỗ**
    - **Given:** Sự kiện đã nhận đủ số lượng đăng ký bằng đúng `Capacity`.
    - **When:** Một sinh viên khác cố gắng đăng ký tham gia sự kiện này.
    - **Then:** Hệ thống từ chối tạo phiếu đăng ký và trả về thông báo lỗi sự kiện đã hết chỗ tiếp nhận.

---

#### US-03: Điểm danh (Check-in) tại ngày diễn ra sự kiện
- **User Story:**
  > *As a* sinh viên đã đăng ký sự kiện,  
  > *I want to* quét mã QR hoặc trình mã phiếu để điểm danh (check-in) tại bàn check-in sự kiện,  
  > *So that* ban tổ chức xác nhận sự có mặt thực tế của tôi để tính điểm rèn luyện.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Check-in thành công với phiếu đã xác nhận (Confirmed)**
    - **Given:** Sinh viên sở hữu phiếu đăng ký đang ở trạng thái `Confirmed` (đã được ban tổ chức duyệt).
    - **When:** Thực hiện quét mã điểm danh check-in bằng phương thức hợp lệ (QR hoặc Manual).
    - **Then:** Hệ thống tạo bản ghi `CheckIn` thành công, lưu lại thời gian điểm danh thực tế và thông tin hình thức check-in.
  - **Kịch bản 2: Từ chối check-in khi phiếu chưa duyệt (Pending) hoặc đã huỷ (Cancelled)**
    - **Given:** Phiếu đăng ký của sinh viên đang ở trạng thái `Pending` hoặc `Cancelled`.
    - **When:** Thao tác điểm danh được thực hiện với phiếu này.
    - **Then:** Hệ thống ném ngoại lệ nghiệp vụ (`InvalidOperationException`) từ chối điểm danh và thông báo chỉ phiếu `Confirmed` mới được check-in.

---

### 2.2 Nhóm vai trò: Ban Chủ nhiệm CLB (Club Manager / Organizer)

#### US-04: Tạo sự kiện mới cho CLB
- **User Story:**
  > *As a* cán bộ Ban Chủ nhiệm CLB,  
  > *I want to* tạo một sự kiện mới trên EduHub với tiêu đề, ngày diễn ra, sức chứa và hình thức tổ chức,  
  > *So that* truyền thông và thu hút sinh viên trong trường đăng ký tham gia.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Tạo sự kiện thành công với thông tin hợp lệ**
    - **Given:** Người dùng nhập tiêu đề không rỗng, sức chứa `Capacity > 0`, ngày tổ chức hợp lệ và chỉ định đúng mã định danh CLB (`ClubId`).
    - **When:** Gọi phương thức tạo sự kiện `Event.Create(...)`.
    - **Then:** Đối tượng `Event` được khởi tạo thành công với mã `Id` duy nhất kiểu `Guid`, lưu trữ đúng các thuộc tính đã cung cấp.
  - **Kịch bản 2: Báo lỗi khi thông tin không hợp lệ**
    - **Given:** Người dùng để trống tiêu đề sự kiện hoặc thiết lập `Capacity <= 0`.
    - **When:** Gọi phương thức `Event.Create(...)`.
    - **Then:** Hệ thống ném ngoại lệ kiểm tra dữ liệu (`ArgumentException` hoặc `ArgumentOutOfRangeException`) và không tạo sự kiện.

---

#### US-05: Xác nhận (Confirm) và huỷ (Cancel) phiếu đăng ký sự kiện
- **User Story:**
  > *As a* người quản lý sự kiện của CLB,  
  > *I want to* duyệt xác nhận hoặc huỷ bỏ phiếu đăng ký của sinh viên,  
  > *So that* kiểm soát chính xác danh sách tham dự chính thức trước giờ sự kiện bắt đầu.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Duyệt xác nhận phiếu đang chờ (Pending -> Confirmed)**
    - **Given:** Phiếu đăng ký của sinh viên đang ở trạng thái `Pending`.
    - **When:** Người quản trị gọi phương thức `Confirm()` trên phiếu đăng ký.
    - **Then:** Trạng thái của phiếu đăng ký chuyển thành `Confirmed`.
  - **Kịch bản 2: Không cho phép duyệt lại phiếu đã bị huỷ (Cancelled)**
    - **Given:** Phiếu đăng ký đã bị huỷ trước đó (`Cancelled`).
    - **When:** Cố gắng gọi phương thức `Confirm()` trên phiếu này.
    - **Then:** Hệ thống ném ngoại lệ `InvalidOperationException` thông báo không thể xác nhận phiếu đã huỷ.

---

#### US-06: Xem báo cáo tổng hợp tham dự sự kiện
- **User Story:**
  > *As a* Ban Chủ nhiệm CLB,  
  > *I want to* xem báo cáo tổng hợp số lượng đăng ký, số lượng check-in thực tế kèm danh sách chi tiết,  
  > *So that* đánh giá mức độ quan tâm của sinh viên và gửi dữ liệu về Phòng Công tác Sinh viên.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Báo cáo số liệu thống kê đầy đủ**
    - **Given:** Sự kiện có các phiếu đăng ký và một số sinh viên đã hoàn thành check-in.
    - **When:** Quản trị viên truy xuất báo cáo sự kiện qua giao diện console.
    - **Then:** Màn hình hiển thị rõ ràng: Tên sự kiện, tổng sức chứa, số lượt đăng ký, số lượt đã xác nhận và số lượt đã check-in thực tế.
  - **Kịch bản 2: Liệt kê chi tiết trạng thái từng sinh viên**
    - **Given:** Danh sách sinh viên đã đăng ký sự kiện.
    - **When:** Báo cáo được xuất ra.
    - **Then:** Mỗi sinh viên hiển thị rõ thông tin họ tên, mã số sinh viên (MSSV), trạng thái phiếu và dấu hiệu đã check-in (`ĐÃ CHECK-IN [QR]`) hoặc chưa check-in (`CHƯA CHECK-IN`).

---

### 2.3 Nhóm vai trò: Quản trị viên hệ thống (Admin)

#### US-07: Tạm dừng hoạt động tài khoản thành viên
- **User Story:**
  > *As a* Quản trị viên hệ thống EduHub,  
  > *I want to* tạm dừng (deactivate) tài khoản thành viên vi phạm quy chế hoặc đã tốt nghiệp,  
  > *So that* bảo vệ an toàn hệ thống và đảm bảo quyền lợi cho các thành viên tích cực khác.
- **Tiêu chí nghiệm thu (Acceptance Criteria):**
  - **Kịch bản 1: Tạm dừng thành viên đang hoạt động**
    - **Given:** Thành viên đang ở trạng thái `Active` (`IsActive() == true`).
    - **When:** Quản trị viên kích hoạt hành vi `Deactivate()`.
    - **Then:** Trạng thái thành viên chuyển thành `Inactive`, hàm `IsActive()` trả về `false`, nhưng toàn bộ lịch sử đăng ký trước đó vẫn được lưu trữ nguyên vẹn để phục vụ hậu kiểm.
  - **Kịch bản 2: Kích hoạt lại thành viên đã tạm dừng**
    - **Given:** Thành viên đang ở trạng thái `Inactive`.
    - **When:** Quản trị viên kích hoạt hành vi `Activate()`.
    - **Then:** Trạng thái thành viên chuyển lại thành `Active` và có thể tiếp tục tham gia các hoạt động bình thường.
