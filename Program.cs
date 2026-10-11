using System.Text;
using EduHub.Ch01.Domain;

// Thiết lập mã hóa UTF-8 để hiển thị tốt tiếng Việt có dấu trên console
Console.OutputEncoding = Encoding.UTF8;

const string Separator = "================================================================================";
const string SubSeparator = "--------------------------------------------------------------------------------";

Console.WriteLine(Separator);
Console.WriteLine("       TRƯỜNG ĐẠI HỌC ĐỒNG THÁP - KHOA CÔNG NGHỆ VÀ KỸ THUẬT");
Console.WriteLine("             IN4211: PHÁT TRIỂN PHẦN MỀM HƯỚNG ĐỐI TƯỢNG");
Console.WriteLine("           BÀI TẬP CH01: DOMAIN MODELING & CONSOLE DEMO (EDUHUB)");
Console.WriteLine(Separator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 1: TẠO CÂU LẠC BỘ (CLUB)
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 1] Khởi tạo Câu lạc bộ (Club)");
var club = Club.Create(
    name: "Tech Club",
    description: "CLB Lập trình & Đổi mới sáng tạo - Trường Đại học Đồng Tháp"
);
Console.WriteLine($"[Thành công] Đã tạo CLB: {club.Name} (Mã Id: {club.Id})");
Console.WriteLine($"            Mô tả: {club.Description}");
Console.WriteLine(SubSeparator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 2: THÊM 3 THÀNH VIÊN VÀO CÂU LẠC BỘ
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 2] Khởi tạo và thêm 3 thành viên (Member) vào CLB");
var member1 = Member.Create(
    name: "Nguyễn Văn An",
    email: "an.nguyen@dthu.edu.vn",
    studentCode: "DH24001"
);
var member2 = Member.Create(
    name: "Trần Thị Bình",
    email: "binh.tran@dthu.edu.vn",
    studentCode: "DH24002"
);
var member3 = Member.Create(
    name: "Lê Hoàng Cường",
    email: "cuong.le@dthu.edu.vn",
    studentCode: "DH24003"
);

club.AddMember(member1);
club.AddMember(member2);
club.AddMember(member3);

Console.WriteLine($"[Thành công] Đã thêm 3 thành viên vào CLB. Tổng thành viên hiện tại: {club.MemberCount()}");
foreach (var m in club.Members)
{
    Console.WriteLine($"  - {m.Name} | MSSV: {m.StudentCode} | Email: {m.Email} | Trạng thái: {m.Status}");
}
Console.WriteLine(SubSeparator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 3: TẠO 1 SỰ KIỆN CHO CLB (EVENT)
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 3] Tạo sự kiện (Event) cho CLB");
var ev = Event.Create(
    clubId: club.Id,
    title: "Workshop: Lập trình .NET 10 & Clean Architecture",
    date: DateTime.Now.AddDays(7),
    capacity: 50,
    isOnline: false
);
Console.WriteLine($"[Thành công] Đã tạo sự kiện: {ev.Title}");
Console.WriteLine($"            CLB tổ chức: {club.Name} (ClubId: {ev.ClubId})");
Console.WriteLine($"            Thời gian: {ev.Date:dd/MM/yyyy HH:mm} | Sức chứa: {ev.Capacity} chỗ");
Console.WriteLine(SubSeparator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 4: 2 THÀNH VIÊN ĐĂNG KÝ THAM DỰ SỰ KIỆN & XÁC NHẬN PHIẾU
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 4] Đăng ký sự kiện (Registration) và duyệt xác nhận (Confirm)");
var registrations = new List<Registration>();

// Member 1 đăng ký
if (ev.HasCapacity(registrations.Count))
{
    var reg1 = Registration.Create(member1.Id, ev.Id);
    Console.WriteLine($"[Tạo phiếu #1] Sinh viên {member1.Name} gửi đăng ký -> Trạng thái ban đầu: {reg1.Status}");
    reg1.Confirm();
    Console.WriteLine($"              Ban tổ chức duyệt -> Trạng thái mới: {reg1.Status}");
    registrations.Add(reg1);
}

// Member 2 đăng ký
if (ev.HasCapacity(registrations.Count))
{
    var reg2 = Registration.Create(member2.Id, ev.Id);
    Console.WriteLine($"[Tạo phiếu #2] Sinh viên {member2.Name} gửi đăng ký -> Trạng thái ban đầu: {reg2.Status}");
    reg2.Confirm();
    Console.WriteLine($"              Ban tổ chức duyệt -> Trạng thái mới: {reg2.Status}");
    registrations.Add(reg2);
}

Console.WriteLine($"[Thống kê đăng ký] Đã xác nhận thành công {registrations.Count} phiếu đăng ký.");
Console.WriteLine(SubSeparator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 5: 1 THÀNH VIÊN CHECK-IN (ĐIỂM DANH TẠI NGÀY SỰ KIỆN)
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 5] Thực hiện Check-in điểm danh tại bàn tiếp đón");
var checkIns = new List<CheckIn>();

// Member 1 đến bàn check-in quét QR
var checkIn1 = CheckIn.Create(registrations[0], CheckInMethod.QR);
checkIns.Add(checkIn1);

Console.WriteLine($"[Điểm danh thành công] Sinh viên: {member1.Name} ({member1.StudentCode})");
Console.WriteLine($"                     {checkIn1.Summary()}");
Console.WriteLine(SubSeparator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 6: IN BÁO CÁO THAM DỰ SỰ KIỆN
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 6] Báo cáo tổng kết tham dự sự kiện");
Console.WriteLine(Separator);
Console.WriteLine($"  BÁO CÁO THAM DỰ SỰ KIỆN: {ev.Title.ToUpperInvariant()}");
Console.WriteLine($"  Đơn vị tổ chức: {club.Name} | Sức chứa: {ev.Capacity} sinh viên");
Console.WriteLine($"  Tổng số đăng ký hợp lệ: {registrations.Count} | Tổng số đã check-in: {checkIns.Count}");
Console.WriteLine(Separator);
Console.WriteLine(string.Format("{0,-12} | {1,-20} | {2,-15} | {3,-20}", "MSSV", "Họ và Tên", "Trạng thái phiếu", "Tình trạng điểm danh"));
Console.WriteLine(SubSeparator);

// Mapping kiểm tra
var registeredMembers = new (Member Member, Registration Reg)[]
{
    (member1, registrations[0]),
    (member2, registrations[1])
};

foreach (var item in registeredMembers)
{
    var isCheckedIn = checkIns.Any(c => c.RegistrationId == item.Reg.Id);
    var checkInStatusText = isCheckedIn ? "[x] ĐÃ CHECK-IN (QR)" : "[ ] CHƯA CHECK-IN";
    Console.WriteLine(string.Format("{0,-12} | {1,-20} | {2,-15} | {3,-20}",
        item.Member.StudentCode,
        item.Member.Name,
        item.Reg.Status,
        checkInStatusText
    ));
}
Console.WriteLine(Separator);
Console.WriteLine();

// -----------------------------------------------------------------------------
// BƯỚC 7 & 8: THỬ NGHIỆM VI PHẠM LUẬT NGHIỆP VỤ (BUSINESS RULES VALIDATION)
// -----------------------------------------------------------------------------
Console.WriteLine(">>> [BƯỚC 7 & 8] Kiểm tra cơ chế tự bảo vệ luật nghiệp vụ (Encapsulation / Business Rules)");

// Trường hợp vi phạm 1: Cố tình thêm trùng thành viên vào CLB
Console.WriteLine("\n[Thử nghiệm 1] Cố tình thêm lại sinh viên đã có trong CLB (chặn trùng):");
try
{
    club.AddMember(member1); // Thử thêm lại Nguyễn Văn An
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: {ex.Message}");
}

// Trường hợp vi phạm 2: Phiếu đã huỷ (Cancelled) thì không thể xác nhận lại (Confirm)
Console.WriteLine("\n[Thử nghiệm 2] Thử xác nhận lại một phiếu đăng ký đã bị huỷ (Cancelled):");
var testReg = Registration.Create(member3.Id, ev.Id);
Console.WriteLine($"  Tạo phiếu cho {member3.Name} -> Status: {testReg.Status}");
testReg.Cancel();
Console.WriteLine($"  Đã gọi Cancel() -> Status: {testReg.Status}");
try
{
    testReg.Confirm(); // Không được phép confirm phiếu đã huỷ
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: {ex.Message}");
}

// Trường hợp vi phạm 3: Phiếu chưa được duyệt (Pending) không được phép Check-in
Console.WriteLine("\n[Thử nghiệm 3] Cố tình check-in một phiếu đang ở trạng thái Pending:");
var pendingReg = Registration.Create(member3.Id, ev.Id);
try
{
    CheckIn.Create(pendingReg, CheckInMethod.Manual);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: {ex.Message}");
}

// Trường hợp vi phạm 4: Khởi tạo Member với email sai định dạng
Console.WriteLine("\n[Thử nghiệm 4] Tạo Member với email không chứa '@':");
try
{
    Member.Create("Lỗi Email", "email-khong-hop-le", "DH24999");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  --> [BẮT NGOẠI LỆ THÀNH CÔNG]: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine(Separator);
Console.WriteLine("    HOÀN TẤT DEMO 100% CÁC BƯỚC NGHIỆP VỤ VÀ LUẬT TOÀN VẸN DỮ LIỆU!");
Console.WriteLine(Separator);
