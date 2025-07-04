namespace StockAPI.Interfaces
{
    public interface IRedisCacheService
    {
        public Task<string> GetProductAsync(string key);
        public Task<bool> SetProductAsync(string key, string value, int time = 1);
        public Task Clear(string key);
    }    
}