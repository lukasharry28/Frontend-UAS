namespace BlazeUTS.Service
{
    public class TokenService
    {
        private string _token;
        public event Action OnTokenChanged;

        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnTokenChanged?.Invoke();
            }
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    }
}