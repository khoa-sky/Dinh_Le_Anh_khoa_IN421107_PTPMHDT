namespace EduHub.Ch01.Domain;

/// <summary>
/// Các phương thức điểm danh (check-in) được hệ thống EduHub hỗ trợ.
/// Sử dụng hằng số để tránh magic string rải rác trong mã nguồn.
/// </summary>
public static class CheckInMethod
{
    /// <summary>
    /// Điểm danh bằng cách quét mã QR trên ứng dụng/phiếu.
    /// </summary>
    public const string QR = "QR";

    /// <summary>
    /// Điểm danh thủ công bằng cách đối soát danh sách tại bàn lễ tân.
    /// </summary>
    public const string Manual = "Manual";
}

/// <summary>
/// Đại diện cho một bản ghi Điểm danh (Check-in) tại sự kiện.
/// Có quan hệ 1 - (0..1) với Phiếu đăng ký (Registration).
/// </summary>
public class CheckIn
{
    /// <summary>
    /// Khóa chính định danh duy nhất của lượt điểm danh (PK).
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Khóa ngoại tham chiếu đến Phiếu đăng ký tương ứng (FK -> Registration.Id).
    /// </summary>
    public Guid RegistrationId { get; private set; }

    /// <summary>
    /// Thời điểm sinh viên được quét mã điểm danh thực tế.
    /// </summary>
    public DateTime CheckedInAt { get; private set; }

    /// <summary>
    /// Hình thức điểm danh (QR, Manual...).
    /// </summary>
    public string Method { get; private set; }

    /// <summary>
    /// Hàm khởi tạo riêng tư (private constructor).
    /// </summary>
    private CheckIn(Guid id, Guid registrationId, DateTime checkedInAt, string method)
    {
        Id = id;
        RegistrationId = registrationId;
        CheckedInAt = checkedInAt;
        Method = method;
    }

    /// <summary>
    /// Phương thức Factory tạo bản ghi điểm danh (Check-in).
    /// Quy tắc nghiệp vụ: Bắt buộc truyền Registration hợp lệ và phiếu phải ở trạng thái Confirmed.
    /// </summary>
    /// <param name="registration">Phiếu đăng ký của sinh viên tham dự sự kiện.</param>
    /// <param name="method">Hình thức điểm danh (mặc định là Manual).</param>
    /// <returns>Đối tượng CheckIn hợp lệ.</returns>
    /// <exception cref="ArgumentNullException">Ném ra khi đối tượng registration bị null.</exception>
    /// <exception cref="InvalidOperationException">Ném ra khi phiếu đăng ký chưa được duyệt Confirmed.</exception>
    public static CheckIn Create(Registration registration, string method = CheckInMethod.Manual)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (registration.Status != RegistrationStatus.Confirmed)
        {
            throw new InvalidOperationException($"Chỉ phiếu đăng ký đã xác nhận (Confirmed) mới được phép check-in. Phiếu hiện tại đang ở trạng thái: {registration.Status}.");
        }

        var validMethod = string.IsNullOrWhiteSpace(method) ? CheckInMethod.Manual : method.Trim().ToUpperInvariant();

        return new CheckIn(
            id: Guid.NewGuid(),
            registrationId: registration.Id,
            checkedInAt: DateTime.Now,
            method: validMethod
        );
    }

    /// <summary>
    /// Trả về chuỗi tóm tắt thông tin điểm danh.
    /// </summary>
    /// <returns>Chuỗi thông tin tóm tắt.</returns>
    public string Summary()
    {
        return $"[CheckIn #{Id.ToString()[..8]}] Đã điểm danh lúc {CheckedInAt:HH:mm:ss dd/MM/yyyy} qua phương thức [{Method}] (Phiếu #{RegistrationId.ToString()[..8]})";
    }

    /// <summary>
    /// Trả về chuỗi đại diện mô tả lượt check-in.
    /// </summary>
    public override string ToString() => Summary();
}
