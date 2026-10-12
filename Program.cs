using System.Text;
using EduHub.Ch01.Domain;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== CHƯƠNG TRÌNH QUẢN LÝ CÂU LẠC BỘ & SỰ KIỆN (EDUHUB) ===\n");

// [US01] Khởi tạo Câu lạc bộ thông qua Factory method
var club = Club.Create("Tech Club", "CLB Lập trình & Đổi mới sáng tạo - ĐH Đồng Tháp");
Console.WriteLine($"[US01] Đã tạo CLB: {club.Name} (Mã Id: {club.Id})");

// [US02] Khởi tạo các Thành viên với dữ liệu được kiểm tra hợp lệ
var m1 = Member.Create("Nguyễn Văn An", "an.nguyen@dthu.edu.vn", "DH24001");
var m2 = Member.Create("Trần Thị Bình", "binh.tran@dthu.edu.vn", "DH24002");
var m3 = Member.Create("Lê Hoàng Cường", "cuong.le@dthu.edu.vn", "DH24003");

club.AddMember(m1);
club.AddMember(m2);
club.AddMember(m3);

Console.WriteLine($"[US01/US02] Đã thêm {club.MemberCount()} thành viên vào CLB:");
foreach (var m in club.Members)
{
    Console.WriteLine($"    - {m.Name} ({m.StudentCode}) - {m.Email} [Trạng thái: {m.Status}]");
}

// [US03] Tạo Sự kiện cho CLB
var ev = Event.Create(club.Id, "Workshop Lập trình C# hướng đối tượng", DateTime.Now.AddDays(7), 50, false);
Console.WriteLine($"\n[US03] Đã tạo sự kiện: {ev.Title} | Sức chứa: {ev.Capacity} chỗ");

// [US04] Đăng ký tham gia sự kiện và duyệt phiếu
var registrations = new List<Registration>();

if (ev.HasCapacity(registrations.Count))
{
    var reg1 = Registration.Create(m1.Id, ev.Id);
    reg1.Confirm();
    registrations.Add(reg1);
    Console.WriteLine($"[US04] Sinh viên {m1.Name} đã đăng ký -> Trạng thái: {reg1.Status}");
}

if (ev.HasCapacity(registrations.Count))
{
    var reg2 = Registration.Create(m2.Id, ev.Id);
    reg2.Confirm();
    registrations.Add(reg2);
    Console.WriteLine($"[US04] Sinh viên {m2.Name} đã đăng ký -> Trạng thái: {reg2.Status}");
}

// [US05] Điểm danh (Check-in) sử dụng hằng số CheckInMethod.QR
var checkIns = new List<CheckIn>();
var checkIn1 = CheckIn.Create(registrations[0], CheckInMethod.QR);
checkIns.Add(checkIn1);

Console.WriteLine($"\n[US05] Điểm danh sự kiện: {m1.Name} đã điểm danh qua [{checkIn1.Method}] lúc {checkIn1.CheckedInAt:HH:mm:ss dd/MM/yyyy}");

// Báo cáo tổng kết tham dự sự kiện
Console.WriteLine("\n=== BÁO CÁO THAM DỰ SỰ KIỆN ===");
Console.WriteLine($"Sự kiện: {ev.Title}");
Console.WriteLine($"Số lượng đăng ký: {registrations.Count} | Đã check-in: {checkIns.Count}\n");

var participants = new[]
{
    (Member: m1, Reg: registrations[0]),
    (Member: m2, Reg: registrations[1])
};

Console.WriteLine("{0,-10} | {1,-18} | {2,-12} | {3}", "MSSV", "Họ tên", "Trạng thái", "Điểm danh");
Console.WriteLine(new string('-', 60));

foreach (var p in participants)
{
    var checkedIn = checkIns.Any(c => c.RegistrationId == p.Reg.Id);
    var statusText = checkedIn ? $"Đã check-in ({CheckInMethod.QR})" : "Chưa check-in";
    Console.WriteLine("{0,-10} | {1,-18} | {2,-12} | {3}", 
        p.Member.StudentCode, 
        p.Member.Name, 
        p.Reg.Status, 
        statusText);
}

// Kiểm tra thử nghiệm quy tắc nghiệp vụ (Business Rules)
Console.WriteLine("\n=== KIỂM TRA QUY TẮC RÀNG BUỘC (VALIDATION TEST) ===");
try
{
    // Thử tạo Member với email sai
    Member.Create("Test Lỗi", "emailsai", "DH99999");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"[Bắt lỗi hợp lệ]: {ex.Message}");
}

try
{
    // Thử thêm trùng thành viên vào CLB
    club.AddMember(m1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[Bắt lỗi hợp lệ]: {ex.Message}");
}

Console.WriteLine("\nHoàn tất chạy demo!");
