# Phân tích các vi phạm SOLID trong ClubManager

`ClubManager` được thiết kế như một **God Class** (lớp toàn năng ôm đồm mọi việc: lưu trữ dữ liệu, kiểm tra quy tắc nghiệp vụ, gửi thông báo, in báo cáo và ghi log). Dưới đây là phân tích chi tiết các vi phạm nguyên lý thiết kế SOLID:

---

## 1. Vi phạm SRP (Single Responsibility Principle - Nguyên tắc đơn trách nhiệm)

- **Vị trí:**
  - `CreateClub` (Dòng 35 - 64)
  - `AddMemberToClub` (Dòng 67 - 84)
  - `CreateEvent` (Dòng 88 - 99)
  - `PrintClubReport` (Dòng 103 - 113)
- **Mô tả:** 
  Mỗi phương thức trong `ClubManager` đang đảm nhận quá nhiều vai trò không liên quan:
  - Kiểm tra tính hợp lệ dữ liệu (Validate tên, sĩ số tối đa 50 thành viên).
  - Thao tác trực tiếp với bộ nhớ lưu trữ (`_clubs`, `_members`).
  - Ghi log trực tiếp ra tệp tin đĩa cứng.
  - Xử lý gửi thông báo email và Slack.
  - Định dạng hiển thị và in báo cáo ra màn hình Console.
- **Hậu quả:** 
  - Lớp có quá nhiều lý do để bị thay đổi (thay đổi luật nghiệp vụ, đổi nơi lưu trữ, đổi cách ghi log hay đổi kênh thông báo đều phải sửa cùng một file `ClubManager.cs`).
  - Code bị ràng buộc chặt chẽ (tight coupling), rất khó viết Unit Test độc lập cho từng chức năng.
- **Cách khắc phục:** 
  - Đưa quy tắc nghiệp vụ về tầng Domain (`Club.Create`, `Club.HasRoom`, `Event.Register`).
  - Tách tầng lưu trữ ra các Repository (`IClubRepository`, `IMemberRepository`, `IEventRepository`).
  - Tách việc gửi thông báo ra `INotificationService`, ghi log ra `IAppLogger`, và báo cáo ra `ReportService`.

---

## 2. Vi phạm OCP (Open/Closed Principle - Nguyên tắc đóng/mở)

- **Vị trí:** 
  - Dòng 50 - 63 trong `CreateClub`
  - Dòng 82 - 84 trong `AddMemberToClub`
- **Mô tả:** 
  Sử dụng chuỗi `if/else` để kiểm tra đuôi email (`@gmail.com`, `@yahoo.com`) và in thông báo cố định. Tương tự, `AddMemberToClub` gắn cứng việc in thông báo qua email và Slack.
- **Hậu quả:** 
  - Vi phạm nguyên tắc *"Mở rộng cho phát triển, đóng cho chỉnh sửa"*.
  - Khi hệ thống muốn thêm kênh thông báo mới (như SMS, Telegram, Zalo) hoặc nhà cung cấp mới, lập trình viên buộc phải can thiệp và sửa trực tiếp mã nguồn cũ.
  - Dễ gây ra lỗi hồi quy (regression bugs) cho các tính năng đang chạy ổn định.
- **Cách khắc phục:** 
  - Áp dụng **Factory Pattern** (`INotificationServiceFactory`) để khởi tạo dịch vụ thông báo tương ứng theo tham số (`NotificationChannel`) mà không cần sửa code cũ.

---

## 3. Vi phạm DIP (Dependency Inversion Principle - Nguyên tắc đảo ngược phụ thuộc)

- **Vị trí:**
  - Dòng 32: `private readonly string _logFilePath = "club-log.txt";`
  - Dòng 48, 79, 94, 111: Gọi trực tiếp `File.AppendAllText`.
  - Các dòng gọi `Console.WriteLine` trực tiếp để xuất thông tin ra màn hình.
- **Mô tả:** 
  Module cấp cao (nghiệp vụ quản lý CLB) lại phụ thuộc trực tiếp vào các chi tiết cấp thấp cụ thể (ghi trực tiếp xuống tệp tin đĩa cứng và màn hình console) thay vì phụ thuộc vào một Abstraction (giao diện trừu tượng).
- **Hậu quả:** 
  - Khó viết Unit Test vì mỗi lần chạy test sẽ ghi rác vào tệp `club-log.txt` thực tế trên ổ cứng.
  - Không thể linh hoạt thay đổi cơ chế ghi log (ví dụ đổi sang Database hoặc Cloud Logging) nếu không sửa lại code.
- **Cách khắc phục:** 
  - Định nghĩa interface trừu tượng `IAppLogger`, sau đó inject `ConsoleLogger` hoặc `FileLogger` thông qua Constructor (Dependency Injection).

---

## 4. Vi phạm OCP và SRP trong tính năng xuất báo cáo

- **Vị trí:** 
  - Dòng 103 - 113: Phương thức `PrintClubReport()`
- **Mô tả:** 
  Phương thức in báo cáo bị gắn chặt với việc duyệt danh sách và in ra console theo một định dạng văn bản duy nhất.
- **Hậu quả:** 
  - Khi cần xuất báo cáo ra các định dạng khác như CSV, JSON, HTML hay Excel, ta buộc phải viết thêm nhiều phương thức mới hoặc nhồi nhét thêm điều kiện `if/else` vào phương thức cũ.
- **Cách khắc phục:** 
  - Áp dụng **Strategy Pattern**: Định nghĩa interface `IReportStrategy` và các chiến lược xuất cụ thể như `ConsoleReportStrategy`, `CsvReportStrategy`.

---

## 5. Vi phạm Observer và Ghép nối lỏng lẻo trong xử lý sự kiện

- **Vị trí:** 
  - Dòng 88 - 99: Phương thức `CreateEvent()`
- **Mô tả:** 
  Khi tạo sự kiện (`CreateEvent`), phương thức tự động lặp qua danh sách thành viên để gửi email thông báo.
- **Hậu quả:** 
  - Nghiệp vụ tạo sự kiện bị ràng buộc chặt chẽ với logic thông báo. Không thể mở rộng cho các bên khác cùng lắng nghe khi sự kiện được tạo (ví dụ: ghi nhận log thống kê, thông báo cho ban quản lý, gửi webhook).
- **Cách khắc phục:** 
  - Áp dụng **Observer Pattern** (`IEventObserver`). Khi tạo sự kiện, `EventService` chỉ cần thông báo (notify) cho danh sách các observer đã đăng ký (`LogNotifier`, `EmailNotifier`).
