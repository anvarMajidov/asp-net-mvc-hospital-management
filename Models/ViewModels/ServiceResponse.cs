namespace HospitalService.Models.ViewModels
{
    public class ServiceResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}