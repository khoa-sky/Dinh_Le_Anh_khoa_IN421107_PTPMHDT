namespace EduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho một Sự kiện (workshop, hội thảo, cuộc thi...) do Câu lạc bộ tổ chức.
/// </summary>
public class Event
{
    /// <summary>
    /// Khóa chính định danh duy nhất của Sự kiện (PK).
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Khóa ngoại tham chiếu đến Câu lạc bộ tổ chức sự kiện (FK).
    /// </summary>
    public Guid ClubId { get; private set; }

    /// <summary>
    /// Tiêu đề hoặc tên của sự kiện.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Thời gian dự kiến diễn ra sự kiện.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Sức chứa / Số lượng người tham dự tối đa cho phép.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Hình thức tổ chức: Trực tuyến (true) hoặc Trực tiếp tại giảng đường (false).
    /// </summary>
    public bool IsOnline { get; private set; }

    /// <summary>
    /// Hàm khởi tạo riêng tư (private constructor).
    /// </summary>
    private Event(Guid id, Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = id;
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }

    /// <summary>
    /// Phương thức Factory khởi tạo Sự kiện với kiểm tra dữ liệu đầu vào nghiêm ngặt.
    /// </summary>
    /// <param name="clubId">Mã định danh CLB tổ chức (không được rỗng).</param>
    /// <param name="title">Tiêu đề sự kiện (không được rỗng).</param>
    /// <param name="date">Thời gian tổ chức sự kiện.</param>
    /// <param name="capacity">Sức chứa tối đa (phải lớn hơn 0).</param>
    /// <param name="isOnline">Tổ chức trực tuyến hay trực tiếp.</param>
    /// <returns>Đối tượng Event hợp lệ.</returns>
    /// <exception cref="ArgumentException">Ném ra khi mã CLB rỗng hoặc tiêu đề bị để trống.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Ném ra khi sức chứa nhỏ hơn hoặc bằng 0.</exception>
    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    {
        if (clubId == Guid.Empty)
        {
            throw new ArgumentException("Mã Câu lạc bộ tổ chức (ClubId) không được rỗng.", nameof(clubId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa (Capacity) của sự kiện phải lớn hơn 0.");
        }

        return new Event(
            id: Guid.NewGuid(),
            clubId: clubId,
            title: title.Trim(),
            date: date,
            capacity: capacity,
            isOnline: isOnline
        );
    }

    /// <summary>
    /// Kiểm tra xem sự kiện còn chỗ trống cho người đăng ký mới hay không.
    /// </summary>
    /// <param name="registeredCount">Số lượng sinh viên đã đăng ký hiện tại.</param>
    /// <returns>True nếu số lượng đã đăng ký còn nhỏ hơn sức chứa tối đa, ngược lại False.</returns>
    public bool HasCapacity(int registeredCount) => registeredCount < Capacity;

    /// <summary>
    /// Trả về chuỗi đại diện mô tả Sự kiện.
    /// </summary>
    public override string ToString()
    {
        var mode = IsOnline ? "Trực tuyến (Online)" : "Trực tiếp (Offline)";
        return $"[Sự kiện] {Title} | Ngày: {Date:dd/MM/yyyy HH:mm} | Sức chứa: {Capacity} chỗ | Hình thức: {mode}";
    }
}
