namespace Web.UI.Iservice
{
    public interface ITokenProvider
    {
        public void SetToken(string token);
        public string GetToken();
        public void ClearToken();
    }
}
