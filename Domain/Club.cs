namespace EduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho một Câu lạc bộ (CLB) sinh viên trong hệ thống EduHub.
/// Quản lý danh sách thành viên nội bộ và bảo đảm toàn vẹn dữ liệu (không trùng lặp thành viên).
/// </summary>
public class Club
{
    private readonly List<Member> _members = new();

    /// <summary>
    /// Khóa chính định danh duy nhất của Câu lạc bộ (PK).
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Tên của Câu lạc bộ.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Mô tả mục đích hoạt động và định hướng của Câu lạc bộ.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Thời điểm thành lập hoặc đăng ký Câu lạc bộ trên hệ thống.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Danh sách thành viên sinh viên thuộc Câu lạc bộ (dưới dạng chỉ đọc để bảo vệ tính đóng gói).
    /// </summary>
    public IReadOnlyList<Member> Members => _members.AsReadOnly();

    /// <summary>
    /// Hàm khởi tạo riêng tư (private constructor).
    /// </summary>
    private Club(Guid id, string name, string description, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Phương thức Factory khởi tạo Câu lạc bộ với kiểm tra dữ liệu đầu vào.
    /// </summary>
    /// <param name="name">Tên Câu lạc bộ (không được rỗng).</param>
    /// <param name="description">Mô tả về Câu lạc bộ (không được rỗng).</param>
    /// <returns>Đối tượng Club hợp lệ.</returns>
    /// <exception cref="ArgumentException">Ném ra khi tên hoặc mô tả để trống.</exception>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Mô tả câu lạc bộ không được để trống.", nameof(description));
        }

        return new Club(
            id: Guid.NewGuid(),
            name: name.Trim(),
            description: description.Trim(),
            createdAt: DateTime.Now
        );
    }

    /// <summary>
    /// Thêm một thành viên sinh viên vào Câu lạc bộ.
    /// Quy tắc nghiệp vụ: Không chấp nhận giá trị null, không cho phép trùng lặp Id hoặc MSSV, thành viên phải Active.
    /// </summary>
    /// <param name="member">Thành viên cần thêm.</param>
    /// <exception cref="ArgumentNullException">Ném ra khi đối tượng thành viên bị null.</exception>
    /// <exception cref="InvalidOperationException">Ném ra khi thành viên đã tồn tại hoặc tài khoản đang bị tạm dừng.</exception>
    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (!member.IsActive())
        {
            throw new InvalidOperationException($"Không thể thêm thành viên '{member.Name}' vì tài khoản đang ở trạng thái Inactive.");
        }

        if (_members.Any(m => m.Id == member.Id || m.StudentCode.Equals(member.StudentCode, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Sinh viên '{member.Name}' (MSSV: {member.StudentCode}) đã là thành viên của CLB {Name}.");
        }

        _members.Add(member);
    }

    /// <summary>
    /// Đếm tổng số lượng thành viên hiện tại của Câu lạc bộ.
    /// </summary>
    /// <returns>Số lượng thành viên.</returns>
    public int MemberCount() => _members.Count;

    /// <summary>
    /// Kiểm tra xem một thành viên có thuộc Câu lạc bộ hay không theo mã định danh.
    /// </summary>
    /// <param name="memberId">Mã Id thành viên.</param>
    /// <returns>True nếu là thành viên, ngược lại False.</returns>
    public bool HasMember(Guid memberId) => _members.Any(m => m.Id == memberId);

    /// <summary>
    /// Trả về chuỗi đại diện mô tả Câu lạc bộ.
    /// </summary>
    public override string ToString()
    {
        return $"[CLB] {Name} - {Description} (Hiện có {MemberCount()} thành viên)";
    }
}
