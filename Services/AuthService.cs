using System.Text;
using System.Security.Cryptography;
using MyJournal.Services.Interfaces;

namespace MyJournal.Services
{
    public class AuthService : IAuthService
    {
        private const string PinKey = "user_pin";
        private const string AttemptsKey = "pin_failed_attempts";
        private const string LockoutEndKey = "pin_lockout_end_timestamp";
        public  int MaxAllowedAttempts => 5;
        private const int LockoutDurationMinutes = 5;

        public async Task<bool> IsPinSetAsync()
        {
            var storedHash = await SecureStorage.Default.GetAsync(PinKey);
            return !string.IsNullOrEmpty(storedHash);
        }


        public async Task SetPinAsync(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                throw new ArgumentException("PIN cannot be empty or whitespace.");

            string hash = ComputeHash(pin);
            await SecureStorage.Default.SetAsync(PinKey, hash);
            ResetLockout();

        }

        public async Task<bool> ValidatePinAsync(string pin)
        {
            if (await IsLockedOutAsync()) return false;

            string? storedHash = await SecureStorage.Default.GetAsync(PinKey);
            if (string.IsNullOrEmpty(storedHash)) return false;

            string inputHash = ComputeHash(pin);
            if (storedHash == inputHash)
            {
                ResetLockout();
                return true;
            }
            else
            {
                HandleFailedAttempt();
                return false;
            }
        }

        public async Task<bool> IsLockedOutAsync()
        {
            if (Preferences.Default.ContainsKey(LockoutEndKey))
            {
                var LockoutEnd = Preferences.Default.Get(LockoutEndKey, DateTime.MinValue);
                if (DateTime.UtcNow < LockoutEnd)
                {
                    return true;
                }
                else
                {
                    ResetLockout();
                    return false;
                }
            }
            return false;
        }

        public Task<(int Attempts, TimeSpan RemainingLockout)> GetLockOutStatusAsync()
        {
            int attempts = Preferences.Default.Get(AttemptsKey, 0);
            TimeSpan remaining = TimeSpan.Zero;
            if (Preferences.Default.ContainsKey(LockoutEndKey))
            {
                var lockoutEnd = Preferences.Default.Get(LockoutEndKey, DateTime.MinValue);
                if (DateTime.UtcNow < lockoutEnd)
                {
                    remaining = lockoutEnd - DateTime.UtcNow;
                }
            }
            return Task.FromResult((attempts, remaining));
        }
        

        private void HandleFailedAttempt()
        {
            int currentAttempts = Preferences.Default.Get(AttemptsKey, 0) +1;
            Preferences.Default.Set(AttemptsKey, currentAttempts);
            if (currentAttempts >= MaxAllowedAttempts)
            {
                var lockoutEndTime = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);
                Preferences.Default.Set(LockoutEndKey, lockoutEndTime);
            }
        }

        public void ResetLockout()
        {
            Preferences.Default.Remove(AttemptsKey);
            Preferences.Default.Remove(LockoutEndKey);
        }
        private string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
