namespace MyJournal.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> IsPinSetAsync();
        Task SetPinAsync(string pin);
        Task<bool> ValidatePinAsync(string pin);
        Task<bool> IsLockedOutAsync();
        Task<(int Attempts, TimeSpan RemainingLockout)> GetLockOutStatusAsync();
        int MaxAllowedAttempts { get; }
    }
}