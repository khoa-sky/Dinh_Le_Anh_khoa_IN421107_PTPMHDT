namespace EduHub.Ch02.Domain;

/// <summary>Câu lạc bộ. Quy tắc nghiệp vụ nên nằm ở đây, không nằm trong ClubManager.</summary>
public class Club
{
    public const int MaxMembers = 50;
    public const int MaxNameLength = 100;

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // TODO (Nhiệm vụ 4): chuyển luật kiểm tra tên từ ClubManager.CreateClub vào đây.
    // Tên rỗng/toàn khoảng trắng -> ArgumentException; dài hơn MaxNameLength -> ArgumentException.
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("club name is required");
        if (name.Length > MaxNameLength)
            throw new ArgumentException("club name too long");
        return new Club{ Id = Guid.NewGuid(), Name = name, Description = description};
    }

    // TODO (Nhiệm vụ 4): trả về true khi số thành viên hiện tại còn nhỏ hơn MaxMembers.
    public bool HasRoom(int currentMemberCount) => currentMemberCount < MaxMembers;
    
}
