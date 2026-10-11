namespace EduHub.Ch01.Domain;

/// <summary>
/// Trạng thái hoạt động của thành viên trong hệ thống EduHub.
/// </summary>
public enum MemberStatus
{
    /// <summary>
    /// Thành viên đang hoạt động bình thường.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Thành viên bị tạm ngừng hoặc đã rời hệ thống.
    /// </summary>
    Inactive = 2
}

/// <summary>
/// Đại diện cho một thành viên sinh viên trong hệ thống EduHub.
/// Tuân thủ nguyên tắc đóng gói (Encapsulation) với constructor private và static factory method Create().
/// </summary>
public class Member
{
    /// <summary>
    /// Khóa chính định danh duy nhất của thành viên (PK).
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Họ và tên của thành viên sinh viên.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Địa chỉ email sinh viên.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Mã số sinh viên (MSSV) tại Đại học Đồng Tháp.
    /// </summary>
    public string StudentCode { get; private set; }

    /// <summary>
    /// Thời điểm sinh viên được thêm vào hệ thống.
    /// </summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Trạng thái hoạt động hiện tại của thành viên.
    /// </summary>
    public MemberStatus Status { get; private set; }

    /// <summary>
    /// Hàm khởi tạo riêng tư (private constructor) để ngăn chặn khởi tạo tự do từ bên ngoài mà không qua kiểm tra dữ liệu.
    /// </summary>
    private Member(Guid id, string name, string email, string studentCode, DateTime joinedAt, MemberStatus status)
    {
        Id = id;
        Name = name;
        Email = email;
        StudentCode = studentCode;
        JoinedAt = joinedAt;
        Status = status;
    }

    /// <summary>
    /// Phương thức khởi tạo dạng Factory (Static Factory Method) có kiểm tra tính hợp lệ của dữ liệu đầu vào.
    /// </summary>
    /// <param name="name">Họ và tên thành viên.</param>
    /// <param name="email">Email sinh viên (bắt buộc chứa ký tự '@').</param>
    /// <param name="studentCode">Mã số sinh viên (MSSV).</param>
    /// <returns>Đối tượng Member hợp lệ.</returns>
    /// <exception cref="ArgumentException">Ném ra khi tên, email hoặc MSSV không hợp lệ.</exception>
    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Họ tên sinh viên không được để trống.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("Email không hợp lệ (phải chứa ký tự '@').", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(studentCode))
        {
            throw new ArgumentException("Mã số sinh viên không được để trống.", nameof(studentCode));
        }

        return new Member(
            id: Guid.NewGuid(),
            name: name.Trim(),
            email: email.Trim().ToLowerInvariant(),
            studentCode: studentCode.Trim().ToUpperInvariant(),
            joinedAt: DateTime.Now,
            status: MemberStatus.Active
        );
    }

    /// <summary>
    /// Kiểm tra xem thành viên có đang ở trạng thái hoạt động (Active) hay không.
    /// </summary>
    /// <returns>True nếu thành viên đang Active, ngược lại False.</returns>
    public bool IsActive() => Status == MemberStatus.Active;

    /// <summary>
    /// Tạm dừng quyền hoạt động của thành viên (chuyển sang Inactive).
    /// </summary>
    public void Deactivate()
    {
        Status = MemberStatus.Inactive;
    }

    /// <summary>
    /// Kích hoạt lại quyền hoạt động của thành viên (chuyển sang Active).
    /// </summary>
    public void Activate()
    {
        Status = MemberStatus.Active;
    }

    /// <summary>
    /// Trả về chuỗi đại diện mô tả thành viên.
    /// </summary>
    public override string ToString()
    {
        return $"{Name} (MSSV: {StudentCode}) - {Email} [{(IsActive() ? "Active" : "Inactive")}]";
    }
}
