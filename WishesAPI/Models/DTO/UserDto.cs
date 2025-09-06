namespace WishesAPI.Models.DTO;

public record UserDto(
    string Id,
    string? Username,
    string? Email
);