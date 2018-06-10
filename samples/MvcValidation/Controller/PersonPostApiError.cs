using WebApp.ApiModels;

namespace WebApp.Controller {
    public class PersonPostApiError {
        public string Message { get; set; }
        public PersonPostApiModel Model { get; set; }
        public string[] Errors { get; set; }
    }
}