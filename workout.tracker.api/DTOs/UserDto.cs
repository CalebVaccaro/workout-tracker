namespace workout_tracker.api.DTOs;

public class UserDto
{
    public string Name { get; set; }

    public static User ToUser(UserDto userDto)
    {
        return new User()
        {
            Name = userDto.Name
        };
    }
    
    public static UserDto ToUserDto(User user)
    {
        return new UserDto()
        {
            Name = user.Name
        };
    }
}