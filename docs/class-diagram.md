# BÀI TẬP CH01: SƠ ĐỒ LỚP SƠ BỘ (CLASS DIAGRAM) — DOMAIN EDUHUB
**Học phần:** IN4211 — Phát triển phần mềm hướng đối tượng  
**Giảng viên:** ThS. Nguyễn Tuấn Kiệt  
**Đơn vị:** Khoa Công nghệ và Kỹ thuật — Trường Đại học Đồng Tháp  

---

## 1. Giới thiệu sơ đồ lớp
Mô hình miền (Domain Model) của **EduHub** trong Chương 1 tập trung vào 5 thực thể cốt lõi (Domain Entities):
- **Member:** Đại diện cho sinh viên tham gia hệ thống và các CLB.
- **Club:** Đại diện cho Câu lạc bộ sinh viên do Nhà trường/Khoa thành lập.
- **Event:** Đại diện cho các sự kiện (workshop, seminar, hackathon) do CLB tổ chức.
- **Registration:** Phiếu đăng ký tham dự của sinh viên đối với một sự kiện cụ thể (đóng vai trò bảng nối/thực thể liên kết nhiều - nhiều giữa `Member` và `Event`).
- **CheckIn:** Bản ghi ghi nhận việc sinh viên có mặt thực tế tại sự kiện (quan hệ 1 - 0..1 với `Registration`).

---

## 2. Sơ đồ lớp Mermaid (classDiagram)

```mermaid
classDiagram
    direction TB

    class MemberStatus {
        <<enumeration>>
        Active
        Inactive
    }

    class RegistrationStatus {
        <<enumeration>>
        Pending
        Confirmed
        Cancelled
    }

    class CheckInMethod {
        <<enumeration>>
        QR
        Manual
    }

    class Member {
        -Guid _id
        -string _name
        -string _email
        -string _studentCode
        -DateTime _joinedAt
        -MemberStatus _status
        +Guid Id : Guid
        +string Name : string
        +string Email : string
        +string StudentCode : string
        +DateTime JoinedAt : DateTime
        +MemberStatus Status : MemberStatus
        +Create(name, email, studentCode)$ Member
        +IsActive() bool
        +Deactivate() void
        +Activate() void
        +ToString() string
    }

    class Club {
        -Guid _id
        -string _name
        -string _description
        -DateTime _createdAt
        -List~Member~ _members
        +Guid Id : Guid
        +string Name : string
        +string Description : string
        +DateTime CreatedAt : DateTime
        +IReadOnlyList~Member~ Members
        +Create(name, description)$ Club
        +AddMember(member) void
        +MemberCount() int
        +HasMember(memberId) bool
        +ToString() string
    }

    class Event {
        -Guid _id
        -Guid _clubId
        -string _title
        -DateTime _date
        -int _capacity
        -bool _isOnline
        +Guid Id : Guid
        +Guid ClubId : Guid
        +string Title : string
        +DateTime Date : DateTime
        +int Capacity : int
        +bool IsOnline : bool
        +Create(clubId, title, date, capacity, isOnline)$ Event
        +HasCapacity(registeredCount) bool
        +ToString() string
    }

    class Registration {
        -Guid _id
        -Guid _memberId
        -Guid _eventId
        -DateTime _registeredAt
        -RegistrationStatus _status
        +Guid Id : Guid
        +Guid MemberId : Guid
        +Guid EventId : Guid
        +DateTime RegisteredAt : DateTime
        +RegistrationStatus Status : RegistrationStatus
        +Create(memberId, eventId)$ Registration
        +Confirm() void
        +Cancel() void
        +ToString() string
    }

    class CheckIn {
        -Guid _id
        -Guid _registrationId
        -DateTime _checkedInAt
        -string _method
        +Guid Id : Guid
        +Guid RegistrationId : Guid
        +DateTime CheckedInAt : DateTime
        +string Method : string
        +Create(registration, method)$ CheckIn
        +Summary() string
        +ToString() string
    }

    Club "1" o-- "*" Member : tập hợp thành viên (nhiều - nhiều logic)
    Club "1" *-- "*" Event : tổ chức (1 - *) [FK: Event.ClubId]
    Member "1" <-- "*" Registration : đăng ký (1 - *) [FK: Registration.MemberId]
    Event "1" <-- "*" Registration : nhận phiếu (1 - *) [FK: Registration.EventId]
    Registration "1" <-- "0..1" CheckIn : điểm danh (1 - 0..1) [FK: CheckIn.RegistrationId]
    
    Member ..> MemberStatus : sử dụng
    Registration ..> RegistrationStatus : sử dụng
    CheckIn ..> CheckInMethod : sử dụng
```

---

## 3. Bảng phân tích quan hệ và khóa ngoại (Foreign Keys)

| Quan hệ | Loại quan hệ | Bội số (Multiplicity) | Khóa ngoại (FK) | Ý nghĩa nghiệp vụ |
| :--- | :---: | :---: | :--- | :--- |
| **Member — Club** | N — N | `* — *` | Bảng liên kết logic (trong Ch01 quản lý qua danh sách `_members` nội bộ của `Club`; Ch04 dùng bảng trung gian `ClubMember`) | Một sinh viên có thể tham gia nhiều CLB. Mỗi CLB có nhiều sinh viên. |
| **Club — Event** | 1 — N | `1 — *` | `Event.ClubId` tham chiếu đến `Club.Id` | Một CLB tổ chức nhiều sự kiện; mỗi sự kiện trực thuộc duy nhất một CLB quản lý. |
| **Member — Registration** | 1 — N | `1 — *` | `Registration.MemberId` tham chiếu đến `Member.Id` | Một sinh viên có thể tạo nhiều phiếu đăng ký cho các sự kiện khác nhau. |
| **Event — Registration** | 1 — N | `1 — *` | `Registration.EventId` tham chiếu đến `Event.Id` | Mỗi sự kiện có thể tiếp nhận nhiều phiếu đăng ký cho đến khi đạt mức tối đa (`Capacity`). |
| **Registration — CheckIn** | 1 — 0..1 | `1 — 0..1` | `CheckIn.RegistrationId` tham chiếu đến `Registration.Id` | Mỗi phiếu đăng ký chỉ được điểm danh tối đa 1 lần duy nhất tại ngày diễn ra sự kiện. |

---

## 4. Các quy tắc thiết kế Hướng đối tượng (OOP) áp dụng
1. **Encapsulation (Đóng gói):**
   - Thuộc tính có `private set` để ngăn chặn thay đổi trạng thái tự do từ bên ngoài (tránh biến class thành "Data bag").
   - Constructor có phạm vi truy cập `private`, bắt buộc mọi luồng khởi tạo phải đi qua Static Factory Method `Create(...)`.
   - Danh sách thành viên trong `Club` được che giấu qua `private readonly List<Member> _members` và chỉ phơi bày qua giao diện chỉ đọc `IReadOnlyList<Member>`.
2. **Business Rule Validation:**
   - Dữ liệu đầu vào được kiểm định ngay tại factory method (ném `ArgumentException` hoặc `ArgumentOutOfRangeException`).
   - Sự chuyển đổi trạng thái (State Lifecycle) của `Registration` được bảo vệ chặt chẽ: Chỉ chuyển sang `Confirmed` khi đang `Pending`; không được phép confirm phiếu đã bị `Cancelled`.
   - `CheckIn` chỉ được phép khởi tạo khi phiếu đăng ký đã được duyệt `Confirmed`.
