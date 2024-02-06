namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface ITvShowValidator
    {
        bool IsValidMonth(int month);
        bool IsValidYear(int year);
        (bool IsValid, string ErrorMessage) ValidateMonthAndYear(int month, int year);
    }
}