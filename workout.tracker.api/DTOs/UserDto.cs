namespace workout_tracker.api.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string? Name { get; set; } = default;

    public static User ToUser(UserDto userDto)
    {
        return new User()
        {
            Id = userDto.Id,
            Name = userDto.Name
        };
    }
    
    public static UserDto ToUserDto(User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        };
    }
}