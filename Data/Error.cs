using Newtonsoft.Json;

namespace HotelApi.Data
{
    public class Error
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public string Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
