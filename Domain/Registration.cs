namespace EduHub.Ch01.Domain;

/// <summary>
/// Trạng thái của phiếu đăng ký tham dự sự kiện.
/// </summary>
public enum RegistrationStatus
{
    /// <summary>
    /// Phiếu đăng ký mới tạo, đang chờ ban tổ chức xác nhận.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Phiếu đăng ký đã được ban tổ chức phê duyệt/xác nhận thành công.
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// Phiếu đăng ký đã bị huỷ bởi sinh viên hoặc ban tổ chức.
    /// </summary>
    Cancelled = 3
}

/// <summary>
/// Đại diện cho một Phiếu đăng ký tham dự sự kiện của sinh viên trong hệ thống EduHub.
/// Đóng vai trò thực thể liên kết (bảng nối) nhiều - nhiều giữa Member và Event.
/// </summary>
public class Registration
{
    /// <summary>
    /// Khóa chính định danh duy nhất của Phiếu đăng ký (PK).
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Khóa ngoại tham chiếu đến sinh viên đăng ký (FK -> Member.Id).
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Khóa ngoại tham chiếu đến sự kiện được đăng ký (FK -> Event.Id).
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Thời điểm sinh viên gửi yêu cầu đăng ký.
    /// </summary>
    public DateTime RegisteredAt { get; private set; }

    /// <summary>
    /// Trạng thái hiện tại của phiếu đăng ký (Pending, Confirmed, Cancelled).
    /// </summary>
    public RegistrationStatus Status { get; private set; }

    /// <summary>
    /// Hàm khởi tạo riêng tư (private constructor).
    /// </summary>
    private Registration(Guid id, Guid memberId, Guid eventId, DateTime registeredAt, RegistrationStatus status)
    {
        Id = id;
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = registeredAt;
        Status = status;
    }

    /// <summary>
    /// Phương thức Factory tạo phiếu đăng ký mới với trạng thái khởi đầu là Pending.
    /// </summary>
    /// <param name="memberId">Mã định danh thành viên đăng ký (không được rỗng).</param>
    /// <param name="eventId">Mã định danh sự kiện (không được rỗng).</param>
    /// <returns>Đối tượng Registration mới.</returns>
    /// <exception cref="ArgumentException">Ném ra khi mã sinh viên hoặc sự kiện rỗng.</exception>
    public static Registration Create(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty)
        {
            throw new ArgumentException("Mã thành viên (MemberId) không được rỗng.", nameof(memberId));
        }

        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Mã sự kiện (EventId) không được rỗng.", nameof(eventId));
        }

        return new Registration(
            id: Guid.NewGuid(),
            memberId: memberId,
            eventId: eventId,
            registeredAt: DateTime.Now,
            status: RegistrationStatus.Pending
        );
    }

    /// <summary>
    /// Xác nhận (duyệt) phiếu đăng ký tham dự.
    /// Quy tắc nghiệp vụ: Chỉ phiếu ở trạng thái Pending mới được phép chuyển sang Confirmed.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ném ra khi phiếu không ở trạng thái Pending.</exception>
    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
        {
            throw new InvalidOperationException($"Không thể xác nhận phiếu đăng ký đang ở trạng thái {Status}.");
        }

        Status = RegistrationStatus.Confirmed;
    }

    /// <summary>
    /// Huỷ phiếu đăng ký tham dự sự kiện.
    /// Quy tắc nghiệp vụ: Chỉ cho phép huỷ khi phiếu đang ở trạng thái Pending hoặc Confirmed.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ném ra khi phiếu đã bị huỷ trước đó.</exception>
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
        {
            throw new InvalidOperationException("Phiếu đăng ký đã ở trạng thái huỷ (Cancelled), không thể huỷ lại.");
        }

        Status = RegistrationStatus.Cancelled;
    }

    /// <summary>
    /// Trả về chuỗi đại diện mô tả phiếu đăng ký.
    /// </summary>
    public override string ToString()
    {
        return $"[Phiếu #{Id.ToString()[..8]}] Sinh viên: {MemberId.ToString()[..8]} | Sự kiện: {EventId.ToString()[..8]} | Trạng thái: {Status} | Thời gian: {RegisteredAt:dd/MM/yyyy HH:mm}";
    }
}
