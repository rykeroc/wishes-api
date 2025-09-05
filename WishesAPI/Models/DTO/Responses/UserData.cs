namespace WishesAPI.Models.DTO.Responses;

public record UserData(
    string Id,
    string? Username,
    string? Email
);